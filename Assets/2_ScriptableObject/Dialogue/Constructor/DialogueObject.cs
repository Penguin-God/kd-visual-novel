using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    //public void Setup(InteractionObject _interactionObject)
    //{
    //    for (int i = 0; i < dialogues.Length; i++)
    //        dialogues[i].Setup(_interactionObject);
    //}
}
