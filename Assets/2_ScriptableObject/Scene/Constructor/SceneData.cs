using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new scene data", menuName = "Scriptable Object / Scenes / Scene Data")]
public class SceneData : ScriptableObject
{
    [SerializeField] List<SpawnInteractionObjectData> spawnObjects = new List<SpawnInteractionObjectData>();
    public List<SpawnInteractionObjectData> SpawnObjects => spawnObjects;
}
