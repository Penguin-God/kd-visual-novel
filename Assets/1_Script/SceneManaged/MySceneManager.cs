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
    public IReadOnlyList<SceneManagerISo> AllSceneManagerISOs => allSceneManagerISOs;

    [SerializeField] SplashManager splashManager = null;

    [Header("Scene Value")]
    [SerializeField] SceneManagerISo currentSceneManagerISO = null;
    public bool CurrentSceneIsOnlyView => currentSceneManagerISO.IsOnlyCameraView;

    //public bool IsSceneLoadingEffect => splashManager.isFade;
    AsyncOperation async;
    public bool IsSceneLoading // 씬 로딩하는 동안 true
    {
        get
        {
            if (async == null) return false;
            return !async.isDone;
        }
    }

    #region Events
    [SerializeField] bool isSceneLoadEffect = true;
    public bool IsSceneLoadEffect => isSceneLoadEffect;

    public event Action<SceneManagerISo> OnOtherSceneLoad = null;
    public void Raise_OnOtherSceneLoad(SceneManagerISo _sceneData) // 유니티 이벤트 매서드로 등록해서 많이 사용함
    {
        isSceneLoadEffect = true;
        currentSceneManagerISO = GetSceneManagerISo(_sceneData);
        OnOtherSceneLoad?.Invoke(currentSceneManagerISO);
        LoadedScene();
    }

    public event Action<SceneManagerISo> OnEnterOtherScene = null;
    private void Raise_OnEnterOtherScene() => OnEnterOtherScene?.Invoke(currentSceneManagerISO);

    public event Action<SceneManagerISo> OnSceneLoadComplete = null;
    private void Raise_OnSceneLoadComplete()
    {
        isSceneLoadEffect = false; 
        OnSceneLoadComplete?.Invoke(currentSceneManagerISO);
    } 
    #endregion

    void Awake()
    {
        SetDataToClones();
    }

    void SetDataToClones() => allSceneManagerISOs = allSceneManagerISOs.Select(x => x.GetClone()).ToArray();

    SceneManagerISo GetSceneManagerISo(SceneManagerISo _sceneManagerISo) 
        => allSceneManagerISOs.FirstOrDefault(x => _sceneManagerISo.SceneName == x.SceneName);

    void LoadedScene()
    {
        Debug.Assert(currentSceneManagerISO.name.Contains("(Clone)"), $"클론 데이터가 아닌 SceneManagerISO 사용 중 : {currentSceneManagerISO.name}");
        StartCoroutine(Co_LoadedScene(currentSceneManagerISO));
    }

    public void LoadedScene(SceneManagerISo _data, bool isFirst)
    {
        currentSceneManagerISO = GetSceneManagerISo(_data);
        Debug.Assert(currentSceneManagerISO.name.Contains("(Clone)"), $"클론 데이터가 아닌 SceneManagerISO 사용 중 : {currentSceneManagerISO.name}");
        StartCoroutine(Co_LoadedScene(currentSceneManagerISO, isFirst));
    }

    IEnumerator Co_LoadedScene(SceneManagerISo _data, bool isFirst = false)
    {
        FadeOut();
        yield return new WaitUntil(() => !splashManager.isFade);

        LoadScene(_data, isFirst);
        yield return new WaitUntil(() => !IsSceneLoading && !CameraController.isCameraEffect);

        EnterScene();
        yield return new WaitUntil(() => !splashManager.isFade);

        SceneChangeEnd();
    }

    void FadeOut() => splashManager.FadeOut(FadeType.Black, true);
    void LoadScene(SceneManagerISo _data, bool isFirst)
    {
        if (!isFirst)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            async = SceneManager.LoadSceneAsync(_data.SceneName, LoadSceneMode.Additive);
        }
    }
    void EnterScene()
    {
        Raise_OnEnterOtherScene();
        splashManager.FadeIn(FadeType.Black, true);
    }
    void SceneChangeEnd()
    {
        Raise_OnSceneLoadComplete();
        currentSceneManagerISO.Start();
    }
}