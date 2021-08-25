using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventConditionValue
{
    public int eventCondition; // ���� ���ǿ� ���ԵǴ� �̺�Ʈ �ѹ� (������ �����Ű���� 0��° event�� �����ϰ� conditionFlag�� false�� �ϸ��)
    public bool conditionFlag;
}

public class EventCondition : MonoBehaviour
{
    [SerializeField] EventConditionValue[] eventConditionValues;

    public int[] endNumbers; // ���� �̺�Ʈ �ѹ�

    private InteractionEvent talkEvent;

    private void Awake()
    {
        talkEvent = GetComponent<InteractionEvent>();
    }

 
    bool CheckAppearCondition()
    {
        bool flag = true;

        // ���� ���ǰ� ��ġ���� ���� ��� �����Ű�� ����
        for (int x = 0; x < eventConditionValues.Length; x++)
        {
            if (EventManager.instance.eventFlags[eventConditionValues[x].eventCondition] != eventConditionValues[x].conditionFlag)
            {
                return false;
            }

            Debug.Log(flag);
            Debug.Log(x);
        }

        // ���� ���ǰ� ��� ���� ���� ������ �����ϸ� �����Ű�� ����
        for(int i = 0; i < endNumbers.Length; i++)
        {
            if (!EventManager.instance.eventFlags[endNumbers[i]]) break;
            if (i == endNumbers.Length - 1) flag = false;
        }

        return flag;
    }
}
