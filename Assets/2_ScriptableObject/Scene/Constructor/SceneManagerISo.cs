using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public List<DialogueObject> GetSpawnDialogueObjects()
    {
        List<DialogueObject> _datas = new List<DialogueObject>();
        foreach (DialogueObject _dialogueObject in dialogueObjects)
        {
            if (_dialogueObject.IsSpawn)
                _datas.Add(_dialogueObject);
        }

        return _datas;
    }

    [SerializeField] List<DialogueDataContainer> allDialogue;

    List<DialogueDataContainer> AddDialogue(List<DialogueObject> _dialogueObjects)
    {

        foreach (var _dialogueObject in _dialogueObjects)
        {
            foreach (var _container in _dialogueObject.Dialogues)
                allDialogue.Add(_container);
        }

        return allDialogue;
    }

    public SceneManagerISo GetClone()
    {
        SceneManagerISo _newManager = Instantiate(this);
        _newManager.dialogueObjects = _newManager.dialogueObjects.Select(x => x.GetClone()).ToList();

        List<DialogueDataContainer> _allContainers = _newManager.AddDialogue(_newManager.dialogueObjects);


        foreach (var _dialogueObject in _newManager.dialogueObjects)
        {
            foreach (var _container in _dialogueObject.Dialogues)
            {
                _container.SetClone(_allContainers);
                _container.Setup(_dialogueObject);
            }
        }

        return _newManager;
    }
}