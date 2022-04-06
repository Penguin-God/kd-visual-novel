using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[Serializable]
class DialogueConditionss
{
    public DialogueDataContainer container;
    public EventCondition eventCondition;

    public DialogueConditionss(DialogueConditionss _init, Action<DialogueDataContainer> _containerChageAct)
    {
        container = _init.container;
        eventCondition = _init.eventCondition;
        eventCondition.Init(container, _containerChageAct);
    }
}



[CreateAssetMenu(fileName = "new Dialogue MC", menuName = "Scriptable Object / Dialogue MC")]
public class DialogueMC : ScriptableObject
{
    [SerializeField] DialogueDataContainer currentDialogue = null;
    public DialogueDataContainer CurrentDialogue => currentDialogue;

    [SerializeField] DialogueConditionss[] conditoins = null;

    void OnEnable()
    {
        for (int i = 0; i < conditoins.Length; i++)
            conditoins[i] = new DialogueConditionss(conditoins[i], ChangeCurrentDialogue);
    }

    void ChangeCurrentDialogue(DialogueDataContainer _newDialogue)
    {
        if (_newDialogue == null || currentDialogue == _newDialogue) return;

        currentDialogue = _newDialogue;
        if (ContainerChangeEvent != null) ContainerChangeEvent();
    }
    public event Action ContainerChangeEvent = null;


    [ContextMenu("Data Reset")]
    public void DataReset()
    {
        currentDialogue = conditoins[0].container;
        for (int i = 0; i < conditoins.Length; i++)
        {
            conditoins[i].container.DataReset();
            conditoins[i].eventCondition.Reset();
        }
    }
}


[Serializable]
public class EventCondition
{
    [SerializeField] DialogueDataContainer afterDialogue = null;
    [SerializeField] bool isNextConditionOnlySelf;

    public void Init(DialogueDataContainer _container, Action<DialogueDataContainer> _containerChageAct)
    {
        SubscribeOthersEvent(defaultEventConditions, _container.SetInteraction);

        if (isNextConditionOnlySelf) _container.ContainerDialogueEndEvent.AddListener(() => _containerChageAct(afterDialogue));
        else SubscribeOthersEvent(nextEventConditions, () => _containerChageAct(afterDialogue));
    }

    // EventCondition은 하나의 이벤트에 대응하는 하나의 조건
    // EventCondition에 대응하는 이벤트를 보기 위해서는 interactionAble이 true여야 함
    // interactionAble이 true이기 위해서는 특정 이벤트들을 진행했어야 함
    // 기본 이벤트의 경우 리스트의 요소가 하나씩 삭제되고 카운트가 0이 되면 상호작용 허용
    [SerializeField] List<DialogueDataContainer> const_DefaultEventCondition = null;
    [SerializeField] List<DialogueDataContainer> defaultEventConditions = new List<DialogueDataContainer>();

    [SerializeField] List<DialogueDataContainer> const_nextEventConditions = null;
    [SerializeField] List<DialogueDataContainer> nextEventConditions = new List<DialogueDataContainer>();

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
        _otherContainer.ContainerDialogueEndEvent.RemoveListener(() => SubscribeEvent(_datas, _otherContainer, _satisfyCondtionAct));
        _datas.Remove(_otherContainer);
        // 조건 만족 시 행동
        if (_datas.Count == 0) _satisfyCondtionAct();
    }

    public void Reset()
    {
        defaultEventConditions = const_DefaultEventCondition;
        nextEventConditions = const_nextEventConditions;
    }
}