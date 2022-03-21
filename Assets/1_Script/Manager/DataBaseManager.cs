using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;
    [SerializeField] TextAsset csvAsset = null;
    Dictionary<string, Dialogue[]> Dic_dialogue = new Dictionary<string, Dialogue[]>();
    public Dictionary<string, TalkEventCondition> dic_TalkCondition = new Dictionary<string, TalkEventCondition>();
    public bool isFinish = false;

    private void Awake()
    {
        Debug.Log("Sciript Awake");
        if(instance == null)
        {
            instance = this;
            DialogueParser theParser = GetComponent<DialogueParser>();

            TalkEventCondition[] talkConditions = theParser.GetTalkCondition(csvAsset);

            List<Dialogue[]> dialoguesList = new List<Dialogue[]>();
            dialoguesList = theParser.Parse(csvAsset);
            string[] eventNames = theParser.GetEventNames(csvAsset);

            for (int i = 0; i < dialoguesList.Count; i++)
            {
                Dic_dialogue.Add(eventNames[i], dialoguesList[i]);
            }

            for(int i = 0; i < eventNames.Length; i++)
            {
                dic_TalkCondition.Add(eventNames[i], talkConditions[i]);
            }
        }
    }

    private void Start()
    {
        Debug.Log("Sciript Start");
        SetEventData();
    }

    void OnEnable()
    {
        Debug.Log("Sciript OnEnable");
    }

    void SetEventData()
    {
        foreach (string eventName in Dic_dialogue.Keys)
        {
            EventManager.instance.eventFlags.Add(eventName, false);
        }
        isFinish = true;
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
