using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// 다른 대화, 맵 이동 등 연출로 인하여 플레이가 불가능할때 false
    /// </summary>
    public bool IsPlayable => (!DialogueManager.instance.isTalking && !EventManager.isAutoEvent && !EventManager.isEvent &&
            !SceneTrasnferManager.isTransfer && !DialogueManager.instance.isCameraEffect);
}
