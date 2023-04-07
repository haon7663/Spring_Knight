using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceKnight : EnemyDashSign
{
    public int m_MaxStack;
    private int curStack;
    private Enemy mEnemy;
    private Defence mDefence;
    private void Start()
    {
        mEnemy = GetComponent<Enemy>();
        mDefence = GetComponent<Defence>();
    }
    public override void AfterDash()
    {
        curStack++;
        if(curStack >= m_MaxStack)
        {
            curStack = 0;
            mEnemy.m_Power++;
            mDefence.DefencePos();
        } 
            
    }
}
