using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ImageFieldMover : MonoBehaviour
{
    [SerializeField] DialogueChannel dialogueChannel = null;
    [SerializeField] GameObject MAIN_IMAGE_FIELD = null;
    [SerializeField] GameObject[] IMAGE_FIELDS = null;
    [SerializeField] float fieldDistance;
    private void Awake()
    {
        fieldDistance = Mathf.Abs(IMAGE_FIELDS[0].transform.position.x - IMAGE_FIELDS[1].transform.position.x);

        //dialogueChannel.ChangeContextEvent += FiledMove_ByTalk;
        dialogueChannel.EndInteractionEvent += ResetField;
    }

    [SerializeField] GameObject currentImageField = null;
    public GameObject CurrentImageField => currentImageField;

    [SerializeField] GameObject previousImageField = null;
    public GameObject PerviousImageField => previousImageField;

    public void FiledMove_ByTalk(DialogueData _data, int _index)
    {
        string _dirSymbol = _data.cameraRotateDir[_index].Trim();
        if (_dirSymbol != "" && (_dirSymbol == "+" || _dirSymbol == "-"))
        {
            bool _cameraRotateDirIsRight = (_dirSymbol == "+") ? true : false;
            ChangeCurrentField(_cameraRotateDirIsRight);
            FieldMove(_cameraRotateDirIsRight);
        }
    }

    
    int CurrentImageFieldIndex => Array.IndexOf(IMAGE_FIELDS, currentImageField);
    public void ChangeCurrentField(bool _cameraRotateDirIsRight)
    {
        previousImageField = currentImageField;
        currentImageField = (_cameraRotateDirIsRight) ? IMAGE_FIELDS[CurrentImageFieldIndex + 1] : IMAGE_FIELDS[CurrentImageFieldIndex - 1];
    }

    [SerializeField] RectTransform filedContainerRect = null;
    void FieldMove(bool _cameraRotateDirIsRight)
    {
        fieldDistance *= (_cameraRotateDirIsRight) ? -1 : 1;
        Vector3 _targetPos = new Vector3(filedContainerRect.position.x + fieldDistance, filedContainerRect.position.y, filedContainerRect.position.z);
        StartCoroutine(Co_FieldMove(_targetPos));
    }

    public Vector3 GetTargetPos(bool _cameraRotateDirIsRight)
    {
        fieldDistance *= (_cameraRotateDirIsRight) ? -1 : 1;
        Vector3 _targetPos = new Vector3(filedContainerRect.position.x + fieldDistance, filedContainerRect.position.y, filedContainerRect.position.z);
        fieldDistance = Math.Abs(fieldDistance);
        return _targetPos;
    }

    [SerializeField] float moveSpeed;
    IEnumerator Co_FieldMove(Vector3 _targetPos)
    {
        while (Vector3.Distance(_targetPos, filedContainerRect.position) > 2)
        {
            filedContainerRect.position = Vector3.Lerp(filedContainerRect.position, _targetPos, moveSpeed);
            yield return new WaitForSeconds(0.03f);
        }
        fieldDistance = Math.Abs(fieldDistance);
    }

    [SerializeField] Vector3 originPos;
    void ResetField()
    {
        fieldDistance = Math.Abs(fieldDistance);
        filedContainerRect.localPosition = originPos;
        currentImageField = MAIN_IMAGE_FIELD;
        previousImageField = null;
    }
}
