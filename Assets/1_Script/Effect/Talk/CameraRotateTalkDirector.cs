using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CameraRotateTalkDirector : MonoBehaviour
{
    [SerializeField] DialogueChannel dialogueChannel = null;
    [SerializeField] Transform cameraTransform = null;
    [SerializeField] GameObject MAIN_IMAGE_FIELD = null;
    [SerializeField] GameObject[] IMAGE_FIELDS = null;

    [SerializeField] float rotateTorque;
    [SerializeField] float fieldDistance;

    private void Awake()
    {
        dialogueChannel.StartTalkEvent += (_con) => filedContainerRect.gameObject.SetActive(true);
        dialogueChannel.EndTalkEvent += (_con) => filedContainerRect.gameObject.SetActive(false);

        fieldDistance = Mathf.Abs(IMAGE_FIELDS[0].transform.position.x - IMAGE_FIELDS[1].transform.position.x);

        dialogueChannel.ChangeContextEvent += CameraRotateTalk;

        dialogueChannel.EndTalkEvent += (_con) => ResetField();
    }

    [SerializeField] GameObject currentImageField = null;
    public GameObject CurrentImageField => currentImageField;

    [SerializeField] GameObject previousImageField = null;
    public GameObject PerviousImageField => previousImageField;

    void CameraRotateTalk(DialogueData _data, int _index)
    {
        string _dirSymbol = _data.cameraRotateDir[_index].Trim();
        if (_dirSymbol != "" && (_dirSymbol == "+" || _dirSymbol == "-"))
        {
            dialogueChannel.IsTalkable = false;
            bool _cameraRotateDirIsRight = (_dirSymbol == "+") ? true : false;
            ChangeCurrentImageField(_cameraRotateDirIsRight);
            CameraRotate_And_ImageMove(_cameraRotateDirIsRight);
        }
    }

    int CurrentImageFieldIndex => Array.IndexOf(IMAGE_FIELDS, currentImageField);
    public void ChangeCurrentImageField(bool _cameraRotateDirIsRight)
    {
        previousImageField = currentImageField;
        currentImageField = (_cameraRotateDirIsRight) ? IMAGE_FIELDS[CurrentImageFieldIndex + 1] : IMAGE_FIELDS[CurrentImageFieldIndex - 1];
    }


    void CameraRotate_And_ImageMove(bool _isRight)
    {
        rotateTorque *= (_isRight) ? 1 : -1;
        fieldDistance *= (_isRight) ? -1 : 1;
        Quaternion _targetRotation = Quaternion.Euler(cameraTransform.eulerAngles + (Vector3.up * rotateTorque));
        Vector3 _targetPos = new Vector3(filedContainerRect.localPosition.x + fieldDistance, filedContainerRect.localPosition.y, filedContainerRect.localPosition.z);
        StartCoroutine(Co_CameraRotate_And_ImageMove(_targetRotation, _targetPos));

        rotateTorque = Mathf.Abs(rotateTorque);
        fieldDistance = Mathf.Abs(fieldDistance);
    }

    [SerializeField] RectTransform filedContainerRect = null;
    [SerializeField] float cameraRotateSpeed;
    [SerializeField] float fieldMoveSpeed;
    IEnumerator Co_CameraRotate_And_ImageMove(Quaternion _targetLookRotation, Vector3 _targetImageFieldPos)
    {
        while (Quaternion.Angle(cameraTransform.rotation, _targetLookRotation) >= 0.8f)
        {
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, _targetLookRotation, cameraRotateSpeed);
            filedContainerRect.localPosition = Vector3.Lerp(filedContainerRect.localPosition, _targetImageFieldPos, fieldMoveSpeed);
            yield return new WaitForSeconds(0.02f);
        }

        cameraTransform.rotation = _targetLookRotation;
        filedContainerRect.localPosition = _targetImageFieldPos;
        dialogueChannel.IsTalkable = true;
    }

    [SerializeField] Vector3 originPos;
    void ResetField()
    {
        AllSpiteReset();
        fieldDistance = Math.Abs(fieldDistance);
        filedContainerRect.localPosition = originPos;
        currentImageField = MAIN_IMAGE_FIELD;
        previousImageField = null;
    }

    void AllSpiteReset()
    {
        for (int i = 0; i < filedContainerRect.childCount; i++)
        {
            Image[] _imges = filedContainerRect.GetChild(i).gameObject.GetComponentsInChildren<Image>();
            for (int j = 0; j < _imges.Length; j++)
            {
                _imges[j].sprite = null;
                _imges[j].color = new Color(1, 1, 1, 0);
            }
        }
    }


    [ContextMenu("test rotate talk")]
    public void TestRect()
    {
        ChangeCurrentImageField(true);
        CameraRotate_And_ImageMove(true);
    }

    [ContextMenu("reset")]
    public void StatusReset()
    {
        cameraTransform.rotation = Quaternion.identity;
        ResetField();
    }
}
