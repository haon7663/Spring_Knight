using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvPotion : Item
{
    public override void UseItem()
    {
        PlayerState.Inst.SetBarrier(true);
        Destroy(gameObject);
    }
}
