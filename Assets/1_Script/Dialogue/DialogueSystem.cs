using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
class DialogueTargetData
{
    public InteractionEvent interaction;
    public DialogueDataContainer[] dialogues;

    public DialogueTargetData(InteractionEvent _interaction, DialogueDataContainer[] _dialogues)
    {
        interaction = _interaction;
        dialogues = _dialogues;
    }
}

[Serializable]
public class InteractionEventByName : SerializableDictionary<string, InteractionEvent>
{

}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] SceneChannel sceneChannel;
    [SerializeField] List<DialogueTargetData> dialogueTargetDatas;

    [SerializeField] List<DialogueObject> dialogueObjects;

    public InteractionEventByName interactionEventByName;
    public event Action<Dictionary<string, InteractionEvent>> OnSetup;

    private static DialogueSystem instance;
    public static DialogueSystem Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<DialogueSystem>();
            return instance;
        }
    }

    private void Awake()
    {
        MySceneManager.Instance.OnDoneSceneSetup += (_view, _dialogueObjs) =>
        {
            interactionEventByName.Clear();
            OnSetup?.Invoke(interactionEventByName);
            OnSetup = null;

            foreach (DialogueObject _obj in _dialogueObjs)
            {
                interactionEventByName.TryGetValue(_obj.CodeName, out InteractionEvent _interactionEvent);
                if(_interactionEvent != null) _obj.Setup(_interactionEvent);
            }
        };
    }
}