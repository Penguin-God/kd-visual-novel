using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static bool isEvent = false;
    public static bool isAutoEvent = false;

    public void GameEventByTalkEnd(EventByTalk eventByTalk)
    {
        if (eventByTalk == null || eventByTalk.eventType == EventType.None) return;

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
