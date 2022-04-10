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