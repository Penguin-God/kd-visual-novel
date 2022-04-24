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

    public bool IsSceneLoadingEffect => splashManager.isFade;
    AsyncOperation async;
    public bool IsSceneLoading // 씬 로딩하는 동안 true
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
        sceneChannel.OnEnterOtherScene += SetupScene;

        for (int i = 0; i < allSceneManagerISOs.Length; i++)
        {
            SceneManagerISo _newManger = allSceneManagerISOs[i].GetClone();
            sceneManagerByOriginal.Add(allSceneManagerISOs[i], _newManger);
            allSceneManagerISOs[i] = _newManger;
        }
    }


    void LoadedScene(SceneManagerISo _data)
    {
        currentSceneManagerISO = sceneManagerByOriginal[_data];
        StartCoroutine(Co_LoadedScene(currentSceneManagerISO));
    }

    public void LoadedScene(SceneManagerISo _data, bool isFirst)
    {
        currentSceneManagerISO = sceneManagerByOriginal[_data];
        StartCoroutine(Co_LoadedScene(currentSceneManagerISO, isFirst));
    }

    void SetupScene(SceneManagerISo _data)
    {
        currentSceneIsOnlyView = _data.IsOnlyCameraView;
    }

    IEnumerator Co_LoadedScene(SceneManagerISo _data, bool isFirst = false)
    {
        splashManager.FadeOut(FadeType.Black, true);
        yield return new WaitUntil(() => !splashManager.isFade);

        if(!isFirst) LoadScene(_data);
        yield return new WaitUntil(() => !IsSceneLoading && !CameraController.isCameraEffect);

        sceneChannel.Raise_OnEnterOtherScene(_data); // 씬 입장 성공
        splashManager.FadeIn(FadeType.Black, true);
        yield return new WaitUntil(() => !splashManager.isFade);

        sceneChannel.Raise_OnSceneLoadComplete(_data);
        loadDialogueProducer.ShowDialogue_When_SceneFadeIn();
    }

    void LoadScene(SceneManagerISo _data)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        async = SceneManager.LoadSceneAsync(_data.SceneName, LoadSceneMode.Additive);
    }
}