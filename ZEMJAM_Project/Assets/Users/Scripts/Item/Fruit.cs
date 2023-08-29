using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Item
{
    public int m_Count;

    public override void UseItem()
    {
        Movement.Inst.count += m_Count;
        Destroy(gameObject);
    }
}
