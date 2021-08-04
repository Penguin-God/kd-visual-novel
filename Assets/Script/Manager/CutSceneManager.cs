using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] CameraController camController;
    [SerializeField] SplashManager splashManager;

    [SerializeField] Image cutSceneImage;
    public bool isCutScene;

    public void CutScene(string cutSceneName, bool isFinish)
    {
        isCutScene = true;
        if (isFinish)
        {
            StartCoroutine(Co_CutScene(false));
            return;
        }

        Sprite _sprite = Resources.Load<Sprite>("CutScenes/" + cutSceneName);
        if (_sprite != null) cutSceneImage.sprite = _sprite;
        else Debug.LogWarning("찾을 수 없는 컷씬 이름 : " + cutSceneName);

        StartCoroutine(Co_CutScene(true));
    }

    IEnumerator Co_CutScene(bool isShow)
    {
        splashManager.FadeOut(true);
        yield return new WaitUntil(() => !splashManager.isFade);

        cutSceneImage.gameObject.SetActive(isShow);

        splashManager.FadeIn(true);
        yield return new WaitUntil(() => !splashManager.isFade);

        yield return new WaitForSeconds(0.5f); // 연출을 위한 대기
        isCutScene = false;
    }
}
