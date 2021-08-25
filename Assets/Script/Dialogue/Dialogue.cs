using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

[System.Serializable] // 데이터를 직렬로 하겠다는 뜻 인스펙터 창에서 수정하기 위해 필요
public class Dialogue // Dialogue 하나는 엑셀 파일 1줄을 의미
{
    [Tooltip("카메라가 타겟팅할 대상")]
    public Transform tf_Target;
    public CameraType cameraType;

    public string name;
    public string[] contexts;

    [HideInInspector]
    public string[] spriteNames;
    [HideInInspector]
    public string[] voiceNames;
    //[HideInInspector]
    public string[] cutSceneName;
}

// InteractionEvent에서 DialogueEvent를 [SerializeField]를 통해 인스펙터에 나타내고 있는데 [System.Serializable]가 없으면 인스펙터 창에 나타나지 않음
[System.Serializable]
public class DialogueEvent
{
    public string eventName; // 개발자 편의를 위한 변수

    public int eventNumber; // 컴포넌트를 가지고 있는 오브젝트가 진행하는 이벤트 넘버 (진행하면 DataBaseManager의 eventFlags의 eventNumber번째가 true가 됨)
    public int[] eventConditions; // 등장 조건에 포함되는 이벤트 넘버 (무조건 등장시키려면 0번째 event를 지정하고 conditionFlag을 false로 하면됨)
    public bool conditionFlag; // 이벤트를 보는게 등장 조건인지 보지 않은게 등장 조건인지 결정하는 변수 
    public int[] endNumbers; // 퇴장 이벤트 넘버

    public Vector2 line; // x는 시작 줄 y는 끝나는 줄
    public Dialogue[] dialogues; // 대사가 한번 치고 끝나지 않기 때문에 위에 Dialogue클래스 배열 변수 선언

    // 대화 한번 한 후에 다음 대화
    public Vector2 afterLine;
}
