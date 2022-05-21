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
    public IReadOnlyList<DialogueObject> SpawnObjects => dialogueObjects.Where(x => x.IsSpawn).ToList();


    public SceneManagerISo GetClone() => new Cloning().GetClone(this);
    class Cloning
    {
        public SceneManagerISo GetClone(SceneManagerISo original)
        {
            SceneManagerISo result = Instantiate(original);
            result.dialogueObjects = GetDialogueObjectsClone(result);
            AllObjectContainerSetup(result);
            Debug.Assert(result.Equals(original) == false, $"SceneManagerISo의 복제본을 만들 때 얕은 복사가 진행되어 원본에 영향을 줄 수 있음 : {result.name}");
            return result;
        }

        List<DialogueObject> GetDialogueObjectsClone(SceneManagerISo _newManager) => _newManager.dialogueObjects.Select(x => x.GetClone()).ToList();

        void AllObjectContainerSetup(SceneManagerISo _newManager)
        {
            DialogueDataContainer[] containers = GetAllDialogueInObjects(_newManager.dialogueObjects.ToArray());
            foreach (var _dialogueObject in _newManager.dialogueObjects)
            {
                SetContainerCondition(_dialogueObject.Dialogues, containers);
                ObjectContainerSetup(_dialogueObject.Dialogues, _dialogueObject);
            }
        }
        void SetContainerCondition(DialogueDataContainer[] dialogueObjectContainers, DialogueDataContainer[] allContainers)
        {
            foreach (var _container in dialogueObjectContainers)
                _container.SetClone(allContainers);
        }
        void ObjectContainerSetup(DialogueDataContainer[] _containers, DialogueObject _dialogueObject)
        {
            foreach (var _container in _containers)
                _container.Setup(_dialogueObject);
        }

        DialogueDataContainer[] GetAllDialogueInObjects(DialogueObject[] _dialogueObjects)
        {
            List<DialogueDataContainer> result = new List<DialogueDataContainer>();

            foreach (var _dialogueObject in _dialogueObjects)
            {
                foreach (var _container in _dialogueObject.Dialogues)
                    result.Add(_container);
            }

            return result.ToArray();
        }
    }
}