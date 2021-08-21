using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    None,
    AppearCharacter,
    DisappearCharacter,
}

public class EventByTalk : MonoBehaviour
{
    public bool disposableEvent;
    public EventType eventType;
    public GameObject[] eventCharacters;
}
