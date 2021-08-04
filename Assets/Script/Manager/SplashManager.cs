using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    [SerializeField] Image fadeImage;

    [SerializeField] Color whiteColor;
    [SerializeField] Color blackColor;

    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeSlowSpeed;

    public bool isFade = false;

    WaitForSeconds ws = new WaitForSeconds(0.05f);



    public void Splash()
    {
        StartCoroutine(Co_Splash());
    }

    IEnumerator Co_Splash()
    {
        isFade = true;
        FadeOut(true);
        yield return new WaitUntil(() => !isFade);
        FadeIn(true);
    }

    public void FadeOut(bool isWhite, bool isSpeed = false)
    {
        Color color = isWhite ? whiteColor : blackColor;
        float speed = isSpeed ? fadeSlowSpeed : fadeSpeed;
        StartCoroutine(Co_FadeOut(color, speed));
    }

    IEnumerator Co_FadeOut(Color color, float speed)
    {
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
    }


    public void FadeIn(bool isWhite, bool isSpeed = false)
    {
        Color color = isWhite ? whiteColor : blackColor;
        float speed = isSpeed ? fadeSlowSpeed : fadeSpeed;
        StartCoroutine(Co_FadeIn(color, speed));
    }

    IEnumerator Co_FadeIn(Color color, float speed)
    {
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
    }
}
