using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new DialogueData Cannel", menuName = "Scriptable Object / Dialogue Cannel")]
public class DialogueChannel : ScriptableObject
{
    [SerializeField] bool isInteractoin;
    public bool IsInteraction => isInteractoin;

    [SerializeField] bool isTalkable;
    public bool IsTalkable
    {
        get => isTalkable;
        set => isTalkable = value;
    }

    void OnDisable()
    {
        StartInteractionEvent = null;
        StartTalkEvent = null;
        EndInteractionEvent = null;
        EndTalkEvent = null;
        isInteractoin = false;
        isTalkable = true;
    }

    // 상호작용 시작, 끝 이벤트
    public event Action<Transform, DialogueDataContainer> StartInteractionEvent = null;
    public void Raise_StartInteractionEvent(Transform _tf, DialogueDataContainer _container)
    {
        isInteractoin = true;
        StartInteractionEvent?.Invoke(_tf, _container);
    }

    // 대화 시작 이벤트
    public event Action<DialogueDataContainer> StartTalkEvent = null;
    public void Raise_StartTalkEvent(DialogueDataContainer _container)
    {
        _container.Raise_ContainerDialogueEndEvent();
        StartTalkEvent?.Invoke(_container);
    }

    public event Action EndTalkEvent= null;
    public void Raise_EndTalkEvent()
    {
        EndTalkEvent?.Invoke();
    }

    public event Action EndInteractionEvent = null;
    public void Raise_EndInteractionEvent()
    {
        isInteractoin = false;
        EndInteractionEvent?.Invoke();
    }

    public event Action<DialogueData, int> ChangeContextEvent = null;
    public void Raise_ChangeContextEvent(DialogueData _data, int _count)
    {
        ChangeContextEvent?.Invoke(_data, _count);
    }
}
