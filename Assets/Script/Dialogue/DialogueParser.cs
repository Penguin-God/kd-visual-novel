using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DebugDialogue 
{
    public string name;
    public Dialogue[] dialogues;
    public TalkEventCondition talkCondition;
}
public class DialogueParser : MonoBehaviour
{
    // 디버그용으로 Parse한 데이터를 인스펙터 창에서 볼 수 있게 하는 리스트
    public List<DebugDialogue> debugData; 

    public List<Dialogue[]> Parse(string _CsvFileName)
    {
        List<Dialogue[]> dialoguesList = new List<Dialogue[]>();

        // 대략적인 csv파일 구조
        // 세로 : 1번째줄은 분류 2번째부터 데이터가 들어감

        // 가로
        // A열 : 이벤트 이름 이벤트를 나누는 기준이 되며 대화가 진행되는 동안 공백이 생김
        // B열 : 캐릭터 이름 역시 한 캐릭터가 여러 번 말하면 그만큼 공백이 생김
        // C열 : 대사 내용으로 공백이 없음
        // D열 : 스프라이트 이름으로 한 스프라이트가 여러번 나오면 공백이 생김
        // E열 : 보이스 사운드 이름으로 사운드를 재생하지 않으면 공백이 생김
        // F열 : Cg 이름으로 대화 중 CG가 삽입되지 않으면 공백

        // Resources폴더에 있는 csv 파일 가져옴
        TextAsset csvData = Resources.Load<TextAsset>(_CsvFileName); // TextAsset : csv파일을 담을 수 있는 데이터 구조
        string csvText = csvData.text.Substring(0, csvData.text.Length -1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        // for문에 i++를 넣지 않고 while문처럼 사용
        for (int i = 1; i < datas.Length; i++) // 엑셀 파일 1번째 줄은 편의를 위한 분류이므로 i = 1부터 시작
        {
            // A, B, C열을 쪼개서 배열에 담음 (CSV파일은 ,로 데이터를 구분하기 때문에 ,를 기준으로 짜름)
            string[] row = datas[i].Split(new char[] { ',' });
            List<Dialogue> dialogueList = new List<Dialogue>();

            // 유효한 이벤트 이름이 나올때까지 
            if (row[0].Trim() == "" || row[0].Trim() == "end") continue;
            Debug.Log("do  :  " + i);

            // 디버그용 클래스
            DebugDialogue DebugDialogue = new DebugDialogue(); 
            DebugDialogue.name = row[0].Trim();

            while (row[0].Trim() != "end") // 1열이 end이면
            {
                Dialogue dialogue = new Dialogue();
                dialogue.name = row[1];

                // 캐릭터가 한번에 치는 대사를 담을 리스트 캐릭터가 치는 대사의 길이를 모르므로 리스트로 선언
                List<string> contextList = new List<string>();
                List<string> spriteList = new List<string>();
                List<string> voiceList = new List<string>();
                List<string> sceneList = new List<string>();

                do // do while : 무조건 한번 실행하고 while의 조건 확인 후 돌지 말지 결정
                {
                    contextList.Add(row[2]);
                    spriteList.Add(row[3]);
                    voiceList.Add(row[4]);
                    sceneList.Add(row[5]);

                    if (++i < datas.Length) row = datas[i].Split(new char[] { ',' });
                    else break;
                } while (row[1].ToString() == "" && row[0] != "end"); // row[0] != "end" 는 마지막 대사 다음에는 캐릭터가 없어서 대화를 탈출하기 위한 조건
                // row[1]이 공백이라는 뜻은 한 캐릭터가 여러 대사를 치고 있다는 뜻이므로 contextList에 대사를 추가하기 공백이 아닐 때까지 반복문을 돔

                // 위에서 생성한 대사 리스트를 dialogue.contexts에 대입
                dialogue.contexts = contextList.ToArray();
                dialogue.spriteNames = spriteList.ToArray();
                dialogue.voiceNames = voiceList.ToArray();
                dialogue.cutSceneName = sceneList.ToArray();

                // for문 한번 돌때마다 dialogueList에 dialogue가 하나씩 추가되며 엑셀파일의 데이터를 dialogueList에 다 담게 됨
                dialogueList.Add(dialogue);
            }

            dialoguesList.Add(dialogueList.ToArray());
            
            // 디버깅 데이터 세팅
            DebugDialogue.dialogues = dialogueList.ToArray();
            debugData.Add(DebugDialogue); // 실제 인스펙터 창에 보일 List에 Add
        }
        
        return dialoguesList;
    }

    // 대화 이벤트 이름들 return
    public string[] GetEventNames(string _CsvFileName)
    {
        TextAsset csvData = Resources.Load<TextAsset>(_CsvFileName); // TextAsset : csv파일을 담을 수 있는 데이터 구조

        string[] datas = csvData.text.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        List<string> eventNames = new List<string>();

        for (int i = 1; i < datas.Length; i++) // 엑셀 파일 1번째 줄은 편의를 위한 분류이므로 i = 1부터 시작
        {
            // A, B, C열을 쪼개서 배열에 담음 (CSV파일은 ,로 데이터를 구분하기 때문에 ,를 기준으로 짜름)
            string[] row = datas[i].Split(new char[] { ',' });

            if (row[0].ToString() != "" && row[0].ToString() != "end") eventNames.Add(row[0].ToString());
        }
        return eventNames.ToArray();
    }

    public TalkEventCondition[] GetTalkCondition(string _CsvFileName)
    {
        TextAsset csvData = Resources.Load<TextAsset>(_CsvFileName); // TextAsset : csv파일을 담을 수 있는 데이터 구조

        string[] datas = csvData.text.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        List<TalkEventCondition> talkConditions = new List<TalkEventCondition>();

        for (int i = 1; i < datas.Length; i++) // 엑셀 파일 1번째 줄은 편의를 위한 분류이므로 i = 1부터 시작
        {
            // A, B, C열을 쪼개서 배열에 담음 (CSV파일은 ,로 데이터를 구분하기 때문에 ,를 기준으로 짜름)
            string[] row = datas[i].Split(new char[] { ',' });

            if (row[0].ToString() == "end" ) talkConditions.Add(new TalkEventCondition(row[1].Split('/'), row[2] == "True", row[3] ));
        }
        return talkConditions.ToArray();
    }
}
