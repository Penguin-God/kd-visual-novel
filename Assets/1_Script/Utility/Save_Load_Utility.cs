using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save_Load_Utility : MonoBehaviour
{
    [ContextMenu("모든 세이브 파일 삭제")]
    void AllSaveFileRemove()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            foreach (string filePath in Directory.GetFiles(Application.persistentDataPath))
            {
                File.Delete(filePath);
            }
        }
    }
}
