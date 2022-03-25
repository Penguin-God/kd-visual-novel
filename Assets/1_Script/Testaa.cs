using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testaa : MonoBehaviour
{
    [SerializeField] DialogueChannel dialogueChannel = null;
    [SerializeField] ImageFieldMover imageFieldMover = null;
    [SerializeField] Transform cameraTrasn = null;
    [SerializeField] float speed = 0f;
    [SerializeField] RectTransform imageRect = null;

    private void Awake()
    {
        dialogueChannel.ChangeContextEvent += CameraRotate_byTalk;
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

    [SerializeField] float rotateTorque;
    void CameraRotate_byTalk(DialogueData _data, int contextCount)
    {
        
        string _dirSymbol = _data.cameraRotateDir[contextCount].Trim();
        if (_dirSymbol != "" && (_dirSymbol == "+" || _dirSymbol == "-"))
        {
            imageFieldMover.ChangeCurrentField(_dirSymbol == "+");
            rotateTorque *= (_dirSymbol == "+") ? 1 : -1;
            Quaternion _targetRotation = Quaternion.Euler(cameraTrasn.eulerAngles + (Vector3.up * rotateTorque));
            StartCoroutine(Co_RotateEffect(_targetRotation, imageFieldMover.GetTargetPos(_dirSymbol == "+")));
        }
        rotateTorque = Mathf.Abs(rotateTorque);
    }

    void CameraRotate_byTalk()
    {
        int _torque = 45;
        Vector3 currentEuler = cameraTrasn.eulerAngles;
        Quaternion _targetRatation = Quaternion.Euler(currentEuler += (Vector3.up * _torque));
        
    }

    IEnumerator Co_RotateEffect(Quaternion _targetRatation, Vector3 _targetPos)
    {
        while (Quaternion.Angle(cameraTrasn.rotation, _targetRatation) >= 0.1f)
        {
            imageRect.localPosition = Vector3.Lerp(imageRect.localPosition, _targetPos, speed);
            cameraTrasn.rotation = Quaternion.Lerp(cameraTrasn.rotation, _targetRatation, speed * 0.8f);
            yield return new WaitForSeconds(0.02f);
        }
        cameraTrasn.rotation = _targetRatation;
        imageRect.localPosition = _targetPos;
    }
}