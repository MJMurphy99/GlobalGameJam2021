using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarController : MonoBehaviour
{
    public static event Action Blip = delegate { };
    private bool cooling = false;
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
        cooling = true;
        Blip();
        yield return new WaitForSeconds(.5f);
        Blip();

        yield return new WaitForSeconds(10);

        cooling = false;
    }
}
