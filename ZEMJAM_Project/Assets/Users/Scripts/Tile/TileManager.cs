using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    public static TileManager Inst { get; private set; }

    [Serializable]
    public struct TileMaps { public GameObject[] tileMaps; }
    public TileMaps[] stageTileMaps;

    [Serializable]
    public struct Tile
    { 
        public bool onTile;
        public Vector2 position;
    }
    public Tile[] tiles;

    public int tileSize;

    void Awake() => Inst = this;

    void Start()
    {
        var sizeX = tileSize - 1;
        var sizeY = sizeX*2;
        tiles = new Tile[sizeX * sizeY];

        for (int i = 0; i < tiles.Length; i++)
        {
            int quotient = Mathf.FloorToInt((float)i / sizeX);
            int remain = i % sizeX;
            float posX = -sizeX / 2 + remain + 0.5f;
            float posY = sizeY / 2 - quotient - 0.5f;

            tiles[i].position = new Vector2(posX, posY);
        }

        var tilemap = stageTileMaps[GameManager.Inst.curPaze].tileMaps;
        Instantiate(tilemap[Random.Range(0, tilemap.Length)]);
    }

    public void TakeTile(int value, bool onTile)
    {
        tiles[value].onTile = onTile;
    }

    public int GetOnTileCount()
    {
        int value = 0;
        for(int i = 0; i < tiles.Length; i++)
            if (tiles[i].onTile)
                value++;
        return value;
    }
}
