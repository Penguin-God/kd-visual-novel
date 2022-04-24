using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionObject : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] protected DialogueChannel dialogueChannel = null;
    [SerializeField] SceneChannel sceneChannel;

    [Header("Dialogue Data")]
    [SerializeField] string codeName;
    public string CodeName => codeName;
    [SerializeField] string interactionName;
    public string InteractionName => interactionName;
    
    [SerializeField] DialogueObject dialogueObject = null;

    [SerializeField] DialogueDataContainer currentDialogue;
    public DialogueDataContainer CurrentDialogue => currentDialogue;

    void Start()
    {
        if (dialogueObject != null && !dialogueObject.IsSpawn)
            sceneChannel.OnSceneLoadComplete += Setup;
    }

    private void OnDestroy()
    {
        if (dialogueObject != null && !dialogueObject.IsSpawn)
            sceneChannel.OnSceneLoadComplete -= Setup;
    }

    void Setup(SceneManagerISo _data)
    {
        codeName = dialogueObject.CodeName;
        Setup(DialogueSystem.Instance.GetDialogueObjectClone(codeName));
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

    public void StartInteraction() => dialogueChannel.Raise_StartInteractionEvent(transform, currentDialogue);

    public bool Interactalbe => currentDialogue.Interactable;

    void ChangeDialogue(DialogueObject _dialogueObject, DialogueDataContainer _newDialogue, DialogueDataContainer _prevDialogue)
    {
        currentDialogue = _newDialogue;
    }
}