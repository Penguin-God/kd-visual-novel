using UnityEngine;

public class EventCondition : MonoBehaviour
{
    public int eventNumber; // ������Ʈ�� ������ �ִ� ������Ʈ�� �����ϴ� �̺�Ʈ �ѹ� (�����ϸ� DataBaseManager�� eventFlags�� eventNumber��°�� true�� ��)
    public int[] eventConditions; // ���� ���ǿ� ���ԵǴ� �̺�Ʈ �ѹ� (������ �����Ű���� 0��° event�� �����ϰ� conditionFlag�� false�� �ϸ��)
    public bool conditionFlag; // �̺�Ʈ�� ���°� ���� �������� ���� ������ ���� �������� �����ϴ� ���� 
    public int endNumber; // ���� �̺�Ʈ �ѹ�

    private void Start()
    {
        gameObject.SetActive(CheckEvent());
    }

    bool CheckEvent()
    {
        bool flag = true;

        // ���� ���ǰ� ��ġ���� ���� ��� �����Ű�� ����
        for(int i = 0; i < eventConditions.Length; i++)
        {
            if(EventManager.instance.eventFlags[eventConditions[i]] != conditionFlag)
            {
                flag = false;
                break;
            }
        }
        // ���� ���ǰ� ������� ���� ���ǿ� �����ϸ� �����Ű�� ����
        if(EventManager.instance.eventFlags[endNumber]) flag = false;

        return flag;
    }
}
