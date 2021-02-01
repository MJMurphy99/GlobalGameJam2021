﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDuck : MonoBehaviour
{
    private bool canSpawnDuck = true;
    public GameObject duck, flag;
    private float duckCoolDown = 10f;

    private Animator anim;

    //public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawnDuck && Input.GetKeyDown(KeyCode.Q))
        {
            DuckManager.duckDeadorSuccessful = false;
            anim.SetTrigger("Duck");
            Instantiate(flag, transform.position, transform.rotation);
            canSpawnDuck = false;
            Instantiate(duck);
            //play sound
            //StartCoroutine(DuckSpawn());
        }
    }

    //public static IEnumerator DuckSpawn()
    //{
    //    yield return new WaitForSeconds(2.5f);
    //    Instantiate(flag, Player.transform.position, Player.transform.rotation);
    //    canSpawnDuck = false;
    //    Instantiate(duck);
    //}

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(duckCoolDown);
        canSpawnDuck = true;
    }

    public void cd()
    {
        StartCoroutine(CoolDown());
    }
}
