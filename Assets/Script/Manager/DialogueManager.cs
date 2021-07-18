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
        if (++contextCount >= dialogues[talkIndex].contexts.Length)
        {
            contextCount = 0;
            talkIndex++;
        }

        if (talkIndex < dialogues.Length) StartCoroutine(Co_TypeWriter());
        else EndTalk();
    }

    void EndTalk()
    {
        dialogues = null;
        isTalking = false;
        talkIndex = 0;
        contextCount = 0;
        Set_DialogueUI(false);
        UIManager.instance.ShowUI();
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
    public void ShowDialogue(Dialogue[] p_Dialogues)
    {
        UIManager.instance.HideUI();
        Set_DialogueUI(true);
        isTalking = true;

        dialogues = p_Dialogues;
        StartCoroutine(Co_TypeWriter());
    }

    [SerializeField] float textDelayTime;
    IEnumerator Co_TypeWriter()
    {
        Set_DialogueUI(true);
        txt_Name.text = dialogues[talkIndex].name;
        txt_Dialogue.text = "";

        string replaceText = dialogues[talkIndex].contexts[contextCount];
        replaceText = ReplaceText_ToComma(replaceText);
        for (int i = 0; i < replaceText.Length; i++)
        {
            txt_Dialogue.text += replaceText[i];
            yield return new WaitForSeconds(textDelayTime);
        }
        isNext = true;
    }

    string ReplaceText_ToComma(string p_Context) // 특수문자 콤마로 치환
    {
        string replaceText = p_Context.Replace("|", ",");
        return replaceText;
    }

    void Set_DialogueUI(bool _flag)
    {
        obj_DialogueBar.SetActive(_flag);
        obj_NameBar.SetActive(_flag);
    }
}
