using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionObject : MonoBehaviour
{
    [Header("Dialogue Data")]
    [SerializeField] string codeName;
    [SerializeField] string interactionName;
    public string InteractionName => interactionName;

    [SerializeField] DialogueObject dialogueObject = null;

    [SerializeField] DialogueDataContainer currentDialogue;
    public DialogueDataContainer CurrentDialogue => currentDialogue;
    // 아직 사용은 안하고 있고 언젠가는 쓸거 같은 유용한 이벤트라 만들어둠
    public event Action<InteractionObject, DialogueDataContainer, DialogueDataContainer> OnDialogueChanged = null;
    public void ChangeDialogue(DialogueDataContainer _newDialogue)
    {
        OnDialogueChanged?.Invoke(this, currentDialogue, _newDialogue);
        currentDialogue = _newDialogue;
    }

    void Start()
    {
        if (dialogueObject != null && !dialogueObject.IsSpawn)
        {
            Setup(dialogueObject);
            DialogueSystem.Instance.interactionObjectByCodeName.Add(codeName, this);
        }
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
}