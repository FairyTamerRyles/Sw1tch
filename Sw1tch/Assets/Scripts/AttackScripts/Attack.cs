using System.Reflection;
using System.Security.Permissions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [SerializeField]
    private bool upgraded;
    [SerializeField]
    private string upgradeTag;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float fireSpeed;
    [SerializeField]
    private UpgradeTags attackTags;

    private float timer = 0f;
    private InputMaster playerInput;
    private bool isShooting;
    private bool shotOnce = false;
    private bool waitingForShot = false;

    #region GetSetters
    public GameObject Projectile
    {
        get { return projectile; }
        set { projectile = value; }
    }
    public Transform SpawnPoint
    {
        get { return spawnPoint; }
        set { spawnPoint = value; }
    }
    public float FireSpeed
    {
        get { return fireSpeed; }
        set { fireSpeed = value; }
    }
    public float Timer 
    {
        get { return timer; }
        set { timer = value; }
    }
    public bool IsShooting
    {
        get { return isShooting; }
        set { isShooting = value; }
    }
    public bool ShotOnce
    {
        get { return shotOnce; }
        set { shotOnce = value; }
    }
    public bool WaitingForShot
    {
        get { return waitingForShot; }
        set { waitingForShot = value; }
    }
    #endregion


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
                Shoot(upgradeTag);
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

    void Shoot(string tag)
    {
        string attackFunction = "Attack_" + tag;
        Invoke(attackFunction, 0f);

        //shotOnce = true;
        //waitingForShot = false;
        //call function based on what the tag is
        /*timer = fireSpeed;
        Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        */
    }

    void Attack_Basic()
    {
        timer = fireSpeed;
        Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        shotOnce = true;
        waitingForShot = false;
    }

    public void Attack_Burst()
    {
        Debug.LogWarning("Attack Burst on " + gameObject.name + " does not have an applicable override!");
    }

    void Attack_Strong()
    {
        Debug.LogWarning("Attack Range on " + gameObject.name + " does not have an applicable override!");
    }

    void Attack_Spread()
    {
        Debug.LogWarning("Attack Spread on " + gameObject.name + " does not have an applicable override!");
    }
}
