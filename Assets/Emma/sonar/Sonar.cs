using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    public GameObject ringPrefab;
    public GameObject a;
    // Start is called before the first frame update
    void Start()
    {
        SonarController.Blip += SpawnRing;
    }

    public void SpawnRing()
    {
        a = Instantiate(ringPrefab);
        a.transform.position = transform.position;
        a.GetComponent<Animator>().SetTrigger("Go");
    }
}
