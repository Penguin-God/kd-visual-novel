using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueCondition
{
    [SerializeField] List<DialogueDataContainer> prevConditions = null;
    public bool IsReadyToTalk => prevConditions.Count == 0;

    //public event Action<InteractionObject, DialogueDataContainer> OnDialogueChange;
    void RemoveCondition(DialogueObject _dialogueObject, DialogueDataContainer _removeDialogue, DialogueDataContainer _newDialogue)
    {
        prevConditions.Remove(_removeDialogue);

        Debug.Log(_removeDialogue.name);
        Debug.Log(_newDialogue.name);
        
        if (IsReadyToTalk) _dialogueObject.ChangeDialogue(_newDialogue);
        
    }

    public void Setup(DialogueObject _dialogueObject, DialogueDataContainer _newDialogue)
    {
        foreach (var _con in prevConditions)
        {
            _con.OnFirstInteraction += () => RemoveCondition(_dialogueObject, _con, _newDialogue);
        }
    }

    public void ConditionChangeAsClone(List<DialogueDataContainer> _containers)
    {
        for (int conditionIndex = 0; conditionIndex <  prevConditions.Count; conditionIndex++)
        {
            for (int i = 0; i < _containers.Count; i++)
            {
                if(prevConditions[conditionIndex].CodeName == _containers[i].CodeName)
                {
                    prevConditions[conditionIndex] = _containers[i];
                    break;
                }
            }
        }
    }
}