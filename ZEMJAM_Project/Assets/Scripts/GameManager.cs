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

    public int[,] array = new int[5, 5];    
    
    public LayerMask m_PlatformLayer;

    public CinemachineVirtualCamera cinevirtual;

    public int TileSize;
    public int MaxPower;
    private Transform mPlayer;

    public Camera m_MainCamera;

    public GameObject[] m_EnemyArray = new GameObject[15];
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

    int[,] summonPos = new int[30, 30];

    float tilesize;
    public int horizontalHalfPos;
    public int verticalHalfPos;
    public int horizontalPos;
    public int verticalPos;

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
        CinemacineSize = TileSize;
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
        mPlayer.GetComponent<Hp>().curhp = Managerhp;
        mPlayer.GetComponent<Hp>().OnHealth();
        StartCoroutine(DrawMap());
        int[,] summonPos = new int[TileSize, TileSize + 2];
        StartCoroutine(SummonEnemy(true));
        isLoadScene = false;
        CinemacineSize = TileSize;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        CinemacineSize = isJoom ? (1.5f + TileSize / 1.25f) / 1.9f : TileSize;

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
            else if (SummonCount > 0)
            {
                SummonCount--;
                StartCoroutine(SummonEnemy(false));
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
    private IEnumerator DrawMap()
    {
        yield return new WaitForSeconds(0.1f);
        tilesize = TileSize * 2;
        horizontalHalfPos = Mathf.CeilToInt(tilesize / 2f * 0.5625f);
        verticalHalfPos = Mathf.CeilToInt(tilesize / 2);

        int j = 0;

        if (Physics2D.OverlapCircle(new Vector2(horizontalHalfPos, 0), 0.1f, m_PlatformLayer)) horizontalHalfPos -= 1;
        if (Physics2D.OverlapCircle(new Vector2(0, verticalHalfPos), 0.1f, m_PlatformLayer)) verticalHalfPos -= 1;

        mTiles[j++] = Instantiate(mWall_Left_Up_Deco, new Vector3(-horizontalHalfPos, verticalHalfPos + 2), Quaternion.identity);
        mTiles[j++] = Instantiate(mWall_Left_Up, new Vector3(-horizontalHalfPos, verticalHalfPos+1), Quaternion.identity);
        mTiles[j++] = Instantiate(mWall_Left_Down, new Vector3(-horizontalHalfPos, -verticalHalfPos), Quaternion.identity);
        mTiles[j++] = Instantiate(mWall_Right_Up_Deco, new Vector3(horizontalHalfPos, verticalHalfPos + 2), Quaternion.identity);
        mTiles[j++] = Instantiate(mWall_Right_Up, new Vector3(horizontalHalfPos, verticalHalfPos+1), Quaternion.identity);
        mTiles[j++] = Instantiate(mWall_Right_Down, new Vector3(horizontalHalfPos, -verticalHalfPos), Quaternion.identity);

        for (int i = verticalHalfPos+1; i >= -verticalHalfPos; i--)
        {
            mTiles[j++] = Instantiate(mWall_Left, new Vector3(-horizontalHalfPos, i), Quaternion.identity);
            mTiles[j++] = Instantiate(mWall_Right, new Vector3(horizontalHalfPos, i), Quaternion.identity);
        }

        for (int i = -horizontalHalfPos; i <= horizontalHalfPos; i++)
        {
            mTiles[j++] = Instantiate(mWall_Up, new Vector3(i, verticalHalfPos+1), Quaternion.identity);
            mTiles[j++] = Instantiate(mWall_Up_Deco, new Vector3(i, verticalHalfPos + 2), Quaternion.identity);
            mTiles[j++] = Instantiate(mWall_Down, new Vector3(i, -verticalHalfPos), Quaternion.identity);
            mTiles[j++] = Instantiate(mWall_DownDown, new Vector3(i, -verticalHalfPos - 1), Quaternion.identity);
        }

        for (int i = 0; i < j; i++)
        {
            mTiles[i].transform.SetParent(mTile);
        }
        yield return null;
    }

    private IEnumerator SummonEnemy(bool isTile)
    {
        yield return new WaitForSeconds(0.15f);
        horizontalPos = horizontalHalfPos * 2 + 1;
        verticalPos = verticalHalfPos * 2;

        for (int i = 0; i < horizontalPos; i++)
        {
            for (int j = 0; j < verticalPos; j++)
            {
                summonPos[i, j] = 0;
            }
        }

        for (int i = 0; i < (int)m_EnemySummonCount; i++)
        {
            int m = Random.Range(0, horizontalPos), n = Random.Range(0, verticalPos);
            while (true)
            {
                if (summonPos[m, n] == 0)
                {
                    summonPos[m, n]++;
                    break;
                }
                else
                {
                    m = Random.Range(0, horizontalPos);
                    n = Random.Range(0, verticalPos);
                }
            }
            GameObject enemy = Instantiate(SummonRandom(persent[paze].arr[0], persent[paze].arr[1], persent[paze].arr[2], persent[paze].arr[3], persent[paze].arr[4]), new Vector3(m - horizontalHalfPos, n - verticalHalfPos + 1), Quaternion.identity);
            m_EnemyArray[m_EnemyCount-1] = enemy;
            enemy.GetComponent<Enemy>().m_Count = m_EnemyCount-1;
            m_EnemyCount++;
        }
        m_EnemyCount--;

        if (isTile)
        {
            for (int i = 0; i < m_BlockCount[paze]; i++)
            {
                int k = Random.Range(0, horizontalPos), l = Random.Range(0, verticalPos);
                while (true)
                {

                    if (summonPos[k, l] == 0)
                    {
                        summonPos[k, l]++;
                        break;
                    }
                    else
                    {
                        k = Random.Range(0, horizontalPos);
                        l = Random.Range(0, verticalPos);
                    }
                }
                Instantiate(m_BlockTile, new Vector3(k - horizontalHalfPos, l - verticalHalfPos + 1), Quaternion.identity);
            }
            for (int i = 0; i < m_DamageCount[paze]; i++)
            {
                int k = Random.Range(0, horizontalPos), l = Random.Range(0, verticalPos);
                while (true)
                {

                    if (summonPos[k, l] == 0)
                    {
                        summonPos[k, l]++;
                        break;
                    }
                    else
                    {
                        k = Random.Range(0, horizontalPos);
                        l = Random.Range(0, verticalPos);
                    }
                }
                Instantiate(m_DamageTile, new Vector3(k - horizontalHalfPos, l - verticalHalfPos + 1), Quaternion.identity);
            }
            for (int i = 0; i < m_ItemCount[paze]; i++)
            {
                int k = Random.Range(0, horizontalPos), l = Random.Range(0, verticalPos);
                while (true)
                {

                    if (summonPos[k, l] == 0)
                    {
                        summonPos[k, l]++;
                        break;
                    }
                    else
                    {
                        k = Random.Range(0, horizontalPos);
                        l = Random.Range(0, verticalPos);
                    }
                }
                Instantiate(m_Item[Random.Range(0, 4)], new Vector3(k - horizontalHalfPos, l - verticalHalfPos + 1), Quaternion.identity);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector2(horizontalHalfPos, 0), 0.1f);
        Gizmos.DrawWireSphere(new Vector2(0, verticalHalfPos), 0.1f);
    }
}
