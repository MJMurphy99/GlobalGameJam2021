using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDuck : MonoBehaviour
{
    private bool canSpawnDuck = true;
    public GameObject duck, flag;
    private float duckCoolDown = 5f;

    private Animator anim;

    private FMOD.Studio.EventInstance instance;
    [FMODUnity.EventRef]
    public string fmodEvent;

    public static bool DuckOut = true;

    //public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        //instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        //instance.start();
    }

    // Update is called once per frame
    void Update()
    {
        if (DuckOut == false)
        {
            StartCoroutine(DuckNOTOutAudio());
        } 

        if (canSpawnDuck && Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(canMove());

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Summon_Duck");

            StartCoroutine(DuckOutAudio());

            DuckManager.duckDeadorSuccessful = false;
            anim.SetTrigger("Duck");
            Instantiate(flag, transform.position, transform.rotation);
            canSpawnDuck = false;
            Instantiate(duck);
            //StartCoroutine(DuckSpawn());
        }

        if(canSpawnDuck == true)
        {
            DuckOutAudio();
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

    IEnumerator canMove()
    {
        GridMovement.canMove = false;
        yield return new WaitForSeconds(2f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Duck_Quack");

        yield return new WaitForSeconds(0.5f);
        GridMovement.canMove = true;
    }

    //public void DuckOutAudio()
    //{
    //    if(DuckOut == true)
    //    {
    //        instance.setParameterByName("Duck Out", 1);
    //    }

    //    if (DuckOut == false)
    //    {
    //        instance.setParameterByName("Duck Out", 0);
    //    }
    //}

    public void cd()
    {
        StartCoroutine(CoolDown());
    }

    IEnumerator DuckOutAudio()
    {
        yield return new WaitForSeconds(0.2f);
        instance.setParameterByName("Duck Out", 1);
    }

    IEnumerator DuckNOTOutAudio()
    {
        yield return new WaitForSeconds(0.2f);
        instance.setParameterByName("Duck Out", 0);
        DuckOut = true;
    }
}
