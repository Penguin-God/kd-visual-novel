using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrasnferManager : MonoBehaviour
{
    [SerializeField] string locationName;

    private SplashManager spManager;

    private void Start()
    {
        spManager = FindObjectOfType<SplashManager>();
    }

    public void SceneTransfer(string sceneName, string p_locationName)
    {
        StartCoroutine(Co_SceneTransfer(sceneName, p_locationName));
    }

    IEnumerator Co_SceneTransfer(string sceneName, string p_locationName)
    {
        UIManager.instance.HideUI();
        spManager.FadeOut(false, true);
        yield return new WaitUntil(() => !spManager.isFade);
        
        locationName = p_locationName;
        ScenePositonManger.spawn_able = true;
        SceneManager.LoadScene(sceneName);
    }

    public void SceneChangeDone()
    {
        StartCoroutine(Co_SceneChangeDone());
    }

    IEnumerator Co_SceneChangeDone()
    {
        spManager.FadeIn(false, true);
        yield return new WaitUntil(() => !spManager.isFade);
        UIManager.instance.ShowUI();
    }

    public string GetLocationName()
    {
        return locationName;
    }
}
