using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideManager : MonoBehaviour
{
    Animation anim;
    Image img_Slide;
    public bool isSildeMoving;

    private void Awake()
    {
        anim = GetComponentInChildren<Animation>();
        img_Slide = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        DialogueManager.instance.BeforeTalkEvent += SlideAnimation_byTalk;
    }

    void SlideAnimation_byTalk(Dialogue dialogue, int contextCount)
    {
        string cutName = dialogue.cutSceneName[contextCount].Trim();
        switch (dialogue.cameraType)
        {
            case CameraType.AppearSlide: StartCoroutine(Co_AppearSlide(cutName)); break;
            case CameraType.DisappearSlide: StartCoroutine(Co_DisappearSlide()); break;
            case CameraType.ChangeSlide: StartCoroutine(Co_ChangeSlide(cutName)); break;
        }
    }

    public IEnumerator Co_AppearSlide(string name)
    {
        isSildeMoving = true;
        DialogueManager.instance.isCameraEffect = true;
        Sprite _sprite = Resources.Load<Sprite>("Slide_Image/" + name);
        if (_sprite != null)
        {
            img_Slide.gameObject.SetActive(true);
            img_Slide.sprite = _sprite;
            anim.Play("Appear_MenuPad");
        }
        else Debug.LogWarning("찾을 수 없는 슬라이더 이름 : " + name);

        yield return new WaitForSeconds(0.5f);
        isSildeMoving = false;
        DialogueManager.instance.isCameraEffect = false;
    }

    public IEnumerator Co_DisappearSlide()
    {
        isSildeMoving = true;
        DialogueManager.instance.isCameraEffect = true;
        anim.Play("Disappear_MenuPad");

        yield return new WaitForSeconds(0.5f);
        img_Slide.sprite = null;
        isSildeMoving = false;
        DialogueManager.instance.isCameraEffect = false;
    }

    public bool isSlideChange;
    public IEnumerator Co_ChangeSlide(string name)
    {
        DialogueManager.instance.isCameraEffect = true;
        isSlideChange = true;

        StartCoroutine(Co_DisappearSlide());
        yield return new WaitUntil(() => isSildeMoving);

        StartCoroutine(Co_AppearSlide(name));
        yield return new WaitUntil(() => isSildeMoving);

        isSlideChange = false;
        DialogueManager.instance.isCameraEffect = false;
    }
}
