using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new DialogueData Container", menuName = "Scriptable Object / Dialogue Container")]
public class DialogueDataContainer : ScriptableObject
{
    [Header("data")]
    [SerializeField] string codeName;
    public string CodeName => codeName;

    [SerializeField] DialogueDataParser dataParser = null;
    
    [SerializeField] DialogueData[] dialogueData = null;
    public DialogueData[] DialogueData => dialogueData;

    public void Init(DialogueDataParser _dataParser, string _eventName)
    {
        dataParser = _dataParser;
        codeName = _eventName;
    }

    void OnEnable()
    {
        if (dataParser == null) return;

        dialogueData = dataParser.GetDialogue(codeName);
    }

    void OnDisable()
    {
        interactable = true;
        OnFirstInteraction = null;
    }


    [SerializeField] bool interactable;
    public bool Interactable => interactable;
    // 인스펙터에서 사용할 함수
    public void SetInteractable(bool _interactable) => interactable = _interactable;


    [Header("Condition")]
    [SerializeField] DialogueCondition dialogueCondition = null;
    public DialogueCondition DialogueCondition => dialogueCondition;

    #region events

    public event Action OnFirstInteraction;
    public void Raise_OnDialogueStart()
    {
        OnFirstInteraction?.Invoke();
        OnFirstInteraction = null;
    }

    [Header("Unity Event"), Space]
    [SerializeField] UnityEvent OnDialogueEnd;
    public void Raise_OnDialogueEnd() => OnDialogueEnd?.Invoke();

    #endregion

    // 게임 오브젝트 내에 들어가면 실행되는 Start (언젠가는 만들겠지?)
    public void Start()
    {
        
    }

    // 씬 세팅시 동작하는 셋업
    public void Setup(DialogueObject _dialogueObject)
    {
        dialogueCondition.Setup(_dialogueObject, this);
    }

    public DialogueDataContainer GetClone()
    {
        var _newContainer = Instantiate(this);
        _newContainer.SetClone += _newContainer.DialogueCondition.ConditionChangeAsClone;
        return _newContainer;
    }

    // 나중에 다른 변수들도 바꿔야 할 수도 있으니까 확장성 고려해서 Action으로 만듬
    public Action<List<DialogueDataContainer>> SetClone = null;
}


public enum CameraType
{
    Default,
    FadeIn,
    FadeOut,
    FlashIn,
    FlashOut,
    ShowCutScene,
    HideCutScene,
    AppearSlide,
    DisappearSlide,
    ChangeSlide,
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