using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new scene manager", menuName = "Scriptable Object / Scenes / Scene Manager")]
public class SceneManagerISo : ScriptableObject
{
    [Header("Constant Value")]
    [SerializeField] string sceneName;
    public string SceneName => sceneName;

    [SerializeField] bool isOnlyCameraView;
    public bool IsOnlyCameraView => isOnlyCameraView;


    [Header("Variable Value")]
    [SerializeField] Vector3 playerSpawnPos;
    public Vector3 PlayerSpawnPos => playerSpawnPos;

    [SerializeField] SceneData sceneData = null;
    public void ChangeData(SceneData _newSceneData) => sceneData = _newSceneData;

    [SerializeField] List<DialogueGroup> dialogueGroups;
    public List<DialogueGroup> DialogueGroups => dialogueGroups;

    public List<GameObject> CreateInteractionObjects()
    {
        List<GameObject> _objs = new List<GameObject>();
        for (int i = 0; i < sceneData.SpawnObjects.Count; i++)
            _objs.Add(sceneData.SpawnObjects[i].GetInteractionObject());

        return _objs;
    }

    public event Action OnChangeList = null;
    public void AddInteractionObject(SpawnInteractionObjectData _data)
    {
        sceneData.SpawnObjects.Add(_data);
        OnChangeList?.Invoke();
    }
}