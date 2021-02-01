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

    public static bool playerCanMove;

    public Tilemap tm;
    public TileBase t;

    public int mDurr;
    private int cDurr;

    private Animator anim;

    public static bool canMove = true;

    private FMOD.Studio.EventInstance instance;
    [FMODUnity.EventRef]
    public string fmodEvent;

    private void Start()
    {
        transform.position = tm.CellToWorld(tm.WorldToCell(transform.position));
        playerHealth = 3;
        Debug.Log(playerHealth);
        playerAttackCooldown = 0.5f;
        keyPressedLast = "right";
        playerCanMove = true;

        anim = GetComponent<Animator>();
        cDurr = mDurr;

        ScoreKeeper.playerScoreNum = 0;

        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
    }

    // Update is called once per frame
    void Update()
    {
        playerAttackCooldown -= Time.deltaTime;

        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !isMoving && canMove)
        {
            //Up
            anim.SetInteger("Direction", 1);

            boundryCheckerUp.SetActive(true);
            boundryCheckerDown.SetActive(false);
            boundryCheckerHor.SetActive(false);
            playerMoveable(boundryCheckerUp);
            StartCoroutine(MovePlayer(Vector3Int.up));
            keyPressedLast = "up";

        }
        else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !isMoving && canMove)
        {
            //Down
            anim.SetInteger("Direction", 0);

            boundryCheckerUp.SetActive(false);
            boundryCheckerDown.SetActive(true);
            boundryCheckerHor.SetActive(false);
            playerMoveable(boundryCheckerDown);
            StartCoroutine(MovePlayer(Vector3Int.down));
            keyPressedLast = "down";

        }
        else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !isMoving && canMove)
        {
            //Left
            anim.SetInteger("Direction", 2);

            boundryCheckerUp.SetActive(false);
            boundryCheckerDown.SetActive(false);
            boundryCheckerHor.SetActive(true);
            playerMoveable(boundryCheckerHor);
            StartCoroutine(MovePlayer(Vector3Int.left));
            keyPressedLast = "left";
            transform.localScale = new Vector3(-1, 1, 1); //flip the sprite


        }
        else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !isMoving && canMove)
        {
            //Right
            anim.SetInteger("Direction", 2);

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
            if (Input.GetKeyDown(KeyCode.Space) && canMove)
            {
                anim.SetTrigger("Throw");

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

            Vector3Int targetCell = tm.WorldToCell(originPos) + direction;
            if(targetCell.x>7|| targetCell.x < -8 || targetCell.y<-11 || targetCell.y>3)
            {
                isMoving = false;
                yield break;
            }
            targetPos = tm.CellToWorld(targetCell);
            

            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(originPos, targetPos, (elapsedTime / timeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;

            Vector3Int pos = tm.WorldToCell(transform.position);

            if (tm.GetTile(pos) == null && transform.position.y < 3)
            {
                tm.SetTile(pos, t);
                cDurr--;
                //instance.setParameterByName("Digging", 1);
            }
            else
            {
                //instance.setParameterByName("Digging", 0);
            }

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
        if(keyPressedLast == "up" && canMove)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Throw");

            Instantiate(playerAttack, attackSpawnUp.transform.position, attackSpawnUp.transform.rotation);
        }
        else if (keyPressedLast == "down" && canMove)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Throw");
            //play sound
            Instantiate(playerAttack, attackSpawnDown.transform.position, attackSpawnDown.transform.rotation);
        }
        else if (keyPressedLast == "right" || keyPressedLast == "left" && canMove)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Throw");
            //play sound
            Instantiate(playerAttack, attackSpawnHor.transform.position, attackSpawnHor.transform.rotation);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Throw");
            //play sound
            Instantiate(playerAttack, attackSpawnHor.transform.position, attackSpawnHor.transform.rotation);
        }

        playerAttackCooldown = 0.5f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player_Death");
            anim.SetTrigger("Death");
            canMove = false;
            Destroy(collision.gameObject);
            StartCoroutine(PlayerDeath());
        }

        //baseline of how score can work, when you collide with it add to the score
        //easy to adapt, this is just baseline system that is super easy to alter
        if (collision.gameObject.tag == "Mineable")
        {
            ScoreKeeper.playerScoreNum++;
            //LevelCompletionCheck.numOreRemaining--;
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

    IEnumerator PlayerDeath()
    {
        yield return new WaitForSeconds(2f);
        //when the player collides with the enemy, lose one health and get set to a certain position in the world space
        transform.position = new Vector3(6.75f, 3.5f, 0);
        canMove = true;
        playerHealth--;
        Debug.Log(playerHealth);
    }
}
