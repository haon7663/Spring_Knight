using System;
using System.IO;
using UnityEngine;


public static class SceneVariable
{
    public static GameMode gameMode;
    public static PlayerType playerType;
}


public class SaveManager : MonoBehaviour
{
    public Data saveData;
    private static SaveManager inst = null;
    private readonly string _key = "aes256=32CharA49AScdg5135=48Fk63";

    [Serializable]
    public class Data
    {
        public float[] volume = { -20, -20, -20 };
        public bool playSoundToggle;
        public bool cameraFollowToggle;
        public bool vibrationToggle;
        public int cameraShakeSize = 2;

        public int clearedTheme;
        public int clearedStage;
        public int gold;
        public int chest;
        public int chest2;

        public MissionData[] missionDatas;
        public string connectedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");

        public bool isConnect;
        public int dailyCount;
    }

    private void Awake()
    {
        if (null == inst)
        {
            inst = this;
            DontDestroyOnLoad(inst);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Load();
    }

    public static SaveManager Inst
    {
        get
        {
            if (null == inst)
            {
                return null;
            }

            return inst;
        }
    }

    public void Save()
    {
        // saveData 변수를 json 형식으로 변환한다
        var jsonData = JsonUtility.ToJson(saveData, true);
        // jsonData를 save.json에 저장한다

        var Encrypt = AES256Encrypt.Encrypt256(jsonData, _key);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "save.json"), Encrypt);
    }

    public void Load()
    {
        // save.json이 존재하지않는가?
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "save.json")))
        {
            // saveData 변수를 새로 작성
            saveData = new Data();
            // Load 메서드 종료
            return;
        }

        // 파일이 존재하면 save.json을 불러온다
        var jsonData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "save.json"));
        // saveData 변수에 덮어씌운다
        var Decrypt256 = AES256Encrypt.Decrypt256(jsonData, _key);
        saveData = JsonUtility.FromJson<Data>(Decrypt256);
    }

    public void Delete()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, "save.json"));
    }

    public void SaveInfo(int currentTheme, int currentStage, int gold)
    {
        saveData.clearedTheme = currentTheme;
        saveData.clearedStage = currentStage;
        saveData.gold = gold;

        Save();
    }
    public void SaveSetttingInfo(bool cameraFollowToggle, bool vibrationToggle, int cameraShakeSize, float[] volume, bool playSoundToggle)
    {
        saveData.cameraFollowToggle = cameraFollowToggle;
        saveData.vibrationToggle = vibrationToggle;
        saveData.cameraShakeSize = cameraShakeSize;
        saveData.volume = volume;
        saveData.playSoundToggle = playSoundToggle;

        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}