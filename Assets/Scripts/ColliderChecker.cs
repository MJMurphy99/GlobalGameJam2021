using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderChecker : MonoBehaviour
{
    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Boundry")
      //  {
     //      GridMovement.playerCanMove = false;
     //   }
    //}

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Boundry")
        {
            GridMovement.playerCanMove = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        GridMovement.playerCanMove = true;
    }


}
