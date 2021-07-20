using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterDictionary : SerializableDictionary<string, Transform>
{
    
}

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    private void Awake()
    {
        if(instance == null) instance = this;
        else
        {
            Debug.LogError("캐릭터 매니저 싱글턴 2개");
            Destroy(gameObject);
        }
    }
    public CharacterDictionary dic_Character;
}
