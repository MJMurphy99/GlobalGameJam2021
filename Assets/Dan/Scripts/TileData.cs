using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public GameObject hiddenObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(hiddenObj, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
