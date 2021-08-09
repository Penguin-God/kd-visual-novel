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
    private void Start()
    {
        talkEvent = GetComponent<InteractionEvent>();
        StartCoroutine(Co_Autotalk());
    }

    IEnumerator Co_Autotalk()
    {
        EventManager.isAutoEvent = true;
        yield return new WaitUntil(() => talkEvent.isSetDialogeu);
        yield return new WaitForSeconds(0.5f);
        DialogueManager.instance.StartTalk(talkEvent.GetDialogues());
        DialogueManager.instance.SetEvent(transform);

        yield return new WaitUntil(() => !DialogueManager.instance.isTalking);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        SetNextEvent();
        SetAutoEventStatus();
    }

    void SetNextEvent() // 연속 이벤트
    {
        if (nextEvent != null)  nextEvent.SetActive(true);
    }

    void SetAutoEventStatus()
    {
        if(autoEventType == AutoEventType.LastEvent) EventManager.isAutoEvent = false;
    }
}
