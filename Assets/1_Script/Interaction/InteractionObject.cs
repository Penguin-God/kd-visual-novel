using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionObject : Interaction
{
    [SerializeField] protected DialogueObject dialogueObject = null;

    protected virtual void Init() { }
    protected virtual void Clear() { }

    void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Clear();
    }

    public void Setup(DialogueObject _dialogueObject)
    {
        codeName = _dialogueObject.CodeName;
        interactionName = _dialogueObject.InteractionName;
        dialogueObject = _dialogueObject;
        currentDialogue = _dialogueObject.CurrentDialogue;
        _dialogueObject.OnDialogueChanged += ChangeDialogue;
        DialogueSystem.Instance.interactionObjectByCodeName.Add(CodeName, this);
    }

    void ChangeDialogue(DialogueObject _dialogueObject, DialogueDataContainer _newDialogue, DialogueDataContainer _prevDialogue)
    {
        currentDialogue = _newDialogue;
    }
}