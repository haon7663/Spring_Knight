using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase.Database;
public class RankingLeaderBoard : MonoBehaviour
{
    [SerializeField] Transform contents;
    public RankUI otherRank;
    public List<RankUI> otherRanks = new List<RankUI>();

    int[] strScores;
    string[] strIds;
    string[] strLevs;


    private long strLen;

    private bool textLoadBool = false;

    public string seed;
    public string id;
    public string maxScore;
    public string level;

    public class Data
    {
        public string id;
        public string maxScore;
        public string level;
        public Data(string id, string maxScore, string level)
        {
            this.id = id;
            this.maxScore = maxScore;
            this.level = level;
        }
    }

    DatabaseReference reference;
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        for(int i = 0; i < 24; i++)
        {
            var rank = Instantiate(otherRank, contents);
            otherRanks.Add(rank);
        }

        DataSave();
    }
    void Update()
    {
        //현재 첫번째 Text UI가 "Loading" 이면,
        //즉, 스크립트를 컴포넌트하고있는 게임 오브젝트가 Activeself(true) 이면.
        if (otherRanks[0].nameText.text == "-")
        {
            DataLoad();
        }
    }
    void LateUpdate()
    {
        if (textLoadBool)
        {
            TextLoad();
        }
        if (Time.timeScale != 0.0f) Time.timeScale = 0.0f;
    }
    public void DataSave()
    {
        var saveData = SaveManager.Inst.saveData;
        //seed = saveData.randSeed.ToString();
        id = saveData.id;
        maxScore = saveData.maxScore.ToString();
        level = saveData.level.ToString();

        var data = new Data(id, maxScore, level);
        string jsonData = JsonUtility.ToJson(data);

        reference.Child("rank").SetRawJsonValueAsync(jsonData);
    }

    void DataLoad()
    {
        //데이터 로드
        reference.Child("rank").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //에러 데이터로드 실패 시 다시 데이터 로드
                DataLoad();
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                int count = 0;
                strLen = snapshot.ChildrenCount;
                strScores = new int[strLen];
                strIds = new string[strLen];
                strLevs = new string[strLen];

                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary rankInfo = (IDictionary)data.Value;

                    strScores[count] = (int)rankInfo["maxScore"];
                    strLevs[count] = rankInfo["level"].ToString();
                    strIds[count] = rankInfo["id"].ToString();

                    count++;
                }
                //LateUpdate의 TextLoad() 함수 실행
                textLoadBool = true;
            }
        });
    }
    void TextLoad()
    {
        textLoadBool = false;
        try
        {
            //받아온 데이터 정렬 = > 위에서부터 아래로
            Array.Sort(strScores);
        }
        catch (NullReferenceException e)
        {
            return;
        }

        for (int i = 0; i < otherRanks.Count; i++)
        {
            if (strLen <= i) return;
            otherRanks[i].rankText.text = i.ToString() + "위";
            otherRanks[i].scoreText.text = strScores[i].ToString();
            otherRanks[i].nameText.text = strIds[i];
            otherRanks[i].levelText.text = strLevs[i];
        }
    }
}