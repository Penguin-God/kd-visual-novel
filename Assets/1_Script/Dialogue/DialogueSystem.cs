using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] // View Dictionary In Inspector
public class InteractionEventByName : SerializableDictionary<string, InteractionObject> {}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] SceneChannel sceneChannel;

    public InteractionEventByName interactionEventByCodeName;
    public event Action<Dictionary<string, InteractionObject>> OnSetup;

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
        MySceneManager.Instance.OnSceneSetupDone += (_view, _dialogueObjs) =>
        {
            interactionEventByCodeName.Clear();
            OnSetup?.Invoke(interactionEventByCodeName);
            OnSetup = null;

            foreach (DialogueObject _obj in _dialogueObjs)
            {
                interactionEventByCodeName.TryGetValue(_obj.CodeName, out InteractionObject interactionObject);
                if(interactionObject != null) _obj.Setup(interactionObject);
            }
        };
    }
}