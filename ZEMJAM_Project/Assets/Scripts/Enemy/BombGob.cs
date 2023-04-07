using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGob : EnemyDestroy
{
    public GameObject BombParticle;

    public override void AfterDestroy()
    {
        Instantiate(BombParticle, transform.position, Quaternion.identity);
        Movement player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        player.m_BoostPower = 3;
        player.m_Count += 3;
    }
}
