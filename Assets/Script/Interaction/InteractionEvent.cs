using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogueEvent;
    Transform currentTarget = null;
    public Dialogue[] GetDialogues() // 객체가 가지는 dialogueEvent를 세팅 후 반환 2가지 일을 함
    {
        dialogueEvent.dialogues = new Dialogue[(int)dialogueEvent.line.y - (int)dialogueEvent.line.x + 1]; // dialogues의 크기 결정

        dialogueEvent.dialogues = DataBaseManager.instance.GetDialogues((int)dialogueEvent.line.x, (int)dialogueEvent.line.y); // dialogues 선언
        for(int i = 0; i < dialogueEvent.dialogues.Length; i++) // tf_Target 대입
        {
            Dialogue dialogue = dialogueEvent.dialogues[i];

            // 이름 앞에 ⒳가 붙어 있으면 타겟팅 안하는거임 
            if (dialogue.name[0] != '⒳') dialogue.tf_Target = CharacterManager.instance.dic_Character[dialogue.name];
            // target은 현재 대화 상대를 의미 주인공이 말하거나 독백할때도 target에는 대화상대가 들어감
            if (dialogue.tf_Target == null) dialogue.tf_Target = currentTarget;
            else currentTarget = CharacterManager.instance.dic_Character[dialogue.name];

            dialogueEvent.dialogues[i] = dialogue;
        }
        return dialogueEvent.dialogues; // 선언 후 반환
    }
}
