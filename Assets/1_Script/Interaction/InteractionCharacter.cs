using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCharacter : InteractionObject
{
    private void Awake()
    {
        if (GetComponent<CharacterSpin>() == null) gameObject.AddComponent<CharacterSpin>();
    }

    public override void StartInteraction()
    {
        base.StartInteraction();
        //DialogueManager.instance.SetEvent(transform);
    }
}