using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable] // View Dictionary In Inspector
public class InteractionEventByName : SerializableDictionary<string, InteractionObject> {}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] SceneChannel sceneChannel;

    public InteractionEventByName interactionEventByCodeName;
    [SerializeField] List<DialogueDataContainer> allContainers = new List<DialogueDataContainer>();
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

            Dictionary<string, DialogueDataContainer[]> _containersByCodeName = new Dictionary<string, DialogueDataContainer[]>();

            InteractionObject interactionObject = null;
            foreach (DialogueObject _obj in _dialogueObjs)
            {
                List<DialogueDataContainer> containers = new List<DialogueDataContainer>();

                interactionEventByCodeName.TryGetValue(_obj.CodeName, out interactionObject);
                if(interactionObject != null)
                {
                    DialogueObject _newObj = _obj.GetClone();
                    interactionObject.Setup(_newObj);
                    foreach (var _container in _newObj.Dialogues)
                    {
                        allContainers.Add(_container);
                        containers.Add(_container);
                    }
                    _containersByCodeName.Add(_obj.CodeName, containers.ToArray());
                }
            }

            // container 값 복제값으로 변경
            foreach (var _pair in _containersByCodeName)
            {
                interactionEventByCodeName.TryGetValue(_pair.Key, out interactionObject);
                foreach (var _container in _pair.Value)
                {
                    //print(_container.SetClone == null);
                    _container.SetClone(allContainers);
                    _container.Setup(interactionObject);
                }
            }
        };
    }
}