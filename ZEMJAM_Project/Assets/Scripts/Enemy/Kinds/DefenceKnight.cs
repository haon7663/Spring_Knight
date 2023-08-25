using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DefenceKnight : EnemyDashSign
{
    public int maxPower;
    public int maxStack;

    int curStack;
    Enemy enemy;
    Defence defence;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        defence = GetComponent<Defence>();
    }
    public override void AfterDash()
    {
        if(enemy.m_Power < maxPower)
        {
            curStack++;
            if(curStack >= maxStack)
            {
                curStack = 0;

                enemy.m_Power++;
                defence.defence++;
                defence.SetDefence();

                transform.DOScale(transform.localScale*1.1f, 0.4f).SetEase(Ease.OutCubic);
            } 
        }
    }
}
