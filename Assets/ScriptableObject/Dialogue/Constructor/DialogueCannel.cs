using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new DialogueData Cannel", menuName = "Scriptable Object / Dialogue Cannel")]
public class DialogueCannel : ScriptableObject
{
    void OnDisable()
    {
        StartInteractionEvent = null;
        StartTalkEvent = null;
        EndInteractionEvent = null;
        EndTalkEvent = null;
    }

    // 상호작용 시작, 끝 이벤트
    public event Action<Transform, DialogueDataContainer> StartInteractionEvent = null;
    public void Raise_StartInteractionEvent(Transform _tf, DialogueDataContainer _container)
    {
        if (StartInteractionEvent != null) StartInteractionEvent.Invoke(_tf, _container);
    }

    public event Action EndInteractionEvent = null;
    public void Raise_EndInteractionEvent()
    {
        if (EndInteractionEvent != null) EndInteractionEvent.Invoke();
    }

    // 대화 중 이벤트
    public event Action<DialogueDataContainer> StartTalkEvent = null;
    public void Raise_StartTalkEvent(DialogueDataContainer _container)
    {
        if (StartTalkEvent != null) StartTalkEvent.Invoke(_container);
    }

    public event Action EndTalkEvent= null;
    public void Raise_EndTalkEvent()
    {
        if (EndTalkEvent != null) EndTalkEvent.Invoke();
    }

    public event Action<DialogueData, int> ChangeContextEvent = null;
    public void Raise_ChangeContextEvent(DialogueData _data, int _count)
    {
        if (EndTalkEvent != null) ChangeContextEvent.Invoke(_data, _count);
    }
}
