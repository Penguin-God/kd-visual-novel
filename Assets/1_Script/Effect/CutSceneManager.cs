using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] DialogueChannel dialogueChannel = null;
    [SerializeField] SplashManager splashManager;
    [SerializeField] Image cutSceneImage;

    // 현재 컷씬 진행중인지 확인하는 프로퍼티 변수
    public bool CheckCutScene {get { return cutSceneImage.gameObject.activeSelf; } }

    private void Start()
    {
        dialogueChannel.ChangeContextEvent += CutScene_byTalk;
    }

    void CutScene_byTalk(DialogueData dialogue, int contextCount)
    {
        string cutName = dialogue.cutSceneName[contextCount].Trim();
        if(cutName != "" && cutName != "Hide") CutScene(cutName, false);
        else if(cutName == "Hide") CutScene("", true);
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
    }

    IEnumerator Co_CutScene(bool isShow)
    {
        splashManager.FadeOut(FadeType.White);
        yield return new WaitUntil(() => !splashManager.isFade);

        cutSceneImage.gameObject.SetActive(isShow);

        splashManager.FadeIn(FadeType.White);
        yield return new WaitUntil(() => !splashManager.isFade);

        yield return new WaitForSeconds(0.5f); // 연출을 위한 대기
    }
}
