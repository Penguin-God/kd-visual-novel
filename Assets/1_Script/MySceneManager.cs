using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MySceneManager : MonoBehaviour
{
    [SerializeField] SceneChannel sceneChannel = null;
    [SerializeField] SceneManagerISo sceneManagerISo = null;
    SplashManager splashManager = null;

    void Awake()
    {
        DestroyChildObject();
        StartCoroutine(LoadDefaultScene());
    }

    // 개발 편의상 씬에 남겨둔 기타 오브젝트들(카메라, 라이팅) 삭제
    void DestroyChildObject()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
    IEnumerator LoadDefaultScene()
    {
        if (!sceneChannel.isLoadDefualtScene)
        {
            AsyncOperation _async = SceneManager.LoadSceneAsync("DefaultScene", LoadSceneMode.Additive);
            yield return new WaitUntil(() => _async.isDone);
            sceneChannel.isLoadDefualtScene = true;
        }

        yield return new WaitUntil(() => !sceneChannel.IsSceneLoading && !CameraController.isCameraEffect);
        sceneManagerISo.Setup();
        sceneChannel.Raise_OnSceneLoadComplete();

        splashManager = FindObjectOfType<SplashManager>();
        Action _fadeInAct = sceneChannel.Raise_OnSceneFadeIn;
        _fadeInAct += sceneManagerISo.StartDialogueByLoad;

        splashManager.FadeIn(FadeType.Black, true, _fadeInAct);
    }
}