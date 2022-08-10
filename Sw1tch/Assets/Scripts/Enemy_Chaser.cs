using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chaser : EnemyAI
{

    private bool chasing = false;
    private GameObject target;
    [SerializeField]
    private float chaseDistance;
    [SerializeField]
    private LayerMask players;
    [SerializeField]
    private float chaseSpeed = 2000000f;


    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdatePath_Chase()
    {
        if(chasing && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
        }
    }

    public override void PersonalFixedUpdates()
    {
        if(!chasing)
        {
            if(target == null)
            {
                target = GameObject.Find("GameController").GetComponent<GameController>().GetCurrentPlayer();
            }

            float distance = Vector2.Distance(transform.position, target.transform.position);
            if(distance <= chaseDistance)
            {
                //The player is close enough to chase, but we gotta see if there is a wall in the way.
                Vector2 direction = (target.transform.position - transform.position).normalized;
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
