using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogueEvent;

    public Dialogue[] GetDialogues() // 객체가 가지는 dialogueEvent를 세팅 후 반환 2가지 일을 함
    {
        dialogueEvent.dialogues = new Dialogue[(int)dialogueEvent.line.y - (int)dialogueEvent.line.x + 1]; // dialogues의 크기 결정

        dialogueEvent.dialogues = DataBaseManager.instance.GetDialogues((int)dialogueEvent.line.x, (int)dialogueEvent.line.y); // dialogues 선언
        return dialogueEvent.dialogues; // 선언 후 반환
    }
}
