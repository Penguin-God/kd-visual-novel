using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{

    private void Start()
    {
        //FindObjectOfType<DialogueManager>().AfterTalkEvent += ChangeSprite_byTalk;
    }

    void ChangeSprite_byTalk(Dialogue dialogue, int contextCount)
    {
        Transform target = dialogue.tf_Target;
        if (target != null) SpriteChange(target, dialogue.spriteNames[contextCount]);
    }

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
        SpriteRenderer[] sr = targer.GetComponentsInChildren<SpriteRenderer>();
        spriteName = spriteName.Trim(); // 원인은 모르겠지만 글자 마지막에 공백이 들어가서 공백 지우는 함수 사용
        Sprite sprite = Resources.Load("Characters/" + spriteName, typeof(Sprite)) as Sprite;

        if (sprite != null && !Check_SameSprite(sr[0], sprite))
        {
            Color color = sr[0].color;
            color.a = 0;
            sr[0].color = color;
            sr[0].sprite = sprite;

            Color shadowColor = sr[1].color;
            shadowColor.a = 0;
            sr[1].color = shadowColor;
            sr[1].sprite = sprite;

            while (color.a < 1f)
            {
                color.a += fadeSpeed;
                sr[0].color = color;

                shadowColor.a += fadeSpeed;
                sr[1].color = shadowColor;
                yield return null;
            }
        }
    }
}
