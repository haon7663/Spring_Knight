using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotFrashFruit : Item
{
    public override void UseItem()
    {
        Movement.instance.m_Count = 0;
        Destroy(gameObject);
    }
}
