using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static bool isOnlyView = true;
    [SerializeField] DialogueChannel dialogueChannel = null;
    [SerializeField] float targettionDistance;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;

    //Transform tf_CurrentTalkCharacter = null;
    WaitForSeconds ws = new WaitForSeconds(0.03f);
    
    private void Start()
    {
        dialogueChannel.StartInteractionEvent += (_taget, _con) => CameraTargetting(_taget);
        dialogueChannel.ChangeContextEvent += CameraRotate_byTalk;
        dialogueChannel.EndTalkEvent += CameraReset;
    }

    [SerializeField] float viewUp;
    void CameraTargetting(Transform p_Targer)
    {
        Vector3 targetPosition = p_Targer.position + (Vector3.up * viewUp);
        Vector3 forwardTargerPosition = targetPosition + p_Targer.forward * targettionDistance;
        Vector3 camDirection = (targetPosition - forwardTargerPosition).normalized;

        CameraTargetttig(forwardTargerPosition, Quaternion.LookRotation(camDirection));
    }

    void CameraRotate_byTalk(DialogueData _data, int contextCount)
    {
        if (!Int32.TryParse(_data.cameraTorque[contextCount], out int _torque)) return;

        Vector3 currentEuler = transform.eulerAngles;
        Quaternion _targetRotation = Quaternion.Euler(currentEuler += (Vector3.up * _torque));
        CameraRotateToTarget(_targetRotation);
    }

    void CameraReset()
    {
        StopAllCoroutines();
        StartCoroutine(Co_CameraReset());
    }

    
    void CameraTargetttig(Vector3 _targetPos, Quaternion _LookTargetRot, Action _targettingEndAct = null)
    { 
        CamOriginSetting();  
        StartCoroutine(Co_CameraTargetttig(_targetPos, _LookTargetRot, _targettingEndAct)); 
    }             
    IEnumerator Co_CameraTargetttig(Vector3 _targetPos, Quaternion _LookTargetRot, Action _targettingEndAct = null)
    {
        DialogueManager.instance.isCameraEffect = true;
        while (Vector3.Distance(transform.position, _targetPos) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPos, moveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, _LookTargetRot, rotateSpeed);
            yield return ws;
        }

        SetCameraTransform(_targetPos, _LookTargetRot);
        DialogueManager.instance.isCameraEffect = false;
        if (_targettingEndAct != null) _targettingEndAct();
    }

    Vector3 originPosition;
    Quaternion originRotation;
    void CamOriginSetting()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    void SetCameraTransform(Vector3 p_Position, Quaternion p_Rotation)
    {
        transform.position = p_Position;
        transform.rotation = p_Rotation;
    }


    void CameraMoveToTarget(Vector3 _targetPos, Action _moveEndAct = null) => StartCoroutine(Co_CameraMoveToTarget(_targetPos, _moveEndAct));
    IEnumerator Co_CameraMoveToTarget(Vector3 _targetPos, Action _moveEndAct = null)
    {
        while (Vector3.Distance(transform.position, _targetPos ) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPos, moveSpeed);
            yield return ws;
        }
        transform.position = _targetPos;
        if (_moveEndAct != null) _moveEndAct();
    }

    void CameraRotateToTarget(Quaternion _LookTargetRot, Action _rotateEndAct = null) => StartCoroutine(Co_CameraRotateToTarget(_LookTargetRot, _rotateEndAct));
    IEnumerator Co_CameraRotateToTarget(Quaternion _LookTargetRot, Action _rotateEndAct = null)
    {
        while (Quaternion.Angle(transform.rotation, _LookTargetRot) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _LookTargetRot, rotateSpeed);
            yield return ws;
        }
        transform.rotation = _LookTargetRot;
        if (_rotateEndAct != null) _rotateEndAct();
    }


    IEnumerator Co_CameraReset()
    {
        yield return new WaitForSeconds(0.2f);
        CameraTargetttig(originPosition, originRotation, dialogueChannel.Raise_EndInteractionEvent);
    }
}
