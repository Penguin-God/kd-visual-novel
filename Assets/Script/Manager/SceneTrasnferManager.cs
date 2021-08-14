using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrasnferManager : MonoBehaviour
{
    [SerializeField] string locationName;

    public void SceneTransfer(string sceneName, string p_locationName)
    {
        locationName = p_locationName;
        ScenePositonManger.spawn_able = true;
        SceneManager.LoadScene(sceneName);
    }

    public string GetLocationName()
    {
        return locationName;
    }
}
