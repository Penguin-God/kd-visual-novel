using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public enum DialogueTriggerType
{
    PlayerInteraction, // 일반적인 상호작용
    GameProduction, // 게임 내에서 컷씬같은 느낌으로 실행
}

[CreateAssetMenu(fileName = "new Dialogue", menuName = "Scriptable Object / Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [SerializeField] DialogueFileData fileData = null;
    // 컴포넌트를 가지고 있는 오브젝트가 진행하는 이벤트 이름 (진행하면 EventManager의 eventFlags의 eventName에 맞는 value가 true가 됨)
    [SerializeField] string eventName;

    public Action interactionStartAct = null;
    public Action interactionEndAct = null;

    [Header("대화 관련 변수")]
    [Space][Space][Space]
    [SerializeField] LineData[] liens;

    [Header("이벤트 조건 관련 변수")]
    [SerializeField] EventCondition eventCondition;

    public LineData[] GetDialogue() => fileData.GetDialogue(eventName);
    public EventCondition GetCondition() => eventCondition;

    [SerializeField] DialogueData afterDialogue = null;
    public DialogueData GetAfterDialogue(out bool _ischange)
    {
        if (afterDialogue == null)
        {
            _ischange = false;
            return this;
        }
        else
        {
            _ischange = true;
            return afterDialogue;
        }
    }
}


[Serializable]
public class LineData
{
    [Tooltip("카메라가 타겟팅할 대상")]
    public Transform tf_Target;
    public CameraType cameraType;

    public string characterName;
    public string[] contexts;

    [HideInInspector]
    public string[] spriteNames;
    [HideInInspector]
    public string[] voiceNames;
    //[HideInInspector]
    public string[] cutSceneName;
}

[Serializable]
public class EventCondition
{
    private bool interactionAble;
    private bool eventDie; // 이 이벤트는 이제 못봄
    public bool GetCondition() => interactionAble;

    public void SubscribeOtherEvent()
    {
        if(mustSeeEvents == null && mustDontSeeEvents == null)
        {
            interactionAble = true;
            return;
        }

        SubscribeOtherEvent(mustSeeEvents);
        //SubscribeOtherEvent(mustDontSeeEvents);
    }

    // EventCondition은 하나의 이벤트에 대응하는 하나의 조건
    // EventCondition에 대응하는 이벤트를 보기 위해서는 interactionAble이 true여야 함
    // interactionAble이 true이기 위해서는 특정 이벤트를 진행하거나 하지 않았어야함
    // 진행했어야 하는 이벤트의 경우 차곡차곡 쌓아둔 큐가 날라가고 카운트가 0이 되면 상호작용 허용
    // 
    [SerializeField] List<DialogueData> mustSeeEvents;

    [SerializeField] Queue<bool> conditionFlgs = new Queue<bool>();
    void SubscribeOtherEvent(List<DialogueData> _datas)
    {
        if (_datas == null) return;

        for (int i = 0; i < _datas.Count; i++)
        {
            conditionFlgs.Enqueue(false);
            _datas[i].interactionEndAct += () =>
            {
                conditionFlgs.Dequeue();
                if (conditionFlgs.Count == 0) interactionAble = true;

                _datas[i].interactionEndAct -= () =>
                {
                    conditionFlgs.Dequeue();
                    if (conditionFlgs.Count == 0) interactionAble = true;
                };
            };
        }
    }


    [SerializeField] List<DialogueData> mustDontSeeEvents;
    void SubscribeCancelOtherEvent(List<DialogueData> _datas)
    {
        if (_datas == null) return;

        for (int i = 0; i < _datas.Count; i++)
        {
            _datas[i].interactionEndAct -= () =>
            {
                conditionFlgs.Dequeue();
                if (conditionFlgs.Count == 0) interactionAble = true;
            };
        }
    }

    public string[] eventConditions; // 등장 조건에 포함되는 이벤트 넘버 (무조건 등장시키려면 0번째 event를 지정하고 conditionFlag을 false로 하면됨)
    public bool conditionFlag; // 이벤트를 보는게 등장 조건인지 보지 않은게 등장 조건인지 결정하는 변수 
    public string endEvnetName; // 퇴장 이벤트 넘버
}