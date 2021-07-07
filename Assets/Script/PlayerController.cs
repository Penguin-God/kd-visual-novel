using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        MovingCorsshair();
    }

    [SerializeField] Transform tf_Corsshair;
    private float cursorMargin = 30;
    void MovingCorsshair()
    {
        // 크로스 헤어의 좌표값 구함
        float corsshair_X = Input.mousePosition.x - Screen.width / 2;
        float corsshair_Y = Input.mousePosition.y - Screen.height / 2;

        // corsshair가 화면 밖으로 나가는거 방지하기 위해 스크린 크기만큼 제한한 값을 다시 할당
        corsshair_X = Mathf.Clamp(corsshair_X, -Screen.width / 2 + cursorMargin, Screen.width / 2 - cursorMargin);
        corsshair_Y = Mathf.Clamp(corsshair_Y, -Screen.height / 2 + cursorMargin, Screen.height / 2 - cursorMargin);
        tf_Corsshair.localPosition = new Vector2(corsshair_X, corsshair_Y);
    }
}
