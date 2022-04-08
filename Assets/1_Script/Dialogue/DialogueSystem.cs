using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
class DialogueTargetData
{
    public InteractionEvent interaction;
    public DialogueDataContainer[] dialogues;

    public DialogueTargetData(InteractionEvent _interaction, DialogueDataContainer[] _dialogues)
    {
        interaction = _interaction;
        dialogues = _dialogues;
    }
}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] SceneChannel sceneChannel;
    [SerializeField] List<DialogueTargetData> dialogueTargetDatas;

    private void Awake()
    {
        MySceneManager.Instance.OnDoneSceneSetup += (_view, _chars) =>
        {
            dialogueTargetDatas.Clear();
            foreach (var obj in _chars)
            {
                InteractionEvent _interaction = obj.GetComponent<InteractionEvent>();
                if (_interaction == null) return;

                dialogueTargetDatas.Add(new DialogueTargetData(_interaction, _interaction.DialogueMC.TestDialogues));
            };

            foreach (DialogueTargetData _dialogueTargetData in dialogueTargetDatas)
            {
                foreach (DialogueDataContainer _con in _dialogueTargetData.dialogues)
                    _con.Setup(_dialogueTargetData.interaction);
            }
        };
    }
}