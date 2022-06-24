using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Alt : MonoBehaviour
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
    [SerializeField]
    private Camera cam;
    private bool isShooting;
    private bool shotOnce = false;
    private bool waitingForShot = false;

    [SerializeField]
    private Attack atk;

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
        StopFire();
    }

    void Awake()
    {
        if(playerInput == null)
        {
            playerInput = new InputMaster();
        }
        playerInput.Player.Fire.performed += context => StartFire();
        playerInput.Player.StopFire.performed += context => StopFire();
    }

    void FixedUpdate()
    {
        if(timer <= 0.1) //timer makes sure you are within firing timeframe
        {
            if(isShooting) //checks if we are shooting right now, or if we shot, let go, but need to fire once
            {
                FireAlt(upgradeTag);
            }
        }
        else
        {
            timer -= Time.fixedDeltaTime;
        }

    }

    
    void StartFire()
    {
        if(timer <= 0.12)
        {
            //timer = fireSpeed;
            FireAlt(upgradeTag);
        }
        isShooting = true;
    }
    void StopFire()
    {
        if(!waitingForShot)
        {
            isShooting = false;
            shotOnce = false;
        }
    }

    void FireAlt(string tag)
    {
        atk.SetCanShoot(false);
        string attackFunction = "Attack_" + tag;
        Invoke(attackFunction, 0f);
    }

    void Alt_Basic()
    {
        //Fire the animation, which should handle all the alts different calculations
        //At the end of this animation, the fire rate needs to be activated with atk.SetCanShoot(true);
    }

    void Alt_Burst()
    {
        Debug.LogWarning("Alt Burst on " + gameObject.name + " does not have an applicable override!");
    }

    void Alt_Strong()
    {
        Debug.LogWarning("Alt Range on " + gameObject.name + " does not have an applicable override!");
    }

    void Alt_Spread()
    {
        Debug.LogWarning("Alt Spread on " + gameObject.name + " does not have an applicable override!");
    }
}
