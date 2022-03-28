using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new scene manager", menuName = "Scriptable Object / Scenes / Scene Manager")]
public class SceneManagerISo : ScriptableObject
{
    [SerializeField] bool isOnlyCameraView;
    public bool IsOnlyCameraView => isOnlyCameraView;
    [SerializeField] string sceneName;
    public string SceneName => sceneName;

    [SerializeField] SceneData sceneData = null;
    [SerializeField] SceneChannel sceneChannel = null;

    public void SetUp()
    {
        sceneChannel.SetSceneView(isOnlyCameraView);
        sceneChannel.SetSceneCharacters(CreateInteractionObjects().ToArray());
    }

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


    public void ChangeData(SceneData _newSceneData)
    {
        sceneData = _newSceneData;
    }
}
