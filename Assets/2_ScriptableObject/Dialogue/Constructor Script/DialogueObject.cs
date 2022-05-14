using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class SpawnData
{
    public Vector3 spawnPos;
    public Vector3 spawnEulerAngles;
    public GameObject characterContainer = null;
    public Sprite spawnSprite = null;
}

[CreateAssetMenu(fileName = "new Dialogue Object", menuName = "Scriptable Object / Dialogue / Dialogue Object")]
public class DialogueObject : ScriptableObject
{
    [Header("Text")]
    [SerializeField] string interactionName;
    public string InteractionName => interactionName;

    [SerializeField] string codeName;
    public string CodeName => codeName;

    [SerializeField] DialogueDataContainer[] dialogues;
    public DialogueDataContainer[] Dialogues => dialogues;

    [SerializeField] int currentDialogueIndex = 0;
    public int CurrentDialogueIndex => currentDialogueIndex;
    public DialogueDataContainer CurrentDialogue => dialogues[currentDialogueIndex];

    public event Action<DialogueObject, DialogueDataContainer, DialogueDataContainer> OnDialogueChanged = null;
    public void ChangeDialogue(DialogueDataContainer _newDialogue)
    {
        OnDialogueChanged?.Invoke(this, _newDialogue, CurrentDialogue);
        currentDialogueIndex = Array.IndexOf(dialogues, _newDialogue);
    }

    [Header("Spawn Data")]
    [SerializeField] bool isSpawn;
    public bool IsSpawn => isSpawn;

    [SerializeField] SpawnData spawnData;
    public SpawnData SpawnData => spawnData;

    public DialogueObject GetClone()
    {
        var _newDialogueObject = Instantiate(this);
        _newDialogueObject.dialogues = dialogues.Select(x => x.GetClone()).ToArray();
        return _newDialogueObject;
    }

    public GameObject GetGameObject()
    {
        GameObject _obj = SpawnData.characterContainer;
        _obj.transform.position = SpawnData.spawnPos;
        _obj.transform.rotation = Quaternion.Euler(SpawnData.spawnEulerAngles);

        // 렌더러 세팅
        SpriteRenderer[] _srs = _obj.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < _srs.Length; i++)
        {
            _srs[i].color = new Color(1, 1, 1, 0);
            _srs[i].sprite = SpawnData.spawnSprite;
        }

        return _obj;
    }

    public DialogueSaveData GetSaveData()
    {
        return new DialogueSaveData
        {
            codeName = this.codeName,
            currentDialogueIndex = this.currentDialogueIndex,
            prevConditions = null, // 후에 구현해야됨
        };
    }

    public void LoadData(DialogueSaveData _data)
    {
        codeName = _data.codeName;
        currentDialogueIndex = _data.currentDialogueIndex;
    }
}