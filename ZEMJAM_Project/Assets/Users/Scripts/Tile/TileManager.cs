using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    public static TileManager Inst { get; private set; }
    void Awake() => Inst = this;

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
    [SerializeField] Transform tileGrid;
    [SerializeField] Transform cinemachineConfiner;

    void Start()
    {
        var tilemap = stageTileMaps[GameManager.Inst.curStage].tileMaps[GameManager.Inst.curPaze];
        GameObject tile = Instantiate(tilemap);
        tile.transform.SetParent(tileGrid);

        var tileIndex = int.Parse(tile.name.Substring(0, tile.name.IndexOf('-')));
        tileSize = tileIndex;
        float scale = tileIndex / 7f;
        cinemachineConfiner.localScale = new Vector3(scale + 0.05f, scale + 0.05f, 1);
        Movement.Inst.tileMultiSpeed = Mathf.Lerp(scale, 1, 0.25f);

        var sizeX = tileSize - 3;
        var sizeY = sizeX * 2;
        tiles = new Tile[sizeX * sizeY];

        for (int i = 0; i < tiles.Length; i++)
        {
            int quotient = Mathf.FloorToInt((float)i / sizeX);
            int remain = i % sizeX;
            float posX = -sizeX / 2 + remain + 0.5f;
            float posY = sizeY / 2 - quotient - 0.5f;

            tiles[i].position = new Vector2(posX, posY);
        }
    }

    public void TakeTile(int value, bool onTile)
    {
        tiles[value].onTile = onTile;
    }
}
