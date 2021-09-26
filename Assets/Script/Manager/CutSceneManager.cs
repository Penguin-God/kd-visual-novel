using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] SplashManager splashManager;

    [SerializeField] Image cutSceneImage;

    // ���� �ƾ� ���������� Ȯ���ϴ� ������Ƽ ����
    public bool CheckCutScene {get { return cutSceneImage.gameObject.activeSelf; } }

    private void Start()
    {
        DialogueManager.instance.BeforeTalkEvent += CutScene_byTalk;
    }

    void CutScene_byTalk(Dialogue dialogue, int contextCount)
    {
        string cutName = dialogue.cutSceneName[contextCount].Trim();
        switch (dialogue.cameraType)
        {
            case CameraType.ShowCutScene: CutScene(cutName, false); break;
            case CameraType.HideCutScene: CutScene("", true); break;
        }
    }

    public void CutScene(string cutSceneName, bool isFinish)
    {
        if (isFinish)
        {
            StartCoroutine(Co_CutScene(false));
            return;
        }

        Sprite _sprite = Resources.Load<Sprite>("CutScenes/" + cutSceneName);
        if (_sprite != null)
        {
            cutSceneImage.sprite = _sprite;
            StartCoroutine(Co_CutScene(true));
        }
        else Debug.LogWarning("ã�� �� ���� �ƾ� �̸� : " + cutSceneName);

    }

    IEnumerator Co_CutScene(bool isShow)
    {
        DialogueManager.instance.isCameraEffect = true;
        splashManager.FadeOut(true);
        yield return new WaitUntil(() => !splashManager.isFade);

        cutSceneImage.gameObject.SetActive(isShow);

        splashManager.FadeIn(true);
        yield return new WaitUntil(() => !splashManager.isFade);

        yield return new WaitForSeconds(0.5f); // ������ ���� ���
        DialogueManager.instance.isCameraEffect = false;
    }
}
