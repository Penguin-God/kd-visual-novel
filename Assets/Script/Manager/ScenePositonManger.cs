using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Location
{
    public string name;
    public Transform tf_Spawn;
}

public class ScenePositonManger : MonoBehaviour
{
    [SerializeField] Location[] locations;
    Dictionary<string, Transform> dic_Location = new Dictionary<string, Transform>();
    public static bool spawn_able;
    void AddDic()
    {
        for(int i = 0; i < locations.Length; i++)
        {
            dic_Location.Add(locations[i].name, locations[i].tf_Spawn);
        }
    }

    private void Start() // Start에 달려있으니까 자동으로 스폰됨
    {
        AddDic();
        if (spawn_able)
        {
            SetPlayerSpawnTransform();
        }
    }

    void SetPlayerSpawnTransform()
    {
        SceneTrasnferManager theSceneMove = FindObjectOfType<SceneTrasnferManager>();
        string locationName = theSceneMove.GetLocationName();
        Transform spawnTransform = null;
        if (dic_Location.TryGetValue(locationName, out spawnTransform))
        {
            PlayerController.instance.transform.position = spawnTransform.position;
            PlayerController.instance.transform.rotation = spawnTransform.rotation;
            PlayerController.instance.AngleValueReset();
            PlayerController.instance.ResetCamera();

            spawn_able = false;
            theSceneMove.SceneChangeDone();
        }
        else Debug.LogWarning("찾을 수 없는 스폰 위치 : " + locationName);
    }
}
