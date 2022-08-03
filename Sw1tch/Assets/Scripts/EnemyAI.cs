using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public float speed = 20f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedPathEnd = false;

    [SerializeField]
    float moveRadius = 5f;
    [SerializeField]
    private Transform movementPointer;


    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 5f);
    }

    void UpdatePath()
    {
        Debug.Log("Updating path");
        if(seeker.IsDone())
        {
            Debug.Log("seeker is done");
            if(transform.parent != null)
            {
                if(transform.parent.GetComponent<Room>() != null)
                {
                    CompositeCollider2D inBounds = transform.parent.GetComponent<Room>().InBoundsCol();
                    bool pointFound = false;
                    Vector3 randomPosition = new Vector3(0,0,0);
                    
                    randomPosition.x = Random.Range(transform.position.x - moveRadius, transform.position.x + moveRadius);
                    randomPosition.y = Random.Range(transform.position.y - moveRadius, transform.position.y + moveRadius);
                    movementPointer.position = randomPosition;
                    if(inBounds.bounds.Contains(movementPointer.position))
                    {
                        Debug.Log("movement pointer good");
                    }
                    else
                    {
                        Debug.Log("thingy broken");
                        movementPointer.position = transform.position;
                    }
                    
                    seeker.StartPath(rb.position, movementPointer.position, OnPathComplete);
                }
            }
        }
    }

    void OnPathComplete(Path p)
    {
        path = p;
        currentWaypoint = 0;
        movementPointer.position = new Vector2(0,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedPathEnd = true;
            return;
        }
        else
        {
            reachedPathEnd = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
