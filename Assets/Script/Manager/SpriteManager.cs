using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] float fadeSpeed;

    public void SpriteChange(Transform targer, string spriteName)
    {
        StopAllCoroutines();
        StartCoroutine(Co_SpriteChange(targer, spriteName));
    }

    bool Check_SameSprite(SpriteRenderer p_Sr, Sprite p_Sprite)
    {
        if (p_Sr.sprite == p_Sprite) return true;
        else return false;
    }

    IEnumerator Co_SpriteChange(Transform targer, string spriteName)
    {
        SpriteRenderer sr = targer.GetComponentInChildren<SpriteRenderer>();
        spriteName = spriteName.Trim(); // 원인은 모르겠지만 글자 마지막에 공백이 들어가서 공백 지우는 함수 사용
        Sprite sprite = Resources.Load("Characters/" + spriteName, typeof(Sprite)) as Sprite;

        if (sprite != null && !Check_SameSprite(sr, sprite))
        {
            Color color = sr.color;
            color.a = 0;
            sr.color = color;

            sr.sprite = sprite;
            while(color.a < 1f)
            {
                color.a += fadeSpeed;
                sr.color = color;
                yield return null;
            }
        }
    }
}
