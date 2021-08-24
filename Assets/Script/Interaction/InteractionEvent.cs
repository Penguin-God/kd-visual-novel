using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent[] dialogueEvent;
    public int currentEvent;
    public bool tryEvent; // 오브젝트와 대화를 했는지 여부
    Transform currentTarget = null;

    public event Action OnSetDialogue;

    private void Start()
    {
        StartCoroutine(Co_SetDialogueEvent());
    }

    [HideInInspector]
    public bool isSetDialogeu = false; // 다이로그 정보 세팅 끝나면 true
    IEnumerator Co_SetDialogueEvent()
    {
        yield return new WaitUntil(() => DataBaseManager.instance.isFinish);
        //DialogueEvent instanceDialogues = new DialogueEvent();
        //instanceDialogues[currentEvent].dialogues = new Dialogue[(int)dialogueEvent.line.y - (int)dialogueEvent.line.x + 1]; //[currentEvent] dialogues의 크기 결정 (크기는 dialogueEvent에서 결정)
        //instanceDialogues[currentEvent].dialogues = DataBaseManager.instance.GetDialogues((int)dialogueEvent.line.x, (int)dialogueEvent.line.y); //[currentEvent] dialogues 선언

        //for (int i = 0; i < instanceDialogues[currentEvent].dialogues.Length; i++) // 각종 변수 대입
        //{
        //    Dialogue dialogue = instanceDialogues[currentEvent].dialogues[i];

        //    // 이름 앞에 ⒳가 붙어 있으면 타겟팅 안하는거임 
        //    if (dialogue.name[0] != '⒳') dialogue.tf_Target = CharacterManager.instance.dic_Character[dialogue.name];

        //    // target은 현재 대화 상대를 의미 주인공이 말하거나 독백할때도 target에는 대화상대가 들어감
        //    if (dialogue.tf_Target == null) dialogue.tf_Target = currentTarget;
        //    else currentTarget = dialogue.tf_Target;

        //    if (dialogueEvent[currentEvent].dialogues.Length > i) // 인스펙처 장에서 선언한 dialogueEvent 덮어쓰기용
        //    {
        //        dialogue.cameraType = dialogueEvent[currentEvent].dialogues[i].cameraType;
        //    }
        //}
        //// 선언 후 반환
        //dialogueEvent[currentEvent].dialogues = instanceDialogues[currentEvent].dialogues;

        dialogueEvent[currentEvent].dialogues = SetDialogueEvent(dialogueEvent[currentEvent].dialogues, (int)dialogueEvent[currentEvent].line.x, (int)dialogueEvent[currentEvent].line.y);
        isSetDialogeu = true;
    }

    Dialogue[] SetDialogueEvent(Dialogue[] p_Dialogues, int p_LineX, int p_LineY)
    {
        OnSetDialogue();

        Dialogue[] t_Dialogue = DataBaseManager.instance.GetDialogues(p_LineX, p_LineY); //[currentEvent] dialogues 선언
        for (int i = 0; i < t_Dialogue.Length; i++) // 각종 변수 대입
        {
            // 이름 앞에 ⒳가 붙어 있으면 타겟팅 안하는거임 
            if (t_Dialogue[i].name[0] != '⒳') t_Dialogue[i].tf_Target = CharacterManager.instance.dic_Character[t_Dialogue[i].name];

            // target은 현재 대화 상대를 의미 주인공이 말하거나 독백할때도 target에는 대화상대가 들어감
            if (t_Dialogue[i].tf_Target == null) t_Dialogue[i].tf_Target = currentTarget;
            else currentTarget = t_Dialogue[i].tf_Target;

            if (dialogueEvent[currentEvent].dialogues.Length > i) // 인스펙처 장에서 선언한 dialogueEvent 덮어쓰기용
            {
                t_Dialogue[i].cameraType = dialogueEvent[currentEvent].dialogues[i].cameraType;
            }
        }
        return t_Dialogue;
    }

    public Dialogue[] GetDialogues() // 대화 여부에 따라 다른 대화 정보를 보냄
    {
        if (!tryEvent)
        {
            tryEvent = true;
            return dialogueEvent[currentEvent].dialogues;
        }
        else return SetDialogueEvent(dialogueEvent[currentEvent].dialogues, (int)dialogueEvent[currentEvent].afterLine.x, (int)dialogueEvent[currentEvent].afterLine.y);
    }
}
