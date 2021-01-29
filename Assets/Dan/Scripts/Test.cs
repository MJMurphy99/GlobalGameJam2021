using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Tilemap tm;

    private void Move()
    {

    }

    private void Break()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 p = collision.GetContact(0).point;
        Vector3Int v3i = new Vector3Int((int)p.x - 1, (int)p.y, 0);
        print(v3i);
        tm.SetTile(v3i, null);
    }
}
