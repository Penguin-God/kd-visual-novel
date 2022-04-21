using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
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

    int currentDialogueIndex = 0;
    public DialogueDataContainer CurrentDialogue => dialogues[currentDialogueIndex];

    public event Action<DialogueObject, DialogueDataContainer, DialogueDataContainer> OnDialogueChanged = null;
    public void ChangeDialogue(DialogueDataContainer _newDialogue)
    {
        OnDialogueChanged?.Invoke(this, CurrentDialogue, _newDialogue);
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
        //_newDialogueObject.Setup();
        return _newDialogueObject;
    }

    void Setup()
    {
        for (int i = 0; i < dialogues.Length; i++)
            dialogues[i].Setup(this);
    }
}
