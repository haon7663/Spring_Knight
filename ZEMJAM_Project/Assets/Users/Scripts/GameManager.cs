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
    public Character m_Character;

    public GameMode m_GameMode;
    public GameState m_GameState;

    public bool isInGame;
    public bool isLoadScene;
    public bool isSetting;

    public int gold;

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
    public int maxHealth = 3;
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
    public List<Sprite> selectedPropertySprite;
    public delegate void KillEnemyAction();
    public static event KillEnemyAction KillEnemy;
    public delegate void SpawnEnemyAction(EnemyDefence enemy);
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

        if (SceneManager.GetActiveScene().name != "InGame") return;

        m_Character = GetComponent<Character>();
        m_PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_PlayerController.maxPower = managePower;
        maxPower = managePower;
        maxPaze = TileManager.Inst.stageTileMaps[curStage].tileMaps.Length;

        HealthManager.Inst.SetHealth(maxHealth);
        HealthManager.Inst.curhp = manageHealth;
        HealthManager.Inst.OnHealth(0);

        UIManager.Inst.SetPaze(curPaze, maxPaze);

        ChangeMode(GameMode.INFINITE);

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
        if (!isInGame) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (SummonManager.Inst.enemyList.Count <= 0)
        {
            if (summonCount <= 0 && !isLoadScene)
            {
                if(curPaze == maxPaze - 1)
                {
                    StartCoroutine(UIManager.Inst.ShowResultPanel(true));
                    return;
                }
                isLoadScene = true;
                StartCoroutine(MoveScene());
            }
            else if (summonCount > 0)
            {
                summonCount--;

                var isAssassin = m_Character.playerType == PlayerType.ASSASSIN;
                for (int i = 0; i < (int)enemySummonCount; i++)
                {
                    var enemy = SummonManager.Inst.SummonEnemy();
                    if (isAssassin && i == 0)
                        enemy.AddComponent<AssassinMark>();
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
        maxHealth = 3;
        manageHealth = 3;
        enemySummonCount = 3;

        ResetProperty();
    }
    
    IEnumerator MoveScene()
    {
        ChangeState(GameState.LOADING);
        yield return new WaitForSeconds(1);
        Fade.Inst.Fadein();
        yield return new WaitForSeconds(0.3f);

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
        maxHealth++;
        HealthManager.Inst.SetHealth(maxHealth);
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
        if (Calculate(killGoldPersent))
            Debug.Log("Gold");
    }
    void KillDecDef()
    {
        Debug.Log("DecreaseDef");
    }
    #endregion
    #region SpawnEvent

    public void SpawnEvent(EnemyDefence enemy)
    {
        SpawnEnemy?.Invoke(enemy);
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
    void SpawnDecDef(EnemyDefence enemy)
    {
        if (Calculate(spawnDecDefPersent))
        {
            enemy.SetDefence(-1);
        }
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
