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

    [Header("Chapter Data")]
    [SerializeField] SceneChapterData chapterData;
    public SceneChapterData ChapterData => chapterData;

    public Vector3 PlayerSpawnPos => chapterData.PlayerSpawnPos;
    public IReadOnlyList<DialogueObject> DialogueObjects => chapterData.DialogueObjects;
    public IReadOnlyList<DialogueObject> SpawnObjects => chapterData.SpawnObjects;

    public void Start()
    {
        chapterData.Start();
    }

    public SceneManagerISo GetClone()
    {
        SceneManagerISo result = Instantiate(this);
        result.chapterData = result.chapterData.GetClone();
        return result;
    }
}