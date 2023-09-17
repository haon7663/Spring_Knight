using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { LOADING, PAUSE, PLAY, DEATH }
public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; set; }

    public PlayerController m_PlayerController;

    public GameState m_GameState;

    public bool isLoadScene;
    public bool isSetting;

    [Space]
    [Header("Stats")]
    public bool onPlay;
    public bool onPause;
    public bool onDeath;

    [Space]
    [Header("Stats")]
    public int curPaze;
    public int maxPaze;
    public int managerHealth = 3;
    public int managerPower = 3;
    public int summonCount = 3;
    public float enemySummonCount;

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
        m_PlayerController.maxPower = managerPower;

        HealthManager.Inst.SetHealth(2);
        HealthManager.Inst.curhp = managerHealth;
        HealthManager.Inst.OnHealth(1);

        UIManager.Inst.SetPowerGrid(managerPower);
        UIManager.Inst.SetPaze(curPaze, maxPaze);

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
                SummonManager.Inst.SummonItem();

                isLoadScene = false;
            }
        }
    }

    public void SetGame()
    {
        curPaze = 0;
        managerPower = 3;
        managerHealth = 3;
        enemySummonCount = 3;
        TileManager.Inst.tileSize = 7;
    }
    
    IEnumerator MoveScene()
    {
        ChangeState(GameState.LOADING);
        yield return new WaitForSeconds(1);
        Fade.Inst.Fadein();
        yield return new WaitForSeconds(0.1f);

        managerHealth = HealthManager.Inst.curhp;
        TileManager.Inst.tileSize += 2;
        enemySummonCount += 0.75f;
        managerPower += 1;
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
