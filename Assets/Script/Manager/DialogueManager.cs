using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Debug.LogError("다이어로그 매니저 싱글톤 2개");
            Destroy(gameObject);
        }
    }

    [SerializeField] CameraController cameraController;
    [SerializeField] CharacterManager characterManager;
    [SerializeField] SplashManager splashManager;
    [SerializeField] CutSceneManager cutSceneManager;
    [SerializeField] SpriteManager spriteManager;
    

    private void Update()
    {
        if(isNext && isTalking && ( Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) ) )
        {
            Talk();
        }
    }

    void Talk()
    {
        isNext = false;
        txt_Dialogue.text = "";
        if (++contextCount >= dialogues[talkIndex].contexts.Length) // 대화의 화자가 바뀔 때
        {
            contextCount = 0;
            if (++talkIndex < dialogues.Length) // 각종 카메라 연출
            {
                if (dialogues[talkIndex].cameraType == CameraType.Default) StartCoroutine(Co_CameraTargetTing());
                else if (dialogues[talkIndex].cutSceneName[contextCount].Trim() != "") StartCoroutine(Co_CameraCutScene(dialogues[talkIndex].cameraType));
                else StartCoroutine(Co_FadeCamera(dialogues[talkIndex].cameraType));
            }
            else EndTalk(); // 화자가 바뀔때만 talkIndex가 오르기 때문에 여기서 대화 종료 여부 결정
            return;
        }
        // 똑같은 화자가 2번 이상 말할 때 별도의 조건 없이 그냥 대사 출력
        StartCoroutine(Co_TypeWriter());
    }

    [SerializeField] PlayerController playerController;
    void EndTalk()
    {
        playerController.AngleReset();
        dialogues = null;
        talkIndex = 0;
        contextCount = 0;
        Set_DialogueUI(false);
        cameraController.CameraReset();
    }

    IEnumerator Co_CameraTargetTing()
    {
        cameraController.CameraTargettion(dialogues[talkIndex].tf_Target);
        yield return new WaitUntil(() => !cameraController.isTargetTing);
        StartCoroutine(Co_TypeWriter());
    }

    IEnumerator Co_FadeCamera(CameraType cameraType)
    {
        switch (cameraType)
        {
            case CameraType.FadeIn: splashManager.FadeIn(false); break;
            case CameraType.FadeOut: splashManager.FadeOut(false); break;
            case CameraType.FlashIn: splashManager.FadeIn(true); break;
            case CameraType.FlashOut: splashManager.FadeOut(true); break;
        }
        Set_DialogueUI(false);
        yield return new WaitUntil(() => !splashManager.isFade);
        StartCoroutine(Co_TypeWriter());
    }

    IEnumerator Co_CameraCutScene(CameraType cameraType)
    {
        string cutName = dialogues[talkIndex].cutSceneName[contextCount].Trim();
        switch (cameraType)
        {
            case CameraType.ShowCutScene: cutSceneManager.CutScene(cutName, false); break;
            case CameraType.HideCutScene: cutSceneManager.CutScene("", true); break;
        }

        Set_DialogueUI(false);
        yield return new WaitUntil(() => !cutSceneManager.isCutScene);
        StartCoroutine(Co_TypeWriter());
    }



    [SerializeField] GameObject obj_DialogueBar;
    [SerializeField] GameObject obj_NameBar;

    [SerializeField] Text txt_Dialogue;
    [SerializeField] Text txt_Name;

    
    Dialogue[] dialogues;
    public bool isTalking = false;
    bool isNext = false;
    int talkIndex;
    int contextCount;
    public void StartTalk(Dialogue[] p_Dialogues)
    {
        UIManager.instance.HideUI();
        isTalking = true;
        // 대화 시작
        dialogues = p_Dialogues;
        cameraController.CamOriginSetting();
        cameraController.CameraTargettion(dialogues[talkIndex].tf_Target);
        StartCoroutine(Co_TypeWriter());
    }

    void ChangeSprite()
    {
        Dialogue dialogue = dialogues[talkIndex];
        Transform target = dialogue.tf_Target;
        if (target != null)
        {
            spriteManager.SpriteChange(target, dialogue.spriteNames[contextCount]);
        }
    }

    void PlayVoice()
    {
        string _voiceName = dialogues[talkIndex].voiceNames[contextCount].Trim(); // Trim()은 감지가 안되는 " " 가 존재하는오류 때문에 사용
        if (_voiceName != "") SoundManager.instance.PlaySound(_voiceName, 2);
    }

    [SerializeField] float textDelayTime;
    IEnumerator Co_TypeWriter()
    {
        Set_DialogueUI(true);
        ChangeSprite();
        PlayVoice();
        txt_Dialogue.text = "";

        string replaceText = dialogues[talkIndex].contexts[contextCount];
        replaceText = ReplaceText(replaceText);

        char effectChar = ' '; // 어떤 효과를 줄지 구분하는 문자

        for (int i = 0; i < replaceText.Length; i++) // 글자 크기만큼 한글자씩 더하는 반복문
        {
            if (Set_IsColorText(replaceText[i])) // 더할 텍스트가 특수문자라면
            {
                // 색깔을 강조하고 싶은 글자 앞에 색깔 특수문자를 뒤에는 ⓦ를 넣어서 색깔 강조 탈출
                effectChar = replaceText[i];
                continue;
            }

            string addText = replaceText[i].ToString();
            txt_Dialogue.text += (effectChar != ' ' && effectChar != 'ⓦ') ? ColoringText(effectChar, addText) : addText;
            yield return new WaitForSeconds(textDelayTime);
        }
        isNext = true;
    }

    string ReplaceText(string p_Context) // 특수문자 치환
    {
        string replaceText = p_Context.Replace("|", ",");
        // unity에서 줄바꿈은 \n(escape문)이지만 엑셀에서의 \n은 텍스트이기 때문에 escape문으로 인식하기 위해 대체 
        replaceText = replaceText.Replace("\\n", "\n"); // \n 앞에 \(escape문)을 붙이면 뒤에 문자는 텍스트로 인식
        return replaceText;
    }

    void Set_DialogueUI(bool _flag)
    {
        obj_DialogueBar.SetActive(_flag);
        SetNameBar(_flag);
    }

    void SetNameBar(bool p_Flag)
    {
        if (!p_Flag)
        {
            obj_NameBar.SetActive(false);
            txt_Name.text = "";
            return;
        }

        if (dialogues[talkIndex].name == "" || dialogues[talkIndex].name == "독백") obj_NameBar.SetActive(false); 
        else
        {
            obj_NameBar.SetActive(true);
            txt_Name.text = ReturnName();
        }
    }

    string ReturnName()
    {
        string name = dialogues[talkIndex].name;
        if (name[0] == '⒳') name = name.Replace("⒳", "");
        return name;
    }

    //delegate void TalkEffect();

    bool Set_IsColorText(char char_Context) // 받은 인자가 특수문자면 true 아니면 false return
    {
        switch (char_Context)
        {
            case 'ⓦ':
            case 'ⓨ':
            case 'ⓒ':
                return true;
            case '①': // 여기부분 코드 바꾸기
            case '②':
            case '③':
            case '④':
            case '⑤':
                SoundManager.instance.PlaySound(ReturnSoundEffectName(char_Context), 1);
                splashManager.Splash();
                return false;
            default:
                return false;
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
            case 'ⓨ':
                return AddColorTag(affectText, "FFFF00");
            case 'ⓒ':
                return AddColorTag(affectText, "42DEE3");
            default:
                if(t_Effect != 'ⓦ') Debug.LogError("지정하지 않은 특수기호");
                return affectText;
        }
    }

    string AddColorTag(string p_ColoringText, string p_Color)
    {
        return "<color=#" + p_Color + ">" + p_ColoringText + "</color>";
    }
}
