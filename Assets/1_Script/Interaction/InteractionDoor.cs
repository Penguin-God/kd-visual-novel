using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoor : InteractionEvent
{
    [SerializeField] SceneChannel SceneCannel;

    //[SerializeField] bool isNextMapOnlyView;
    //[SerializeField] string changeSceneName;
    //[SerializeField] string locationName;

    //public bool GetMapView()
    //{
    //    return isNextMapOnlyView;
    //}

    //public string GetChangeSceneName()
    //{
    //    return changeSceneName;
    //}

    //public string GetLocationName()
    //{
    //    return locationName;
    //}

    //public void SceneTransfer()
    //{
    //    CameraController.isOnlyView = GetMapView();
    //    string sceneName = GetChangeSceneName();
    //    string locationName = GetLocationName();
    //    FindObjectOfType<SceneTrasnferManager>().SceneTransfer(sceneName, locationName);
    //}

    public override void StartInteraction()
    {
        base.StartInteraction();
        //if (IsTransfer) SceneTransfer();
        //else DialogueManager.instance.StartTalk(GetDialogues());
    }
}
