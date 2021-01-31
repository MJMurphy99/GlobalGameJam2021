using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionCheck : MonoBehaviour
{
    public static int numOreRemaining;

    public void Start()
    {
        //we can set the number of ores in each level in here
        //EX: level one has two, level two has 5 etc.
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            numOreRemaining = 1;
        }

    }

    public void Update()
    {
        if (numOreRemaining <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }

}
