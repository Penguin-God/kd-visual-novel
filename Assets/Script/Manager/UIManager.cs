using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Debug.LogError("UI 싱글턴 2개");
            Destroy(gameObject);
        }
    }

    [SerializeField] GameObject UI_Crosshair;
    [SerializeField] GameObject UI_Arrow;

    public void HideUI()
    {
        UI_Crosshair.SetActive(false);
        UI_Arrow.SetActive(false);
    }
}
