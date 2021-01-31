using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boiGoBoom : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //trigger the boom animation 
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
