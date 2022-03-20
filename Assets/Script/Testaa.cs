using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testaa : MonoBehaviour
{
    [SerializeField] Transform cameraTrasn = null;
    [SerializeField] float speed = 0f;
    [SerializeField] RectTransform targetRect = null;
    [SerializeField] RectTransform imageRect = null;

    [ContextMenu("test rect")]
    public void TestRect()
    {

        StartCoroutine(Co_Test());
        CameraRotate_byTalk();
    }

    IEnumerator Co_Test()
    {
        while (Vector3.Distance(targetRect.position, imageRect.position) > 2)
        {
            imageRect.position = Vector3.Lerp(imageRect.position, targetRect.position, speed * 0.5f);
            yield return null;
        }
    }

    void CameraRotate_byTalk()
    {
        int _torque = 45;
        Vector3 currentEuler = cameraTrasn.eulerAngles;
        Quaternion _targetRatation = Quaternion.Euler(currentEuler += (Vector3.up * _torque));
        StartCoroutine(Co_CameraTargetting(_targetRatation));
    }

    IEnumerator Co_CameraTargetting(Quaternion _targetRatation)
    {
        while (Quaternion.Angle(cameraTrasn.rotation, _targetRatation) >= 3f)
        {
            cameraTrasn.rotation = Quaternion.Lerp(cameraTrasn.rotation, _targetRatation, speed);
            yield return null;
        }
    }
}