using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    [SerializeField] SceneChannel sceneChannel = null;
    [SerializeField] bool isOnlyViewScene;

    void Awake()
    {
        sceneChannel.SetSceneView(isOnlyViewScene);
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
        AsyncOperation _async = SceneManager.LoadSceneAsync("DefaultScene", LoadSceneMode.Additive);
        yield return new WaitUntil(() => _async.isDone);
        DestroyChildObject();
    }
}
