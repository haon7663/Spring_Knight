using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public int index;

    public abstract void UseItem();
    void OnDisable()
    {
        TileManager.Inst.tiles[index].onTile = false;
    }
}
