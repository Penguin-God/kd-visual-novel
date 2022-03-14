using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new DialogueData Cannel", menuName = "Scriptable Object / Dialogue Cannel")]
public class DialogueCannel : ScriptableObject
{
    void OnDisable()
    {
        StartDialogueEvent = null;
        StartTalkEvent = null;
        EndDialogueEvent = null;
        EndTalkEvent = null;
    }

    public event Action<Transform, DialogueDataContainer> StartDialogueEvent = null;
    public void Raise_StartDialogueEvent(Transform _tf, DialogueDataContainer _container)
    {
        if(StartDialogueEvent != null) StartDialogueEvent.Invoke(_tf, _container);
    }

    
    public event Action<DialogueDataContainer> StartTalkEvent = null;
    public void Raise_StartTalkEvent(DialogueDataContainer _container)
    {
        if (StartTalkEvent != null) StartTalkEvent.Invoke(_container);
    }

    public event Action EndDialogueEvent = null;
    public void Raise_EndDialogueEvent()
    {
        if (EndDialogueEvent != null) EndDialogueEvent.Invoke();
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
