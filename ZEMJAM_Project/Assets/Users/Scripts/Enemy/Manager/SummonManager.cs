using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class SummonManager : MonoBehaviour
{
    public static SummonManager Inst { get; private set; }
    void Awake() => Inst = this;


    [Serializable]
    public struct EnemyPersent
    {
        public GameObject prefab;
        public int persent;
    }
    [Serializable]
    public struct SpawnPersent
    {
        public EnemyPersent[] enemyPersents;
    }
    public SpawnPersent[] stagePersents;

    public List<GameObject> enemyList;
    public GameObject[] itemPrfs;

    public Transform EnemyBundle;
    public Transform ItemBundle;

    public void SummonEnemy()
    {
        GameObject enemy = Instantiate(GetRandomEnemy(), TileManager.Inst.tiles[FindTileIndex()].position, Quaternion.identity);
        enemy.transform.SetParent(EnemyBundle);
        enemyList.Add(enemy);
    }

    public void SummonItem()
    {
        GameObject item = Instantiate(GetRandomItem(), TileManager.Inst.tiles[FindTileIndex()].position, Quaternion.identity);
        item.transform.SetParent(ItemBundle);
    }

    int FindTileIndex()
    {
        var preIndex = 0;
        do
        {
            preIndex = Random.Range(0, TileManager.Inst.tiles.Length);
        } while (TileManager.Inst.tiles[preIndex].onTile);

        return preIndex;
    }

    public GameObject GetRandomEnemy()
     {
        var enemy = stagePersents[GameManager.Inst.paze].enemyPersents;

        GameObject summon = null;
        int ran = Random.Range(0, 100);
        int temp = 0;

        for (int i = 0; i < enemy.Length; i++)
         {
             if (temp + enemy[i].persent > ran)
             {
                 summon = enemy[i].prefab;
                 break;
             }
             else
             {
                 temp += enemy[i].persent;
             }
         }

         return summon;
     }

    public GameObject GetRandomItem()
    {
        GameObject summon = itemPrfs[Random.Range(0, 5)];

        return summon;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if(enemyList.Contains(enemy))
            enemyList.Remove(enemy);
    }
}
