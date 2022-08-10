using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private AIPath aiPath;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Taking Fire");
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Move()
    {

    }
}
