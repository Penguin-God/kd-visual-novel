using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventConditionValue
{
    public int eventCondition; // 등장 조건에 포함되는 이벤트 넘버 (무조건 등장시키려면 0번째 event를 지정하고 conditionFlag을 false로 하면됨)
    public bool conditionFlag;
}

public class EventCondition : MonoBehaviour
{
    [SerializeField] EventConditionValue[] eventConditionValues;

    public int[] endNumbers; // 퇴장 이벤트 넘버

    private InteractionEvent talkEvent;

    private void Awake()
    {
        talkEvent = GetComponent<InteractionEvent>();
    }

 
    bool CheckAppearCondition()
    {
        bool flag = true;

        // 등장 조건과 일치하지 않을 경우 등장시키지 않음
        for (int x = 0; x < eventConditionValues.Length; x++)
        {
            if (EventManager.instance.eventFlags[eventConditionValues[x].eventCondition] != eventConditionValues[x].conditionFlag)
            {
                return false;
            }

            Debug.Log(flag);
            Debug.Log(x);
        }

        // 등장 조건과 상관 없이 퇴장 조건을 만족하면 등장시키지 않음
        for(int i = 0; i < endNumbers.Length; i++)
        {
            if (!EventManager.instance.eventFlags[endNumbers[i]]) break;
            if (i == endNumbers.Length - 1) flag = false;
        }

        return flag;
    }
}
