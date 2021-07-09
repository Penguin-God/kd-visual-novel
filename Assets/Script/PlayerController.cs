using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float half_ScreenWidth;
    float half_ScreenHeight;

    private void Awake()
    {
        half_ScreenWidth = Screen.width / 2;
        half_ScreenHeight = Screen.height / 2;
    }

    void Update()
    {
        MovingCorsshair();
        RotateView();
    }

    [SerializeField] Transform tf_Corsshair;
    private float cursorMargin = 30;
    float corsshair_X;
    float corsshair_Y;
    void MovingCorsshair()
    {
        // RectTranform을 사용할 경우 크로스헤어가 마우스포지션을 따라가지만 크로스헤어의 범위에 제한을 둘 때 범위에 문제가 생김
        //rect = tf_Corsshair.gameObject.GetComponent<RectTransform>();
        //float corsshair_X = Input.mousePosition.x;
        //float corsshair_Y = Input.mousePosition.y;
        //rect.position = new Vector2(corsshair_X, corsshair_Y);

        // 크로스 헤어의 좌표값 구함
        corsshair_X = Input.mousePosition.x - half_ScreenWidth;
        corsshair_Y = Input.mousePosition.y - half_ScreenHeight;

        // corsshair가 화면 밖으로 나가는거 방지하기 위해 스크린 크기만큼 제한한 값을 다시 할당
        corsshair_X = Mathf.Clamp(corsshair_X, -half_ScreenWidth + cursorMargin, half_ScreenWidth - cursorMargin);
        corsshair_Y = Mathf.Clamp(corsshair_Y, -half_ScreenHeight + cursorMargin, half_ScreenHeight - cursorMargin);
        tf_Corsshair.localPosition = new Vector2(corsshair_X, corsshair_Y);
    }


    [SerializeField] Transform th_Camera;

    [SerializeField] float rotateSpeed;
    [SerializeField] float look_X_Limit;
    [SerializeField] float look_Y_Limit;
    float currentCameraAngle_X;
    float currentCameraAngle_Y;
    void RotateView()
    {
        // 여백까지 고려하여 끝부분에 닿으면 카메라 회전
        if(corsshair_X > half_ScreenWidth - 80 || corsshair_X < -half_ScreenWidth + 80)
        {
            // 크로스헤어는 x축이지만 카메라는 y축 회전을 해야함, 크로스헤어 x축 부호에 따라 더할지 뺄지 결정
            currentCameraAngle_Y += (corsshair_X > 0) ? rotateSpeed : -rotateSpeed;
            currentCameraAngle_Y = Mathf.Clamp(currentCameraAngle_Y, -look_X_Limit, look_X_Limit);
        }

        // 위 코드에서 축만 바꿈
        if (corsshair_Y > half_ScreenHeight - 80 || corsshair_Y < -half_ScreenHeight + 80)
        {
            currentCameraAngle_X += (corsshair_Y > 0) ? -rotateSpeed : rotateSpeed; // x회전값은 더해주면 내려가고 빼면 올라가서 반대로 해야됨
            currentCameraAngle_X = Mathf.Clamp(currentCameraAngle_X, -look_Y_Limit, look_Y_Limit);
        }

        th_Camera.rotation = Quaternion.Euler(new Vector3(currentCameraAngle_X, currentCameraAngle_Y, 0));
    }
}
