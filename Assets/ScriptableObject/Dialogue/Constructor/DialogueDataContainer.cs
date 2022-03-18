using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEditor;

public enum DialogueTriggerType
{
    PlayerInteraction, // 일반적인 상호작용
    GameProduction, // 게임 내에서 컷씬같은 느낌으로 실행
}

[CreateAssetMenu(fileName = "new DialogueData Container", menuName = "Scriptable Object / Dialogue Container")]
public class DialogueDataContainer : ScriptableObject
{
    [SerializeField] string eventName;
    [SerializeField] DialogueDataParser dataParser = null;
    
    [SerializeField] DialogueData[] dialogueData = null;
    public DialogueData[] DialogueData => dialogueData;

    public void Init(DialogueDataParser _dataParser, string _eventName)
    {
        dataParser = _dataParser;
        eventName = _eventName;
    }

    //public bool Interactionable => eventCondition.GetCondition();
    void OnEnable()
    {
        if (dataParser == null) return;

        Debug.Log(eventName);
        dialogueData = dataParser.GetDialogue(eventName);
        eventCondition.Init(this);
    }

    
    public void Raise_ContainerDialogueEndEvent()
    {
        if (ContainerDialogueEndEvent == null) return;
        ContainerDialogueEndEvent.Invoke();
    }

    void OnDisable()
    {
        interactable = false;
        // 인스펙터에서 설정한거 빼고 다 없애는 기능임
        ContainerDialogueEndEvent.RemoveAllListeners();
        ChangeContainerEvent = null;
    }
    
    [Header("이벤트 조건 관련 필드")]
    [SerializeField] EventCondition eventCondition;

    [SerializeField] bool interactable = false;
    public bool Interactable => interactable;
    public void SetInteraction() => interactable = true;

    [SerializeField] DialogueDataContainer afterDialogue = null;
    public DialogueDataContainer AfterDialogue => afterDialogue;
    public event Action ChangeContainerEvent;
    public void Raise_ChangeContainerEvent()
    {
        if (afterDialogue == null || ChangeContainerEvent == null) return;
        ChangeContainerEvent.Invoke();
    }

    [Space][Space][Space]
    public UnityEvent ContainerDialogueEndEvent;

    public void DataReset()
    {
        interactable = false;
        eventCondition.Reset();
    }
    //public DialogueDataContainer GetAfterDialogue(out bool _ischange)
    //{
    //    if (afterDialogue == null)
    //    {
    //        _ischange = false;
    //        return this;
    //    }

    //    _ischange = eventCondition.GetAfterEventConditoin();
    //    return afterDialogue;
    //}
}


[Serializable]
public class DialogueData
{
    [Tooltip("카메라가 타겟팅할 대상")]
    public CameraType cameraType;

    public string characterName;
    public string[] contexts;

    public bool IsTalkEffect => (spriteNames.Length > 0 || voiceNames.Length > 0 || cutSceneName.Length > 0 || cameraTorque.Length > 0);
    public string[] spriteNames;
    public string[] voiceNames;
    public string[] cutSceneName;
    public string[] cameraTorque;
}

[Serializable]
public class EventCondition
{
    public void Init(DialogueDataContainer _container) // 사실상 생성자 함수
    {
        SetFirstNextEventConditon(_container);

        SubscribeOthersEvent(defaultEventConditions, _container.SetInteraction);
        SubscribeOthersEvent(nextEventConditions, _container.Raise_ChangeContainerEvent);
    }
    
    public void Reset()
    {
        defaultEventConditions = const_DefaultEventCondition;
        nextEventConditions = const_nextEventConditions;
    }

    // EventCondition은 하나의 이벤트에 대응하는 하나의 조건
    // EventCondition에 대응하는 이벤트를 보기 위해서는 interactionAble이 true여야 함
    // interactionAble이 true이기 위해서는 특정 이벤트들을 진행했어야 함
    // 기본 이벤트의 경우 리스트의 요소가 하나씩 삭제되고 카운트가 0이 되면 상호작용 허용
    [SerializeField] List<DialogueDataContainer> const_DefaultEventCondition = null;
    [SerializeField] List<DialogueDataContainer> defaultEventConditions = new List<DialogueDataContainer>();

    [SerializeField] List<DialogueDataContainer> const_nextEventConditions = null;
    [SerializeField] List<DialogueDataContainer> nextEventConditions = new List<DialogueDataContainer>();
    void SetFirstNextEventConditon(DialogueDataContainer _container)
    {
        nextEventConditions.Clear();
        nextEventConditions.Add(_container);
    }

    void SubscribeOthersEvent(List<DialogueDataContainer> _datas, Action _satisfyCondtionAct)
    {
        if (_datas == null || _datas.Count == 0)
        {
            _satisfyCondtionAct();
            return;
        }

        for (int i = 0; i < _datas.Count; i++)
        {
            DialogueDataContainer _container = _datas[i];
            _datas[i].ContainerDialogueEndEvent.AddListener(() => SubscribeEvent(_datas, _container, _satisfyCondtionAct));
        }
    }

    // 한번만 실행하고 다시 뺌
    void SubscribeEvent(List<DialogueDataContainer> _datas, DialogueDataContainer _otherContainer, Action _satisfyCondtionAct)
    {
        Debug.Log(_otherContainer.name);
        _otherContainer.ContainerDialogueEndEvent.RemoveListener( () => SubscribeEvent(_datas, _otherContainer, _satisfyCondtionAct));

        _datas.Remove(_otherContainer);
        // 조건 만족 시 행동
        if (_datas.Count == 0) _satisfyCondtionAct();
    }
}