using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent[] dialogueEvent;
    public bool tryEvent; // 오브젝트와 대화를 했는지 여부
    Transform currentTarget = null;

    private void Start()
    {
        StartCoroutine(Co_SetDialogueEvent());
        gameObject.SetActive(CheckEventAppearCondition(dialogueEvent[0]));
    }

    [HideInInspector]
    public bool isSetDialogeu = false; // 다이로그 정보 세팅 끝나면 true
    IEnumerator Co_SetDialogueEvent()
    {
        yield return new WaitUntil(() => DataBaseManager.instance.isFinish);

        for(int i = 0; i < dialogueEvent.Length; i++)
        {
            dialogueEvent[i].dialogues = SetDialogueEvent((int)dialogueEvent[i].line.x, (int)dialogueEvent[i].line.y);
        }
        isSetDialogeu = true;
    }

    Dialogue[] SetDialogueEvent(int p_LineX, int p_LineY)
    {
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
            // tryEvent = true; // 이 코드는 문제가 있음
            return dialogueEvent[currentEvent].dialogues;
        }
        else return SetDialogueEvent((int)dialogueEvent[currentEvent].afterLine.x, (int)dialogueEvent[currentEvent].afterLine.y);
    }

    public int CurrentEventNumber
    {
        get
        {
            return dialogueEvent[number].eventNumber;
        }
    }

    int number;

    int currentEvent
    {
        get
        {
            for(int i = 0; i < dialogueEvent.Length; i++)
            {
                if (CheckEventAppearCondition(dialogueEvent[i]))
                {
                    number = i;
                    return i;
                }
            }
            return 0;
        }
    }

    bool CheckEventAppearCondition(DialogueEvent p_Event)
    {
        bool flag = true;

        for (int i = 0; i < p_Event.eventConditions.Length; i++)
        {
            if (EventManager.instance.eventFlags[p_Event.eventConditions[i]] != p_Event.conditionFlag)
            {
                return false;
            }
        }

        // 등장 조건과 상관 없이 퇴장 조건을 만족하면 등장시키지 않음
        for (int i = 0; i < p_Event.endNumbers.Length; i++)
        {
            if (!EventManager.instance.eventFlags[p_Event.endNumbers[i]]) break;
            if (i == p_Event.endNumbers.Length - 1) flag = false;
        }

        return flag;
    }
}
