using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Events;


public enum DialogueTriggerType
{
    PlayerInteraction, // 일반적인 상호작용
    GameProduction, // 게임 내에서 컷씬같은 느낌으로 실행
}

[CreateAssetMenu(fileName = "new DialogueData Container", menuName = "Scriptable Object / Dialogue Container")]
public class DialogueDataContainer : ScriptableObject
{
    [Header("data")]
    [SerializeField] string codeName;
    public string CodeName => codeName;

    [SerializeField] DialogueDataParser dataParser = null;
    
    [SerializeField] DialogueData[] dialogueData = null;
    public DialogueData[] DialogueData => dialogueData;

    #region interact
    [SerializeField] bool interactable = false;
    public bool Interactable => interactable;
    public void SetInteraction() => interactable = true;
    #endregion

    public void Init(DialogueDataParser _dataParser, string _eventName)
    {
        dataParser = _dataParser;
        codeName = _eventName;
    }

    void OnEnable()
    {
        if (dataParser == null) return;

        codeName = name;
        dialogueData = dataParser.GetDialogue(codeName);
    }

    void OnDisable()
    {
        interactable = false;
        OnFirstDialogueEnd = null;
    }

    [Header("Condition")]
    [SerializeField] DialogueCondition dialogueCondition = null;
    public DialogueCondition DialogueCondition => dialogueCondition;

    [SerializeField] bool isSaw;
    #region events

    [Header("Unity Event"), Space]
    [SerializeField] UnityEvent OnDialogueEnd;
    public void Raise_OnDialogueEnd() => OnDialogueEnd?.Invoke();

    public UnityEvent ContainerDialogueEndEvent;

    public event Action OnFirstDialogueEnd;
    public void Raise_ContainerDialogueEndEvent()
    {
        if (!isSaw)
        {
            OnFirstDialogueEnd?.Invoke();
            isSaw = true;
        }
    }

    #endregion

    public event Action OnDialogueCountChange;
    public void Setup()
    {
        dialogueCondition.Setup();
        dialogueCondition.OnConditionCountChange += () => OnDialogueCountChange();
    }

    public void DataReset()
    {
        interactable = false;
    }
}


[Serializable]
public class DialogueData
{
    [Tooltip("카메라가 타겟팅할 대상")]
    public CameraType cameraType;

    public string characterName;
    public string[] contexts;

    //public bool IsTalkEffect => (spriteNames.Length > 0 || voiceNames.Length > 0 || cutSceneName.Length > 0 || cameraTorque.Length > 0);
    public string[] spriteNames;
    public string[] voiceNames;
    public string[] cutSceneName;
    public string[] cameraRotateDir;
}