using UnityEngine;
public enum MissionRotation { DAILY, WEEKLY, INFINITE }

[System.Serializable]
public class MissionData
{
    public string missionName;
    public string missionExplain;
    public PrizeType prizeType;

    [Space]
    public int id;

    [Space]
    public MissionRotation missionRotation;

    [Space]
    public int rewardAmount;

    [Space]
    public float maxProgress;
    public float curProgress;

    [Space]
    public bool isReceived;
}

public class WriteMission : MonoBehaviour
{
    public MissionData[] missionDatas;
}