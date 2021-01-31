using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToTree : MonoBehaviour
{
    public GameObject pausePanel;
    public Collider2D player;
    private bool entered = false;

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapBox(transform.position, new Vector2(1, 1), 0) == player && !entered)
        {
            pausePanel.SetActive(true);
            entered = true;
        }
        else if (Physics2D.OverlapBox(transform.position, new Vector2(2, 2), 0) != player && entered)
            entered = false;
    }
}
