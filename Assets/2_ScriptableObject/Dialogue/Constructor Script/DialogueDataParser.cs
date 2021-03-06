using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;

[CreateAssetMenu(fileName = "new parser", menuName = "Scriptable Object / Dialogue Parser")]
public class DialogueDataParser : ScriptableObject
{
    [SerializeField] TextAsset targetFile = null;
    [SerializeField] List<DialogueEventData> fileLineDatas = null;
    
    [ContextMenu("Create Dialogue Data")]
    public void CreateMyAsset()
    {
        for (int i = 0; i < staticFileLineDatas.Count; i++)
        {
            string eventName = staticFileLineDatas[i].dialogueName;
            string _name = $"Assets/ScriptableObject/Dialogue/Datas/{eventName}.asset";
            if (File.Exists(_name)) continue;

            DialogueDataContainer _asset = ScriptableObject.CreateInstance<DialogueDataContainer>();
            _asset.Init(this, staticFileLineDatas[i].dialogueName);
            _name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/ScriptableObject/Dialogue/Datas/{eventName}.asset");

            AssetDatabase.CreateAsset(_asset, _name);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = _asset;
        }
    }


    static List<DialogueEventData> staticFileLineDatas;
    void OnEnable()
    {
        fileLineDatas = Parse(targetFile);

        staticFileLineDatas = fileLineDatas;
    }

    public DialogueData[] GetDialogue(string _name)
    {
        for (int i = 0; i < fileLineDatas.Count; i++)
        {
            if (fileLineDatas[i].dialogueName == _name) 
                return fileLineDatas[i].characterDialogueDatas;
        }
        return null;
    }

    string[] GetSell(string _row) => _row.Split(new char[] { '\t' });
    public List<DialogueEventData> Parse(TextAsset _csv)
    {
        List<DialogueEventData> fileLineDatas = new List<DialogueEventData>();

        string csvText = _csv.text.Substring(0, _csv.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        for (int i = 1; i < datas.Length; i++) // 엑셀 파일 1번째 줄은 편의를 위한 분류이므로 i = 1부터 시작
        {
            // A, B, C열을 쪼개서 배열에 담음 (CSV파일은 ,로 데이터를 구분하기 때문에 ,를 기준으로 짜름)
            string[] row = GetSell(datas[i]);

            // 유효한 이벤트 이름이 나올때까지 
            if (row[0].Trim() == "") continue;

            //Debug.Log("do  :  " + row[0]);

            DialogueEventData _fileData = new DialogueEventData();
            _fileData.dialogueName = row[0].Trim();
            _fileData.characterDialogueDatas = GetEventData(datas, ref i);
            fileLineDatas.Add(_fileData);
        }

        return fileLineDatas;
    }

    DialogueData[] GetEventData(string[] _datas, ref int _index)
    {
        List<DialogueData> lineDataList = new List<DialogueData>();
        string[] _rows = GetSell(_datas[_index]);

        // DialogueEventData 하나를 만드는 반복문
        while (_datas.Length > _index && _rows[0] != "")
        {
            // 캐릭터가 한번에 치는 대사의 길이를 모르므로 리스트로 선언
            List<string> contextList = new List<string>();
            List<string> spriteList = new List<string>();
            List<string> voiceList = new List<string>();
            List<string> sceneList = new List<string>();
            List<string> dirList = new List<string>();

            _rows = GetSell(_datas[_index]);
            DialogueData lineData = new DialogueData();
            lineData.characterName = _rows[1]; // 캐릭터 이름 세팅

            do
            {
                // 데이터 Add
                contextList.Add(_rows[2].Trim());
                spriteList.Add(_rows[3].Trim());
                voiceList.Add(_rows[4].Trim());
                sceneList.Add(_rows[5].Trim());
                dirList.Add(_rows[6].Trim());

                // 줄바꿈 및 탈출
                if (_datas.Length > ++_index) _rows = GetSell(_datas[_index]);
                else break;

            } while (_rows[1] == "" && _rows[0] != ""); // ++_index해서 _rows[0] 해도 됨

            lineData.contexts = contextList.ToArray();
            lineData.spriteNames = spriteList.ToArray();
            lineData.voiceNames = voiceList.ToArray();
            lineData.cutSceneName = sceneList.ToArray();
            lineData.cameraRotateDir = dirList.ToArray();

            lineDataList.Add(lineData);
        }

        return lineDataList.ToArray();
    }

}

[Serializable]
public class DialogueEventData
{
    public string dialogueName;
    public DialogueData[] characterDialogueDatas;
}
