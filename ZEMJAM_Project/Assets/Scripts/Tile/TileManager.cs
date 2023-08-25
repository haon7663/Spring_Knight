using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    public static TileManager Inst { get; set; }

    [Serializable]
    public struct Tiles { public GameObject[] tileMaps; }
    public Tiles[] stageTiles;

    public int tileSize;

    void Awake() => Inst = this;

    void Start()
    {
        var tilemap = stageTiles[GameManager.Inst.paze].tileMaps;
        Instantiate(tilemap[Random.Range(0, tilemap.Length)]);
    }
}
