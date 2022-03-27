using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnInteractionObjectData : ScriptableObject
{
    [SerializeField] protected DialogueMC dialogueMC = null;

    [SerializeField] protected string interactionName;
    [SerializeField] protected Vector3 spawnPos;
    [SerializeField] protected Vector3 spawnEulerAngles;

    /// <summary>
    /// 오브젝트를 생성한 후 생성된 오브젝트를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public abstract GameObject GetInteractionObject();
}