using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] SplashManager splashManager;

    [SerializeField] Image cutSceneImage;

    // 현재 컷씬 진행중인지 확인하는 프로퍼티 변수
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
        else Debug.LogWarning("찾을 수 없는 컷씬 이름 : " + cutSceneName);

    }

    IEnumerator Co_CutScene(bool isShow)
    {
        DialogueManager.instance.isCameraEffect = true;
        splashManager.FadeOut(true);
        yield return new WaitUntil(() => !splashManager.isFade);

        cutSceneImage.gameObject.SetActive(isShow);

        splashManager.FadeIn(true);
        yield return new WaitUntil(() => !splashManager.isFade);

        yield return new WaitForSeconds(0.5f); // 연출을 위한 대기
        DialogueManager.instance.isCameraEffect = false;
    }
}
