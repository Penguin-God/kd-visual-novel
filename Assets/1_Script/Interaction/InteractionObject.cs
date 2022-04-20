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
        if (!dialogueObject.IsSpawn)
        {
            DialogueSystem.Instance.OnSetup += (_dic) => _dic.Add(codeName, this);
            currentDialogue = dialogueObject.Dialogues[0];
        }
    }

    public void Setup(string _codeName, string _interactionName, DialogueObject _dialogueObject)
    {
        codeName = _codeName;
        interactionName = _interactionName;
        dialogueObject = _dialogueObject;
        currentDialogue = _dialogueObject.Dialogues[0];
        DialogueSystem.Instance.OnSetup += (_dic) => _dic.Add(codeName, this);
    }

    [Header("Channel")]
    [SerializeField] protected DialogueChannel dialogueChannel = null;

    public void StartInteraction() => dialogueChannel.Raise_StartInteractionEvent(transform, currentDialogue);

    public bool Interactalbe => currentDialogue.Interactable;
    public void ChangeDialogue(DialogueDataContainer _newDialogue) => currentDialogue = _newDialogue;
}