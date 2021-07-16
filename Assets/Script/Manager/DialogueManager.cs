using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Debug.LogError("다이어로그 매니저 싱글톤 2개");
            Destroy(gameObject);
        }
    }

    [SerializeField] GameObject obj_DialogueBar;
    [SerializeField] GameObject obj_NameBar;

    [SerializeField] Text txt_Dialogue;
    [SerializeField] Text txt_Name;

    public bool isTalking = false;
    public void ShowDialogue()
    {
        UIManager.instance.HideUI();
        Set_DialogueUI(true);
        isTalking = true;
    }

    void Set_DialogueUI(bool _flag)
    {
        obj_DialogueBar.SetActive(_flag);
        obj_NameBar.SetActive(_flag);
    }
}
