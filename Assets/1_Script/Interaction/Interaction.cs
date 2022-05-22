using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] protected DialogueChannel dialogueChannel = null;

    [Header("Dialogue Data")]
    [SerializeField] protected string codeName;
    public string CodeName => codeName;

    [SerializeField] protected string interactionName;
    public string InteractionName => interactionName;

    public void StartInteraction() => dialogueChannel.Raise_StartInteractionEvent(transform, currentDialogue);

    public bool Interactalbe => currentDialogue.Interactable;

    [SerializeField] protected DialogueDataContainer currentDialogue;
    public DialogueDataContainer CurrentDialogue => currentDialogue;
}
