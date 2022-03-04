using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "file data", menuName = "Scriptable Object / Dialogue File Data")]
public class DialogueFileData : ScriptableObject
{
    [SerializeField] TextAsset targetFile = null;
    [SerializeField] List<FileLineData> fileLineDatas = null;

    void OnEnable()
    {
        fileLineDatas = Parse(targetFile);
    }

    public LineData[] GetDialogue(string _name)
    {
        for (int i = 0; i < fileLineDatas.Count; i++)
        {
            if (fileLineDatas[i].dialogueName == _name) 
                return fileLineDatas[i].lineDatas;
        }
        return null;
    }

    public List<FileLineData> Parse(TextAsset _csv)
    {
        List<FileLineData> fileLineDatas = new List<FileLineData>();

        string csvText = _csv.text.Substring(0, _csv.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        // for문에 i++를 넣지 않고 while문처럼 사용
        for (int i = 1; i < datas.Length; i++) // 엑셀 파일 1번째 줄은 편의를 위한 분류이므로 i = 1부터 시작
        {
            // A, B, C열을 쪼개서 배열에 담음 (CSV파일은 ,로 데이터를 구분하기 때문에 ,를 기준으로 짜름)
            string[] row = datas[i].Split(new char[] { ',' });
            List<LineData> LineDataList = new List<LineData>();

            // 유효한 이벤트 이름이 나올때까지 
            if (row[0].Trim() == "" || row[0].Trim() == "end") continue;
            //Debug.Log("do  :  " + i);
            FileLineData _fileData = new FileLineData();
            _fileData.dialogueName = row[0].Trim();

            while (row[0].Trim() != "end") // 1열이 end이면
            {
                LineData _line = new LineData();
                _line.characterName = row[1];

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
                _line.contexts = contextList.ToArray();
                _line.spriteNames = spriteList.ToArray();
                _line.voiceNames = voiceList.ToArray();
                _line.cutSceneName = sceneList.ToArray();

                // for문 한번 돌때마다 dialogueList에 dialogue가 하나씩 추가되며 엑셀파일의 데이터를 dialogueList에 다 담게 됨
                LineDataList.Add(_line);
            }

            _fileData.lineDatas = LineDataList.ToArray();
            fileLineDatas.Add(_fileData);
        }

        return fileLineDatas;
    }
}

[Serializable]
public class FileLineData
{
    public string dialogueName;
    public LineData[] lineDatas;
}
