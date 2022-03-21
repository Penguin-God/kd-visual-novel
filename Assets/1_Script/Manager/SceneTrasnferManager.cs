using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrasnferManager : MonoBehaviour
{
    [SerializeField] string locationName;

    private SplashManager spManager;
    public static bool isTransfer;

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
        isTransfer = true;
        UIManager.instance.HideUI();
        spManager.FadeOut(FadeType.Black, true);
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
        spManager.FadeIn(FadeType.Black, true);
        yield return new WaitUntil(() => !spManager.isFade);
        if(!EventManager.isAutoEvent) UIManager.instance.ShowUI();
        isTransfer = false;
    }

    public string GetLocationName()
    {
        return locationName;
    }
}
