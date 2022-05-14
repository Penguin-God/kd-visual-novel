using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

public class Save_Load_Utility : MonoBehaviour
{
    
    [ContextMenu("모든 세이브 파일 삭제"), Conditional("UNITY_EDITOR")]
    void AllSaveFileRemove()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            foreach (string filePath in Directory.GetFiles(Application.persistentDataPath))
            {
                File.Delete(filePath);
            }
        }
        else Debug.LogWarning("경로에 폴더가 존재하지 않음");
    }
}
