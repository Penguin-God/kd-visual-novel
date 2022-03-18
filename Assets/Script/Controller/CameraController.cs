using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static bool isOnlyView = true;
    [SerializeField] DialogueCannel dialogueCannel = null;

    Transform tf_CurrentTalkCharacter = null;
    WaitForSeconds ws = new WaitForSeconds(0.02f);
    
    private void Start()
    {
        dialogueCannel.StartInteractionEvent += CameraEffect_byEventStart;
        dialogueCannel.EndTalkEvent += CameraReset;
        dialogueCannel.ChangeContextEvent += CameraRotate_byTalk;
    }

    void CameraEffect_byEventStart(Transform target, DialogueDataContainer _container)
    {
        CamOriginSetting();
        CameraTargettion(target, _container);
    }

    [SerializeField] float rotateSpeed = 0;
    void CameraRotate_byTalk(DialogueData _data, int contextCount)
    {
        //if(dialogue.cameraType == CameraType.Default) CameraTargettion(dialogue.tf_Target);
        if (!Int32.TryParse(_data.cameraTorque[contextCount], out int _torque)) return;
        Vector3 currentEuler = transform.eulerAngles;
        Quaternion _targetRatation = Quaternion.Euler(currentEuler += (Vector3.up * _torque));
        StartCoroutine(Co_CameraTargetting(_targetRatation));
    }

    void CameraReset()
    {
        StopAllCoroutines();
        StartCoroutine(Co_CameraReset());
    }

    void CameraTargettion(Transform p_Targer, DialogueDataContainer _container, float p_CameraSpeed = 0.15f)
    {
        if (p_Targer == null || p_Targer == tf_CurrentTalkCharacter) return;
        StopAllCoroutines();
        StartCoroutine(Co_CameraTargetting(p_Targer, _container, p_CameraSpeed));
    }

    [SerializeField] float viewUp;
    IEnumerator Co_CameraTargetting(Transform p_Targer, DialogueDataContainer _container, float p_CameraSpeed = 0.05f)
    {
        DialogueManager.instance.isCameraEffect = true;
        Vector3 targetPosition = p_Targer.position + (Vector3.up * viewUp);
        Vector3 forwardTargerPosition = targetPosition + p_Targer.forward * 1.2f;
        Vector3 camDirection = (targetPosition - forwardTargerPosition).normalized;
        
        while (Vector3.Distance(transform.position, forwardTargerPosition) > 0.1 || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(camDirection) ) >= 3f )
        {
            CameraMove(forwardTargerPosition, Quaternion.LookRotation(camDirection), p_CameraSpeed);
            yield return ws;
        }

        SetCameraTransform(forwardTargerPosition, Quaternion.LookRotation(camDirection)); // 오차 없애기
        dialogueCannel.Raise_StartTalkEvent(_container); // 대화 시작
        DialogueManager.instance.isCameraEffect = false;
    }

    IEnumerator Co_CameraTargetting(Quaternion _targetRatation, float p_CameraSpeed = 0.05f)
    {
        while (Quaternion.Angle(transform.rotation, _targetRatation) >= 3f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRatation, p_CameraSpeed);
            yield return ws;
        }
    }

    Vector3 originPosition;
    Quaternion originRotation;
    void CamOriginSetting()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }
    
    IEnumerator Co_CameraReset(float camSpeed = 0.05f)
    {
        DialogueManager.instance.isCameraEffect = true;
        yield return new WaitForSeconds(0.2f);
        
        while (transform.position != originPosition || Quaternion.Angle(transform.rotation, originRotation ) >= 1f)
        {
            CameraMove(originPosition, originRotation, camSpeed);
            yield return ws;
        }

        SetCameraTransform(originPosition, originRotation);
        //DialogueManager.instance.isTalking = false;
        //yield return null; // EventManager.isAutoEvent 선언 대기
        //if (!EventManager.isAutoEvent) UIManager.instance.ShowUI();
        dialogueCannel.Raise_EndInteractionEvent();
        DialogueManager.instance.isCameraEffect = false;
    }

    void CameraMove(Vector3 target_Position, Quaternion target_Rotation, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target_Position, speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, target_Rotation, speed);
    }

    void SetCameraTransform(Vector3 p_Position, Quaternion p_Rotation)
    {
        transform.position = p_Position;
        transform.rotation = p_Rotation;
    }
}
