using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boe_Attack : Attack
{
    [SerializeField]
    private Transform leftSpawnPoint, rightSpawnPoint, backSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    new public void Attack_Burst()
    {
        Timer = FireSpeed;
        Instantiate(Projectile, SpawnPoint.position, SpawnPoint.rotation);
        Instantiate(Projectile, leftSpawnPoint.position, leftSpawnPoint.rotation);
        Instantiate(Projectile, rightSpawnPoint.position, rightSpawnPoint.rotation);
        Instantiate(Projectile, backSpawnPoint.position, backSpawnPoint.rotation);
        ShotOnce = true;
        WaitingForShot = false;
    }

    void Attack_Range()
    {
        Debug.LogWarning("Attack Range on " + gameObject.name + " does not have an applicable override!");
    }

    void Attack_Spread()
    {
        Debug.LogWarning("Attack Spread on " + gameObject.name + " does not have an applicable override!");
    }
}
