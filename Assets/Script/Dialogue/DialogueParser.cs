using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CsvFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        // 대략적인 csv파일 구조
        // 세로 : 1번째줄은 분류 2번째부터 데이터가 들어감
        // 가로
        // A열 : 대화 순서 한 캐릭터가 여러 번 말하면 그만큼 공백이 생김
        // B열 : 캐릭터 이름 역시 한 캐릭터가 여러 번 말하면 그만큼 공백이 생김
        // C열 : 대사 내용으로 공백이 없음

        // Resources폴더에 있는 csv 파일 가져옴
        TextAsset csvData = Resources.Load<TextAsset>(_CsvFileName); // TextAsset : csv파일을 담을 수 있는 데이터 구조

        string[] datas = csvData.text.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        for (int i = 1; i < datas.Length;) // 엑셀 파일 1번째 줄은 편의를 위한 분류이므로 i = 1부터 시작
        {
            // A, B, C열을 쪼개서 배열에 담음 (CSV파일은 ,로 데이터를 구분하기 때문에 ,를 기준으로 짜름)
            string[] row = datas[i].Split(new char[] { ',' });

            Dialogue dialogue = new Dialogue();
            dialogue.name = row[1];

            // 캐릭터가 한번에 치는 대사를 담을 리스트 캐릭터가 치는 대사의 길이를 모르므로 리스트로 선언
            List<string> contextList = new List<string>();

            do // do while : 무조건 한번 실행하고 while의 조건 확인 후 돌지 말지 결정
            {
                contextList.Add(row[2]);
                // 다음 대사를 추가하려면 row로 넘겨야 하는데 그럴려면 i를 올려야 되서 for문에 i++을 정의하지 않음
                if (++i < datas.Length) row = datas[i].Split(new char[] { ',' });
                else break; // csv파일의 길이를 넘기면 break
            } while (row[0].ToString() == "");
            // row[0]이 공백이라는 뜻은 한 캐릭터가 여러 대사를 치고 있다는 뜻이므로 contextList에 대사를 추가하기 공백이 아닐 때까지 반복문을 돔

            // 위에서 생성한 대사 리스트를 dialogue.contexts에 대입
            dialogue.contexts = contextList.ToArray();
            // for문 한번 돌때마다 dialogueList에 dialogue가 하나씩 추가되며 엑셀파일의 데이터를 dialogueList에 다 담게 됨
            dialogueList.Add(dialogue);
        }
        return dialogueList.ToArray(); // 배열 형태로 반환
    }
}
