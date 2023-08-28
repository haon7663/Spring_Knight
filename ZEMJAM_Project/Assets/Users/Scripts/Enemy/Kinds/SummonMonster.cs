using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMonster : EnemyDestroy
{
    public GameObject entity;
    public int Count;
    public override void AfterDestroy()
    {
        for (int i = 0; i < Count; i++)
        {
            GameObject summonedEntity = Instantiate(entity, transform.position, Quaternion.identity);
            SummonManager.Inst.enemyList.Add(summonedEntity);
        }
    }
}