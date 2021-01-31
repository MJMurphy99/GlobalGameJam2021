using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDuck : MonoBehaviour
{
    public static bool canSpawnDuck = true;
    public GameObject duck, flag;
    private static float duckCoolDown = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawnDuck && Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(flag, transform.position, transform.rotation);
            canSpawnDuck = false;
            Instantiate(duck);
        }
    }

    public static IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(duckCoolDown);
        canSpawnDuck = true;
    }
}
