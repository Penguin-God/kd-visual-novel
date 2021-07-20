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

    IEnumerator Co_CameraTargetting(Transform p_Targer, float p_CameraSpeed = 0.05f)
    {
        Vector3 targetPosition = p_Targer.position;
        Vector3 forwardTargerPosition = targetPosition + p_Targer.forward * 1.5f;
        Vector3 camDirection = (targetPosition - forwardTargerPosition).normalized;

        while (transform.position != forwardTargerPosition || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(camDirection) ) >= 0.5f )
        {
            transform.position = Vector3.MoveTowards(transform.position, forwardTargerPosition, p_CameraSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(camDirection), p_CameraSpeed);
            yield return null;
        }
    }
}
