using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGoblin : EnemyDestroy
{
    public override void AfterDestroy()
    {
        Instantiate(GameManager.Gm.m_Item[Random.Range(0, GameManager.Gm.m_Item.Length)], transform.position, Quaternion.identity);
    }
}
