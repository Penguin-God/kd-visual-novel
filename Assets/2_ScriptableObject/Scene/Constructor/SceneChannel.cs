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
    [SerializeField] bool isInteraction_With_SceneLoadTrigger;
    public bool IsInteraction_With_SceneLoadTrigger => isInteraction_With_SceneLoadTrigger;

    private void OnDisable()
    {
        OnOtherSceneLoad = null;
        OnSceneLoadComplete = null;

        isInteraction_With_SceneLoadTrigger = false;
    }

    #region events
    public event Action OnInteraction_With_SceneLoadTrigger = null;
    public void Raise_OnInteraction_With_SceneLoadTrigger()
    {
        isInteraction_With_SceneLoadTrigger = true;
        OnInteraction_With_SceneLoadTrigger?.Invoke();
    }

    public event Action<SceneManagerISo> OnOtherSceneLoad = null;
    public void Raise_OnOtherSceneLoad(SceneManagerISo _sceneData) // 유니티 이벤트 매서드로 등록해서 많이 사용함
    {
        isInteraction_With_SceneLoadTrigger = true;
        OnOtherSceneLoad?.Invoke(_sceneData);
    }

    public event Action<SceneManagerISo> OnEnterOtherScene = null;
    public void Raise_OnEnterOtherScene(SceneManagerISo _sceneManagerISo)
    {
        isInteraction_With_SceneLoadTrigger = false;
        OnEnterOtherScene?.Invoke(_sceneManagerISo);
    }

    public event Action<SceneManagerISo> OnSceneLoadComplete = null;
    public void Raise_OnSceneLoadComplete(SceneManagerISo _sceneManagerISo) => OnSceneLoadComplete?.Invoke(_sceneManagerISo);
    #endregion
}