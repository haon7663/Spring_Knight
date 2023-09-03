using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; set; }

    public PlayerController m_PlayerController;

    public bool isLoadScene;
    public bool isSetting;

    public int paze;
    float enemySummonCount;
    int managerHealth = 3;
    int managerPower = 5;
    int summonCount = 3;

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
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;

        m_PlayerController.maxPower = managerPower;
        HealthManager.Inst.SetHealth(3);
        HealthManager.Inst.curhp = managerHealth;
        HealthManager.Inst.OnHealth(1);

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
        paze = 0;
        managerHealth = 3;
        enemySummonCount = 3;
        TileManager.Inst.tileSize = 7;
    }
    
    IEnumerator MoveScene()
    {
        yield return new WaitForSeconds(1);
        Fade.instance.Fadein();
        yield return new WaitForSeconds(0.4f);
        if (TileManager.Inst.tileSize < 13)
        {
            managerHealth = HealthManager.Inst.curhp;
            TileManager.Inst.tileSize += 2;
            enemySummonCount += 0.75f;
            managerPower += 1;
            paze++;
        }
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
