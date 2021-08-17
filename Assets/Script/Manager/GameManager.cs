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
    /// �ٸ� ��ȭ, �� �̵� �� ����� ���Ͽ� �÷��̰� �Ұ����Ҷ� false
    /// </summary>
    public bool IsPlayable
    {
        get
        {
            if (DialogueManager.instance.isTalking || EventManager.isAutoEvent || EventManager.isEvent || SceneTrasnferManager.isTransfer)
            {
                return false;
            }
            else return true;
        }
    }
}
