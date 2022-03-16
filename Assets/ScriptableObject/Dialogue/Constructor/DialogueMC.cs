using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Dialogue MC", menuName = "Scriptable Object / Dialogue MC")]
public class DialogueMC : ScriptableObject
{
    [SerializeField] DialogueDataContainer currentDialogue = null;
    public DialogueDataContainer CurrentDialogue => currentDialogue;

    [SerializeField] DialogueDataContainer[] dialogues = null;

    void ChangeCurrentDialogue(DialogueDataContainer _newDialogue)
    {
        if (_newDialogue != null && currentDialogue != _newDialogue) currentDialogue = _newDialogue;
    }

    void OnEnable()
    {
        for (int i = 0; i < dialogues.Length; i++)
        {
            DialogueDataContainer _afterContainer = dialogues[i].AfterDialogue;
            dialogues[i].ChangeDialogueEvent += () => ChangeCurrentDialogue(_afterContainer);
        }
    }
}