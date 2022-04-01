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
    [SerializeField] DialogueChannel dialogueChannel = null;
    public bool isLoadDefualtScene = false;

    AsyncOperation CurrentSceneAscy = null;
    public bool IsSceneLoading
    {
        get
        {
            if (CurrentSceneAscy == null) return false;
            else return !CurrentSceneAscy.isDone;
        }
    }

    [SerializeField] bool isSceneLoading;
    private void OnDisable()
    {
        CurrentSceneAscy = null;
        isLoadDefualtScene = false;
        isSceneLoading = false;
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
        CurrentSceneCharacters = null;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        CurrentSceneAscy = SceneManager.LoadSceneAsync(sceneNameData[(SceneKey)_key], LoadSceneMode.Additive);
    }

    public bool CurrentSceneIsOnlyView { get; private set; }
    public void SetSceneView(bool _isOnlyViewScene) => CurrentSceneIsOnlyView = _isOnlyViewScene;

    [SerializeField] GameObject[] debugCharacters = null;
    public GameObject[] CurrentSceneCharacters { get; private set; }
    public void SetSceneCharacters(GameObject[] _characters)
    {
        CurrentSceneCharacters = _characters;
        debugCharacters = _characters;
    }

    #region events
    public Action OnCutScene = null;
    public void SetCutScene(DialogueDataContainer _data) => OnCutScene = () => dialogueChannel.Raise_StartInteractionEvent(null, _data);

    public event Action<bool> OnSceneLoadComplete = null;
    public void Raise_OnSceneLoadComplete(bool _isCameraOnlyView) => OnSceneLoadComplete?.Invoke(_isCameraOnlyView);

    public event Action<int> OnTryOtherSceneLoad = null;
    public void Raise_OnTryOtherSceneLoad(int _sceneNumber) => OnTryOtherSceneLoad?.Invoke(_sceneNumber);
    #endregion
}