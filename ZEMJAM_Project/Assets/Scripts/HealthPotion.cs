using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    public override void UseItem()
    {
        Movement.instance.GetComponent<Hp>().OnHealth();
        Destroy(gameObject);
    }
}
