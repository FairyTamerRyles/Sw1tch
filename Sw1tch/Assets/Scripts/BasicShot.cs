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



    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
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
        if(timer <= 0)
        {
            Shoot();
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
        isShooting = false;
    }

    void Shoot()
    {
        timer = fireSpeed;
        if(isShooting)
        {
            Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
