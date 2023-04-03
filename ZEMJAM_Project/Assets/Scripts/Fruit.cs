using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int m_ChangeCount;
    public float m_Boost;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Movement>().m_Count += m_ChangeCount;
            collision.GetComponent<Movement>().m_BoostPower = 1.75f;
            Destroy(gameObject);
        }
    }
}
