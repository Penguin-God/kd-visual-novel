using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoor : MonoBehaviour
{
    [SerializeField] bool isNextMapOnlyView;
    [SerializeField] string changeSceneName;
    [SerializeField] string locationName;

    public bool GetMapView()
    {
        return isNextMapOnlyView;
    }

    public string GetChangeSceneName()
    {
        return changeSceneName;
    }

    public string GetLocationName()
    {
        return locationName;
    }
}
