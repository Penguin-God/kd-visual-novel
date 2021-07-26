using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 데이터를 직렬로 하겠다는 뜻 인스펙터 창에서 수정하기 위해 필요
public class Dialogue // Dialogue 하나는 엑셀 파일 1줄을 의미
{
    [Tooltip("카메라가 타겟팅할 대상")]
    public Transform tf_Target;
    
    public string name;
    public string[] contexts;

    //[HideInInspector]
    public string[] spriteNames;
}

// InteractionEvent에서 DialogueEvent를 [SerializeField]를 통해 인스펙터에 나타내고 있는데 [System.Serializable]가 없으면 인스펙터 창에 나타나지 않음
[System.Serializable]
public class DialogueEvent
{
    public string eventName; // 개발자 편의를 위한 변수

    public Vector2 line; // x는 시작 줄 y는 끝나는 줄
    public Dialogue[] dialogues; // 대사가 한번 치고 끝나지 않기 때문에 위에 Dialogue클래스 배열 변수 선언
}
