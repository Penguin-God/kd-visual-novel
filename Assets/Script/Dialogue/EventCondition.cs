using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventConditionValue
{
    public int eventNumber; // ������Ʈ�� ������ �ִ� ������Ʈ�� �����ϴ� �̺�Ʈ �ѹ� (�����ϸ� DataBaseManager�� eventFlags�� eventNumber��°�� true�� ��)
    public int[] eventConditions; // ���� ���ǿ� ���ԵǴ� �̺�Ʈ �ѹ� (������ �����Ű���� 0��° event�� �����ϰ� conditionFlag�� false�� �ϸ��)
    public bool conditionFlag; // �̺�Ʈ�� ���°� ���� �������� ���� ������ ���� �������� �����ϴ� ���� 
    public int endNumber; // ���� �̺�Ʈ �ѹ�
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

            // ���� ���ǰ� ��ġ���� ���� ��� �����Ű�� ����
            for (int i = 0; i < eventConditionValues[x].eventConditions.Length; i++)
            {
                if (EventManager.instance.eventFlags[eventConditionValues[x].eventConditions[i]] != eventConditionValues[x].conditionFlag)
                {
                    flag = false;
                    break;
                }
            }
            // ���� ���ǰ� ������� ���� ���ǿ� �����ϸ� �����Ű�� ����
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
