using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Scene Load Dialogue Producer", menuName = "Scriptable Object / Scenes / Scene Load Dialogue Producer")]
public class SceneLoadDialogueProducer : ScriptableObject
{
    [SerializeField] DialogueChannel dialogueChannel = null;
    [SerializeField] DialogueDataContainer dialogue = null;

    public void ShowDialogue_When_SceneFadeIn()
    {
        if (dialogue == null) return;
        
        dialogueChannel.Raise_StartInteractionEvent(null, dialogue);
    } 
}