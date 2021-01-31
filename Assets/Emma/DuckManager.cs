using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class DuckManager : MonoBehaviour
{
    public static event Action startEnemies = delegate { };
    public static bool duckDeadorSuccessful = false;

    bool mineable;

    public GameObject canvasPrefab;
    public float mineTime;
    private float elapsedTime;
    private bool isMining =false;

    private GameObject mineableObject;

    private Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PackageSecured()
    {
        StartCoroutine(SpawnDuck.CoolDown());
        duckDeadorSuccessful = true;
    }

    public void ReturnToTree()
    {
        Destroy(GameObject.FindGameObjectWithTag("Flag"));
        enemyMovement em = GetComponent<enemyMovement>();
        em.readyToMove();
        em.goalTag = "Tree";
        em.StartPathfinding();
        em.readyToMove();
        GetComponent<Animator>().SetBool("Arrive", false);
    }

    public void CheckForObject()
    {
        Destroy(GameObject.FindGameObjectWithTag("Flag"));

        Collider2D duckCol = GetComponent<Collider2D>();
        GameObject[] nuts = GameObject.FindGameObjectsWithTag("Mineable");

        for(int i=0; i<nuts.Length; i++)
        {
            
            Collider2D nutCol = nuts[i].GetComponent<Collider2D>();
            if (nutCol!=null && nutCol.IsTouching(duckCol))
            {
                Debug.Log(3);
                mineable = true;
                mineableObject = nuts[i];
                break;
            }
        }

        if(mineable)
        {
            if(!isMining)
            {
                StartCoroutine(Mine());
            }
        }
        else
        {
            Explode();
        }
    }

    IEnumerator Mine()
    {
        isMining = true;
        startEnemies();
        gameObject.GetComponent<Animator>().SetBool("Arrive", true);
        GameObject bar = Instantiate(canvasPrefab).transform.GetChild(0).gameObject;
        elapsedTime = 0f;

        while(elapsedTime<mineTime)
        {
            elapsedTime += Time.deltaTime;
            bar.GetComponent<Slider>().value = elapsedTime / mineTime;
            yield return null;
        }

        Destroy(bar.transform.parent.gameObject);
        ReturnToTree();

        if(mineableObject!=null)
        {
            Destroy(mineableObject);
        }

        isMining = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Explode();
        }
    }

    public void Explode()
    {
        PackageSecured();

        anim.SetTrigger("Death");
    }

}
