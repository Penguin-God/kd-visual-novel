using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Test : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject);
    }

    [ContextMenu("Test Foreach")]
    void TestForeach() // 테스트 결과 : Foreach내에서 이뤄지는 연산은 참조형에만 영향을 준다
    {
        List<Teste> ad = new List<Teste>();
        ad.Add(new Teste());
        ad.Add(new Teste());
        ad.Add(new Teste());

        ad.ForEach(x => print(x.count));
        print("=====================================");
        ad.ForEach(x => ++x.count);
        ad.ForEach(x => print(x.count));
    }

    class Teste
    {
        public int count = 0;
        public void Setup()
        {
            count++;
        }

    }
}
