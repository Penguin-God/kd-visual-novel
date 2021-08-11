using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoor : MonoBehaviour
{
    [SerializeField] string changeSceneName;
    [SerializeField] string locationName;

    public string GetChangeSceneName()
    {
        return changeSceneName;
    }

    public string GetLocationName()
    {
        return locationName;
    }
}
