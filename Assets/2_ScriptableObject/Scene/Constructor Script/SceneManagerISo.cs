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

    // 클론 생성
    #region Coloning

    public SceneManagerISo GetClone()
    {
        SceneManagerISo _newManager = Instantiate(this);
        _newManager.dialogueObjects = _newManager.dialogueObjects.Select(x => x.GetClone()).ToList();

        foreach (var _dialogueObject in _newManager.dialogueObjects)
        {
            SetContainerCondition(_dialogueObject.Dialogues, _newManager.dialogueObjects.ToArray());
            ContainerSetup(_dialogueObject.Dialogues, _dialogueObject);
        }

        return _newManager;
    }

    void SetContainerCondition(DialogueDataContainer[] _containers, DialogueObject[] _dialogueObjects)
    {
        foreach (var _container in _containers)
            _container.SetClone(GetAllDialogueInObjects(_dialogueObjects));
    }

    void ContainerSetup(DialogueDataContainer[] _containers, DialogueObject _dialogueObject)
    {
        foreach (var _container in _containers)
            _container.Setup(_dialogueObject);
    }

    [SerializeField] List<DialogueDataContainer> test;

    List<DialogueDataContainer> GetAllDialogueInObjects(DialogueObject[] _dialogueObjects)
    {
        List<DialogueDataContainer> result = new List<DialogueDataContainer>();

        foreach (var _dialogueObject in _dialogueObjects)
        {
            foreach (var _container in _dialogueObject.Dialogues)
                result.Add(_container);
        }
        test = result;
        return result;
    }
    #endregion
}