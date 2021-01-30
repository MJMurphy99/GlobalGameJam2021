using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]

public class enemyMovement : MonoBehaviour
{
    public int xMin, xMax, yMin, yMax;
    private char[,] pathRecord;
    

    public Tilemap tm;
    Vector2Int goal;
    private Transform duck;


    bool move;
    bool isMoving = false;
    Vector3 origPos, targetPos;
    float moveTime = 0.4f;
    Vector2Int pathPoint;

    // Start is called before the first frame update
    void Start()
    {
        pathRecord = new char[xMax - xMin, yMax - yMin];
        findPath();


        duck = GameObject.FindGameObjectWithTag("duck").transform;
        goal = new Vector2Int(xMax, yMax) - (Vector2Int)tm.WorldToCell(duck.position);

        Vector3Int location = tm.WorldToCell(transform.position);
        transform.position = tm.CellToWorld(location);
        pathPoint = new Vector2Int(xMax, yMax) - (Vector2Int)location;


        move = findGoal(pathPoint);

    }

    // Update is called once per frame
    void Update()
    {
        if (move && !isMoving && !tm.WorldToCell(duck.position).Equals(tm.WorldToCell(transform.position)))
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
                        pathPoint = adj[i];
                        break;
                    }
                }
            }
            StartCoroutine (moveEnemy(direction));
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
}
