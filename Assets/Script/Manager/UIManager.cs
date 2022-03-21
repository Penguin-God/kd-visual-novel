using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] DialogueChannel dialogueChannel = null;
    [SerializeField] SceneChannel sceneChannel = null;
    void Start()
    {
        ShowUI();
        dialogueChannel.StartTalkEvent += (DialogueDataContainer _con) => HideUI();
        dialogueChannel.EndInteractionEvent += ShowUI;
    }

    [SerializeField] GameObject UI_Crosshair;
    [SerializeField] GameObject UI_Arrow;
    [SerializeField] GameObject UI_FiledArrow;
    [SerializeField] GameObject UI_Tooltip;
    [SerializeField] GameObject obj_Interaction;
    [SerializeField] Image img_Interaction;
    [SerializeField] Image img_InteractionEffect;
    public void HideUI()
    {
        UI_Crosshair.SetActive(false);
        UI_Arrow.SetActive(false);
        UI_Tooltip.SetActive(false);
        UI_FiledArrow.SetActive(false);

        HideInteractionImage();
    }

    public void HideInteractionImage()
    {
        img_Interaction.color = new Color(1, 1, 1, 0);
        img_InteractionEffect.color = new Color(1, 1, 1, 0);
        obj_Interaction.SetActive(false);
    }

    public void ShowUI()
    {
        UI_Crosshair.SetActive(true);
        if (sceneChannel.CurrentSceneIsOnlyView) UI_Arrow.SetActive(true);
        else UI_FiledArrow.SetActive(true);
        obj_Interaction.SetActive(!sceneChannel.CurrentSceneIsOnlyView);
    }
}