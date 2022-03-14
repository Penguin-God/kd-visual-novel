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

[CreateAssetMenu(fileName = "new DialogueData Container", menuName = "Scriptable Object / Dialogue Data")]
public class DialogueDataContainer : ScriptableObject
{
    [SerializeField] DialogueDataParser dataParser = null;
    // 컴포넌트를 가지고 있는 오브젝트가 진행하는 이벤트 이름 (진행하면 EventManager의 eventFlags의 eventName에 맞는 value가 true가 됨)
    [SerializeField] string eventName;

    [SerializeField] DialogueData[] dialogueData = null;
    public DialogueData[] DialogueData => dialogueData;

    public bool Interactionable => eventCondition.GetCondition();
    void OnEnable()
    {
        Debug.Log(eventName);
        dialogueData = dataParser.GetDialogue(eventName);
        eventCondition.Init();
    }

    [Header("대화 관련 변수")]
    [Space][Space][Space]
    [SerializeField] DialogueData[] liens;


    [Header("이벤트 조건 관련 변수")]
    [SerializeField] EventCondition eventCondition;
    public Action interactionEndAct;

    [SerializeField] DialogueDataContainer afterDialogue = null;
    public DialogueDataContainer GetAfterDialogue(out bool _ischange)
    {
        if (afterDialogue == null)
        {
            _ischange = false;
            return this;
        }

        _ischange = eventCondition.GetAfterEventConditoin();
        return afterDialogue;
    }
}


[Serializable]
public class DialogueData
{
    [Tooltip("카메라가 타겟팅할 대상")]
    public Transform tf_Target;
    public CameraType cameraType;

    public string characterName;
    public string[] contexts;

    public string[] spriteNames;
    public string[] voiceNames;
    public string[] cutSceneName;
    public string[] fadeType;
}

[Serializable]
public class EventCondition
{
    public void Init() // 사실상 생성자 함수
    {
        SubscribeOtherEvents(defaultEventConditions, defaultEventConditionQueue);
        SubscribeOtherEvents(nextEventConditions, nextEventConditionQueue);
    }

    // EventCondition은 하나의 이벤트에 대응하는 하나의 조건
    // EventCondition에 대응하는 이벤트를 보기 위해서는 interactionAble이 true여야 함
    // interactionAble이 true이기 위해서는 특정 이벤트들을 진행했어야 함
    // 진행했어야 하는 이벤트의 경우 차곡차곡 쌓아둔 큐가 날라가고 카운트가 0이 되면 상호작용 허용
    private bool interactionAble = false;
    public bool GetCondition() => interactionAble;
    [SerializeField] DialogueDataContainer[] defaultEventConditions;
    Queue<bool> defaultEventConditionQueue = new Queue<bool>();


    private bool isNextEvent = false;
    public bool GetAfterEventConditoin() => isNextEvent;
    [SerializeField] DialogueDataContainer[] nextEventConditions;
    Queue<bool> nextEventConditionQueue = new Queue<bool>();

    void SubscribeOtherEvents(DialogueDataContainer[] _datas, Queue<bool> _conditionQueue)
    {
        if (_datas == null) return;

        for (int i = 0; i < _datas.Length; i++)
        {
            _conditionQueue.Enqueue(false);

            // 한번만 실행하고 다시 뺌
            _datas[i].interactionEndAct += () =>
            {
                SubscribeEvent(_conditionQueue);
                _datas[i].interactionEndAct -= () => SubscribeEvent(_conditionQueue);
            };
        }
    }

    void SubscribeEvent(Queue<bool> _conditionQueue)
    {
        _conditionQueue.Dequeue();
        if (_conditionQueue.Count == 0) interactionAble = true;
    }
}