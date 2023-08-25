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

    public Hp m_Health;
    public Movement m_Movement;

    public bool doSetting;
    public int maxPower;

    public float enemySummonCount;
    public int Managerhp;

    public int paze;
    public int m_Score = 0;
    public Text m_ScoreText;

    bool isLoadScene;
    int SummonCount = 3;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        var obj = FindObjectsOfType<GameManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
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

        m_Movement.m_MaxPower = maxPower;
        m_Health.curhp = Managerhp;
        m_Health.OnHealth();

        isLoadScene = false;
        doSetting = false;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (SummonManager.Inst.enemyList.Count <= 0)
        {
            if (SummonCount <= 0 && !isLoadScene)
            {
                isLoadScene = true;
                StartCoroutine(MoveScene());
            }
            else if (SummonCount > 0)
            {
                SummonCount--;
                for (int i = 0; i < (int)enemySummonCount; i++)
                {
                    SummonManager.Inst.SummonEnemy();
                }
                SummonManager.Inst.SummonItem();
            }
        }

        if(m_ScoreText) m_ScoreText.text = m_Score.ToString();
    }
    IEnumerator MoveScene()
    {
        yield return new WaitForSeconds(1);
        Fade.instance.Fadein();
        yield return new WaitForSeconds(0.4f);
        if (TileManager.Inst.tileSize < 13)
        {
            Managerhp = m_Health.curhp;
            TileManager.Inst.tileSize += 2;
            enemySummonCount += 0.75f;
            maxPower += 1;
            paze++;
        }
        SceneManager.LoadScene("Faze");
    }
    /*private IEnumerator DrawMap()
    {
        yield return new WaitForSeconds(0.1f);
        tilesize = TileManager.Inst.tileSize * 2;
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
    }*/

    /*private IEnumerator SummonEnemy(bool isTile)
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
            int m = Random.Range(1, horizontalPos-1), n = Random.Range(1, verticalPos-1);
            while (true)
            {
                if (summonPos[m, n] == 0)
                {
                    summonPos[m, n]++;
                    break;
                }
                else
                {
                    m = Random.Range(1, horizontalPos-1);
                    n = Random.Range(1, verticalPos-1);
                }
            }
            GameObject enemy = Instantiate(SummonRandom(stagePersent[paze].persent), new Vector3(m - horizontalHalfPos, n - verticalHalfPos + 1), Quaternion.identity);
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
                Instantiate(m_Item[Random.Range(0, 5)], new Vector3(k - horizontalHalfPos, l - verticalHalfPos + 1), Quaternion.identity);
            }
        }
    }

    private GameObject SummonRandom(int[] enemy)
    {
        GameObject summon = m_Enemy[0];
        int ran = Random.Range(1, 101);
        int temp = 0;

        for(int i = 0; i < enemy.Length; i++)
        {
            if(temp + enemy[i] > ran)
            {
                summon = m_Enemy[i];
                break;
            }
            else
            {
                temp += enemy[i];
            }
        }

        return summon;
    }*/


    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(Vector2.zero, new Vector2(TileManager.Inst.tileSize - 1, (TileManager.Inst.tileSize - 1)*2));
    }
}
