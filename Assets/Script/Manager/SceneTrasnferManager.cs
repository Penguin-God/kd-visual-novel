using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrasnferManager : MonoBehaviour
{
    public void SceneTransfer(string sceneName, string locationName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
