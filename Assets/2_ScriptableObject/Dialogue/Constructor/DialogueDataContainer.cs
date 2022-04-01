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
    [SerializeField] string eventName;
    [SerializeField] DialogueDataParser dataParser = null;
    
    [SerializeField] DialogueData[] dialogueData = null;
    public DialogueData[] DialogueData => dialogueData;

    public void Init(DialogueDataParser _dataParser, string _eventName)
    {
        dataParser = _dataParser;
        eventName = _eventName;
    }

    //public bool Interactionable => eventCondition.GetCondition();
    void OnEnable()
    {
        if (dataParser == null) return;

        //Debug.Log(eventName);
        dialogueData = dataParser.GetDialogue(eventName);
    }

    void OnDisable()
    {
        interactable = false;
        // 인스펙터에서 설정한거 빼고 다 없애는 기능임
        ContainerDialogueEndEvent.RemoveAllListeners();
    }

    [SerializeField] bool interactable = false;
    public bool Interactable => interactable;
    public void SetInteraction() => interactable = true;


    [Header("Unity Event")]  [Space]
    [SerializeField] UnityEvent OnDialogueEnd;
    public void Raise_OnDialogueEnd() => OnDialogueEnd?.Invoke();

    public UnityEvent ContainerDialogueEndEvent;
    public void Raise_ContainerDialogueEndEvent() => ContainerDialogueEndEvent?.Invoke();

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