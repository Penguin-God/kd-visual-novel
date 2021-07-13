using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private bool interactable = false;
    void Update()
    {
        CheckObject();
        Set_Corsshair(interactable);
    }

    private Camera cam;
    private Vector3 mousePosition;
    RaycastHit rayHit;

    void CheckObject()
    {
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        if (Physics.Raycast(cam.ScreenPointToRay(mousePosition), out rayHit, 100))
        {
            interactable = Set_ObjectInteractable(rayHit.transform);
        }
        else interactable = false;
    }
    bool Set_ObjectInteractable(Transform hitTransform)
    {
        bool interactable;
        if (hitTransform.CompareTag("Interaction")) interactable = true;
        else interactable = false;
        return interactable;
    }

    [SerializeField] GameObject normalCorsshair;
    [SerializeField] GameObject interactiveCorsshair;
    void Set_Corsshair(bool interactable)
    {
        interactiveCorsshair.SetActive(interactable);
        normalCorsshair.SetActive(!interactable);
    }
}
