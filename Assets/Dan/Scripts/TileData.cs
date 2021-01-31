using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public GameObject hiddenObj;
    public string type;

    private void OnTriggerStay2D(Collider2D collision)
    {
        print(1);
        Instantiate(hiddenObj, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
