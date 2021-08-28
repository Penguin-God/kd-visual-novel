﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;
    [SerializeField] string csv_FileName;
    Dictionary<string, Dialogue[]> Dic_dialogue = new Dictionary<string, Dialogue[]>();

    public bool isFinish = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DialogueParser theParser = GetComponent<DialogueParser>();

            List<Dialogue[]> dialoguesList = new List<Dialogue[]>();
            dialoguesList = theParser.Parse(csv_FileName);
            string[] eventNames = theParser.GetEventNames(csv_FileName);

            for (int i = 0; i < dialoguesList.Count; i++)
            {
                Dic_dialogue.Add(eventNames[i], dialoguesList[i]);
            }

            isFinish = true;
        }
    }

    // eventName의 시작부터 끝까지의 엑셀파일 정보를 담은 dialogue[] 반환
    public Dialogue[] GetDialogues(string name)
    {
        Dialogue[] dialogues = null;
        if (Dic_dialogue.TryGetValue(name, out dialogues)) return dialogues;
        else
        {
            Debug.Log("찾을 수 없는 이벤트 이름 : " + name);
            return null;
        }
    }
}
