using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class SummonManager : MonoBehaviour
{
    public static SummonManager Inst { get; private set; }

    [Serializable]
    public struct EnemyPersent
    {
        public GameObject prefab;
        public int persent;
    }
    [Serializable]
    public struct SpawnPersent
    {
        public int[] persent;
    }
    public SpawnPersent[] stagePersent;

    public List<GameObject> enemyList;
    public GameObject[] enemyPrfs;
    public GameObject[] itemPrfs;

    void Awake() => Inst = this;

    public void SummonEnemy()
     {
        var preIndex = 0;
        do
        {
            preIndex = Random.Range(0, TileManager.Inst.tiles.Length);
        } while (TileManager.Inst.tiles[preIndex].onTile);

        GameObject enemy = Instantiate(GetRandomEnemy(stagePersent[GameManager.Inst.paze].persent), TileManager.Inst.tiles[preIndex].position, Quaternion.identity);
        enemy.GetComponent<Enemy>().m_Count = enemyList.Count;
        enemyList.Add(enemy);
    }

    public void SummonItem()
    {
        var preIndex = 0;
        do
        {
            preIndex = Random.Range(0, TileManager.Inst.tiles.Length);
        } while (TileManager.Inst.tiles[preIndex].onTile);

        Instantiate(GetRandomItem(), TileManager.Inst.tiles[preIndex].position, Quaternion.identity);
    }

    public GameObject GetRandomEnemy(int[] enemy)
     {
         GameObject summon = enemyPrfs[0];
         int ran = Random.Range(0, 100);
         int temp = 0;

         for (int i = 0; i < enemyPrfs.Length; i++)
         {
             if (temp + enemy[i] > ran)
             {
                 summon = enemyPrfs[i];
                 break;
             }
             else
             {
                 temp += enemy[i];
             }
         }

         return summon;
     }

    public GameObject GetRandomItem()
    {
        GameObject summon = itemPrfs[Random.Range(0, 5)];

        return summon;
    }
}
