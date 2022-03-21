using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[CreateAssetMenu(fileName = "new Scene Cannel", menuName = "Scriptable Object / Scene Channel")]
public class SceneChannel : ScriptableObject
{
    public event Action<bool> SceneLoadEvent = null;
    public void Raise_SceneLoadEvent(bool _isOnlyViewScene)
    {
        if (SceneLoadEvent != null) SceneLoadEvent.Invoke(_isOnlyViewScene);
    }

    public bool CurrentSceneIsOnlyView { get; private set; }
    public void SetSceneView(bool _isOnlyViewScene) => CurrentSceneIsOnlyView = _isOnlyViewScene;

    public void LoadCafeScene()
    {
        SceneManager.LoadScene("Cafeteria");
    }

    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}