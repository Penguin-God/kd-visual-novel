using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    [SerializeField] Vector3 spinDir;

    void Update()
    {
        transform.Rotate(spinDir * spinSpeed * Time.deltaTime);
    }
}
