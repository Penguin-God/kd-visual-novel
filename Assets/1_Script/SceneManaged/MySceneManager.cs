using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

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

    [SerializeField] SceneManagerISo[] allSceneManagerISOs = null;
    public Dictionary<SceneManagerISo, SceneManagerISo> sceneManagerByOriginal = new Dictionary<SceneManagerISo, SceneManagerISo>();

    [Header("Channel")]
    [SerializeField] SceneChannel sceneChannel = null;
    [SerializeField] SplashManager splashManager = null;
    [SerializeField] SceneLoadDialogueProducer loadDialogueProducer = null;

    [Header("Scene Value")]
    [SerializeField] SceneManagerISo currentSceneManagerISO = null;
    [SerializeField] bool currentSceneIsOnlyView;
    public bool CurrentSceneIsOnlyView => currentSceneIsOnlyView;

    [SerializeField] GameObject[] currentSceneCharacters;
    public GameObject[] CurrentSceneCharacters => currentSceneCharacters;


    [SerializeField] List<GameObject> spawnDialogueObjects;
    public IReadOnlyList<GameObject> DynamicDialogueObjects => spawnDialogueObjects;

    [SerializeField] List<DialogueObject> allDialogueObjects;
    public List<DialogueObject> AllDialogueObjects => allDialogueObjects;


    [SerializeField] bool isSceneLoadingEffect; // 화면 연출하는 동안도 포함
    public bool IsSceneLoadingEffect => isSceneLoadingEffect;

    AsyncOperation async;
    public bool IsSceneLoading // 씬 로딩하는 동안 true
    {
        get
        {
            if (async == null) return false;
            return !async.isDone;
        }
    }

    public event Action OnSceneSetupDone = null;

    void Awake()
    {
        sceneChannel.OnOtherSceneLoad += LoadedScene;
        sceneChannel.OnEnterOtherScene += Setup;
        //sceneChannel.OnSceneLoadComplete += (_iso) => OnSceneSetupDone?.Invoke(currentSceneIsOnlyView, allDialogueObjects);

        for (int i = 0; i < allSceneManagerISOs.Length; i++)
        {
            SceneManagerISo _newManger = allSceneManagerISOs[i].GetClone();
            sceneManagerByOriginal.Add(allSceneManagerISOs[i], _newManger);
            allSceneManagerISOs[i] = _newManger;
        }
    }

    public void LoadedScene(SceneManagerISo _data, bool isFirst)
    {
        currentSceneManagerISO = sceneManagerByOriginal[_data];
        StartCoroutine(Co_LoadedScene(currentSceneManagerISO, isFirst));
    }

    void LoadedScene(SceneManagerISo _data)
    {
        currentSceneManagerISO = sceneManagerByOriginal[_data];
        StartCoroutine(Co_LoadedScene(currentSceneManagerISO));
    }
    IEnumerator Co_LoadedScene(SceneManagerISo _data, bool isFirst = false)
    {
        isSceneLoadingEffect = true;
        splashManager.FadeOut(FadeType.Black, true);
        yield return new WaitUntil(() => !splashManager.isFade);
        if(!isFirst) LoadScene(_data);
        yield return new WaitUntil(() => !IsSceneLoading && !CameraController.isCameraEffect);
        sceneChannel.Raise_OnEnterOtherScene(_data); // 씬 입장 성공
        splashManager.FadeIn(FadeType.Black, true);
        yield return new WaitUntil(() => !splashManager.isFade);
        isSceneLoadingEffect = false;

        sceneChannel.Raise_OnSceneLoadComplete(_data);
        OnSceneSetupDone?.Invoke();
        OnSceneSetupDone = null;

        loadDialogueProducer.ShowDialogue_When_SceneFadeIn();
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

        // spawnDialogueObjects 세팅
        spawnDialogueObjects.Clear();
        foreach(DialogueObject _dialogueObject in _data.GetSpawnDialogueObjects())
        {
            GameObject _obj = 
                Instantiate(_dialogueObject.SpawnData.characterContainer, _dialogueObject.SpawnData.spawnPos, Quaternion.Euler(_dialogueObject.SpawnData.spawnEulerAngles));

            // 렌더러 세팅
            SpriteRenderer[] _srs = _obj.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < _srs.Length; i++)
            {
                _srs[i].color = new Color(1, 1, 1, 0);
                _srs[i].sprite = _dialogueObject.SpawnData.spawnSprite;
            }

            InteractionObject _interaction = _obj.GetComponent<InteractionObject>();
            _interaction.Setup(_dialogueObject); // interactionObject Setup
            DialogueSystem.Instance.interactionObjectByCodeName.Add(_dialogueObject.CodeName, _interaction);
            spawnDialogueObjects.Add(_obj);
        }
    }

    public void SetClone(InteractionObject _interaction)
    {
        DialogueObject _dialogueObject = currentSceneManagerISO.DialogueObjects.FirstOrDefault(x => x.CodeName == _interaction.CodeName);
        if (_dialogueObject != null) _interaction.Setup(_dialogueObject);
    }
}