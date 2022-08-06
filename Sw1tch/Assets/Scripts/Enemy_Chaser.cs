using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : EnemyAI
{

    private bool chasing = false;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("EnteredTrigger");
        if(col.gameObject.GetComponent<PlayerChar>() != null)
        {
            //enemy found. Switch target to chase
            movementPointer.position = new Vector2(0, 0);
            target = col.gameObject;
            chasing = true;
            CancelInvoke("UpdatePath");
            InvokeRepeating("UpdatePath_Chase", 0f, 1f);
        }
    }

    void UpdatePath_Chase(bool chasing)
    {
        if(chasing && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
        }
    }
}
