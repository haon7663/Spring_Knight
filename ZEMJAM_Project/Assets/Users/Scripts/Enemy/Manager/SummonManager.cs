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

    public Transform enemyBundle;
    public Transform itemBundle;

    public void SummonEnemy()
    {
        var index = FindTileIndex();
        GameObject enemy = Instantiate(GetRandomEnemy(), TileManager.Inst.tiles[index].position, Quaternion.identity);
        enemy.transform.SetParent(enemyBundle);
        var scriptBundle = enemy.GetComponent<EnemyBundle>();
        scriptBundle.defence.index = index;

        enemyList.Add(enemy);
    }

    public void SummonItem()
    {
        var index = FindTileIndex();
        GameObject item = Instantiate(GetRandomItem(), TileManager.Inst.tiles[index].position, Quaternion.identity);
        item.transform.SetParent(itemBundle);

    }

    public int FindTileIndex(bool isTakeTile = true)
    {
        int preIndex;
        do
        {
            preIndex = Random.Range(0, TileManager.Inst.tiles.Length);
        } while (TileManager.Inst.tiles[preIndex].onTile);

        if(isTakeTile)
            TileManager.Inst.TakeTile(preIndex, true);
        return preIndex;
    }

    public GameObject GetRandomEnemy()
     {
        var enemy = stagePersents[GameManager.Inst.curPaze].enemyPersents;

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
        GameObject summon = itemPrfs[Random.Range(0, itemPrfs.Length)];

        return summon;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if(enemyList.Contains(enemy))
            enemyList.Remove(enemy);
    }
}
