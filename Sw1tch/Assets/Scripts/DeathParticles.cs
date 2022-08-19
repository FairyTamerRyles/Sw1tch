using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
