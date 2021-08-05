using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform tf_CurrentTalkCharacter = null;
    public bool isTargetTing = false;
    WaitForSeconds ws = new WaitForSeconds(0.008f);

    private void Start()
    {
        DialogueManager.instance.OnEndTalk += CameraReset;
    }

    void CameraReset()
    {
        StopAllCoroutines();
        StartCoroutine(Co_CameraReset());
    }

    public void CameraTargettion(Transform p_Targer, float p_CameraSpeed = 0.05f)
    {
        if (p_Targer == null || p_Targer == tf_CurrentTalkCharacter) return;
        StopAllCoroutines();
        StartCoroutine(Co_CameraTargetting(p_Targer, p_CameraSpeed));
    }

    [SerializeField] float viewUp;
    IEnumerator Co_CameraTargetting(Transform p_Targer, float p_CameraSpeed = 0.05f)
    {
        isTargetTing = true;
        Vector3 targetPosition = p_Targer.position + (Vector3.up * viewUp);
        Vector3 forwardTargerPosition = targetPosition + p_Targer.forward * 1.2f;
        Vector3 camDirection = (targetPosition - forwardTargerPosition).normalized;
        
        StartCoroutine(Co_SetIsTargetTing(forwardTargerPosition));
        while (transform.position != forwardTargerPosition || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(camDirection) ) >= 1f )
        {
            CameraMove(forwardTargerPosition, Quaternion.LookRotation(camDirection), p_CameraSpeed);
            yield return ws;
        }
    }

    Vector3 originPosition;
    Quaternion originRotation;
    public void CamOriginSetting()
    {
        originPosition = transform.position;
        originRotation = Quaternion.Euler(0, 0, 0);
    }
    
    IEnumerator Co_CameraReset(float camSpeed = 0.05f)
    {
        yield return new WaitForSeconds(0.2f);
        
        while (transform.position != originPosition || Quaternion.Angle(transform.rotation, originRotation ) >= 1f)
        {
            CameraMove(originPosition, originRotation, camSpeed);
            yield return ws;
        }

        transform.position = originPosition;
        DialogueManager.instance.isTalking = false;
        UIManager.instance.ShowUI();
    }

    void CameraMove(Vector3 target_Position, Quaternion target_Rotation, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target_Position, speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, target_Rotation, speed);
    }

    IEnumerator Co_SetIsTargetTing(Vector3 targetPosition)
    {
        while (isTargetTing)
        {
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f) isTargetTing = false;
            yield return null;
        }
    }
}
