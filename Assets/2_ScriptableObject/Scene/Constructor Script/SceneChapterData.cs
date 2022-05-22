using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Chapter Data", menuName = "Scriptable Object / Scenes / ChapterData")]
public class SceneChapterData : ScriptableObject
{
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

    public SceneChapterData GetClone() => new Cloning().GetClone(this);
    class Cloning
    {
        public SceneChapterData GetClone(SceneChapterData original)
        {
            SceneChapterData result = Instantiate(original);
            if (result.producer != null) result.producer = Instantiate(original.producer);

            result.dialogueObjects = GetDialogueObjectsClone(result);
            DialogueDataContainer[] containers = GetAllDialogueInObjects(result.dialogueObjects.ToArray());
            result.dialogueObjects.ForEach(x => x.Setup(containers));

            Debug.Assert(result.IsClone, $"SceneManagerISo의 복사가 안벽하지 않음: { result.name}");
            return result;
        }

        List<DialogueObject> GetDialogueObjectsClone(SceneChapterData _newManager)
            => _newManager.dialogueObjects.Select(x => x.GetClone()).ToList();

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

    bool IsClone => dialogueObjects.All(x => x.IsClone) && name.Contains("(Clone)");
}
