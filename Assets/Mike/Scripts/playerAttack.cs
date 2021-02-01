using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    public float attackDestroyCooldown;

    void Start()
    {
        if (GridMovement.keyPressedLast == "left")
        {
            rb.velocity = new Vector3(-1, 0, 0) * speed;
        } else if (GridMovement.keyPressedLast == "right")
        {
            rb.velocity = new Vector3(1, 0, 0) * speed;
        } else if (GridMovement.keyPressedLast == "up")
        {
            rb.velocity = new Vector3(0, 1, 0) * speed;
        } else if (GridMovement.keyPressedLast == "down")
        {
            rb.velocity = new Vector3(0, -1, 0) * speed;
        } else
        {
            rb.velocity = new Vector3(1, 0, 0) * speed;
        }

        attackDestroyCooldown = 0.25f;

    }

    public void Update()
    {
        attackDestroyCooldown -= Time.deltaTime;

        if (attackDestroyCooldown <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "WallTile" || collision.gameObject.tag == "Boundry")
        {
            //trigger the boom animation 
            //Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Hit_Attack");

            //trigger the item pickup animation
            ScoreKeeper.playerScoreNum++;
            if (ScoreKeeper.playerScoreNum > ScoreKeeper.playerHighScoreNum)
            {
                ScoreKeeper.playerHighScoreNum++;
            }
            Destroy(collision.gameObject);
            Destroy(gameObject);

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemy_Death");

        }
    }

}
