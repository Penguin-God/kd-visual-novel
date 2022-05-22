using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class DialogueCondition
{
    [SerializeField] List<DialogueDataContainer> prevConditions = null;
    public bool IsReadyToTalk => prevConditions.Count == 0;

    void RemoveCondition(DialogueObject _dialogueObject, DialogueDataContainer _removeDialogue, DialogueDataContainer _newDialogue)
    {
        prevConditions.Remove(_removeDialogue);
        Debug.Assert(_newDialogue != null, "새로운 다이어로그가 null임");
        if (IsReadyToTalk) _dialogueObject.ChangeDialogue(_newDialogue);
    }

    public void Setup(DialogueObject _dialogueObject, DialogueDataContainer _newDialogue)
        => prevConditions.ForEach(x => x.OnFirstInteractionEnd += () => RemoveCondition(_dialogueObject, x, _newDialogue));
    
    // prevConditions 복제본으로 값 변경
    public void ConditionChangeAsClone(DialogueDataContainer[] _containers)
    {
        for (int i = 0; i < prevConditions.Count; i++)
        {
            if(FindContainerWithCodeName(_containers, prevConditions[i].CodeName) != null)
                prevConditions[i] = FindContainerWithCodeName(_containers, prevConditions[i].CodeName);
        }
    }

    DialogueDataContainer FindContainerWithCodeName(DialogueDataContainer[] _containers, string origianlCodeName)
    {
        for (int i = 0; i < _containers.Length; i++)
        {
            if (origianlCodeName == _containers[i].CodeName)
                return _containers[i];
        }

        Debug.LogError($"이벤트 조건 복사본을 찾지 못함 {origianlCodeName}");
        return null;
    }

    public bool IsClone => prevConditions.All(x => x.name.Contains("(Clone)"));
}