using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShot : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float fireSpeed;
    

    private float timer = 0f;
    private InputMaster playerInput;
    private bool isShooting;
    private bool shotOnce = false;
    private bool waitingForShot = false;



    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
        StopShoot();
    }

    void Awake()
    {
        if(playerInput == null)
        {
            playerInput = new InputMaster();
        }
        playerInput.Player.Fire.performed += context => StartShoot();
        playerInput.Player.StopFire.performed += context => StopShoot();
    }

    void FixedUpdate()
    {
        if(timer <= 0.1) //timer makes sure you are within firing timeframe
        {
            if(isShooting || waitingForShot) //checks if we are shooting right now, or if we shot, let go, but need to fire once
            {
                Shoot();
            }
        }
        else
        {
            timer -= Time.fixedDeltaTime;
        }

    }
    void StartShoot()
    {
        isShooting = true;
    }
    void StopShoot()
    {
        if(!waitingForShot)
        {
            isShooting = false;
            shotOnce = false;
        }
    }

    void Shoot()
    {
        timer = fireSpeed;
        Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        shotOnce = true;
        waitingForShot = false;
    }
}
