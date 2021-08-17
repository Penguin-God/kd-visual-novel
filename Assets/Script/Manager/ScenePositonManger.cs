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
    public static bool spawn_able = false;
    void AddDic()
    {
        for(int i = 0; i < locations.Length; i++)
        {
            dic_Location.Add(locations[i].name, locations[i].tf_Spawn);
        }
    }

    private void Start() // Start�� �޷������ϱ� �ڵ����� ������
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
        string locationName = theSceneMove.GetLocationName(); // ��� �̸� ��������
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
        else Debug.LogWarning("ã�� �� ���� ���� ��ġ : " + locationName);
    }
}
