using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable] // View Dictionary In Inspector
public class InteractionEventByName : SerializableDictionary<string, InteractionObject> {}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] SceneChannel sceneChannel = null;

    // 그냥 debug용으로 남겨둠
    public InteractionEventByName interactionObjectByCodeName; // dialogueObject의 코드네임은 interactionObject의 코드네임과 같음

    private static DialogueSystem instance;
    public static DialogueSystem Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<DialogueSystem>();
            return instance;
        }
    }

    [Header("Dialogue Objects")]
    [SerializeField] List<GameObject> spawnDialogueObjects;
    public IReadOnlyList<GameObject> DynamicDialogueObjects => spawnDialogueObjects;

    [SerializeField] List<DialogueObject> allDialogueObjects;
    public List<DialogueObject> AllDialogueObjects => allDialogueObjects;

    public DialogueObject GetDialogueObjectClone(string _codeName) => allDialogueObjects.FirstOrDefault(x => x.CodeName == _codeName);

    private void Awake()
    {
        sceneChannel.OnEnterOtherScene += SetupSpawnDialgoueObject;
    }

    // spawnDialogueObjects 세팅
    void SetupSpawnDialgoueObject(SceneManagerISo _data)
    {
        allDialogueObjects = _data.DialogueObjects;

        spawnDialogueObjects.Clear();
        List<DialogueObject> dialogueObjects = _data.GetSpawnDialogueObjects();
        foreach (DialogueObject _dialogueObject in dialogueObjects)
        {
            // new GameObject형식으로 Instantiate해야 Scene오브젝트로 생성됨
            GameObject _obj = Instantiate(_dialogueObject.GetGameObject());
            spawnDialogueObjects.Add(_obj);

            // asset 오브젝트 내의 InteractionObject가 아니라 Scene에 있는 오브젝트의 InteractionObject로 진행해야 제대로 진행됨
            InteractionObject _interaction = _obj.GetComponent<InteractionObject>();
            _interaction.Setup(_dialogueObject); // InteractionObject 세팅
        }
    }
}