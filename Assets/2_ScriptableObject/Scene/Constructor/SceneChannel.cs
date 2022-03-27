using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum SceneKey
{
    Start,
    Cafe,
}

[CreateAssetMenu(fileName = "new Scene Cannel", menuName = "Scriptable Object / Scenes / Scene Channel")]
public class SceneChannel : ScriptableObject
{
    public bool isLoadDefualtScene = false;
    private void OnDisable()
    {
        isLoadDefualtScene = false;
    }


    public Dictionary<SceneKey, string> sceneNameData = new Dictionary<SceneKey, string>();
    private void OnEnable()
    {
        sceneNameData.Clear();
        sceneNameData.Add(SceneKey.Start, "SampleScene");
        sceneNameData.Add(SceneKey.Cafe, "Cafeteria");
    }

    public void LoadScene(int _key)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadSceneAsync(sceneNameData[(SceneKey)_key], LoadSceneMode.Additive);
    }

    public event Action<bool> SceneLoadEvent = null;
    public void Raise_SceneLoadEvent(bool _isOnlyViewScene)
    {
        if (SceneLoadEvent != null) SceneLoadEvent.Invoke(_isOnlyViewScene);
    }

    public bool CurrentSceneIsOnlyView { get; private set; }
    public void SetSceneView(bool _isOnlyViewScene) => CurrentSceneIsOnlyView = _isOnlyViewScene;

    public GameObject[] CurrentSceneCharacters { get; private set; }
    public void SetSceneCharacters(GameObject[] _characters) => CurrentSceneCharacters = _characters;

    public event Action OnSceneLoadEnd = null;
    public void Raise_SceneLoadEndEvent()
    {
        OnSceneLoadEnd?.Invoke();
        OnSceneLoadEnd = null;
    }
}