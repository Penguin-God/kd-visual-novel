using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionType : MonoBehaviour
{
    public bool isDoor; // 객체가 문인지 판별
    public bool isObject; // 객체가 일반 객체인지 판별

    [SerializeField] string interactionName;

    public string GetName()
    {
        return interactionName;
    }
}
