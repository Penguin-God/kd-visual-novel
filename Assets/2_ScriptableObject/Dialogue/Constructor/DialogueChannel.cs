using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new DialogueData Cannel", menuName = "Scriptable Object / Dialogue Cannel")]
public class DialogueChannel : ScriptableObject
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
    // 대화 시작 이벤트
    public event Action<DialogueDataContainer> StartTalkEvent = null;
    // 두개 이벤트 묶어서 같이 사용함
    public void Raise_StartTalkEvent(Transform _tf, DialogueDataContainer _container)
    {
        if (StartInteractionEvent != null) StartInteractionEvent.Invoke(_tf, _container);
        if (StartTalkEvent != null) StartTalkEvent.Invoke(_container);
    }

    public event Action EndInteractionEvent = null;
    public void Raise_EndInteractionEvent()
    {
        if (EndInteractionEvent != null) EndInteractionEvent.Invoke();
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
