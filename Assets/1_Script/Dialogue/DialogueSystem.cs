using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] SceneChannel sceneChannel;
    [SerializeField] DialogueGroup[] dialogueGroups;

    private void Awake()
    {
        sceneChannel.OnSceneLoadComplete += (_data) =>
        { 
            dialogueGroups = _data.DialogueGroups.ToArray(); 
            foreach (var _group in dialogueGroups) _group.Setup(); 
        };
    }
}