using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Gm;

    [Serializable]
    public class _2dArray
    {
        public int[] arr = new int[5];
    }

    public _2dArray[] persent = new _2dArray[5];

    public int[,] array = new int[5,5];

    public CinemachineVirtualCamera cinevirtual;

    public int TileSize;
    public int MaxPower;
    private Transform mPlayer;

    public Camera m_MainCamera;

    public GameObject m_Enemy;
    public GameObject m_Goblin;
    public GameObject m_BoomGoblin;
    public GameObject m_Mirror;
    public GameObject m_Ork;
    public GameObject m_ForestSpirit;

    public float m_EnemySummonCount;
    public int m_EnemyCount = 1;
    public int Managerhp;

    public GameObject m_BlockTile;
    public int[] m_BlockCount = new int[6];
    public GameObject m_DamageTile;
    public int[] m_DamageCount = new int[6];
    public GameObject[] m_Item;
    public int[] m_ItemCount = new int[6];


    public Transform mTile;

    private GameObject[] mTiles = new GameObject[150];

    public GameObject mWall_Left_Up;
    public GameObject mWall_Up;
    public GameObject mWall_Right_Up;

    public GameObject mWall_Up_Deco;
    public GameObject mWall_Left_Up_Deco;
    public GameObject mWall_Right_Up_Deco;

    public GameObject mWall_Right;
    public GameObject mWall_Left;

    public GameObject mWall_Down;
    public GameObject mWall_Left_Down;
    public GameObject mWall_Right_Down;

    public GameObject mWall_DownDown;

    private float real_CineSize;
    private float CinemacineSize;
    public bool isJoom;

    private bool isLoadScene;

    private int SummonCount = 2;

    public int paze;
    public int m_Score = 0;
    public Text m_ScoreText;

    private void Awake()
    {
        Screen.SetResolution(1080, 1920, true);
        var obj = FindObjectsOfType<GameManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Gm = this;
        mTile = GameObject.FindGameObjectWithTag("Wall").transform;
        mPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        mPlayer.GetComponent<Movement>().m_MaxPower = MaxPower;
        m_ScoreText = GameObject.Find("Score").GetComponent<Text>();
        cinevirtual = Camera.main.transform.GetComponentInChildren<CinemachineVirtualCamera>();
        m_MainCamera = Camera.main;
        SummonCount = 2;
        m_EnemyCount = 1;
        CinemacineSize = 3f + TileSize;
        mPlayer.GetComponent<Hp>().curhp = Managerhp;
        mPlayer.GetComponent<Hp>().OnHealth();        
        DrawMap();
        SummonEnemy(true);
        isLoadScene = false;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        CinemacineSize = isJoom ? (1.5f + TileSize/1.25f)/1.9f : 2.5f + TileSize / 1.25f;

        if (CinemacineSize < 6) CinemacineSize = 6;
        real_CineSize = Mathf.Lerp(real_CineSize, CinemacineSize, Time.deltaTime * 4);
        cinevirtual.m_Lens.OrthographicSize = real_CineSize;

        cinevirtual.Follow = isJoom ? mPlayer : mTile;

        if (m_EnemyCount <= 0)
        {
            if (SummonCount <= 0 && !isLoadScene)
            {
                isLoadScene = true;
                StartCoroutine(MoveScene());
            }
            else if(SummonCount > 0)
            {
                SummonCount--;
                SummonEnemy(false);
                m_EnemyCount++;
            }
        }

        m_ScoreText.text = m_Score.ToString();
    }
    IEnumerator MoveScene()
    {
        yield return new WaitForSeconds(1);
        Fade.instance.Fadein();
        yield return new WaitForSeconds(0.4f);
        m_EnemyCount = 1;
        if (TileSize < 15)
        {
            Managerhp = mPlayer.GetComponent<Hp>().curhp;
            MaxPower += 1;
            TileSize += 2;
            paze++;
            m_EnemySummonCount += 0.5f;
        }
        SceneManager.LoadScene(1);
    }
    private void DrawMap()
    {
        int j = 0;
        mTiles[j++] = Instantiate(mWall_Left_Up_Deco, new Vector3(-(TileSize - 1) / 2, (TileSize - 1) / 2 + 2), Quaternion.identity);
        mTiles[j++] = Instantiate(mWall_Left_Up, new Vector3(-(TileSize - 1) / 2, (TileSize - 1) / 2 + 1), Quaternion.identity);
        mTiles[j++] = Instantiate(mWall_Left_Down, new Vector3(-(TileSize - 1) / 2, -(TileSize - 1) / 2 - 6), Quaternion.identity);
        mTiles[j++] = Instantiate(mWall_Right_Up_Deco, new Vector3((TileSize - 1) / 2, (TileSize - 1) / 2 + 2), Quaternion.identity);
        mTiles[j++] = Instantiate(mWall_Right_Up, new Vector3((TileSize - 1) / 2, (TileSize - 1) / 2 - 1 + 2), Quaternion.identity);
        mTiles[j++] = Instantiate(mWall_Right_Down, new Vector3((TileSize - 1) / 2, -(TileSize - 1) / 2 - 6), Quaternion.identity);

        for (int i = (TileSize - 1) / 2; i >= -(TileSize - 1) / 2 -5; i--)
        {
            mTiles[j++] = Instantiate(mWall_Left, new Vector3(-(TileSize - 1) / 2, i), Quaternion.identity);
            mTiles[j++] = Instantiate(mWall_Right, new Vector3((TileSize - 1) / 2, i), Quaternion.identity);
        }

        for(int i = -(TileSize - 1) / 2 + 1; i <= (TileSize - 1) / 2 - 1; i++)
        {
            mTiles[j++] = Instantiate(mWall_Up, new Vector3(i, (TileSize - 1) / 2 + 1), Quaternion.identity);
            mTiles[j++] = Instantiate(mWall_Up_Deco, new Vector3(i, (TileSize - 1) / 2 + 2), Quaternion.identity);
            mTiles[j++] = Instantiate(mWall_Down, new Vector3(i, -(TileSize - 1) / 2 - 6), Quaternion.identity);
        }

        for (int i = -(TileSize - 1) / 2; i <= (TileSize - 1) / 2; i++)
        {
            mTiles[j++] = Instantiate(mWall_DownDown, new Vector3(i, -(TileSize - 1) / 2-7), Quaternion.identity);
        }

        for (int i = 0; i < j; i++)
        {
            mTiles[i].transform.SetParent(mTile);
        }
    }

    private void SummonEnemy(bool isTile)
    {
        int[,] summonPos = new int[TileSize, TileSize+2];

        for (int i = 0; i < TileSize; i++)
        {
            for (int j = 0; j < TileSize+2; j++)
            {
                summonPos[i, j] = 0;
            }
        }

        for(int i = 0; i < (int)m_EnemySummonCount; i++)
        {
            int m = Random.Range(0, TileSize), n = Random.Range(0, TileSize+2);
            while (true)
            {
                if(summonPos[m, n] == 0)
                {
                    summonPos[m, n]++;
                    break;
                }
                else
                {
                    m = Random.Range(0, TileSize);
                    n = Random.Range(0, TileSize + 2);
                }
            }
            Instantiate(SummonRandom(persent[paze].arr[0], persent[paze].arr[1], persent[paze].arr[2], persent[paze].arr[3], persent[paze].arr[4]), new Vector3(m - (TileSize - 1) / 2, n - (TileSize - 1) / 2 -2), Quaternion.identity);
            m_EnemyCount++;
        }
        m_EnemyCount--;

        if(isTile)
        {
            for (int i = 0; i < m_BlockCount[paze]; i++)
            {
                int k = Random.Range(0, TileSize - 2), l = Random.Range(0, TileSize + 1);
                while (true)
                {

                    if (summonPos[k, l] == 0)
                    {
                        summonPos[k, l]++;
                        break;
                    }
                    else
                    {
                        k = Random.Range(0, TileSize - 2);
                        l = Random.Range(0, TileSize + 1);
                    }
                }
                Instantiate(m_BlockTile, new Vector3(k - (TileSize - 1) / 2 + 1, l - (TileSize - 1) / 2 - 2), Quaternion.identity);
            }
            for (int i = 0; i < m_DamageCount[paze]; i++)
            {
                int k = Random.Range(0, TileSize - 2), l = Random.Range(0, TileSize + 1);
                while (true)
                {

                    if (summonPos[k, l] == 0)
                    {
                        summonPos[k, l]++;
                        break;
                    }
                    else
                    {
                        k = Random.Range(0, TileSize - 2);
                        l = Random.Range(0, TileSize + 1);
                    }
                }
                Instantiate(m_DamageTile, new Vector3(k - (TileSize - 1) / 2 + 1, l - (TileSize - 1) / 2 - 2), Quaternion.identity);
            }
            for (int i = 0; i < m_ItemCount[paze]; i++)
            {
                int k = Random.Range(0, TileSize - 2), l = Random.Range(0, TileSize + 1);
                while (true)
                {

                    if (summonPos[k, l] == 0)
                    {
                        summonPos[k, l]++;
                        break;
                    }
                    else
                    {
                        k = Random.Range(0, TileSize - 2);
                        l = Random.Range(0, TileSize + 1);
                    }
                }
                Instantiate(m_Item[Random.Range(0, 4)], new Vector3(k - (TileSize - 1) / 2 + 1, l - (TileSize - 1) / 2 - 2), Quaternion.identity);
            }
        }
    }

    private GameObject SummonRandom(int sloth, int goblin, int bom, int mirror, int ork)
    {
        GameObject summon = m_Enemy;
        int ran = Random.Range(1, 101);

        if (ran <= sloth) summon = m_Enemy;
        else if (ran > sloth && ran < sloth + goblin) summon = m_Goblin;
        else if (ran > sloth + goblin && ran < sloth + goblin + bom) summon = m_BoomGoblin;
        else if (ran > sloth + goblin + bom && ran < sloth + goblin + bom + mirror) summon = m_Mirror;
        else if (ran > sloth + goblin + bom + mirror && ran < sloth + goblin + bom + mirror + ork) summon = m_Ork;

        return summon;
    }
}
