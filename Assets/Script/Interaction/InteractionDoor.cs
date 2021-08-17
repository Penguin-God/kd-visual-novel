using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoor : MonoBehaviour
{
    [SerializeField] bool isNextMapOnlyView;
    [SerializeField] string changeSceneName;
    [SerializeField] string locationName;

    public string GetChangeSceneName()
    {
        CameraController.isOnlyView = isNextMapOnlyView;
        return changeSceneName;
    }

    public string GetLocationName()
    {
        return locationName;
    }
}
