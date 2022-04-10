using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpin : MonoBehaviour
{
    Transform tf_Target = null;
    Quaternion lookAngle;
    Vector3 lookEuler;

    private void Start()
    {
        tf_Target = PlayerController.instance.transform;
    }

    void Update()
    { 
        if (tf_Target == null || isSpin) return;

        lookAngle = Quaternion.LookRotation(tf_Target.position);
        lookEuler = Vector3.up * lookAngle.eulerAngles.y;
        transform.eulerAngles = lookEuler;
    }

    bool isSpin = false;
    public void CharactersSpinEvent(bool isShow)
    {
        isSpin = true;
        if (isShow) 
        {
            gameObject.SetActive(true);
            StartCoroutine(Co_Appear()); 
        }
        else StartCoroutine(Co_DIsappear());
        StartCoroutine(Co_Spin(3)); // 투명도 처리랑 회전이랑 따로임
    }

    IEnumerator Co_Appear()
    {
        SpriteRenderer[] spriteRenderer = transform.GetComponentsInChildren<SpriteRenderer>();
        Color front_Color = spriteRenderer[0].color; front_Color.a = 0; spriteRenderer[0].color = front_Color;
        Color shadow_Color = spriteRenderer[1].color; shadow_Color.a = 0; spriteRenderer[1].color = shadow_Color;

        yield return new WaitForSeconds(0.3f);

        while(front_Color.a < 1)
        {
            front_Color.a += 0.01f; spriteRenderer[0].color = front_Color;
            shadow_Color.a += 0.01f; spriteRenderer[1].color = shadow_Color;
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator Co_DIsappear()
    {
        SpriteRenderer[] spriteRenderer = transform.GetComponentsInChildren<SpriteRenderer>();
        Color front_Color = spriteRenderer[0].color; front_Color.a = 1; spriteRenderer[0].color = front_Color;
        Color shadow_Color = spriteRenderer[1].color; shadow_Color.a = 1; spriteRenderer[1].color = shadow_Color;

        yield return new WaitForSeconds(0.3f);

        while (front_Color.a > 0)
        {
            front_Color.a -= 0.01f; spriteRenderer[0].color = front_Color;
            shadow_Color.a -= 0.01f; spriteRenderer[1].color = shadow_Color;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitUntil(() => !isSpin);
        gameObject.SetActive(false);
    }

    IEnumerator Co_Spin(int spinCount)
    {
        float delayTime = 0.06f;
        float timeCount = 0;
        int currentSpinCount = 0;
        while (currentSpinCount < spinCount)
        {
            transform.Rotate(Vector3.up * 1080 * delayTime);

            if (timeCount < 1) timeCount += delayTime;
            else
            {
                timeCount = 0;
                currentSpinCount++;
            }
            yield return new WaitForSeconds(delayTime);
        }
        isSpin = false;
    }
}
