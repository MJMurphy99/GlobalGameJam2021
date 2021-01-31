using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

//https://youtu.be/AiZ4z4qKy44 used to baseline moevment system
public class GridMovement : MonoBehaviour
{
    //this is weird jank but it is 2am and i can't think straight so
    private bool isMoving;
    private Vector3 originPos, targetPos;
    private float timeToMove = 0.2f;

    public GameObject playerAttack;
    public GameObject attackSpawnHor;
    public GameObject attackSpawnUp;
    public GameObject attackSpawnDown;
    public GameObject boundryCheckerHor, boundryCheckerUp, boundryCheckerDown;

    public static string keyPressedLast = "right";

    public static int playerHealth = 3;

    public float playerAttackCooldown;

    public bool playerCanMove;

    public Tilemap tm;

    private void Start()
    {
        transform.position = tm.CellToWorld(tm.WorldToCell(transform.position));
        playerHealth = 3;
        Debug.Log(playerHealth);
        playerAttackCooldown = 0.5f;
        keyPressedLast = "right";
        playerCanMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerAttackCooldown -= Time.deltaTime;

        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !isMoving)
        {
            //Up
            boundryCheckerUp.SetActive(true);
            boundryCheckerDown.SetActive(false);
            boundryCheckerHor.SetActive(false);
            playerMoveable(boundryCheckerUp);
            StartCoroutine(MovePlayer(Vector3Int.up));
            keyPressedLast = "up";

        }
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !isMoving)
        {
            //Down
            boundryCheckerUp.SetActive(false);
            boundryCheckerDown.SetActive(true);
            boundryCheckerHor.SetActive(false);
            playerMoveable(boundryCheckerDown);
            StartCoroutine(MovePlayer(Vector3Int.down));
            keyPressedLast = "down";

        }
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !isMoving)
        {
            //Left
            boundryCheckerUp.SetActive(false);
            boundryCheckerDown.SetActive(false);
            boundryCheckerHor.SetActive(true);
            playerMoveable(boundryCheckerHor);
            StartCoroutine(MovePlayer(Vector3Int.left));
            keyPressedLast = "left";
            transform.localScale = new Vector3(-1, 1, 1); //flip the sprite


        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !isMoving)
        {
            //Right
            boundryCheckerUp.SetActive(false);
            boundryCheckerDown.SetActive(false);
            boundryCheckerHor.SetActive(true);
            playerMoveable(boundryCheckerHor);
            StartCoroutine(MovePlayer(Vector3Int.right));
            keyPressedLast = "right";
            transform.localScale = new Vector3(1, 1, 1); //flip the sprite
        }

        if (playerAttackCooldown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }
        }

        //if the player runs out of lives, load the death screen
        if(playerHealth <= 0)
        {
            SceneManager.LoadScene(1);
        }

    }

    private IEnumerator MovePlayer(Vector3Int direction)
    {

        if (playerCanMove)
        {
            isMoving = true;

            float elapsedTime = 0;

            originPos = transform.position;
            targetPos = tm.CellToWorld(tm.WorldToCell(originPos) + direction);

            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(originPos, targetPos, (elapsedTime / timeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;

            Debug.Log(transform.position);

            isMoving = false;

            boundryCheckerUp.SetActive(true);
            boundryCheckerDown.SetActive(true);
            boundryCheckerHor.SetActive(true);
        } else
        {
            //dont move 
        }

    }

    public void Shoot()
    {
        if(keyPressedLast == "up")
        {
            Instantiate(playerAttack, attackSpawnUp.transform.position, attackSpawnUp.transform.rotation);
        }
        else if (keyPressedLast == "down")
        {
            Instantiate(playerAttack, attackSpawnDown.transform.position, attackSpawnDown.transform.rotation);
        }
        else if (keyPressedLast == "right" || keyPressedLast == "left")
        {
            Instantiate(playerAttack, attackSpawnHor.transform.position, attackSpawnHor.transform.rotation);
        }
        else
        {
            Instantiate(playerAttack, attackSpawnHor.transform.position, attackSpawnHor.transform.rotation);
        }

        playerAttackCooldown = 0.5f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //when the player collides with the enemy, lose one health and get set to a certain position in the world space
            transform.position = new Vector3(0, 0, 0);
            Destroy(collision.gameObject);
            playerHealth--;
            Debug.Log(playerHealth);
        }

        //baseline of how score can work, when you collide with it add to the score
        //easy to adapt, this is just baseline system that is super easy to alter
        if (collision.gameObject.tag == "Ore")
        {
            ScoreKeeper.playerScoreNum++;
            LevelCompletionCheck.numOreRemaining--;
        }
    }

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.tag == "Boundry")
    //    {
    //        playerCanMove = false;
    //    }
    //}

    //public void OnTriggerExit2D(Collider2D collision)
    //{
    //    playerCanMove = true;
    //}

    void playerMoveable(GameObject wall)
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Boundry");
        playerCanMove = true;
        for(int i=0; i<walls.Length; i++)
        {
            if(wall.GetComponent<Collider2D>().IsTouching(walls[i].GetComponent<Collider2D>()))
            {
                playerCanMove = false;
                break;
            }
        }
    }
}
