using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogue : InteractionEvent
{
    SpriteFadeManager spriteFadeManager;
    SpriteRenderer[] renderers = null;
    private void Awake()
    {
        if (GetComponent<CharacterSpin>() == null) gameObject.AddComponent<CharacterSpin>();
        spriteFadeManager = gameObject.AddComponent<SpriteFadeManager>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    //private void OnEnable()
    //{
    //    dialogueChannel.StartTalkEvent += (_con) => FadeOut();
    //    dialogueChannel.EndTalkEvent += FadeIn;
    //}

    //void OnDisable()
    //{
    //    dialogueChannel.StartTalkEvent -= (_con) => FadeOut();
    //    dialogueChannel.EndTalkEvent -= FadeIn;
    //}

    public override void StartInteraction()
    {
        base.StartInteraction();
        //DialogueManager.instance.SetEvent(transform);
    }

    void FadeOut() => spriteFadeManager.SpriteFadeOut(renderers);
    void FadeIn() => spriteFadeManager.SpriteFadeIn(renderers);
}