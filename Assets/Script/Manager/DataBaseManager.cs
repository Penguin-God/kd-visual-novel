using System.Collections;
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

    // 시작하는(_StartNumber) 줄부터 마지막 줄까지의(_EndNumber) 엑셀파일 정보를 담은 dialogueList를 배열 형태로 반환
    public Dialogue[] GetDialogues(string name)
    {
        //List<Dialogue> dialogueList = new List<Dialogue>();

        //for (int i = 0; i <= _EndNumber - _StartNumber; i++)
        //{
        //    dialogueList.Add(Dic_dialogue[_StartNumber + i]);
        //}
        Dialogue[] dialogues = null;
        if (Dic_dialogue.TryGetValue(name, out dialogues)) return dialogues;
        else
        {
            Debug.Log("찾을 수 없는 이벤트 이름 : " + name);
            return null;
        }
    }
}
