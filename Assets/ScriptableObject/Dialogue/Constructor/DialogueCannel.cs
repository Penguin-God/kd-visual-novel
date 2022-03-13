using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new DialogueData Cannel", menuName = "Scriptable Object / Dialogue Cannel")]
public class DialogueCannel : ScriptableObject
{
    void OnDisable()
    {
        startDialogueEvent = null;
        startTalk = null;
        endDialogueEvent = null;
        endTalk = null;
    }

    public event Action<Transform, DialogueDataContainer> startDialogueEvent = null;
    public void StartDialogueEvent(Transform _tf, DialogueDataContainer _container)
    {
        if(startDialogueEvent != null) startDialogueEvent.Invoke(_tf, _container);
    }

    
    public event Action<DialogueDataContainer> startTalk = null;
    public void StartTalk(DialogueDataContainer _container)
    {
        if (startTalk != null) startTalk.Invoke(_container);
    }

    public event Action endDialogueEvent = null;
    public void EndDialogueEvent()
    {
        if (endDialogueEvent != null) endDialogueEvent.Invoke();
    }

    public event Action endTalk = null;
    public void EndTalk()
    {
        if (endTalk != null) endTalk.Invoke();
    }
}
