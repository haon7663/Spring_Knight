using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGob : MonoBehaviour
{
    public GameObject BombParticle;

    public void SumBomb()
    {
        Instantiate(BombParticle, transform.position, Quaternion.identity);
        Movement player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        player.takeBomb(transform);
    }    
}
