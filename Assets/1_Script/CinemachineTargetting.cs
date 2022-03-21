using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CinemachineTargetting : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    [SerializeField] Transform[] targets;

    IEnumerator Co_Targetting()
    {
        for(int i = 0; i < targets.Length; i++)
        {
            virtualCamera.Follow = targets[i];
            virtualCamera.LookAt = targets[i];
            yield return new WaitForSeconds(2f);
        }
    }

    private void Start()
    {
        StartCoroutine(Co_Targetting());
    }
}
