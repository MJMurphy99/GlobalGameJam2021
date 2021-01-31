using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public float width, height;
    public GameObject[] relics;
    public GameObject enemyCave;
    public GameObject container;

    public int minCaves, maxCaves;

    // Start is called before the first frame update
    void Start()
    {
        BuildLevel();
    }

    private void BuildLevel()
    {
        PlaceRelics();
        PlaceEnemies();
    }

    private void PlaceRelics()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector2 point;

            int x = (int)Random.Range(-width / 2, width / 2);
            int y = (int)Random.Range(-height / 2, height / 2);

            point = new Vector2(x, y);

            GameObject instance = Instantiate(container, point, Quaternion.identity);
            instance.GetComponent<TileData>().hiddenObj = relics[i];
        }
    }

    private void PlaceEnemies()
    {
        int numCaves = Random.Range(minCaves, maxCaves + 1);

        for(int i = 0; i < numCaves; i++)
        {
            Vector2 point;

            int x = (int)Random.Range(0, width);
            int y = (int)Random.Range(0, height);

            point = new Vector2(x, y);

            GameObject instance = Instantiate(container, point, Quaternion.identity);
            instance.GetComponent<TileData>().hiddenObj = enemyCave;
        }
    }
}
