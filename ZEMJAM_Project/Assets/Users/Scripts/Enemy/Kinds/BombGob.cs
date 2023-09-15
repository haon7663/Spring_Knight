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
        player.boostPower = 3;
        player.count += 3;
        UIManager.Inst.SetPower(player.count);
    }
}
