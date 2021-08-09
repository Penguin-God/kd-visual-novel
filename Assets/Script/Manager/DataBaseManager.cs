using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;
    [SerializeField] string csv_FileName;
    Dictionary<int, Dialogue> Dic_dialogue = new Dictionary<int, Dialogue>();

    public bool isFinish = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DialogueParser theParser = GetComponent<DialogueParser>();

            Dialogue[] dialogues = theParser.Parse(csv_FileName);
            for(int i = 0; i < dialogues.Length; i++)
            {
                Dic_dialogue.Add(i + 1, dialogues[i]);
            }

            isFinish = true;
        }
    }

    // 시작하는(_StartNumber) 줄부터 마지막 줄까지의(_EndNumber) 엑셀파일 정보를 담은 dialogueList를 배열 형태로 반환
    public Dialogue[] GetDialogues(int _StartNumber, int _EndNumber)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        for(int i = 0; i <= _EndNumber - _StartNumber; i++)
        {
            dialogueList.Add(Dic_dialogue[_StartNumber + i]);
        }
        return dialogueList.ToArray();
    }
}
