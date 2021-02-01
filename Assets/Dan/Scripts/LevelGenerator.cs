using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    public int width, height;
    public GameObject[] relics;
    public GameObject enemyCave;
    public GameObject container;
    public Collider2D player;
    public int minCaves;
    public int proximityExclusionRadius;

    public static GameObject[] randoSpawns;
    public static int randoCount = 0;

    private GameObject[] containers;
    private List<Vector2> cells;
    private int placedObj = 0;

    private Tilemap tm;

    private void Start()
    {
        tm = GameObject.FindGameObjectWithTag("Path").GetComponent<Tilemap>();
        CallNewLevel();
    }

    private void AvailableCells()
    {
        cells = new List<Vector2>();

        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                Vector2 point = new Vector2(i - height / 2, j - height / 2 - 5);

                if(point != Vector2.zero)
                    cells.Add(point);
            }
        }
    }

    private void BuildLevel()
    {
        AvailableCells();
        PlaceEnemies();
        PlaceRelics();
    }

    private void PlaceEnemies()
    {
        int n = (int)Mathf.Pow(width / proximityExclusionRadius, 2);
        int maxCaves = (int)(n - (n * .2f));

        int numCaves = Random.Range(minCaves, maxCaves + 1);

        containers = new GameObject[numCaves + 3];
        randoSpawns = new GameObject[numCaves + 3];

        for(int i = 0; i < numCaves; i++)
        {
            int maxItt = 100;
            bool creatable = false;

            int cell;

            while (!creatable && maxItt > 0)
            {
                bool failedCheck = false;
                cell = Random.Range(0, cells.Count - 1);
                Vector2 point = cells[cell];

                for (int j = 0; j < i; j++)
                {
                    if (j < placedObj)
                    {
                        if (Mathf.Abs(point.x - containers[j].transform.position.x) <= proximityExclusionRadius)
                        {
                            failedCheck = true;
                            break;
                        }
                    }
                    else break;
                }
                maxItt--;

                creatable = !failedCheck;

                if (creatable)
                {
                    cells.RemoveAt(cell);
                    
                    containers[placedObj] = Instantiate(container, tm.CellToWorld(tm.WorldToCell(point)), Quaternion.identity);
                    TileData td = containers[placedObj].GetComponent<TileData>();
                    td.hiddenObj = enemyCave;
                    td.type = "Enemy Tunnel";
                    td.player = player;
                    placedObj++;
                }
            }
        }
    }

    private void PlaceRelics()
    {
        for (int i = 0; i < 3; i++)
        {
            bool creatable = false;
            int cell;
            int maxItt = 100;

            while(!creatable && maxItt > 0)
            {
                bool failCheck = false;
                cell = Random.Range(0, cells.Count - 1);
                Vector2 point = cells[cell];

                for (int j = 0; j < placedObj; j++)
                {
                    bool isClose = Mathf.Abs(point.x - containers[j].transform.position.x) <= 4;
                    bool isRelic = containers[j].GetComponent<TileData>().type.CompareTo("Relic") == 0;
                    
                    if (isClose && isRelic && maxItt > 1)
                    {
                        failCheck = true;
                        break;
                    }
                }
                maxItt--;

                creatable = !failCheck;

                if(creatable)
                {
                    cells.RemoveAt(cell);

                    containers[placedObj] = Instantiate(container, tm.CellToWorld(tm.WorldToCell(point)), Quaternion.identity);
                    TileData td = containers[placedObj].GetComponent<TileData>();
                    td.hiddenObj = relics[i];
                    td.type = "Relic";
                    td.player = player;

                    placedObj++;
                }
            }
        }
    }

    private void ClearLevel()
    {
        if(containers != null)
        {
            for (int i = 0; i < containers.Length; i++)
            {
                Destroy(containers[i]);
                Destroy(randoSpawns[i]);
            }
            placedObj = 0;
            randoCount = 0;
            tm.ClearAllTiles();

        }
    }

    public void CallNewLevel()
    {
        ClearLevel();
        BuildLevel();
    }
}
