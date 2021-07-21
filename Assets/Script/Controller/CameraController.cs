using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform tf_CurrentTalkCharacter = null;

    public void CameraTargettion(Transform p_Targer, float p_CameraSpeed = 0.05f)
    {
        if (p_Targer == null || p_Targer == tf_CurrentTalkCharacter) return;
        StopAllCoroutines();
        StartCoroutine(Co_CameraTargetting(p_Targer, p_CameraSpeed));
    }

    [SerializeField] float viewUp;
    IEnumerator Co_CameraTargetting(Transform p_Targer, float p_CameraSpeed = 0.05f)
    {
        Vector3 targetPosition = p_Targer.position + (Vector3.up * viewUp);
        Vector3 forwardTargerPosition = targetPosition + p_Targer.forward * 1.2f;
        Vector3 camDirection = (targetPosition - forwardTargerPosition).normalized;

        while (transform.position != forwardTargerPosition || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(camDirection) ) >= 0.5f )
        {
            CameraMove(forwardTargerPosition, Quaternion.LookRotation(camDirection), p_CameraSpeed);
            yield return null;
        }
    }

    Vector3 originPosition;
    Quaternion originRotation;
    public void CamOriginSetting()
    {
        originPosition = transform.position;
        originRotation = Quaternion.Euler(0, 0, 0);
    }

    public void CameraReset(float p_CameraSpeed = 0.05f)
    {
        StopAllCoroutines();
        StartCoroutine(Co_CameraReset(p_CameraSpeed));
    }

    IEnumerator Co_CameraReset(float camSpeed = 0.05f)
    {
        yield return new WaitForSeconds(0.2f);

        while (transform.position != originPosition || Quaternion.Angle(transform.rotation, originRotation ) >= 0.5f)
        {
            CameraMove(originPosition, originRotation, camSpeed);
            yield return null;
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
}
