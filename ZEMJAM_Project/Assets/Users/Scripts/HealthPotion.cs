using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    public override void UseItem()
    {
        HealthManager.Inst.OnHealth(1);
        Destroy(gameObject);
    }
}
