using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int curPaze;
    public int maxPaze;
    public int maxHealth = 3;
    public int maxPower = 3;
    public int summonCount = 3;
    public float enemySummonCount;

    [Space]
    [Header("Time")]
    public float maxTimer = 45;

    [Space]
    [Header("Score")]
    public int score;

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
        m_PlayerController.maxPower = maxPower;

        HealthManager.Inst.SetHealth(3);
        HealthManager.Inst.curhp = maxHealth;
        HealthManager.Inst.OnHealth(1);

        UIManager.Inst.SetPowerGrid(maxPower);
        UIManager.Inst.SetPaze(curPaze, maxPaze);
        ChangeMode(m_GameMode);

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
        maxPower = 3;
        maxHealth = 3;
        enemySummonCount = 3;
        TileManager.Inst.tileSize = 7;
    }
    
    IEnumerator MoveScene()
    {
        ChangeState(GameState.LOADING);
        yield return new WaitForSeconds(1);
        Fade.Inst.Fadein();
        yield return new WaitForSeconds(0.1f);

        maxHealth = HealthManager.Inst.curhp;
        TileManager.Inst.tileSize += 2;
        enemySummonCount += 0.75f;
        maxPower += 1;
        summonCount = 3;
        curPaze++;

        SceneManager.LoadScene("InGame");
    }
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
