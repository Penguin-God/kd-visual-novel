using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] SplashManager splashManager;

    [SerializeField] Image cutSceneImage;
    public bool isCutSceneEffect;
    // ���� �ƾ� ���������� Ȯ���ϴ� ������Ƽ ����
    public bool chectCutScene {get { return cutSceneImage.gameObject.activeSelf; } }


    public void CutScene(string cutSceneName, bool isFinish)
    {
        isCutSceneEffect = true;
        if (isFinish)
        {
            StartCoroutine(Co_CutScene(false));
            return;
        }

        Sprite _sprite = Resources.Load<Sprite>("CutScenes/" + cutSceneName);
        if (_sprite != null) cutSceneImage.sprite = _sprite;
        else Debug.LogWarning("ã�� �� ���� �ƾ� �̸� : " + cutSceneName);

        StartCoroutine(Co_CutScene(true));
    }

    IEnumerator Co_CutScene(bool isShow)
    {
        splashManager.FadeOut(true);
        yield return new WaitUntil(() => !splashManager.isFade);

        cutSceneImage.gameObject.SetActive(isShow);

        splashManager.FadeIn(true);
        yield return new WaitUntil(() => !splashManager.isFade);

        yield return new WaitForSeconds(0.5f); // ������ ���� ���
        isCutSceneEffect = false;
    }
}
