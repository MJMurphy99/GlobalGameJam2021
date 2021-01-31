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

    void Spawn()
    {
        if(gameObject.GetComponent<SpriteRenderer>().enabled==true)
        {
            int max = Random.Range(1,3);
            StartCoroutine(spaceOutSpawn(max));
        }
    }

    IEnumerator spaceOutSpawn(int max)
    {
        for (int i=0; i<max; i++)
        {
            GameObject a = Instantiate(enemy, transform.position, transform.rotation);
            
            yield return new WaitForSeconds(a.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
