using UnityEngine;

public class EventCondition : MonoBehaviour
{
    public int eventNumber; // 컴포넌트를 가지고 있는 오브젝트가 진행하는 이벤트 넘버 (진행하면 DataBaseManager의 eventFlags의 eventNumber번째가 true가 됨)
    public int[] eventConditions; // 등장 조건에 포함되는 이벤트 넘버 (무조건 등장시키려면 0번째 event를 지정하고 conditionFlag을 false로 하면됨)
    public bool conditionFlag; // 이벤트를 보는게 등장 조건인지 보지 않은게 등장 조건인지 결정하는 변수 
    public int endNumber; // 퇴장 이벤트 넘버

    private void Start()
    {
        gameObject.SetActive(CheckEvent());
    }

    bool CheckEvent()
    {
        bool flag = true;

        // 등장 조건과 일치하지 않을 경우 등장시키지 않음
        for(int i = 0; i < eventConditions.Length; i++)
        {
            if(EventManager.instance.eventFlags[eventConditions[i]] != conditionFlag)
            {
                flag = false;
                break;
            }
        }
        // 등장 조건과 관계없이 퇴장 조건에 만족하면 등장시키지 않음
        if(EventManager.instance.eventFlags[endNumber]) flag = false;

        return flag;
    }
}
