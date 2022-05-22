using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionObject : Interaction
{
    //[Header("Channel")]
    //[SerializeField] protected DialogueChannel dialogueChannel = null;

    //[Header("Dialogue Data")]
    //[SerializeField] protected string codeName;
    //public string CodeName => codeName;

    //[SerializeField] string interactionName;
    //public string InteractionName => interactionName;
    
    [SerializeField] protected DialogueObject dialogueObject = null;

    //[SerializeField] DialogueDataContainer currentDialogue;
    //public DialogueDataContainer CurrentDialogue => currentDialogue;

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