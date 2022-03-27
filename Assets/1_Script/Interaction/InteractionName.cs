using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionName : MonoBehaviour
{
    [SerializeField] string interactionName;

    public string GetName()
    {
        return interactionName;
    }

    public void SetName(string _newName)
    {
        interactionName = _newName;
    }
}
