using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    //private bool interactable = false;
    void Update()
    {
        if (!GameManager.instance.IsPlayable) return;

        ObjectInteraction();
        ClickLeftButton();
    }

    [SerializeField] GameObject obj_Qestion;
    private Transform interactTransform = null;
    void ClickLeftButton()
    {
        QuestionEffect questionEffect = obj_Qestion.GetComponent<QuestionEffect>();

        if (Input.GetMouseButtonDown(0) && !DialogueManager.instance.isTalking && InteractionAble && !questionEffect.isThrow && rayHit.transform != null)
        {
            interactTransform = rayHit.transform;
            obj_Qestion.SetActive(true);
            obj_Qestion.transform.position = cam.transform.position;
            questionEffect.Throw_QuestionMark(interactTransform.position);
            StartCoroutine(Co_Interaction(interactTransform));
        }
    }

    IEnumerator Co_Interaction(Transform interactTransform)
    {
        QuestionEffect questionEffect = obj_Qestion.GetComponent<QuestionEffect>();
        yield return new WaitUntil(() => questionEffect.isQuestionHit);
        questionEffect.isQuestionHit = false;

        InteractionType interactionType = interactTransform.GetComponent<InteractionType>();
        if (interactionType.isObject) CallDialogue();
        else if (interactionType.isDoor) CallTransfer();

        if (interactTransform.GetComponent<InteractionEvent>() != null)
            EventManager.instance.eventFlags[interactTransform.GetComponent<InteractionEvent>().CurrentEventName] = true;
    }

    void CallDialogue() // 이 부분을 InteractionEvent에서 구현
    {
        DialogueManager.instance.SetEvent(interactTransform);
        DialogueManager.instance.StartTalk(interactTransform.GetComponent<InteractionEvent>().GetDialogues());
    }

    void CallTransfer()
    {
        // 코드 바꾸기 
        if(interactTransform.GetComponent<InteractionEvent>() != null && interactTransform.GetComponent<InteractionEvent>().number == 0)
        {
            CallDialogue();
            return;
        }

        InteractionDoor door = interactTransform.GetComponent<InteractionDoor>();
        CameraController.isOnlyView = door.GetMapView();
        string sceneName = door.GetChangeSceneName();
        string locationName = door.GetLocationName();
        FindObjectOfType<SceneTrasnferManager>().SceneTransfer(sceneName, locationName);
    }

    private Camera cam;
    private Vector3 mousePosition;
    RaycastHit rayHit;

    void ObjectInteraction()
    {
        if (CameraController.isOnlyView)
        {
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            if (Physics.Raycast(cam.ScreenPointToRay(mousePosition), out rayHit, 100)) { };
        }
        else
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, 15)) { };
        }
        Set_InteractionUI(InteractionAble);
    }

    // 현재 상호작용이 가능한 상태인지를 나타내는 프로퍼티
    bool InteractionAble
    {
        get
        {
            if (rayHit.transform != null && rayHit.transform.CompareTag("Interaction")) return true;
            else return false;
        }
    }

    [SerializeField] GameObject normalCorsshair;
    [SerializeField] GameObject interactiveCorsshair;

    [SerializeField] GameObject obj_TargetNameBar;
    [SerializeField] Text txt_TargetName;

    private Transform contactTransform = null;
    void Set_InteractionUI(bool interactable)
    {
        // 중복 실행 방지 코드
        if (contactTransform == rayHit.transform) return;
        contactTransform = rayHit.transform;

        // 크로스헤어 설정
        interactiveCorsshair.SetActive(interactable);
        normalCorsshair.SetActive(!interactable);

        // 상호작용 객체 툴팁 설정
        obj_TargetNameBar.SetActive(interactable);
        txt_TargetName.text = (interactable) ? rayHit.transform.GetComponent<InteractionType>().GetName() : "";

        // 상호작용 이펙트 설정
        if (CameraController.isOnlyView) // 움직일 떄만 이펙트 보여줌
        {
            UIManager.instance.HideInteractionImage();
        }
        else
        {
            AppearInteractionImg(interactable);
            if (interactable) InteractionEffect();
            else img_InteractionEffect.color = new Color(1, 1, 1, 0);
        }
    }



    [SerializeField] Image img_Interaction;

    void AppearInteractionImg(bool _appear)
    {
        StopCoroutine("Co_AppearInteractionImg");
        StartCoroutine("Co_AppearInteractionImg", _appear);
    }

    IEnumerator Co_AppearInteractionImg(bool _appear) // 한번 나타나고 끝
    {
        Color color = img_Interaction.color;
        WaitForSeconds ws = new WaitForSeconds(0.02f);
        if (_appear)
        {
            color.a = 0;
            while(color.a < 1 && !DialogueManager.instance.isTalking)
            {
                color.a += 0.1f;
                img_Interaction.color = color;
                yield return ws;
            }
        }
        else
        {
            while (color.a > 0 && !DialogueManager.instance.isTalking)
            {
                color.a -= 0.1f;
                img_Interaction.color = color;
                yield return ws;
            }
        }
    }

    void InteractionEffect()
    {
        StopCoroutine("Co_InteractionEffect");
        StartCoroutine("Co_InteractionEffect");
    }

    [SerializeField] Image img_InteractionEffect;
    IEnumerator Co_InteractionEffect()
    {
        float delayTime = 0.02f;
        WaitForSeconds ws = new WaitForSeconds(delayTime);

        // 상호작용 가능한 객체에 크로스헤어를 올리고 있으며 대화중이 아닌 상태에서 계속 돌아가는 무한반복 코루틴
        while (InteractionAble && !DialogueManager.instance.isTalking)
        {
            Color color = img_InteractionEffect.color;
            color.a = 0.5f;

            img_InteractionEffect.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            Vector3 img_Scale = img_InteractionEffect.transform.localScale;

            while(color.a > 0 && !DialogueManager.instance.isTalking)
            {
                color.a -= 0.01f;
                img_InteractionEffect.color = color;
                img_Scale.Set(img_Scale.x + delayTime, img_Scale.y + delayTime, img_Scale.z + delayTime);
                img_InteractionEffect.transform.localScale = img_Scale;
                yield return ws;
            }
            yield return null;
        }
    }
}
