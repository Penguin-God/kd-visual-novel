using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DialogueTriggerType
{
    PlayerInteraction, // 일반적인 상호작용
    GameProduction, // 게임 내에서 컷씬같은 느낌으로 실행
}

[CreateAssetMenu(fileName = "new Dialogue", menuName = "Scriptable Object / Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [SerializeField] DialogueFileData fileData = null;
    // 컴포넌트를 가지고 있는 오브젝트가 진행하는 이벤트 이름 (진행하면 EventManager의 eventFlags의 eventName에 맞는 value가 true가 됨)
    [SerializeField] string eventName;
    public string EventName => eventName;

    [Header("이벤트 조건 관련 변수")]
    [SerializeField] EventCondition eventCondition;
    public EventCondition EventCondition => eventCondition;

    [Header("대화 관련 변수")]
    [Space][Space][Space]
    [SerializeField] LineData[] liens;
    public LineData[] Dialogues => liens;

    [Space]
    public bool isAfter;
    public LineData[] afterDialogues;

    void OnEnable()
    {
        liens = fileData.GetDialogue(eventName);
    }
}


[Serializable]
public class LineData
{
    [Tooltip("카메라가 타겟팅할 대상")]
    public Transform tf_Target;
    public CameraType cameraType;

    public string characterName;
    public string[] contexts;

    [HideInInspector]
    public string[] spriteNames;
    [HideInInspector]
    public string[] voiceNames;
    //[HideInInspector]
    public string[] cutSceneName;
}

[Serializable]
public class EventCondition
{
    public EventCondition(string[] _conditions, bool _flag, string _endName)
    {
        eventConditions = _conditions; conditionFlag = _flag; endEvnetName = _endName;
    }

    public string[] eventConditions; // 등장 조건에 포함되는 이벤트 넘버 (무조건 등장시키려면 0번째 event를 지정하고 conditionFlag을 false로 하면됨)
    public bool conditionFlag; // 이벤트를 보는게 등장 조건인지 보지 않은게 등장 조건인지 결정하는 변수 
    public string endEvnetName; // 퇴장 이벤트 넘버
}