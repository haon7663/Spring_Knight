using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum GameMode { STAGE, INFINITE, GOLD }
public enum GameState { LOADING, PAUSE, PLAY, DEATH }
public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; set; }

    public PlayerController m_PlayerController;

    public GameMode m_GameMode;
    public GameState m_GameState;

    public bool isLoadScene;
    public bool isSetting;

    [Space]
    [Header("State")]
    public bool onPlay;
    public bool onPause;
    public bool onDeath;

    [Space]
    [Header("Stats")]
    public int curStage;
    public int curPaze;
    public int maxPaze;
    public int manageHealth = 3;
    public int maxPower = 3;
    public int managePower = 3;
    public int summonCount = 3;
    public float enemySummonCount;

    [Space]
    [Header("Time")]
    public float maxTimer = 45;

    [Space]
    [Header("Score")]
    public int score;

    [Space]
    [Header("Property")]
    public Property[] saveProperty;
    public delegate void KillEnemyAction();
    public static event KillEnemyAction KillEnemy;
    public delegate void SpawnEnemyAction();
    public static event SpawnEnemyAction SpawnEnemy;

    void Awake()
    {
        Application.targetFrameRate = 60;

        var obj = FindObjectsOfType<GameManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            SetGame();
        }
        else
        {
            Destroy(gameObject);
        }
        Inst = this;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ChangeMode(GameMode gameMode)
    {
        m_GameMode = gameMode;
        switch (m_GameMode)
        {
            case GameMode.STAGE:
                break;
            case GameMode.INFINITE:
                UIManager.Inst.OpenTimer(maxTimer);
                UIManager.Inst.OpenScore();
                break;
            case GameMode.GOLD:
                break;
        }
    }
    public void ChangeState(GameState gameState)
    {
        m_GameState = gameState;
        switch(m_GameState)
        {
            case GameState.PLAY:
                Time.timeScale = 1;
                onPlay = true;
                onPause = false;
                break;
            case GameState.LOADING:
                Time.timeScale = 1;
                onPlay = false;
                onPause = false;
                break;
            case GameState.PAUSE:
                Time.timeScale = 0;
                onPlay = false;
                onPause = true;
                break;
            case GameState.DEATH:
                onDeath = true;
                onPlay = false;
                onPause = false;
                break;
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Inst = this;
        Time.timeScale = 1;

        m_PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_PlayerController.maxPower = managePower;
        maxPower = managePower;

        HealthManager.Inst.SetHealth(3);
        HealthManager.Inst.curhp = manageHealth;

        UIManager.Inst.SetPaze(curPaze, maxPaze);

        ChangeMode(m_GameMode);

        if(isSummonItems)
            for (int i = 0; i < summonItemValue; i++)
                SummonManager.Inst.SummonItem();

        isSetting = false;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (SummonManager.Inst.enemyList.Count <= 0)
        {
            if (summonCount <= 0 && !isLoadScene)
            {
                isLoadScene = true;
                StartCoroutine(MoveScene());
            }
            else if (summonCount > 0)
            {
                summonCount--;
                for (int i = 0; i < (int)enemySummonCount; i++)
                {
                    SummonManager.Inst.SummonEnemy();
                }
                //SummonManager.Inst.SummonItem();

                isLoadScene = false;
            }
        }
    }

    public void SetGame()
    {
        curPaze = 0;
        managePower = 3;
        manageHealth = 3;
        enemySummonCount = 3;

        ResetProperty();
    }
    
    IEnumerator MoveScene()
    {
        ChangeState(GameState.LOADING);
        yield return new WaitForSeconds(1);
        Fade.Inst.Fadein();
        yield return new WaitForSeconds(0.1f);

        manageHealth = HealthManager.Inst.curhp;
        enemySummonCount += 0.75f;
        managePower += 1;
        summonCount = 3;
        curPaze++;

        SceneManager.LoadScene("InGame");
    }

    #region PropertyEffects
    bool isSummonItems;
    int summonItemValue, killGoldPersent, decreaseDef, spawnDecDefPersent;

    void ResetProperty()
    {
        isSummonItems = false;
    }
    public void OneTimePowerUp(int value)
    {
        maxPower = managePower + value;
        m_PlayerController.maxPower += value;
        Debug.Log(m_PlayerController.maxPower);
    }
    public void IncreaseMaxHealth()
    {
        manageHealth++;
        HealthManager.Inst.SetHealth(manageHealth);
        HealthManager.Inst.OnHealth(1);
    }
    public void IncreaseMaxPower()
    {
        managePower++;
        maxPower = managePower;
        m_PlayerController.maxPower = managePower;
    }
    public void SummonItem(int value)
    {
        isSummonItems = true;
        summonItemValue += value;
        SummonManager.Inst.SummonItem();
    }

    #region KillEvent
    public void KillEvent()
    {
        KillEnemy?.Invoke();
    }
    public void AddKillGold(int persent)
    {
        if(KillEnemy != KillGold)
        {
            KillEnemy += KillGold;
            Debug.Log("ADDKILLGOLD");
        }
        killGoldPersent += persent;
    }
    public void AddKillDecDef(int value)
    {
        if (KillEnemy != KillDecDef)
        {
            KillEnemy += KillDecDef;
            Debug.Log("ADDDECDEF");
        }
        decreaseDef += value;
    }
    void KillGold()
    {
        var succes = Calculate(killGoldPersent);
        if (succes)
            Debug.Log("Gold");
    }
    void KillDecDef()
    {
        Debug.Log("DecreaseDef");
    }
    #endregion
    #region SpawnEvent

    public void SpawnEvent()
    {
        SpawnEnemy?.Invoke();
    }
    public void AddSpawnDecDef(int persent)
    {
        if (SpawnEnemy != SpawnDecDef)
        {
            SpawnEnemy += SpawnDecDef;
            Debug.Log("ADDSPAWNDECDEF");
        }
        spawnDecDefPersent += persent;
    }
    void SpawnDecDef()
    {
        var succes = Calculate(spawnDecDefPersent);
        if (succes)
            Debug.Log("SpawnDecreaseDef");
    }

    #endregion

    bool Calculate(int value)
    {
        float ran = Random.Range(0, 100f);
        return ran <= value;
    }

    #endregion
}

internal static class YieldInstructionCache
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!waitForSeconds.TryGetValue(seconds, out wfs))
            waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
}
