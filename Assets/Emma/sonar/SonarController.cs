using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SonarController : MonoBehaviour
{
    public static event Action Blip = delegate { };
    private bool cooling = false;
    public float coolDownTime;
    public float elapsedTime;

    //public Slider slider;
    //public Image image;

    // Update is called once per frame
    void Update()
    {
        if (!cooling && Input.GetKeyDown(KeyCode.E))
        {
            //Ping();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Sonar");
            StartCoroutine(Sonar());
        }
    }

    IEnumerator Sonar()
    {
        cooling = true;
        elapsedTime = 0;
        Blip();
        yield return new WaitForSeconds(.5f);
        Blip();

        while(elapsedTime < coolDownTime)
        {
            elapsedTime += Time.deltaTime;
            //this is where the UI element should go. I didn't know whether to use sliders or images
                //slider.value = elapsedTime / coolDownTime;
                //image.fillAmount = elapsedTime / coolDownTime;
            yield return null;
        }

        cooling = false;
    }
}
