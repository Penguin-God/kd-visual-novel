using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeType
{
    White,
    Black,
}

public class SplashManager : MonoBehaviour
{
    [SerializeField] DialogueCannel dialogueCannel = null;
    [SerializeField] Image fadeImage;

    [SerializeField] Color whiteColor;
    [SerializeField] Color blackColor;

    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeSlowSpeed;

    public bool isFade = false;

    WaitForSeconds ws = new WaitForSeconds(0.05f);

    private void Start()
    {
        dialogueCannel.ChangeContextEvent += FadeCamara_byTalk;

        SoundManager.instance.EffectSoundEvent += Splash;
    }

    void FadeCamara_byTalk(DialogueData _data, int _count)
    {
        string _effectType = _data.fadeType[_count];

        switch (_effectType)
        {
            case "FadeOut : Balck": FadeOut(FadeType.Black); break;
            case "FadeIn : Balck": FadeIn(FadeType.Black); break;
            case "FadeOut : White": FadeOut(FadeType.White); break;
            case "FadeIn : White": FadeIn(FadeType.White); break;
            default: break;
        }
    }

    public void Splash()
    {
        StartCoroutine(Co_Splash());
    }

    IEnumerator Co_Splash()
    {
        DialogueManager.instance.isCameraEffect = true;
        isFade = true;
        FadeOut(FadeType.White);
        yield return new WaitUntil(() => !isFade);
        FadeIn(FadeType.White);
        DialogueManager.instance.isCameraEffect = false;
    }

    // 창이 생기고 빛이 없어짐
    public void FadeOut(FadeType _fadeType, bool isSpeed = false)
    {
        Color _color = new Color();
        switch (_fadeType)
        {
            case FadeType.White: _color = whiteColor; break;
            case FadeType.Black: _color = blackColor; break;
        }

        float speed = isSpeed ? fadeSlowSpeed : fadeSpeed;
        StartCoroutine(Co_FadeOut(_color, speed));
    }

    IEnumerator Co_FadeOut(Color color, float speed)
    {
        DialogueManager.instance.isCameraEffect = true;
        isFade = true;
        color.a = 0;
        fadeImage.color = color;


        while(color.a < 1)
        {
            color.a += speed;
            fadeImage.color = color;
            yield return ws;
        }
        isFade = false;
        DialogueManager.instance.isCameraEffect = false;
    }

    // 창이 걷히고 빛이 바래짐
    public void FadeIn(FadeType _fadeType, bool isSpeed = false)
    {
        Color _color = new Color();
        switch (_fadeType)
        {
            case FadeType.White: _color = whiteColor; break;
            case FadeType.Black: _color = blackColor; break;
        }
        float speed = isSpeed ? fadeSlowSpeed : fadeSpeed;
        StartCoroutine(Co_FadeIn(_color, speed));
    }

    IEnumerator Co_FadeIn(Color color, float speed)
    {
        DialogueManager.instance.isCameraEffect = true;
        isFade = true;
        color.a = 1;
        fadeImage.color = color;

        while (color.a > 0)
        {
            color.a -= speed;
            fadeImage.color = color;
            yield return ws;
        }
        isFade = false;
        DialogueManager.instance.isCameraEffect = false;
    }
}
