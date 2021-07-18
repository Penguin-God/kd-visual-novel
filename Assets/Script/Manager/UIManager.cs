using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] GameObject UI_Tooltip;
    [SerializeField] Image img_Interaction;
    [SerializeField] Image img_InteractionEffect;
    public void HideUI()
    {
        UI_Crosshair.SetActive(false);
        UI_Arrow.SetActive(false);
        UI_Tooltip.SetActive(false);

        img_Interaction.color = new Color(1, 1, 1, 0);
        img_InteractionEffect.color = new Color(1, 1, 1, 0);
    }

    public void ShowUI()
    {
        UI_Crosshair.SetActive(true);
        UI_Arrow.SetActive(true);
    }
}
