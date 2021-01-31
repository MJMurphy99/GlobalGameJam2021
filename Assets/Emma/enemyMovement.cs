using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class enemyMovement : MonoBehaviour
{
    public int xMin, xMax, yMin, yMax;
    private char[,] pathRecord;


    public string tilemapTag;
    private Tilemap tm;

    public string goalTag;
    private Transform goalTransform;
    public Vector3Int goalTilemapPos;
    Vector2Int goal;

    private bool move;
    private bool entry;
    private bool isMoving = false;
    private Vector3 origPos, targetPos;
    private float moveTime = 0.4f;
    private Vector2Int pathPoint;

    // Start is called before the first frame update
    void Start()
    {
        tm = GameObject.FindGameObjectWithTag(tilemapTag).GetComponent<Tilemap>();


        Vector3Int location = tm.WorldToCell(transform.position);
        transform.position = tm.CellToWorld(location);
        pathPoint = new Vector2Int(xMax, yMax) - (Vector2Int)location;

        GetComponent<SpriteRenderer>().enabled = true;

        entry = false;
        pathRecord = new char[xMax - xMin, yMax - yMin];
        findPath();

        if(!goalTag.Equals(""))
        {
            goalTransform = GameObject.FindGameObjectWithTag(goalTag).transform;
            goalTilemapPos = tm.WorldToCell(goalTransform.position);
        }
        
        goal = new Vector2Int(xMax, yMax) - (Vector2Int)goalTilemapPos;

        move = findGoal(pathPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if(entry)
        {
            if (move && !MovementManager.duckDeadorSuccessful)
            {
                if (!goalTilemapPos.Equals(tm.WorldToCell(transform.position)) && !isMoving)
                {
                    pathRecord[pathPoint.x, pathPoint.y] = 'Z';
                    Vector3Int direction = Vector3Int.zero;
                    Vector2Int[] adj = adjacentPoints(pathPoint);

                    for (int i = 0; i < 4; i++)
                    {
                        if (validPoint(adj[i]))
                        {
                            if (pathRecord[adj[i].x, adj[i].y] == 'P')
                            {
                                direction = (Vector3Int)(pathPoint - adj[i]);
                                //this direction variable is always either Up(0,1,0) Down(0,-1,0) Left(-1,0,0) or Right(1,0,0)
                                pathPoint = adj[i];
                                break;
                            }
                        }
                    }
                    StartCoroutine(moveEnemy(direction));
                }
            }
            else
            {
                Destroy(gameObject.GetComponent<enemyMovement>());
                gameObject.GetComponent<Animator>().SetTrigger("Exit");
            }
        }
        
    }

    private IEnumerator moveEnemy(Vector3Int direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;

        targetPos = tm.CellToWorld(tm.WorldToCell(origPos) + direction);


        while(elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }



    bool findGoal(Vector2Int curr)
    {
        if(curr.Equals(goal))
        {
            pathRecord[curr.x, curr.y] = 'P';
            return true;
        }
        else if(pathRecord[curr.x,curr.y]!=' ')
        {
            return false;
        }

        pathRecord[curr.x, curr.y] = 'P';

        Vector2Int[] adjacent = adjacentPoints(curr);

        for (int i = 0; i < adjacent.Length; i++)
        {
            if (validPoint(adjacent[i]))
            {
                if (findGoal(adjacent[i]))
                {
                    return true;
                }
            }
        }

        pathRecord[curr.x, curr.y] = 'V';
        return false;
    }

    bool validPoint(Vector2Int p)
    {
        return (p.x >= 0 && p.x < pathRecord.GetLength(0) && p.y >= 0 && p.y <pathRecord.GetLength(1));
    }

    Vector2Int[] adjacentPoints(Vector2Int curr)
    {
        Vector2Int[] adj ={
            new Vector2Int(curr.x+1,curr.y),
            new Vector2Int(curr.x,curr.y+1),
            new Vector2Int(curr.x-1,curr.y),
            new Vector2Int(curr.x,curr.y-1)
        };

        return adj;
    }

    void findPath()
    {
        for(int x=0; x< pathRecord.GetLength(0); x++)
        {
            for(int y = 0; y < pathRecord.GetLength(1); y++){
                if (tm.HasTile(new Vector3Int(xMax - x, yMax - y, 0)))
                {
                    pathRecord[x, y] = ' ';
                }
                else
                {
                    pathRecord[x, y] = '#';
                }
            }
        }
    }

    void printPath()
    {
        string pr = "";
        for (int y = 0; y < pathRecord.GetLength(1); y++)
        {
            for (int x = pathRecord.GetLength(0)-1; x >=0 ; x--)
            {
                pr = pr + pathRecord[x, y];
            }
            pr += "\n";
        }
        Debug.Log(pr);
    }

    public void readyToMove()
    {
        entry = true;
    }
}
