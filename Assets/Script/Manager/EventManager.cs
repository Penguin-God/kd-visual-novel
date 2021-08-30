using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventFlagsDictionary : SerializableDictionary<string, bool> {}

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public static bool isEvent = false;
    public static bool isAutoEvent = false;
    public EventFlagsDictionary eventFlags; // 0��°�� �ǵ帮�� ����

    private void Awake()
    {
        if (instance == null) instance = this;
        eventFlags = new EventFlagsDictionary();


    }

    public void GameEventByTalkEnd(EventByTalk eventByTalk)
    {
        if (eventByTalk == null || eventByTalk.eventType == EventType.None || eventByTalk.showEvent) return;
        eventByTalk.showEvent = true;
        AppearOrDisappearCharacter(eventByTalk.eventType, eventByTalk.eventCharacters);
    }

    void AppearOrDisappearCharacter(EventType eventType, GameObject[] characters)
    {
        if (eventType == EventType.AppearCharacter) SetCharacters(true, characters);
        else if (eventType == EventType.DisappearCharacter) SetCharacters(false, characters);

    }

    void SetCharacters(bool isAppear, GameObject[] eventCharacters)
    {
        for (int i = 0; i < eventCharacters.Length; i++)
        {
            eventCharacters[i].GetComponent<CharacterSpin>().CharactersSpinEvent(isAppear);
        }
    }
}