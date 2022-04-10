using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [Header("Dialogue Data")]
    [SerializeField] string codeName;
    [SerializeField] string interactionName;
    public string InteractionName => interactionName;

    [SerializeField] DialogueObject dialogueObject = null;

    [SerializeField] DialogueDataContainer currentDialogue;
    public DialogueDataContainer CurrentDialogue => currentDialogue;
    
    public void Setup(string _codeName, string _interactionName, DialogueObject _dialogueObject)
    {
        codeName = _codeName;
        interactionName = _interactionName;
        dialogueObject = _dialogueObject;
        currentDialogue = _dialogueObject.Dialogues[0];
        DialogueSystem.Instance.OnSetup += (_dic) => _dic.Add(codeName, this);
    }

    [Header("Channel")]
    [SerializeField] protected DialogueChannel dialogueChannel = null;
    [SerializeField] protected SceneChannel sceneChannel = null;

    public bool Interactalbe => currentDialogue.Interactable;
    public void ChangeDialogue(DialogueDataContainer _newDialogue) => currentDialogue = _newDialogue;

    // 가상 함수
    public virtual void StartInteraction() 
    {
        SetDialogueEvent();
    }
    void SetDialogueEvent()
    {
        dialogueChannel.EndTalkEvent += SubscribeEvent;

        dialogueChannel.Raise_StartInteractionEvent(transform, currentDialogue);
    }

    // 여기 안에 있는 내용은 실행 후 바로 구취됨. 즉 1회용 이벤트
    void SubscribeEvent()
    {
        currentDialogue.Raise_OnDialogueEnd();

        dialogueChannel.EndTalkEvent -= SubscribeEvent;
    }

    
    private void Start()
    {
        if (!dialogueObject.IsSpawn)
        {
            DialogueSystem.Instance.OnSetup += (_dic) => _dic.Add(codeName, this);
            currentDialogue = dialogueObject.Dialogues[0];
        }
    }
}