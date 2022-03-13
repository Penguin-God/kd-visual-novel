using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogue : InteractionEvent
{
    private void Awake()
    {
        if (GetComponent<CharacterSpin>() == null) gameObject.AddComponent<CharacterSpin>();
    }

    public override void StartInteraction()
    {
        //base.StartInteraction(); // 현재 비었음
        DialogueManager.instance.SetEvent(transform);

        dialogueCannel.StartDialogueEvent(transform, container);
    }
}
