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

    [Header("Value")]
    [SerializeField] bool currentSceneIsOnlyView;
    public bool CurrentSceneIsOnlyView => currentSceneIsOnlyView;

    [SerializeField] GameObject[] currentSceneCharacters;
    public GameObject[] CurrentSceneCharacters => currentSceneCharacters;

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
        sceneChannel.OnSceneLoadComplete += (_iso) => OnDoneSceneSetup?.Invoke(currentSceneIsOnlyView, currentSceneCharacters);
    }

    public event Action<bool, GameObject[]> OnDoneSceneSetup = null;

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
    }

    void LoadScene(SceneManagerISo _data)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        async = SceneManager.LoadSceneAsync(_data.SceneName, LoadSceneMode.Additive);
    }

    public void Setup(SceneManagerISo _data)
    {
        currentSceneIsOnlyView = _data.IsOnlyCameraView;
        currentSceneCharacters = _data.CreateInteractionObjects().ToArray();
    }
}