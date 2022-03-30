using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondHand : MonoBehaviour
{
    bool rotate;
    public float rotationSpeed;
    float degreesTurned = 0;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("rotateHand", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rotate)
        {
            transform.Rotate(0, 0, -rotationSpeed);
            degreesTurned = degreesTurned - rotationSpeed;
            Debug.Log(degreesTurned);
            if(degreesTurned <= -30)
            {
                rotate = false;
                degreesTurned = 0;
            }
        }
    }

    void rotateHand()
    {
        rotate = true;
    }
}
