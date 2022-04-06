using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueCondition
{
    [SerializeField] InteractionEvent target = null;

    [SerializeField] List<DialogueDataContainer> prevConditions = null;
    public bool IsReady => prevConditions.Count == 0;

    public event Action OnConditionCountChange;
    void RemoveCondition(DialogueDataContainer _con)
    {
        prevConditions.Remove(_con);
        if(IsReady) OnConditionCountChange?.Invoke();
    }

    public void Setup()
    {
        foreach (var _con in prevConditions)
            _con.OnFirstDialogueEnd += () => RemoveCondition(_con);
    }
}