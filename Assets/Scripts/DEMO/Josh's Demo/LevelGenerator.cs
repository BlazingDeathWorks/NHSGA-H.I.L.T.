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
    private GameObject doorPrefab;

    private int layoutPos;
    private int[] layoutSeed;
    private int layoutSeedIndex;
    private float holdLayoutCount;
    private List<GameObject> enemiesInLayout;

    void Start()
    {
        //TODO CHANGE THIS
        Physics2D.IgnoreLayerCollision(8, 8);

        layoutPos = 0;
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
                enemiesInLayout[randEnemy].GetComponent<EnemyController>().SetElite();
                enemiesInLayout[randEnemy].AddComponent<KeyHolder>();
            }

            //set up next layout
            layoutPos += bounds.size.x;
        }

        //Create side walls and floor
        BoxFill(maps[0], ruleTiles[0], -30, -10, -40, 60);
        BoxFill(maps[0], ruleTiles[0], -10, 0, -40, 0);
        BoxFill(maps[0], ruleTiles[0], 0, layoutPos, -40, -14);
        BoxFill(maps[0], ruleTiles[0], layoutPos, layoutPos + 20, -40, 0);
        BoxFill(maps[0], ruleTiles[0], layoutPos + 20, layoutPos + 40, -40, 60);
        Instantiate(doorPrefab, new Vector3(layoutPos + 10, 1.5f), Quaternion.identity);

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
            bgMap.SetTile(tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), bgTile);
            minimap.SetTile(tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), minimapTiles[3]);
        }
        //check main tiles
        for (int i = 0; i < ruleTiles.Length; i++)
        {
            if (tilemap.GetTile(tempPos) == ruleTiles[i])
            {
                maps[i].SetTile(tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), ruleTiles[i]);
                minimap.SetTile(tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), minimapTiles[i]);
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
                //randomize existence
                if (Random.Range(0f, 1f) > layoutCount / holdLayoutCount * .7f)
                {
                    //randomize type
                    int randType = Mathf.FloorToInt(i + enemyPrefabs.Length + Random.Range(-.1f, 1.1f)) % enemyPrefabs.Length;
                    //instantiate
                    GameObject holdEnemy = Instantiate(enemyPrefabs[randType], 
                        tempPos + new Vector3Int(layoutPos - bounds.min.x, 0), Quaternion.identity);
                    //randomize stats
                    holdEnemy.GetComponent<EnemyController>().SetStats(layoutCount / holdLayoutCount / 2f);
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

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //Camera.main.transform.position = GameObject.Find("Player").transform.position + new Vector3(0, 0, -10);
    }
}
