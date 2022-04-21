using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] // View Dictionary In Inspector
public class InteractionEventByName : SerializableDictionary<string, InteractionObject> {}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] SceneChannel sceneChannel;

    public InteractionEventByName interactionObjectByCodeName; // dialogueObject의 코드네임은 interactionObject의 코드네임과 같음
    [SerializeField] List<DialogueDataContainer> allContainers = new List<DialogueDataContainer>();

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
        sceneChannel.OnOtherSceneLoad += (_so) => interactionObjectByCodeName.Clear();

        MySceneManager.Instance.OnSceneSetupDone += (_view, _dialogueObjs) =>
        {
            SetAllContainers(_dialogueObjs); // allContainers 세팅

            // container 값 복제값으로 변경
            foreach (var _dialogueObject in _dialogueObjs)
            {
                interactionObjectByCodeName.TryGetValue(_dialogueObject.CodeName, out InteractionObject interactionObject);

                foreach (var _container in _dialogueObject.Dialogues)
                {
                    _container.SetClone(allContainers);
                    _container.Setup(_dialogueObject);
                }
            }
        };
    }

    void SetAllContainers(List<DialogueObject> _dialogueObjs)
    {
        foreach (var _dialogueObject in _dialogueObjs)
        {
            foreach (var _container in _dialogueObject.Dialogues)
                allContainers.Add(_container);
        }
    }
}