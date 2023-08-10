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
    private RuleTile[] actualTiles;
    [SerializeField]
    private Tilemap[] maps;
    [SerializeField]
    private Tilemap minimap;
    [SerializeField]
    private RuleTile[] minimapTiles;
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
    [SerializeField]
    private RuleTile pickupTile;
    [SerializeField]
    private GameObject pickupPrefab;
    [SerializeField]
    private GameObject doorPrefab;
    [SerializeField]
    private GameObject ShopPrefab;
    [SerializeField]
    private float ScalerIncrease;
    [SerializeField]
    private bool canTank;

    private int layoutPos;
    private int[] layoutSeed;
    private int layoutSeedIndex;
    private float holdLayoutCount;
    private List<GameObject> enemiesInLayout;

    void Start()
    {

        layoutPos = 0;
        switch (PlayerPrefs.GetInt("difficulty"))
        {
            case 0:
                layoutCount = 3;
                break;
            case 1:
                break;
            case 2:
                ScalerIncrease += .5f;
                break;
            case 3:
                ScalerIncrease += 1f;
                break;
        }
        holdLayoutCount = layoutCount;
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
            enemiesInLayout = new List<GameObject>();
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
                    GenerateEnemy(tilemap, bounds, r, c);
                }
            }

            //set elite
            if (enemiesInLayout.Count > 0)
            {
                int randEnemy = Random.Range(0, enemiesInLayout.Count);
                enemiesInLayout[randEnemy].GetComponent<Enemy>().SetElite();
            }

            //set up next layout
            layoutPos += bounds.size.x;
            if(layoutCount == 1)
            {
                BoxFill(maps[0], actualTiles[0], layoutPos, layoutPos + 20, -40, 0);
                BoxFill(maps[0], actualTiles[0], layoutPos + 5, layoutPos + 15, 6, 7);
                BoxFill(bgMap, actualTiles[3], layoutPos + 5, layoutPos + 15, 0, 6);
                BoxFill(minimap, minimapTiles[0], layoutPos, layoutPos + 20, -40, 0);
                BoxFill(minimap, minimapTiles[0], layoutPos + 5, layoutPos + 15, 6, 7);
                BoxFill(minimap, minimapTiles[3], layoutPos + 5, layoutPos + 15, 0, 6);
                Instantiate(ShopPrefab, new Vector3(layoutPos + 10, 0), Quaternion.identity);
                layoutPos += 20;
            }
        }

        //Create side walls and floor
        BoxFill(maps[0], actualTiles[0], -30, -10, -40, 60);
        BoxFill(maps[0], actualTiles[0], -10, 0, -40, 0);
        BoxFill(maps[0], actualTiles[0], 0, layoutPos, -40, -14);
        BoxFill(maps[0], actualTiles[0], layoutPos, layoutPos + 20, -40, 0);
        BoxFill(maps[0], actualTiles[0], layoutPos + 20, layoutPos + 40, -40, 60);
        Instantiate(doorPrefab, new Vector3(layoutPos + 10, 0), Quaternion.identity);
        Tilemap holdMap = Instantiate(maps[0], transform);
        holdMap.gameObject.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Polygons;

        //put it on minimap
        BoxFill(minimap, minimapTiles[0], -60, -10, -60, 60);
        BoxFill(minimap, minimapTiles[0], -10, 0, -60, 0);
        BoxFill(minimap, minimapTiles[0], 0, layoutPos, -60, -14);
        BoxFill(minimap, minimapTiles[0], layoutPos, layoutPos + 20, -60, 0);
        BoxFill(minimap, minimapTiles[0], layoutPos + 20, layoutPos + 80, -60, 60);
    }

    private void GenerateTile(Tilemap tilemap, Tilemap bgTilemap, BoundsInt bounds, int r, int c)
    {
        Vector3Int tempPos = new Vector3Int(r, c);
        //check background
        if (bgTilemap.GetTile(tempPos) == bgTile)
        {
            bgMap.SetTile(tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), actualTiles[3]);
            minimap.SetTile(tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), minimapTiles[3]);
        }
        //check main tiles
        for (int i = 0; i < ruleTiles.Length; i++)
        {
            if (tilemap.GetTile(tempPos) == ruleTiles[i])
            {
                maps[i].SetTile(tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), actualTiles[i]);
                minimap.SetTile(tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), minimapTiles[i]);
                continue;
            }
        }
        //check pickup tile
        if (tilemap.GetTile(tempPos) == pickupTile && Random.Range(0f, 1f) < .85f)
        {
            Instantiate(pickupPrefab, tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), Quaternion.identity);
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
                //randomize existence
                if (i == 2 && !canTank) continue;
                if (Random.Range(0f, 1f) > layoutCount / holdLayoutCount * .7f)
                {
                    //randomize type
                    int randType = Mathf.FloorToInt(i + enemyPrefabs.Length + Random.Range(-.1f, 1.1f)) % enemyPrefabs.Length;
                    //instantiate
                    GameObject holdEnemy = Instantiate(enemyPrefabs[randType], 
                        tempPos + new Vector3(layoutPos - bounds.min.x, .1f), Quaternion.identity);
                    //randomize stats
                    holdEnemy.GetComponent<Enemy>().SetStats(ScalerIncrease + (.5f-layoutCount / holdLayoutCount / 2f));
                    //add to elite calculation
                    enemiesInLayout.Add(holdEnemy);
                }
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
}
