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

    [SerializeField] DialogueDataContainer dialogueByLoad;
    public DialogueDataContainer DialogueByLoad => dialogueByLoad;
    public void ChangeDialougeByLoad(DialogueDataContainer _newData) => dialogueByLoad = _newData;

    [SerializeField] SceneData sceneData = null;
    public void ChangeData(SceneData _newSceneData) => sceneData = _newSceneData;


    [Header("Channel")]
    [SerializeField] SceneChannel sceneChannel = null;
    [SerializeField] DialogueChannel dialogueChannel = null;

    private void OnEnable()
    {
        //sceneChannel.OnSceneLoadComplete += Setup;
        //sceneChannel.OnSceneFadeIn += StartDialogueByLoad;
    }

    public void Setup()
    {
        sceneChannel.SetSceneView(isOnlyCameraView);
        // 소환 겸 세팅
        sceneChannel.SetSceneCharacters(CreateInteractionObjects().ToArray());
    }

    public void StartDialogueByLoad()
    {
        if (dialogueByLoad != null) 
            dialogueChannel.Raise_StartInteractionEvent(null, dialogueByLoad);
    }

    List<GameObject> CreateInteractionObjects()
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