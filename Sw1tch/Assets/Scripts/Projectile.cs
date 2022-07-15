using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float fireForce;
    [SerializeField]
    private Rigidbody2D rb;
    public float lifeTime;
    //[SerializeField]
    //private bool damaging = false;
    private int damage = 10;

    private float timer = 0;

    public LayerMask collidableLayers;
    [SerializeField]
    private string playerName;
    private Vector2 playerVelocity;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet Collided");
        if(((1<<collision.gameObject.layer) & collidableLayers) != 0)
        {
            Debug.Log("Triggered " + collision.gameObject.name);
            /*if(collision.gameObject.GetComponent<IDamageable>() != null)
            {
                collision.gameObject.GetComponent<IDamageable>().takeDamage(damage);
            }
            //Instantiate(hitEffect, transform.position, Quaternion.identity);*/
            if(collision.gameObject.GetComponent<Enemy>() != null)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
            Despawn();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //playerVelocity = GameObject.Find(playerName).GetComponent<Rigidbody2D>().velocity;
        //rb.AddForce(playerVelocity, ForceMode2D.Impulse);
        rb.AddForce(transform.up * fireForce, ForceMode2D.Impulse);
        Invoke("Despawn", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    void fixedUpdate()
    {
        //Vector2.Lerp(target, rb.velocity, 1);
    }

    void Despawn()
    {
        Destroy(this.gameObject);
    }
}
