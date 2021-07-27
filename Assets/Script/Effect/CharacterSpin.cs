using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpin : MonoBehaviour
{
    [SerializeField] Transform tf_Target;
    Quaternion lookAngle;
    Vector3 lookEuler;
    void Update()
    {
        lookAngle = Quaternion.LookRotation(tf_Target.position);
        lookEuler = Vector3.up * lookAngle.eulerAngles.y;
        transform.eulerAngles = lookEuler;
    }
}
