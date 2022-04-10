using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] DialogueChannel dialogueChannel = null;
    [SerializeField] SceneChannel sceneChannel = null;
    [SerializeField] SceneLoadDialogueProducer loadDialogueProducer;
    void Start()
    {
        dialogueChannel.StartInteractionEvent += (_target, _con) => HideUI();
        dialogueChannel.EndInteractionEvent += () => ShowUI(MySceneManager.Instance.CurrentSceneIsOnlyView);

        sceneChannel.OnOtherSceneLoad += (_data) => status.gameObject.SetActive(false);

        sceneChannel.OnSceneLoadComplete += (_data) => status.gameObject.SetActive(true);
        sceneChannel.OnSceneLoadComplete += (_data) => ShowUI(MySceneManager.Instance.CurrentSceneIsOnlyView);

        loadDialogueProducer.OnLoadDialogue += HideUI;
    }

    [SerializeField] GameObject status;
    [SerializeField] GameObject UI_Crosshair;
    [SerializeField] GameObject UI_Arrow;
    [SerializeField] GameObject UI_FiledArrow;
    [SerializeField] GameObject UI_Tooltip;
    [SerializeField] GameObject obj_Interaction;
    [SerializeField] Image img_Interaction;
    [SerializeField] Image img_InteractionEffect;
    void HideUI()
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

    void ShowUI(bool _isOnlyView)
    {
        UI_Crosshair.SetActive(true);
        if (_isOnlyView) UI_Arrow.SetActive(true);
        else UI_FiledArrow.SetActive(true);
        obj_Interaction.SetActive(!_isOnlyView);
    }
}