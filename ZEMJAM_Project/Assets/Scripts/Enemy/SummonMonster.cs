using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMonster : EnemyDestroy
{
    public GameObject m_Sloth;
    public int Count;
    public override void AfterDestroy()
    {
        for (int i = 0; i < Count; i++)
        {
            Instantiate(m_Sloth, transform.position, Quaternion.identity);
            GameManager.Inst.m_EnemyCount++;
        }
    }
}