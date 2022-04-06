using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "new Dialogue Group", menuName = "Scriptable Object / Dialogue / Dialogue Group")]
public class DialogueGroup : ScriptableObject
{
    [SerializeField] InteractionEvent target;
    public InteractionEvent Target => target;

    [SerializeField] DialogueDataContainer[] dialogueDataContainers;
    public IReadOnlyList<DialogueDataContainer> DialogueDataContainers => dialogueDataContainers;

    public event Action OnDialogueCountChange;
    public void Setup()
    {
        foreach(var _con in dialogueDataContainers)
        {
            _con.OnDialogueCountChange += () => target.ChangeDialogue(_con);
            _con.Setup();
        }
    }
}