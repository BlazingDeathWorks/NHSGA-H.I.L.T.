using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private int layoutCount;
    [SerializeField]
    private RuleTile[] ruleTiles;
    [SerializeField]
    private Tilemap[] maps;
    [SerializeField]
    private Tilemap bgMap;
    [SerializeField]
    private RuleTile bgTile;
    [SerializeField]
    private RuleTile[] enemyTiles;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private GameObject[] layouts;

    private int layoutPos;
    private int[] layoutSeed;
    private int layoutSeedIndex;

    void Start()
    {
        //TODO CHANGE THIS
        Physics2D.IgnoreLayerCollision(8, 8);

        layoutPos = 0;
        layoutSeed = new int[layouts.Length];
        for (int i = 0; i < layoutSeed.Length; i++)
        {
            layoutSeed[i] = i;
        }

        //randomize layouts
        for (int i = 0; i < layoutCount; i++)
        {
            int randIndex = Random.Range(i, layoutSeed.Length);
            int hold = layoutSeed[i];
            layoutSeed[i] = layoutSeed[randIndex];
            layoutSeed[randIndex] = hold;
        }
        layoutSeedIndex = 0;
        while (layoutCount-- > 0)
        {
            GameObject layout = layouts[layoutSeed[layoutSeedIndex++]];
            Tilemap tilemap = layout.GetComponent<Tilemap>();
            Tilemap bgTilemap = layout.GetComponentsInChildren<Tilemap>()[1];
            BoundsInt bounds = tilemap.cellBounds;

            //copy from layout
            for (int r = bounds.min.x; r <= bounds.max.x; r++)
            {
                for (int c = bounds.min.y; c <= bounds.max.y; c++)
                {
                    GenerateTile(tilemap, bgTilemap, bounds, r, c);
                }
            }
            for (int r = bounds.min.x; r <= bounds.max.x; r++)
            {
                for (int c = bounds.min.y; c <= bounds.max.y; c++)
                {
                    GenerateEnemy(tilemap, bounds, r, c);
                }
            }
            layoutPos += bounds.size.x;
        }

        //Create side walls and floor
        BoxFill(maps[0], ruleTiles[0], -30, -10, -40, 60);
        BoxFill(maps[0], ruleTiles[0], -10, 0, -40, 0);
        BoxFill(maps[0], ruleTiles[0], 0, layoutPos, -40, -14);
        BoxFill(maps[0], ruleTiles[0], layoutPos, layoutPos + 20, -40, 60);
    }

    private void GenerateTile(Tilemap tilemap, Tilemap bgTilemap, BoundsInt bounds, int r, int c)
    {
        Vector3Int tempPos = new Vector3Int(r, c);
        //check background
        if (bgTilemap.GetTile(tempPos) == bgTile)
        {
            bgMap.SetTile(tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), bgTile);
        }
        //check main tiles
        for (int i = 0; i < ruleTiles.Length; i++)
        {
            if (tilemap.GetTile(tempPos) == ruleTiles[i])
            {
                maps[i].SetTile(tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), ruleTiles[i]);
                continue;
            }
        }
    }
    private void GenerateEnemy(Tilemap tilemap, BoundsInt bounds, int r, int c)
    {
        Vector3Int tempPos = new Vector3Int(r, c);
        //check enemy tiles
        for (int i = 0; i < enemyTiles.Length; i++)
        {
            if (tilemap.GetTile(tempPos) == enemyTiles[i])
            {
                Instantiate(enemyPrefabs[i], tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), Quaternion.identity);
                continue;
            }
        }
    }

    private void BoxFill(Tilemap map, TileBase tile, int startX, int endX, int startY, int endY)
    {
        for (var x = startX; x < endX; x++)
        {
            for (var y = startY; y < endY; y++)
            {
                var tilePos = new Vector3Int(x, y, 0);
                map.SetTile(tilePos, tile);
            }
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        Camera.main.transform.position = GameObject.Find("Player").transform.position + new Vector3(0, 0, -10);
    }
}
