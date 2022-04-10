using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class DefaultSceneLoder : MonoBehaviour
{
    [SerializeField] SceneChannel sceneChannel= null;
    [SerializeField] SceneManagerISo sceneManagerISo = null;

    private void Awake()
    {
        DestroyChildObject();

        //print(EditorSceneManager.sceneCount);
        if (EditorSceneManager.sceneCount == 1) StartCoroutine(LoadDefaultScene());
        //print(EditorSceneManager.sceneCount);
    }

    IEnumerator LoadDefaultScene()
    {
        AsyncOperation _async = SceneManager.LoadSceneAsync("DefaultScene", LoadSceneMode.Additive);
        yield return new WaitUntil(() => _async.isDone);
        sceneChannel.Raise_OnEnterOtherScene(sceneManagerISo);
        sceneChannel.Raise_OnSceneLoadComplete(sceneManagerISo);
    }

    void DestroyChildObject()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
}