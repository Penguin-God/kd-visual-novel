﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] protected DialogueEvent[] dialogueEvents;
    Transform currentTarget = null;

    private void Start()
    {
        if (dialogueEvents.Length > 0)
        {
            StartCoroutine(Co_SetDialogueEvent());
        }
    }

    [HideInInspector]
    public bool isSetDialogeu = false; // 다이로그 정보 세팅 끝나면 true
    IEnumerator Co_SetDialogueEvent() // 오브젝트가 가지고 있는 모든 이벤트 세팅
    {
        yield return new WaitUntil(() => DataBaseManager.instance.isFinish);

        for(int i = 0; i < dialogueEvents.Length; i++)
        {
            dialogueEvents[i].dialogues = SetDialogueEvent(dialogueEvents[i].dialogues, dialogueEvents[i].eventName);
            dialogueEvents[i].talkCondition = DataBaseManager.instance.dic_TalkCondition[dialogueEvents[i].eventName];
        }
        isSetDialogeu = true;

        yield return null;
        gameObject.SetActive(CheckEventAppearCondition(dialogueEvents[CurrentEventNumber]));
    }

    // 하나의 이벤트 대화 세팅
    Dialogue[] SetDialogueEvent(Dialogue[] p_Dialogue, string eventName)
    {
        Dialogue[] t_Dialogue = DataBaseManager.instance.GetDialogues(eventName);

        for (int i = 0; i < t_Dialogue.Length; i++) // 각종 변수 대입
        {
            // 이름 앞에 ⒳가 붙어 있으면 타겟팅 안하는거임
            if (t_Dialogue[i].name[0] != '⒳') t_Dialogue[i].tf_Target = CharacterManager.instance.dic_Character[t_Dialogue[i].name];

            // target은 현재 대화 상대를 의미 주인공이 말하거나 독백할때도 target에는 대화상대가 들어감
            if (t_Dialogue[i].tf_Target == null) t_Dialogue[i].tf_Target = currentTarget;
            else currentTarget = t_Dialogue[i].tf_Target;

            if (p_Dialogue.Length > i) // 인스펙처 장에서 선언한 dialogueEvents 덮어쓰기용
            {
                t_Dialogue[i].cameraType = p_Dialogue[i].cameraType;
            }
        }
        return t_Dialogue;
    }

    public Dialogue[] GetDialogues() // 대화 여부에 따라 다른 대화 정보를 보냄
    {
        DialogueEvent dialogueEvent = dialogueEvents[CurrentEventNumber];
        // 대화 이벤트를 보지 않았거나 After 대사가 없으면 같은 대사 출력
        if (!EventManager.instance.eventFlags[dialogueEvent.eventName] || !dialogueEvent.isAfter) 
        {
            DialogueManager.instance.SetEvent(transform);
            EventManager.instance.eventFlags[CurrentEventName] = true;
            return dialogueEvent.dialogues;
        }
        else return SetDialogueEvent(dialogueEvent.afterDialogues, dialogueEvent.eventName + "After");
    }

    public virtual void StartInteraction()
    {

    }

    public string CurrentEventName
    {
        get
        {
            return dialogueEvents[CurrentEventNumber].eventName;
        }
    }
    

    public int CurrentEventNumber
    {
        get
        {
            for(int i = 0; i < dialogueEvents.Length; i++)
            {
                if (CheckEventAppearCondition(dialogueEvents[i]))
                {
                    saveCurrentNumber = i;
                    return i;
                }
            }
            return saveCurrentNumber;
        }
    }
    int saveCurrentNumber = 0;

    // 인자로 온 이벤트가 현재 진행되는게 맞는지 체크하는 함수
    bool CheckEventAppearCondition(DialogueEvent p_Event)
    {
        bool flag = true;

        // 등장 조건과 맞지 않으면 false
        for (int i = 0; i < p_Event.talkCondition.eventConditions.Length; i++)
        {
            if (EventManager.instance.eventFlags[p_Event.eventName] != p_Event.talkCondition.conditionFlag)
            {
                return false;
            }
        }

        // 등장 조건과 상관 없이 퇴장 조건을 만족하면 등장시키지 않음
        if (EventManager.instance.eventFlags.TryGetValue(p_Event.talkCondition.endEvnetName, out bool endFlag))
            flag = !endFlag;

        return flag;
    }
}
