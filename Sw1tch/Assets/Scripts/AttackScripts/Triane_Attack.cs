using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triane_Attack : Attack
{
    [SerializeField]
    private Transform leftSpawnPoint, rightSpawnPoint, backSpawnPoint;
    [SerializeField]
    private GameObject burstProjectile, spreadProjectile, strongProjectile;

    void Attack_Burst()
    {
        Timer = FireSpeed;
        Instantiate(burstProjectile, SpawnPoint.position, SpawnPoint.rotation);
        ShotOnce = true;
        WaitingForShot = false;
    }

    void Attack_Strong()
    {
        Timer = FireSpeed;
        Instantiate(strongProjectile, SpawnPoint.position, SpawnPoint.rotation);
        ShotOnce = true;
        WaitingForShot = false;
    }

    void Attack_Spread()
    {
        Timer = FireSpeed;
        Instantiate(Projectile, SpawnPoint.position, SpawnPoint.rotation);
        Instantiate(spreadProjectile, leftSpawnPoint.position, leftSpawnPoint.rotation);
        Instantiate(spreadProjectile, rightSpawnPoint.position, rightSpawnPoint.rotation);
        Instantiate(spreadProjectile, backSpawnPoint.position, backSpawnPoint.rotation);
        ShotOnce = true;
        WaitingForShot = false;
    }
}
