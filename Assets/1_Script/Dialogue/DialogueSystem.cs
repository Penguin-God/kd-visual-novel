using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InteractionEventByName : SerializableDictionary<string, InteractionObject>
{

}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] SceneChannel sceneChannel;

    public InteractionEventByName interactionEventByName;
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
        MySceneManager.Instance.OnDoneSceneSetup += (_view, _dialogueObjs) =>
        {
            interactionEventByName.Clear();
            OnSetup?.Invoke(interactionEventByName);
            OnSetup = null;

            foreach (DialogueObject _obj in _dialogueObjs)
            {
                interactionEventByName.TryGetValue(_obj.CodeName, out InteractionObject interactionObject);
                if(interactionObject != null) _obj.Setup(interactionObject);
            }
        };
    }
}