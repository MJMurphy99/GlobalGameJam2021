using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarController : MonoBehaviour
{
    public static event Action Blip = delegate { };
    private float coolDown = 30f;
    private bool cooling = false;
    float elapsedTime = 1f;
    // Update is called once per frame
    void Update()
    {
        if (!cooling && Input.GetKeyDown(KeyCode.E))
        {
            //Ping();
            
            StartCoroutine(Sonar());
        }
    }

    IEnumerator Sonar()
    {
        elapsedTime = 0f;
        cooling = true;
        Blip();
        yield return new WaitForSeconds(.5f);
        Blip();

        yield return new WaitForSeconds(10);

        cooling = false;
    }
}
