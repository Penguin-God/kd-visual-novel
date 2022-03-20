using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "new Scene Cannel", menuName = "Scriptable Object / Scene Channel")]
public class SceneChannel : ScriptableObject
{
    public void LoadCafeScene()
    {
        SceneManager.LoadScene("Cafeteria");
    }

    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}