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

    [SerializeField] bool isSceneLoadingEffect; // 화면 연출하는 동안도 포함
    public bool IsSceneLoadingEffect => isSceneLoadingEffect; 

    AsyncOperation async;
    public bool IsSceneLoading // 씬 로딩하는 시간만 포함
    {
        get
        {
            if (async == null) return false;
            return !async.isDone;
        }
    }


    [SerializeField] bool isInteraction_With_SceneLoadTrigger;
    public bool IsInteraction_With_SceneLoadTrigger => isInteraction_With_SceneLoadTrigger;

    private void OnDisable()
    {
        OnOtherSceneLoad = null;
        //OnCutScene = null;
        OnSceneLoadComplete = null;
        OnSceneFadeIn = null;

        isLoadDefualtScene = false;
        isSceneLoadingEffect = true; // 게임을 어떻게 시작하든지 씬을 로딩하기 때문에 기본 상태가 true
        isInteraction_With_SceneLoadTrigger = false; 
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
        async = SceneManager.LoadSceneAsync(sceneNameData[(SceneKey)_key], LoadSceneMode.Additive);
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
    public event Action OnInteraction_With_SceneLoadTrigger = null;
    public void Raise_OnInteraction_With_SceneLoadTrigger()
    {
        isInteraction_With_SceneLoadTrigger = true;
        OnInteraction_With_SceneLoadTrigger?.Invoke();
    }

    public event Action<int> OnOtherSceneLoad = null;
    public void Raise_OnOtherSceneLoad(int _sceneNumber) // 유니티 이벤트 매서드로 등록해서 많이 사용함
    {
        isSceneLoadingEffect = true;
        OnOtherSceneLoad?.Invoke(_sceneNumber);
    }

    //public Action OnCutScene = null;
    //public void SetCutScene(DialogueDataContainer _data) => OnCutScene = () => dialogueChannel.Raise_StartInteractionEvent(null, _data);

    public event Action OnSceneLoadComplete = null;
    public void Raise_OnSceneLoadComplete()
    {
        OnSceneLoadComplete?.Invoke(); 
        isInteraction_With_SceneLoadTrigger = false;
    }

    public event Action OnSceneFadeIn = null;
    public void Raise_OnSceneFadeIn()
    {
        isSceneLoadingEffect = false;
        OnSceneFadeIn?.Invoke();
    }
    #endregion
}