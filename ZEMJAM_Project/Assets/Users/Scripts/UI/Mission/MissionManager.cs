using System;
using System.IO;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Inst;
    void Awake() => Inst = this;

    [SerializeField] ScrollviewTest svTest;
    [SerializeField] WriteMission writeMission;

    void Start()
    {
        if (SaveManager.Inst.saveData.missionDatas.Length == 0)
        {
            SaveManager.Inst.saveData.missionDatas = writeMission.missionDatas;
        }

        DateTime connectDate = DateTime.ParseExact(SaveManager.Inst.saveData.connectedDate, "yyyy-MM-dd", null);
        DateTime currentDate = DateTime.Now.Date;
        int result = DateTime.Compare(connectDate, currentDate);

        string connectDateToString = SaveManager.Inst.saveData.connectedDate;
        string currentDateToString = DateTime.Now.ToString("yyyy-MM-dd");

        print("����� ��¥: " + connectDateToString);
        print("���� ��¥: " + currentDateToString);
        if (result == 0)
        {
            Debug.Log("��¥�� ����");
        }
        else if (result == -1)
        {
            SaveManager.Inst.saveData.connectedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            foreach(var mission in SaveManager.Inst.saveData.missionDatas)
            {
                if (mission.missionRotation == MissionRotation.DAILY)
                {
                    mission.isReceived = false;
                    mission.curProgress = 0;
                }
            }
            SaveManager.Inst.saveData.isConnect = false;

            SaveManager.Inst.Save();
        }
    }

    public void DestroyEnemy(EnemyRace enemyRace, EnemyClass enemyClass)
    {
        SaveManager.Inst.saveData.missionDatas[0].curProgress++;
        SaveManager.Inst.saveData.missionDatas[8].curProgress++;
        if (enemyRace == EnemyRace.GOBLIN)
        {
            SaveManager.Inst.saveData.missionDatas[3].curProgress++;
        }
        else if (enemyRace == EnemyRace.SLOTH)
        {
            SaveManager.Inst.saveData.missionDatas[4].curProgress++;
        }
        else if (enemyRace == EnemyRace.KNIGHT)
        {
            SaveManager.Inst.saveData.missionDatas[9].curProgress++;
        }

        if (enemyClass == EnemyClass.BOSS)
        {
            SaveManager.Inst.saveData.missionDatas[1].curProgress++;
        }
    }

    public void ClearQuest()
    {
        SaveManager.Inst.saveData.missionDatas[2].curProgress++;
        svTest.missions[2].Init(2);
    }
}
