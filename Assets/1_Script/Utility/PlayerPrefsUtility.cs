using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsUtility : MonoBehaviour
{
    [ContextMenu("Player Prefs 모든 데이터 삭제")]
    void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
