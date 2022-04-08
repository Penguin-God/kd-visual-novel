using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueCondition
{
    [SerializeField] InteractionEvent target = null;
    [SerializeField] List<DialogueDataContainer> prevConditions = null;
    public bool IsReady => prevConditions.Count == 0;

    public event Action<InteractionEvent, DialogueDataContainer> OnConditionCountChange;
    void RemoveCondition(InteractionEvent _interaction, DialogueDataContainer _removeDialogue, DialogueDataContainer _afterDialogu)
    {
        prevConditions.Remove(_removeDialogue);
        Debug.Log(_removeDialogue.name);
        Debug.Log(_removeDialogue.DialogueCondition.OnConditionCountChange == null);
        Debug.Log(_afterDialogu.name);
        if (IsReady) _removeDialogue.DialogueCondition.OnConditionCountChange?.Invoke(_interaction, _afterDialogu);
    }

    public void Setup(InteractionEvent _interaction, DialogueDataContainer _afterDialogue)
    {
        foreach (var _con in prevConditions)
            _con.OnFirstInteraction += () => RemoveCondition(_interaction, _con, _afterDialogue);
    }
}