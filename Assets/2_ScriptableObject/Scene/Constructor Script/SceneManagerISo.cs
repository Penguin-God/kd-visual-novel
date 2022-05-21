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

    [SerializeField] SceneLoadDialogueProducer producer = null;
    void ShowLoadDialogue()
    {
        if (producer != null)
        {
            producer.ShowDialogue_When_SceneFadeIn();
            producer = null;
        }
    }

    public void Start()
    {
        ShowLoadDialogue();
        
    }

    public SceneManagerISo GetClone() => new Cloning().GetClone(this);
    class Cloning
    {
        public SceneManagerISo GetClone(SceneManagerISo original)
        {
            SceneManagerISo result = Instantiate(original);
            if(result.producer != null) result.producer = Instantiate(original.producer);

            result.dialogueObjects = GetDialogueObjectsClone(result);
            DialogueDataContainer[] containers = GetAllDialogueInObjects(result.dialogueObjects.ToArray());
            result.dialogueObjects.ForEach(x => x.Setup(containers));

            Debug.Assert(result.Equals(original) == false, $"SceneManagerISo의 복제본을 만들 때 얕은 복사가 진행되어 원본에 영향을 줄 수 있음 : {result.name}");
            return result;
        }

        List<DialogueObject> GetDialogueObjectsClone(SceneManagerISo _newManager) 
            => _newManager.dialogueObjects.Select(x => x.GetClone()).ToList();

        void AllObjectContainerSetup(SceneManagerISo _newManager)
        {
            DialogueDataContainer[] containers = GetAllDialogueInObjects(_newManager.dialogueObjects.ToArray());
            foreach (var _dialogueObject in _newManager.dialogueObjects)
            {
                SetContainerCondition(_dialogueObject.Dialogues, containers);
                ObjectContainerSetup(_dialogueObject.Dialogues, _dialogueObject);
            }
        }
        DialogueDataContainer[] SetContainerCondition(DialogueDataContainer[] dialogueObjectContainers, DialogueDataContainer[] allContainers)
        {
            return dialogueObjectContainers.Select(x => { x.SetClone(allContainers); return x; }).ToArray();
        }
        void ObjectContainerSetup(DialogueDataContainer[] _containers, DialogueObject _dialogueObject)
        {
            _containers.Select(x => x.Setup(_dialogueObject)).ToArray();
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