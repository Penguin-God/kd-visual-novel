using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

[Serializable] // View Dictionary In Inspector
public class InteractionEventByName : SerializableDictionary<string, InteractionObject> {}

public class DialogueSystem : MonoBehaviour
{
    string Key_SaveDatas = "AllData";

    [SerializeField] SceneChannel sceneChannel = null;

    // 미래를 위해 남겨둠
    public InteractionEventByName interactionObjectByCodeName; // dialogueObject의 코드네임은 interactionObject의 코드네임과 같음

    private static DialogueSystem instance;
    public static DialogueSystem Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<DialogueSystem>();
            return instance;
        }
    }

    [Header("Dialogue Objects")]
    [SerializeField] List<GameObject> spawnDialogueObjects;
    public IReadOnlyList<GameObject> DynamicDialogueObjects => spawnDialogueObjects;

    [SerializeField] List<DialogueObject> allDialogueObjects;
    public List<DialogueObject> AllDialogueObjects => allDialogueObjects;

    public DialogueObject GetDialogueObjectClone(string _codeName) => allDialogueObjects.FirstOrDefault(x => x.CodeName == _codeName);

    private void Awake()
    {
        sceneChannel.OnEnterOtherScene += SetupSpawnDialgoueObject;
    }

    void Start()
    {
        Load();
    }

    void OnApplicationQuit()
    {
        Save();
    }

    // spawnDialogueObjects 세팅
    void SetupSpawnDialgoueObject(SceneManagerISo _data)
    {
        allDialogueObjects = _data.DialogueObjects;

        spawnDialogueObjects.Clear();
        List<DialogueObject> dialogueObjects = _data.GetSpawnDialogueObjects();
        foreach (DialogueObject _dialogueObject in dialogueObjects)
        {
            // new GameObject형식으로 Instantiate해야 Scene오브젝트로 생성됨
            GameObject _obj = Instantiate(_dialogueObject.GetGameObject());
            spawnDialogueObjects.Add(_obj);

            // asset 오브젝트 내의 InteractionObject가 아니라 Scene에 있는 오브젝트의 InteractionObject로 진행해야 제대로 진행됨
            InteractionObject _interaction = _obj.GetComponent<InteractionObject>();
            _interaction.Setup(_dialogueObject); // InteractionObject 세팅
        }
    }


    // Save, Load region
    #region Save and Load

    string SavePath => Application.persistentDataPath + "/Test.txt";
    int XOR_Key => -1375631;

    JArray CreateSaveDatas(SceneManagerISo _sceneData)
    {
        JArray _saveDatas = new JArray();
        foreach (var _dialogueObject in _sceneData.DialogueObjects)
        {
            _saveDatas.Add(JObject.FromObject(_dialogueObject.GetSaveData()));
        }
        return _saveDatas;
    }

    void LoadSaveData(JToken _token, SceneManagerISo _sceneData)
    {
        JArray _datas = _token as JArray;
        foreach (JToken _data in _datas)
        {
            DialogueSaveData dialogueSaveData = _data.ToObject<DialogueSaveData>();
            DialogueObject dialogueObject = _sceneData.DialogueObjects.FirstOrDefault(x => x.CodeName == dialogueSaveData.codeName);
            if (dialogueObject != null)
                dialogueObject.LoadData(dialogueSaveData);
        }
    }

    public void Save()
    {
        JObject _root = new JObject();
        foreach (var _sceneData in MySceneManager.Instance.AllSceneManagerISOs)
        {
            _root.Add(_sceneData.SceneName, CreateSaveDatas(_sceneData));
        }

        string jdata = _root.ToString();
        byte[] jbytes = Encoding.UTF8.GetBytes(jdata);

        for (int i = 0; i < jbytes.Length; i++)
        {
            jbytes[i] = (byte)(jbytes[i] ^ XOR_Key);
        }
        string format = System.Convert.ToBase64String(jbytes);

        File.WriteAllText(SavePath, format);
        print(format);
    }

    void Load()
    {
        if (File.Exists(SavePath))
        {
            string jdata = File.ReadAllText(SavePath);
            byte[] jbytes = System.Convert.FromBase64String(jdata);
            for (int i = 0; i < jbytes.Length; i++)
            {
                jbytes[i] = (byte)(jbytes[i] ^ XOR_Key);
            }
            string reFormat = Encoding.UTF8.GetString(jbytes);

            JObject _root = JObject.Parse(reFormat);
            foreach (var _sceneData in MySceneManager.Instance.AllSceneManagerISOs)
            {
                LoadSaveData(_root[_sceneData.SceneName], _sceneData);
            }

            print(reFormat);
        }
    }
    #endregion
}