using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    void Awake()
    {
        Destroy(GetComponentInChildren<Camera>().gameObject);
        StartCoroutine(LoadDefaultScene());
    }

    IEnumerator LoadDefaultScene()
    {
        AsyncOperation _async = SceneManager.LoadSceneAsync("DefaultScene", LoadSceneMode.Additive);
        yield return new WaitUntil(() => _async.isDone);
    }
}
