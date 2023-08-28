using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magichat : Item
{
    public override void UseItem()
    {
        Movement.instance.m_BounceCount *= 2;
        Destroy(gameObject);
    }
}
