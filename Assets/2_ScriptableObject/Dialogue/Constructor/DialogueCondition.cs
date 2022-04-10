using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueCondition
{
    [SerializeField] List<DialogueDataContainer> prevConditions = null;
    public bool IsReady => prevConditions.Count == 0;

    public event Action<InteractionObject, DialogueDataContainer> OnDialogueChange;
    void RemoveCondition(InteractionObject _interaction, DialogueDataContainer _removeDialogue, DialogueDataContainer _newDialogue)
    {
        prevConditions.Remove(_removeDialogue);
        Debug.Log(_removeDialogue.name);
        Debug.Log(_newDialogue.name);
        if (IsReady)
        {
            _interaction.ChangeDialogue(_newDialogue);
            OnDialogueChange?.Invoke(_interaction, _newDialogue);
        }
    }

    public void Setup(InteractionObject _interaction, DialogueDataContainer _newDialogue)
    {
        foreach (var _con in prevConditions)
            _con.OnFirstInteraction += () => RemoveCondition(_interaction, _con, _newDialogue);
    }
}