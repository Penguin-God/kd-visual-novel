using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    float half_ScreenWidth;
    float half_ScreenHeight;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        half_ScreenWidth = Screen.width / 2;
        half_ScreenHeight = Screen.height / 2;

        originPos_CameraY = tf_Camera.localPosition.y;
    }

    [SerializeField] DialogueCannel dialogueCannel;
    private void Start()
    {
        dialogueCannel.EndInteractionEvent += AngleValueReset;
    }

    public void AngleValueReset()
    {
        tf_Corsshair.localPosition = Vector3.zero;
        currentCameraAngle_X = 0;
        currentCameraAngle_Y = 0;
    }

    public void ResetCamera()
    {
        tf_Camera.localPosition = Vector3.up;
        tf_Camera.localEulerAngles = Vector3.zero;
    }

    void Update()
    {
        if (!GameManager.instance.IsPlayable) return;

        if (CameraController.isOnlyView)
        {
            // 크로스 헤어 이동
            MovingCorsshair();

            // 회전 관련 변수 연산
            MovingView_by_Corsshair();
            MovingView_GetKey();

            //연산이 이뤄진 회전 변수 제한 및 대입
            CurbCameraRotation();
            CurbCameraPosition();
            SetRotation();
            NotCameraMoveUI();
        }
        else // 필드 이동
        {
            FiledMoving();
            FiledLooking();
        }
    }

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    private float applySpeed;

    void FiledMoving()
    {
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (!Input.GetKey(KeyCode.LeftShift)) applySpeed = walkSpeed;
            else applySpeed = runSpeed;
            transform.Translate(dir * applySpeed * Time.deltaTime);
        }
    }

    [SerializeField] float rotationSpeed;
    [SerializeField] float filedLookLimit_X;
    private float filedCurrentAngle_X = 0;

    void FiledLooking()
    {
        //if (Input.GetAxis("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        //{
        //    filedCurrentAngle_Y += Input.GetAxisRaw("Mouse X") * rotationSpeed;
        //    filedCurrentAngle_X -= Input.GetAxisRaw("Mouse X") * rotationSpeed * -1;
        //    filedCurrentAngle_X = Mathf.Clamp(filedCurrentAngle_X, -filedLookLimit_X, filedLookLimit_X);

        //    transform.eulerAngles = new Vector3(filedCurrentAngle_X, filedCurrentAngle_Y, 0);
        //}

        if (Input.GetAxis("Mouse X") != 0)
        {
            Vector3 turn_Y = new Vector3(0, Input.GetAxisRaw("Mouse X") * rotationSpeed, 0);
            transform.rotation *= Quaternion.Euler(turn_Y);
        }

        if (Input.GetAxisRaw("Mouse Y") != 0)
        {
            filedCurrentAngle_X -= Input.GetAxisRaw("Mouse Y") * rotationSpeed;
            filedCurrentAngle_X = Mathf.Clamp(filedCurrentAngle_X, -filedLookLimit_X, filedLookLimit_X);
            tf_Camera.localEulerAngles = new Vector3(filedCurrentAngle_X, 0, 0);
        }
    }


    [SerializeField] Transform tf_Corsshair;
    private readonly float cursorMargin = 30;
    private float corsshair_X;
    private float corsshair_Y;

    void MovingCorsshair()
    {
        // 크로스 헤어의 좌표값 구함
        corsshair_X = Input.mousePosition.x - half_ScreenWidth;
        corsshair_Y = Input.mousePosition.y - half_ScreenHeight;

        // corsshair가 화면 밖으로 나가는거 방지하기 위해 스크린 크기만큼 제한한 값을 다시 할당
        corsshair_X = Mathf.Clamp(corsshair_X, -half_ScreenWidth + cursorMargin, half_ScreenWidth - cursorMargin);
        corsshair_Y = Mathf.Clamp(corsshair_Y, -half_ScreenHeight + cursorMargin, half_ScreenHeight - cursorMargin);
        tf_Corsshair.localPosition = new Vector2(corsshair_X, corsshair_Y);
    }


    [SerializeField] float playerRotateSpeed;
    private float currentCameraAngle_X;
    private float currentCameraAngle_Y;

    [SerializeField] float playerMoveSpeed;
    private readonly float sideMargin = 80;
    void MovingView_by_Corsshair()
    {
        // 여백까지 고려하여 끝부분에 닿으면 카메라 회전
        if (corsshair_X > half_ScreenWidth - sideMargin || corsshair_X < -half_ScreenWidth + sideMargin)
        {
            // 크로스헤어는 x축이지만 카메라는 y축 회전을 해야함,  크로스헤어 x축 부호에 따라 더할지 뺄지 결정
            currentCameraAngle_Y += (corsshair_X > 0) ? playerRotateSpeed : -playerRotateSpeed;

            MoveX(corsshair_X);
        }

        // 위 코드에서 축만 바꿈
        if (corsshair_Y > half_ScreenHeight - sideMargin || corsshair_Y < -half_ScreenHeight + sideMargin)
        {
            // x회전값은 더해주면 내려가고 빼면 올라가서 속도 변수 부호를 반대로 해야됨
            currentCameraAngle_X += (corsshair_Y > 0) ? -playerRotateSpeed : playerRotateSpeed;
            MoveY(corsshair_Y);
        }
    }

    void MovingView_GetKey()
    {
        float getKey_X = Input.GetAxisRaw("Horizontal");
        if (getKey_X != 0)
        {
            currentCameraAngle_Y += getKey_X * playerRotateSpeed;
            MoveX(getKey_X);
        }

        float getKey_Y = Input.GetAxisRaw("Vertical");
        if (getKey_Y != 0)
        {
            currentCameraAngle_X += -getKey_Y * playerRotateSpeed;
            MoveY(getKey_Y);
        }
    }

    // 들어온 값의 부호에 따라 플레이어 위치를 움직임
    void MoveX(float moveDirection)
    {
        float move_X = (moveDirection > 0) ? playerMoveSpeed : -playerMoveSpeed;
        tf_Camera.localPosition += Vector3.right * move_X;
    }
    void MoveY(float moveDirection)
    {
        float move_Y = (moveDirection > 0) ? playerMoveSpeed : -playerMoveSpeed;
        tf_Camera.localPosition += Vector3.up * move_Y;
    }


    [SerializeField] float look_X_Limit;
    [SerializeField] float look_Y_Limit;
    void CurbCameraRotation() // 회전값 제한
    {
        currentCameraAngle_Y = Mathf.Clamp(currentCameraAngle_Y, -look_X_Limit, look_X_Limit);
        currentCameraAngle_X = Mathf.Clamp(currentCameraAngle_X, -look_Y_Limit, look_Y_Limit);
    }

    [SerializeField] float move_X_Limit;
    [SerializeField] float move_Y_Limit;
    private float originPos_CameraY;
    void CurbCameraPosition()
    {
        if (tf_Camera.localPosition.x > move_X_Limit || tf_Camera.localPosition.x < -move_X_Limit)
        {
            tf_Camera.localPosition = new Vector3((tf_Camera.localPosition.x > 0) ? move_X_Limit : -move_X_Limit,
                tf_Camera.localPosition.y, tf_Camera.localPosition.z);
        }

        if (tf_Camera.localPosition.y > originPos_CameraY + move_Y_Limit || tf_Camera.localPosition.y < originPos_CameraY - move_Y_Limit)
        {
            tf_Camera.localPosition = new Vector3(tf_Camera.localPosition.x,
                (tf_Camera.localPosition.y > originPos_CameraY) ? originPos_CameraY + move_Y_Limit : originPos_CameraY - move_Y_Limit, tf_Camera.localPosition.z);
        }
    }

    [SerializeField] Transform tf_Camera;
    void SetRotation()
    {
        tf_Camera.localEulerAngles = new Vector3(currentCameraAngle_X, currentCameraAngle_Y, tf_Camera.localEulerAngles.z);
    }


    [SerializeField] GameObject UI_NotCameraMove_Up;
    [SerializeField] GameObject UI_NotCameraMove_Down;
    [SerializeField] GameObject UI_NotCameraMove_Right;
    [SerializeField] GameObject UI_NotCameraMove_Left;
    void NotCameraMoveUI()
    {
        Set_NotCameraMove_UI(UI_NotCameraMove_Up, look_Y_Limit, -currentCameraAngle_X);
        Set_NotCameraMove_UI(UI_NotCameraMove_Down, look_Y_Limit, currentCameraAngle_X);
        Set_NotCameraMove_UI(UI_NotCameraMove_Right, look_X_Limit, currentCameraAngle_Y);
        Set_NotCameraMove_UI(UI_NotCameraMove_Left, look_X_Limit, -currentCameraAngle_Y);
    }

    void Set_NotCameraMove_UI(GameObject UI_NotCameraMove, float limitValue, float currentValue)
    {
        if (currentValue >= limitValue) UI_NotCameraMove.SetActive(true);
        else UI_NotCameraMove.SetActive(false);
    }
}