using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SceneKey
{
    Start,
    Cafe,
}

[CreateAssetMenu(fileName = "new Scene Cannel", menuName = "Scriptable Object / Scenes / Scene Channel")]
public class SceneChannel : ScriptableObject
{
    public void LoadOtherScene(SceneManagerISo _sceneManagerISo) => MySceneManager.Instance.Raise_OnOtherSceneLoad(_sceneManagerISo);
}