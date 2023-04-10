using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DefenceKnight : EnemyDashSign
{
    public int m_MaxPower;
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
        if(mEnemy.m_Power < m_MaxPower)
        {
            curStack++;
            if(curStack >= m_MaxStack)
            {
                curStack = 0;
                mEnemy.m_Power++;
                mDefence.m_Defence++;
                mDefence.DefencePos();
                mDefence.DefPos *= 1.1f;
                transform.DOScale(transform.localScale*1.1f, 0.4f).SetEase(Ease.OutCubic);
            } 
        }
    }
}
