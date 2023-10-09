using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvPotion : PrizeInformation
{
    public override void UseItem()
    {
        PlayerState.Inst.SetItem(true);
        Destroy(gameObject);
    }
}
