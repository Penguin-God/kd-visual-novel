using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Load Dialogue Producer", menuName = "Scriptable Object / Scenes / Scene Load Dialogue Producer")]
public class SceneLoadDialogueProducer : ScriptableObject
{
    [SerializeField] SceneChannel sceneChannel = null;
    [SerializeField] DialogueChannel dialogueChannel = null;
    [SerializeField] DialogueDataContainer dialogue = null;
    void OnEnable() => sceneChannel.OnSceneLoadComplete += (_data) => ShowDialogue_When_SceneFadeIn();

    public void SetDialogue(DialogueDataContainer _newDialogue) => dialogue = _newDialogue;

    void ShowDialogue_When_SceneFadeIn()
    {
        if (dialogue == null) return;

        dialogueChannel.Raise_StartInteractionEvent(null, dialogue);
        dialogue = null;
    }
}
