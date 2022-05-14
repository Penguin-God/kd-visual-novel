using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInteractoinObject : InteractionObject
{
    protected override void Init()
    {
        base.Init();
        sceneChannel.OnSceneLoadComplete += Setup;
    }

    protected override void Clear()
    {
        base.Clear();
        sceneChannel.OnSceneLoadComplete -= Setup;
    }

    void Setup(SceneManagerISo _data)
    {
        codeName = dialogueObject.CodeName;
        Setup(DialogueSystem.Instance.FindDialogueObject_With_CodeName(codeName));
    }
}
