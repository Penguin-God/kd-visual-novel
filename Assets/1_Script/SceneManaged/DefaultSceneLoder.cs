using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class DefaultSceneLoder : MonoBehaviour
{
    //[SerializeField] SceneChannel sceneChannel= null;
    [SerializeField] SceneManagerISo sceneManagerISo = null;

    private void Awake()
    {
        DestroyChildObject();

        if (EditorSceneManager.sceneCount == 1) StartCoroutine(LoadDefaultScene());
    }

    IEnumerator LoadDefaultScene()
    {
        AsyncOperation _async = SceneManager.LoadSceneAsync("DefaultScene", LoadSceneMode.Additive);
        yield return new WaitUntil(() => _async.isDone);
        MySceneManager.Instance.LoadedScene(sceneManagerISo, true);
    }

    void DestroyChildObject()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
}