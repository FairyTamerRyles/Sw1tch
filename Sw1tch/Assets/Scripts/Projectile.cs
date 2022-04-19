using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float fireForce;
    [SerializeField]
    private Rigidbody2D rb;
    public float lifeTime;
    [SerializeField]
    private bool damaging = false;

    private float timer = 0;

    public LayerMask collidableLayers;

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
            Despawn();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.up * fireForce, ForceMode2D.Impulse);
        Invoke("Despawn", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= lifeTime && !damaging)
        {
            //despawn();
        }
    }

    void Despawn()
    {
        Destroy(this.gameObject);
    }
}
