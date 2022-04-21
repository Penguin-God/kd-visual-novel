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

    List<DialogueDataContainer> GetAllDialogue(List<DialogueObject> _dialogueObjects)
    {
        List<DialogueDataContainer> _allDialogue = new List<DialogueDataContainer>();
        foreach (var _dialogueObject in _dialogueObjects)
        {
            foreach (var _container in _dialogueObject.Dialogues)
                _allDialogue.Add(_container);
        }

        allDialogue = _allDialogue;
        return _allDialogue;
    }

    public SceneManagerISo GetClone()
    {
        SceneManagerISo _newManager = Instantiate(this);
        _newManager.dialogueObjects = dialogueObjects.Select(x => x.GetClone()).ToList();

        //List<DialogueDataContainer> _containers = GetAllDialogue(_newManager.dialogueObjects);

        //for (int i = 0; i < _containers.Count; i++)
        //    _containers[i].SetClone(_containers);

        return _newManager;
    }
}