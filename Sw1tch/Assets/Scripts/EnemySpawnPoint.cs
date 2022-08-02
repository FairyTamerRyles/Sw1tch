using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public GameObject enemy;
    public GameObject room;
    // Start is called before the first frame update
    void Start()
    {
        GameObject e = Instantiate(enemy, transform.position, Quaternion.identity);
        e.transform.parent = room.transform;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
