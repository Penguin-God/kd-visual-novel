using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MySceneManager : MonoBehaviour
{
    private static MySceneManager instance;
    public static MySceneManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<MySceneManager>();
            return instance;
        }
    }

    [Header("Channel")]
    [SerializeField] SceneChannel sceneChannel = null;
    [SerializeField] SplashManager splashManager = null;
    [SerializeField] SceneLoadDialogueProducer loadDialogueProducer = null;

    [Header("Scene Value")]
    [SerializeField] bool currentSceneIsOnlyView;
    public bool CurrentSceneIsOnlyView => currentSceneIsOnlyView;

    [SerializeField] GameObject[] currentSceneCharacters;
    public GameObject[] CurrentSceneCharacters => currentSceneCharacters;


    [SerializeField] List<GameObject> dynamicDialogueObjects;
    public IReadOnlyList<GameObject> DynamicDialogueObjects => dynamicDialogueObjects;

    [SerializeField] List<DialogueObject> allDialogueObjects;
    public List<DialogueObject> AllDialogueObjects => allDialogueObjects;


    [SerializeField] bool isSceneLoadingEffect; // 화면 연출하는 동안도 포함
    public bool IsSceneLoadingEffect => isSceneLoadingEffect;

    
    AsyncOperation async;
    public bool IsSceneLoading // 씬 로딩하는 시간만 포함
    {
        get
        {
            if (async == null) return false;
            return !async.isDone;
        }
    }


    void Awake()
    {
        sceneChannel.OnOtherSceneLoad += LoadedScene;
        sceneChannel.OnEnterOtherScene += Setup;
        sceneChannel.OnSceneLoadComplete += (_iso) => OnDoneSceneSetup?.Invoke(currentSceneIsOnlyView, allDialogueObjects);
    }

    public event Action<bool, List<DialogueObject>> OnDoneSceneSetup = null;

    public void LoadedScene(SceneManagerISo _data) => StartCoroutine(Co_LoadedScene(_data));
    IEnumerator Co_LoadedScene(SceneManagerISo _data)
    {
        isSceneLoadingEffect = true;
        splashManager.FadeOut(FadeType.Black, true);
        yield return new WaitUntil(() => !splashManager.isFade);
        LoadScene(_data);
        yield return new WaitUntil(() => !IsSceneLoading && !CameraController.isCameraEffect);
        sceneChannel.Raise_OnEnterOtherScene(_data);
        splashManager.FadeIn(FadeType.Black, true);
        yield return new WaitUntil(() => !splashManager.isFade);
        isSceneLoadingEffect = false;
        sceneChannel.Raise_OnSceneLoadComplete(_data);
        loadDialogueProducer.Raise_OnLoadDialogue();
    }

    void LoadScene(SceneManagerISo _data)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        async = SceneManager.LoadSceneAsync(_data.SceneName, LoadSceneMode.Additive);
    }
    
    void Setup(SceneManagerISo _data)
    {
        currentSceneIsOnlyView = _data.IsOnlyCameraView;

        allDialogueObjects = _data.DialogueObjects;
        dynamicDialogueObjects.Clear();

        List<DialogueObject> _dialogueObjects = _data.GetDynamicDialogueObjects();
        foreach(DialogueObject _dialogueObject in _dialogueObjects)
        {
            GameObject _obj = 
                Instantiate(_dialogueObject.SpawnData.characterContainer, _dialogueObject.SpawnData.spawnPos, Quaternion.Euler(_dialogueObject.SpawnData.spawnEulerAngles));

            SpriteRenderer[] _srs = _obj.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < _srs.Length; i++)
            {
                _srs[i].color = new Color(1, 1, 1, 0);
                _srs[i].sprite = _dialogueObject.SpawnData.spawnSprite;
            }

            _obj.GetComponent<InteractionEvent>().Setup(_dialogueObject.CodeName, _dialogueObject.InteractionName, _dialogueObject);
            dynamicDialogueObjects.Add(_obj);
        }
    }
}