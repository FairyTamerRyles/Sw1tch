using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour, IDamageable, IEnemy
{

    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public GameObject gameController;

    public GameObject currentPlayer;
    public GameObject bulletSpawn;

    public GameObject projectile;
    public float shotIntervalTime;
    public float startShotIntervalTime;

    public Rigidbody2D rb;
    public float health = 100;
    public float slowSpeed;


    public void takeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            die();
        }
    }
    public void die()
    {
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        //gameController = GameObject.Find("GameControllerObject");
        currentPlayer = gameController.GetComponent<GameController>().getCurrentPlayer();
        Debug.Log(currentPlayer);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        runAndGun();
    }

    public void changeTarget(GameObject newTarget)
    {
        Debug.Log("New target is " + newTarget.name);
        currentPlayer = newTarget;
    }

    void runAndGun()
    {
        Vector2 playerPos = currentPlayer.transform.position;
        Vector2 lookDir = rb.position - playerPos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90;
        rb.rotation = angle;

        if(Vector2.Distance(transform.position, currentPlayer.transform.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentPlayer.transform.position, speed * Time.fixedDeltaTime);
            float x = rb.velocity.x;
            float y = rb.velocity.y;

            x -= slowSpeed;
            y -= slowSpeed;

            if(x < 0)
            {
                x = 0;
            }
            if(y < 0)
            {
                y = 0;
            }

            rb.velocity = new Vector2(x, y);
        }
        else if(Vector2.Distance(transform.position, currentPlayer.transform.position) <= stoppingDistance && Vector2.Distance(transform.position, currentPlayer.transform.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }

        if(shotIntervalTime <= 0)
        {
            GameObject proj = Instantiate(projectile, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            proj.transform.SetParent(gameObject.transform.parent);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.AddForce(transform.up * proj.GetComponent<Projectile>().fireForce, ForceMode2D.Impulse);
            proj.GetComponent<Projectile>().Invoke("Despawn", proj.GetComponent<Projectile>().lifeTime);
            shotIntervalTime = startShotIntervalTime;
        }
        else
        {
            shotIntervalTime -= Time.deltaTime;
        }

    }
}
