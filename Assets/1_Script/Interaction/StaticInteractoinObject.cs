using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInteractoinObject : InteractionObject
{
    protected override void Init()
    {
        base.Init();
        MySceneManager.Instance.OnSceneLoadComplete += Setup;
    }

    protected override void Clear()
    {
        base.Clear();
        if(MySceneManager.Instance != null)
        {
            MySceneManager.Instance.OnSceneLoadComplete -= Setup;
        }
    }

    void Setup(SceneManagerISo _data)
    {
        codeName = dialogueObject.CodeName;
        DialogueObject _clone = DialogueSystem.Instance.FindDialogueObject_With_CodeName(codeName);
        if (_clone != null) Setup(_clone);
    }
}
