using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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


}
