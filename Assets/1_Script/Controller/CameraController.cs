using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static bool isOnlyView = true;
    public static bool isCameraEffect = false;
    [SerializeField] DialogueChannel dialogueChannel = null;
    [SerializeField] SceneChannel sceneChannel = null;
    [SerializeField] float targettionDistance;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;

    WaitForSeconds ws = new WaitForSeconds(0.03f);
    
    private void Start()
    {
        dialogueChannel.StartInteractionEvent += (_target, _con) =>
        {
            CamOriginSetting();
            if(_target != null) CameraLookTarget(_target, () => CameraTargetting(_target));
            else dialogueChannel.Raise_StartTalkEvent(_con);
        };

        dialogueChannel.EndTalkEvent += () =>
        {
            if (!sceneChannel.IsInteraction_With_SceneLoadTrigger)
                CameraReset();
        };
    }

    [SerializeField] float viewUp;
    void CameraTargetting(Transform p_Targer)
    {
        Vector3 targetPosition = p_Targer.position + (Vector3.up * viewUp);
        Vector3 forwardTargerPosition = targetPosition + p_Targer.forward * targettionDistance;
        Vector3 camDirection = (targetPosition - forwardTargerPosition).normalized;
        CameraTargettig(forwardTargerPosition, Quaternion.LookRotation(camDirection));

        // 카메라 타겟팅 하면서 대화 시작
        dialogueChannel.Raise_StartTalkEvent(p_Targer.GetComponent<InteractionEvent>().Container);
    }
    void CameraTargettig(Vector3 _targetPos, Quaternion _LookTargetRot, Action _targettingEndAct = null)
    {
        StartCoroutine(Co_CameraTargettig(_targetPos, _LookTargetRot, _targettingEndAct));
    }
    IEnumerator Co_CameraTargettig(Vector3 _targetPos, Quaternion _LookTargetRot, Action _targettingEndAct = null)
    {
        isCameraEffect = true;
        while (Vector3.Distance(transform.position, _targetPos) > 0.1f || Quaternion.Angle(transform.rotation, _LookTargetRot) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPos, moveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, _LookTargetRot, rotateSpeed);
            yield return ws;
        }

        isCameraEffect = false;
        SetCameraTransform(_targetPos, _LookTargetRot);
        if (_targettingEndAct != null) _targettingEndAct();
    }

    void SetCameraTransform(Vector3 p_Position, Quaternion p_Rotation)
    {
        transform.position = p_Position;
        transform.rotation = p_Rotation;
    }

    [SerializeField] float rotateTorque;
    public void CameraRotate_byTalk(DialogueData _data, int contextCount)
    {
        string _dirSymbol = _data.cameraRotateDir[contextCount].Trim();
        if (_dirSymbol != "" && (_dirSymbol == "+" || _dirSymbol == "-"))
        {
            rotateTorque *= (_dirSymbol == "+") ? 1 : -1;
            Quaternion _targetRotation = Quaternion.Euler(transform.eulerAngles + (Vector3.up * rotateTorque));
            CameraLookTarget(_targetRotation);
        }
    }

    //public void FiledMove_ByTalk(DialogueData _data, int _index)
    //{
    //    string _dirSymbol = _data.cameraRotateDir[_index].Trim();
        
    //    if (_dirSymbol != "" && (_dirSymbol == "+" || _dirSymbol == "-"))
    //    {
    //        bool _cameraRotateDirIsRight = (_dirSymbol == "+") ? true : false;
    //        imageFieldMover.ChangeCurrentField(_cameraRotateDirIsRight);
    //    }
    //}

    void RotateEffect_ByTalk(DialogueData _data, int contextCount)
    {
        string _dirSymbol = _data.cameraRotateDir[contextCount].Trim();
        if (_dirSymbol != "" && (_dirSymbol == "+" || _dirSymbol == "-"))
        {
            rotateTorque *= (_dirSymbol == "+") ? 1 : -1;
            Quaternion _targetRotation = Quaternion.Euler(transform.eulerAngles + (Vector3.up * rotateTorque));
            CameraLookTarget(_targetRotation);
        }
    }

    [SerializeField] Vector3 originPosition;
    [SerializeField] Quaternion originRotation;
    void CamOriginSetting()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    // Reset
    void CameraReset()
    {
        StopAllCoroutines();
        StartCoroutine(Co_CameraReset());
    }
    IEnumerator Co_CameraReset()
    {
        yield return new WaitForSeconds(0.2f);
        CameraTargettig(originPosition, originRotation, dialogueChannel.Raise_EndInteractionEvent);
    }

    // Move
    void CameraMoveToTarget(Vector3 _targetPos, Action _moveEndAct = null) => StartCoroutine(Co_CameraMoveToTarget(_targetPos, _moveEndAct));
    IEnumerator Co_CameraMoveToTarget(Vector3 _targetPos, Action _moveEndAct = null)
    {
        isCameraEffect = true;
        while (Vector3.Distance(transform.position, _targetPos ) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPos, moveSpeed);
            yield return ws;
        }
        transform.position = _targetPos;
        isCameraEffect = false;
        if (_moveEndAct != null) _moveEndAct();
    }

    // Rotate
    void CameraLookTarget(Transform _target, Action _rotateEndAct = null)
    {
        Vector3 _dir = _target.position - transform.position;
        Quaternion _lookRotation = Quaternion.LookRotation(_dir);
        CameraLookTarget(_lookRotation, _rotateEndAct);
    }
    void CameraLookTarget(Quaternion _LookTargetRot, Action _rotateEndAct = null) => StartCoroutine(Co_CameraLookTarget(_LookTargetRot, _rotateEndAct));
    IEnumerator Co_CameraLookTarget(Quaternion _LookTargetRot, Action _rotateEndAct = null)
    {
        isCameraEffect = true;
        while (Quaternion.Angle(transform.rotation, _LookTargetRot) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _LookTargetRot, rotateSpeed);
            yield return ws;
        }
        transform.rotation = _LookTargetRot;
        isCameraEffect = false;
        if (_rotateEndAct != null) _rotateEndAct();
    }
}