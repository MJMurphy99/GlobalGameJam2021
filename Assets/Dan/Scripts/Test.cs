using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Tilemap tm;
    private bool moving;
    private Vector2 dir = Vector2.zero;
    private float speed = .25f;
    private int mDurr = 5, cDurr = 5; 

    public TileBase t;

    /*private void Start()
    {
        TileBase tb = (TileBase)(Object)t;
    }*/

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if(!moving)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                dir = Vector2.right * Input.GetAxisRaw("Horizontal");
                StartCoroutine("Digging");
            }
            else if (Input.GetAxisRaw("Vertical") != 0)
            {
                dir = Vector2.up * Input.GetAxisRaw("Vertical");
                StartCoroutine("Digging");
            }
        }        
    }

    private IEnumerator Digging()
    {
        moving = true;

        float timePassed = 0;

        Vector2 origin = transform.position;
        Vector2 target = origin + dir;

        Vector3Int pos = new Vector3Int((int)target.x, (int)target.y, 0);

        //Tunneling
        if (cDurr > 0 || (cDurr == 0 && tm.GetTile(pos) != null))
        {
            while (timePassed < speed)
            {
                transform.position = Vector2.Lerp(origin, target, timePassed / speed);
                timePassed += Time.deltaTime;
                yield return null;
            }
            transform.position = target;

            if (tm.GetTile(pos) == null)
            {
                tm.SetTile(pos, t);
                cDurr--;
            }
        }
        else
        {
            //Not Tunneling
        }

        moving = false;
    }
}
