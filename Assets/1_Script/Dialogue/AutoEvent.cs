using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AutoEventType
{
    FirstAndLastEvent,
    FirstEvent,
    BetweenEvent,
    LastEvent,
}

public class AutoEvent : MonoBehaviour
{
    public AutoEventType autoEventType;
    [SerializeField] GameObject nextEvent;
    InteractionEvent talkEvent;

    private void OnEnable()
    {
        talkEvent = GetComponent<InteractionEvent>();
        StartCoroutine(Co_Autotalk());
    }

    IEnumerator Co_Autotalk()
    {
        EventManager.isAutoEvent = true;
        //yield return new WaitUntil(() => talkEvent.isSetDialogeu);
        yield return new WaitUntil(() => !SceneTrasnferManager.isTransfer);
        yield return new WaitUntil(() => !EventManager.isEvent);

        yield return new WaitForSeconds(0.5f);
        //DialogueManager.instance.StartTalk(talkEvent.GetDialogues());
        //DialogueManager.instance.SetEvent(transform);

        //yield return new WaitUntil(() => !DialogueManager.instance.isTalking);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        SetNextEvent();
    }

    void SetNextEvent() // 연속 이벤트
    {
        if (nextEvent != null) 
        {
            nextEvent.SetActive(true);
            EventManager.isAutoEvent = true;
        }
        else EventManager.isAutoEvent = false;
    }

    void SetAutoEventStatus()
    {
        //switch (autoEventType)
        //{
        //    case AutoEventType.FirstAndLastEvent:
        //    case AutoEventType.LastEvent: 
        //        EventManager.isAutoEvent = false; break;
        //    case AutoEventType.FirstEvent:
        //    case AutoEventType.BetweenEvent:
        //        EventManager.isAutoEvent = true; break;
        //}
        if(autoEventType == AutoEventType.LastEvent) EventManager.isAutoEvent = false;
    }
}
