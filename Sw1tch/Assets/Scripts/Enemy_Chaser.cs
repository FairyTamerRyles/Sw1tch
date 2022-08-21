using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy_Chaser : MonoBehaviour
{
    //Movement variables
    public float speed = 100000f;
    public Rigidbody2D rb;

    //Pathfinding Specific variables    
    public float nextWaypointDistance = 3f;
    public Path path;
    public int currentWaypoint = 0;
    public bool reachedPathEnd = false;
    public Seeker seeker;


    //Random movment variables
    [SerializeField]
    public float moveRadius = 5f;
    [SerializeField]
    public Transform movementPointer;

    //Chase Specific Variables
    private bool chasing = false;
    private GameObject target;
    [SerializeField]
    private float chaseDistance;
    [SerializeField]
    private LayerMask players;
    [SerializeField]
    private float chaseSpeed = 2000000f;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
        {
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
                        //Debug.Log("movement pointer good");
                    }
                    else
                    {
                        //Debug.Log("thingy broken");
                        movementPointer.position = transform.position;
                    }
                    
                    seeker.StartPath(rb.position, movementPointer.position, OnPathComplete);
                }
            }
        }
    }

    public void OnPathComplete(Path p)
    {
        path = p;
        currentWaypoint = 0;
        movementPointer.position = new Vector2(0,0);
    }


    void UpdatePath_Chase()
    {
        if(chasing && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
        }
    }

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

        if(!chasing)
        {
            if(target == null)
            {
                target = GameObject.Find("GameController").GetComponent<GameController>().GetCurrentPlayer();
            }

            distance = Vector2.Distance(transform.position, target.transform.position);
            if(distance <= chaseDistance)
            {
                //The player is close enough to chase, but we gotta see if there is a wall in the way.
                direction = (target.transform.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, players);
                if(hit.collider != null && hit.collider.gameObject.GetComponent<PlayerChar>() != null)
                {
                    //character has been seen. Chase
                    movementPointer.position = new Vector2(0, 0);
                    target = hit.collider.gameObject;
                    chasing = true;
                    speed = chaseSpeed;
                    CancelInvoke("UpdatePath");
                    InvokeRepeating("UpdatePath_Chase", 0f, .25f);
                }
            }
        }
    }
}
