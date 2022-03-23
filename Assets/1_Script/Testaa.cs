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

    private void Update()
    {
        DialogueManager.instance.isCameraEffect = true;
    }

    [ContextMenu("test rect")]
    public void TestRect()
    {
        Debug.Log(imageRect.localPosition);
        CameraRotate_byTalk();
    }

    IEnumerator Co_Test()
    {
        Vector3 _targetPos = new Vector3(imageRect.position.x - 35, imageRect.position.y, imageRect.position.z);
        while (Vector3.Distance(_targetPos, imageRect.position) > 2)
        {
            imageRect.position = Vector3.Lerp(imageRect.position, _targetPos, speed * 0.5f);
            yield return new WaitForSeconds(0.05f);
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
        Vector3 _targetPos = new Vector3(imageRect.localPosition.x - 52, imageRect.localPosition.y, imageRect.localPosition.z);

        while (Quaternion.Angle(cameraTrasn.rotation, _targetRatation) >= 0.1f)
        {
            imageRect.localPosition = Vector3.Lerp(imageRect.localPosition, _targetPos, speed);
            cameraTrasn.rotation = Quaternion.Lerp(cameraTrasn.rotation, _targetRatation, speed * 0.8f);
            yield return new WaitForSeconds(1f);
        }
    }
}