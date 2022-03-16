using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "new Scene Cannel", menuName = "Scriptable Object / Scene Cannel")]
public class SceneCannel : ScriptableObject
{
    public void TestLoad()
    {
        SceneManager.LoadScene("Cafeteria");
    }
}
