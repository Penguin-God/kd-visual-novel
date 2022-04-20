using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [Header("Dialogue Data")]
    [SerializeField] string codeName;
    [SerializeField] string interactionName;
    public string InteractionName => interactionName;

    [SerializeField] DialogueObject dialogueObject = null;

    [SerializeField] DialogueDataContainer currentDialogue;
    public DialogueDataContainer CurrentDialogue => currentDialogue;

    void Start()
    {
        if (dialogueObject != null && !dialogueObject.IsSpawn) DialogueSystem.Instance.OnSetup += (_dic) => _dic.Add(codeName, this);
    }

    public void Setup(DialogueObject _dialogueObject)
    {
        codeName = _dialogueObject.CodeName;
        interactionName = _dialogueObject.InteractionName;
        dialogueObject = _dialogueObject;
        currentDialogue = _dialogueObject.Dialogues[0];
    }

    [Header("Channel")]
    [SerializeField] protected DialogueChannel dialogueChannel = null;

    public void StartInteraction() => dialogueChannel.Raise_StartInteractionEvent(transform, currentDialogue);

    public bool Interactalbe => currentDialogue.Interactable;
    public void ChangeDialogue(DialogueDataContainer _newDialogue)
    {
        print(currentDialogue.CodeName);
        print(_newDialogue.CodeName);
        currentDialogue = _newDialogue;
    }
}