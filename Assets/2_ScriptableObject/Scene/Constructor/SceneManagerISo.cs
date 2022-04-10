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

    public List<GameObject> CreateInteractionObjects()
    {
        List<GameObject> _objs = new List<GameObject>();
        for (int i = 0; i < sceneData.DialougeObjects.Count; i++)
            _objs.Add(sceneData.DialougeObjects[i].GetInteractionObject());

        return _objs;
    }

    public event Action OnChangeList = null;
    public void AddInteractionObject(SpawnInteractionObjectData _data)
    {
        sceneData.DialougeObjects.Add(_data);
        OnChangeList?.Invoke();
    }


    [SerializeField] List<DialogueObject> dialogueObjects;
    public List<DialogueObject> DialogueObjects => dialogueObjects;

    public List<DialogueObject> GetDynamicDialogueObjects()
    {
        List<DialogueObject> _datas = new List<DialogueObject>();
        foreach (DialogueObject _dialogueObject in dialogueObjects)
        {
            if (_dialogueObject.IsSpawn)
                _datas.Add(_dialogueObject);
        }

        return _datas;
    }
}