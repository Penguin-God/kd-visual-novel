using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventConditionValue
{
    public int eventNumber; // 컴포넌트를 가지고 있는 오브젝트가 진행하는 이벤트 넘버 (진행하면 DataBaseManager의 eventFlags의 eventNumber번째가 true가 됨)
    public int[] eventConditions; // 등장 조건에 포함되는 이벤트 넘버 (무조건 등장시키려면 0번째 event를 지정하고 conditionFlag을 false로 하면됨)
    public bool conditionFlag; // 이벤트를 보는게 등장 조건인지 보지 않은게 등장 조건인지 결정하는 변수 
    public int endNumber; // 퇴장 이벤트 넘버
}

public class EventCondition : MonoBehaviour
{
    [SerializeField] EventConditionValue[] eventConditionValues;

    //public int ProgressEventNumber
    //{
    //    get
    //    {
    //        return eventConditionValues[currnetEvent].eventNumber;
    //    }
    //}

    //[SerializeField] List<int[]> eventConditionList;
    //int currnetEvent
    //{
    //    get
    //    {
    //        for (int i = 0; i < eventConditionValues.Length; i++)
    //        {
    //            for (int x = 0; x < eventConditionValues[i].eventConditions.Length; x++)
    //            {
    //                if (EventManager.instance.eventFlags[eventConditionValues[i].eventConditions[x]]) continue;
    //                else break;
    //            }
    //            return i;
    //        }
    //        return 0;
    //    }
    //}

    private InteractionEvent talkEvent;

    private void Awake()
    {
        talkEvent = GetComponent<InteractionEvent>();
    }

    private void Start()
    {
        talkEvent.OnSetDialogue += () => CheckEvent();
        gameObject.SetActive(CheckEvent());
    }

    int currentEvent;
    public int CurrentEventNumber
    {
        get
        {
            return eventConditionValues[currentEvent].eventNumber;
        }
    }

    bool CheckEvent()
    {
        bool flag = true;

        for (int x = 0; x < eventConditionValues.Length; x++)
        {
            flag = true;

            // 등장 조건과 일치하지 않을 경우 등장시키지 않음
            for (int i = 0; i < eventConditionValues[x].eventConditions.Length; i++)
            {
                if (EventManager.instance.eventFlags[eventConditionValues[x].eventConditions[i]] != eventConditionValues[x].conditionFlag)
                {
                    flag = false;
                    break;
                }
            }
            // 등장 조건과 관계없이 퇴장 조건에 만족하면 등장시키지 않음
            if (EventManager.instance.eventFlags[eventConditionValues[x].endNumber]) flag = false;
            Debug.Log(flag);
            Debug.Log(x);
            if (flag)
            {
                currentEvent = x;
                talkEvent.currentEvent = x;
                break;
            }
        }

        return flag;
    }
}
