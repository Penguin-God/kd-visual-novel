using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }
    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] DialogueChannel dialogueChannel;
    void Start()
    {
        dialogueChannel.StartTalkEvent += StartTalk;
        dialogueChannel.EndTalkEvent += () => Set_DialogueUI(false);

        //EventManager eventManager = FindObjectOfType<EventManager>();
    }

    public bool isTalking;
    bool isContextTyping = false;

    void StartTalk(DialogueDataContainer _container)
    {
        isTalking = true;

        // 대화 시작
        Set_DialogueUI(true);
        StartCoroutine(Co_Talk(_container.DialogueData));
    }

    bool TalkInput => (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetButton("Ctrl"));

    IEnumerator Co_Talk(DialogueData[] _data)
    {
        for (int _talkIndex = 0; _talkIndex < _data.Length; _talkIndex++)
        {
            SetNameBar(true, _data[_talkIndex]);
            for (int _contextIndex = 0; _contextIndex < _data[_talkIndex].contexts.Length; _contextIndex++)
            {
                // 이벤트
                dialogueChannel.Raise_ChangeContextEvent(_data[_talkIndex], _contextIndex);
                // 대사 타이핑
                string _typingText = _data[_talkIndex].contexts[_contextIndex];
                StartCoroutine(Co_TypeWriter(_typingText));
                // 대기 후 반복문 넘김
                yield return new WaitUntil(() => !isContextTyping && TalkInput);
            }
        }

        dialogueChannel.Raise_EndTalkEvent();
        isCameraEffect = true;
        isTalking = false;
    }

    public bool isCameraEffect = false; // 나중에 없애야 됨
    //void Talk()
    //{
    //    txt_Dialogue.text = "";
    //    dialogueCannel.Raise_ChangeContextEvent(, contextCount);

    //    if (++contextCount >= dialogues[talkIndex].contexts.Length) // 대화의 화자가 바뀔 때
    //    {
    //        contextCount = 0; // 대사 순번 초기화
    //        if (++talkIndex < dialogues.Length) // 화자는 바뀌지만 대화의 끝이 아닐 때 각종 연출
    //        {
    //            StartCoroutine(Co_BeforeTalkEvent(dialogues[talkIndex], contextCount));
    //        }
    //        else StartCoroutine(EndTalk()); // 화자가 바뀔때만 talkIndex가 오르기 때문에 여기서 대화 종료 여부 결정
    //        return;
    //    }
    //    // 똑같은 화자가 2번 이상 말할 때 별도의 조건 없이 그냥 대사 출력
    //    StartCoroutine(Co_TypeWriter());
    //}

    //public event Action<DialogueData, int> BeforeTalkEvent;
    //IEnumerator Co_BeforeTalkEvent(DialogueData dialogue, int contextCount)
    //{
    //    dialogueCannel.Raise_EndTalkEvent();
    //    if (BeforeTalkEvent != null) BeforeTalkEvent(dialogue, contextCount);
    //    Set_DialogueUI(false);
    //    yield return new WaitUntil(() => !isCameraEffect);
    //    StartCoroutine(Co_TypeWriter());
    //}

    //[SerializeField] CutSceneManager cutSceneManager;
    //public event Action OnEndTalk;
    //private EventByTalk eventByTalk = null;
    //IEnumerator EndTalk()
    //{
    //    if (cutSceneManager.CheckCutScene)
    //    {
    //        cutSceneManager.CutScene("", true);
    //        Set_DialogueUI(false);
    //        yield return new WaitUntil(() => !isCameraEffect);
    //    }

    //    dialogues = null;
    //    talkIndex = 0;
    //    contextCount = 0;
    //    Set_DialogueUI(false);

    //    dialogueCannel.Raise_EndDialogueEvent();
    //    //if(OnEndTalk != null) OnEndTalk();
    //}

    public void SetEvent(Transform target)
    {
        //if (target.GetComponent<EventByTalk>() != null) eventByTalk = target.GetComponent<EventByTalk>();
        //else eventByTalk = null;
    }


    [SerializeField] float textDelayTime;
    private float ApplyTextDelayTime { get { return Input.GetButton("Ctrl") ? 0 : textDelayTime; } }
    public event Action<DialogueData, int> AfterTalkEvent;

    IEnumerator Co_TypeWriter(string _context)
    {
        isContextTyping = true;
        //if (AfterTalkEvent != null) AfterTalkEvent(dialogues[talkIndex], contextCount);
        txt_Dialogue.text = "";

        string replaceText = ReplaceText(_context);

        char effectChar = ' '; // 어떤 효과를 줄지 구분하는 문자

        for (int i = 0; i < replaceText.Length; i++) // 글자 크기만큼 한글자씩 더하는 반복문
        {
            if (Check_IsColorText(replaceText[i])) // 더할 텍스트가 특수문자라면
            {
                // 색깔을 강조하고 싶은 글자 앞에 색깔 특수문자를 뒤에는 ⓦ를 넣어서 색깔 강조 탈출
                effectChar = replaceText[i];
                continue;
            }
            else if(Check_IsEffectSoundText(replaceText[i]) != ' ') // 이펙트 사운드 재생
            {
                SoundManager.instance.PlayEffectSound(ReturnSoundEffectName(replaceText[i]));
                continue;
            }

            string addText = replaceText[i].ToString();
            txt_Dialogue.text += (effectChar != ' ' && effectChar != 'ⓦ') ? ColoringText(effectChar, addText) : addText;
            yield return new WaitForSeconds(ApplyTextDelayTime);
        }

        isContextTyping = false;
    }

    string ReplaceText(string p_Context) // 특수문자 치환
    {
        string replaceText = p_Context.Replace("|", ",");
        // unity에서 줄바꿈은 \n(escape문)이지만 엑셀에서의 \n은 텍스트이기 때문에 escape문으로 인식하기 위해 대체 
        replaceText = replaceText.Replace("\\n", "\n"); // \n 앞에 \(escape문)을 붙이면 뒤에 문자는 텍스트로 인식
        return replaceText;
    }

    bool Check_IsColorText(char char_Context) // 받은 인자가 특수문자면 true혹은 특정 연출 실행 후 
    {
        switch (char_Context)
        {
            case 'ⓦ':
            case 'ⓨ':
            case 'ⓒ':
                return true;
            default:
                return false;
        }
    }

    char Check_IsEffectSoundText(char char_Context) // 받은 인자가 특수문자면 true혹은 특정 연출 실행 후 
    {
        switch (char_Context)
        {
            case '①':
            case '②':
            case '③':
            case '④':
            case '⑤':
                return char_Context;
            default:
                return ' ';
        }
    }
    string ReturnSoundEffectName(char number)
    {
        string name = "Emotion";
        switch (number)
        {
            case '①': name += "1" ; break;
            case '②': name += "2" ; break;
            case '③': name += "3" ; break;
            case '④': name += "4" ; break;
            case '⑤': name += "5" ; break;
        }
        return name;
    }

    string ColoringText(char t_Effect, string affectText) // 받은 특수문자에 맞는 효과를 string 인자에 구현 후 return
    {
        switch (t_Effect)
        {
            case 'ⓨ': return AddColorTag(affectText, "FFFF00");
            case 'ⓒ': return AddColorTag(affectText, "42DEE3");
            default: if(t_Effect != 'ⓦ') Debug.LogError("지정하지 않은 특수기호"); return affectText;
        }
    }
    string AddColorTag(string p_ColoringText, string p_Color)
    {
        return "<color=#" + p_Color + ">" + p_ColoringText + "</color>";
    }


    [SerializeField] GameObject obj_DialogueBar;
    [SerializeField] GameObject obj_NameBar;
    [SerializeField] Text txt_Dialogue;
    [SerializeField] Text txt_Name;

    void Set_DialogueUI(bool _flag)
    {
        obj_DialogueBar.SetActive(_flag);
        SetNameBar(_flag);
    }
    
    void SetNameBar(bool p_Flag, DialogueData _data = null)
    {
        if (!p_Flag || _data == null)
        {
            obj_NameBar.SetActive(false);
            txt_Name.text = "";
            return;
        }

        if(ReturnName(_data) == "독백")
        {
            obj_NameBar.SetActive(false);
            txt_Name.text = "";
        }
        else
        {
            obj_NameBar.SetActive(true);
            txt_Name.text = ReturnName(_data);
        }
    }

    string ReturnName(DialogueData _data)
    {
        string name = _data.characterName;
        if (name[0] == '⒳') name = name.Replace("⒳", "");
        return name;
    }

    
}
