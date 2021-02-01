using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        DuckManager.startEnemies += Spawn;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn(GameObject target)
    {
        int max = Random.Range(1,3);
        StartCoroutine(spaceOutSpawn(max, target));
    }

    IEnumerator spaceOutSpawn(int max, GameObject target)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemy_Active");

        for (int i=0; i<max; i++)
        {
            GameObject a = Instantiate(enemy, transform.position, transform.rotation);
           // a.GetComponent<enemyMovement>().readyToMove();
            yield return new WaitForSeconds(.01f);
        }
    }
}
