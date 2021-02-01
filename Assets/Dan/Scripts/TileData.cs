using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public GameObject hiddenObj;
    public Collider2D player;
    public string type;

    private void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, .3f) == player)
        {
            int n = LevelGenerator.randoCount = LevelGenerator.randoCount + 1;
            LevelGenerator.randoSpawns[n] = Instantiate(hiddenObj, transform.position, Quaternion.identity);
            gameObject.GetComponent<Sonar>().stopRing();
            Destroy(gameObject);
        }
    }
}
