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
        string[] datas = csvText.Split(new char[] { '\n' }); // ?????????(??? ???)??? ???????????? csv ????????? ????????? string????????? ??? ???????????? ??????

        for (int i = 1; i < datas.Length; i++) // ?????? ?????? 1?????? ?????? ????????? ?????? ??????????????? i = 1?????? ??????
        {
            // A, B, C?????? ????????? ????????? ?????? (CSV????????? ,??? ???????????? ???????????? ????????? ,??? ???????????? ??????)
            string[] row = GetSell(datas[i]);

            // ????????? ????????? ????????? ??????????????? 
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

        // DialogueEventData ????????? ????????? ?????????
        while (_datas.Length > _index && _rows[0] != "")
        {
            // ???????????? ????????? ?????? ????????? ????????? ???????????? ???????????? ??????
            List<string> contextList = new List<string>();
            List<string> spriteList = new List<string>();
            List<string> voiceList = new List<string>();
            List<string> sceneList = new List<string>();
            List<string> dirList = new List<string>();

            _rows = GetSell(_datas[_index]);
            DialogueData lineData = new DialogueData();
            lineData.characterName = _rows[1]; // ????????? ?????? ??????

            do
            {
                // ????????? Add
                contextList.Add(_rows[2].Trim());
                spriteList.Add(_rows[3].Trim());
                voiceList.Add(_rows[4].Trim());
                sceneList.Add(_rows[5].Trim());
                dirList.Add(_rows[6].Trim());

                // ????????? ??? ??????
                if (_datas.Length > ++_index) _rows = GetSell(_datas[_index]);
                else break;

            } while (_rows[1] == "" && _rows[0] != ""); // ++_index?????? _rows[0] ?????? ???

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
