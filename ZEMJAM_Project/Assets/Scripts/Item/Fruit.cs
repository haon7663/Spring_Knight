using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Item
{
    public int m_Count;

    public override void UseItem()
    {
        Movement.instance.m_Count += m_Count;
        Destroy(gameObject);
    }
}
