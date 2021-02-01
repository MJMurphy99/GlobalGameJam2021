using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SnapToGrid : MonoBehaviour
{
    private Tilemap tm;
    // Start is called before the first frame update
    void Start()
    {
        tm = GameObject.FindGameObjectWithTag("Path").GetComponent<Tilemap>();
        transform.position = tm.CellToWorld(tm.WorldToCell(transform.position));
    }
}
