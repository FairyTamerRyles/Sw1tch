using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;

    public static CameraController Instance { get; private set; }

    void Awake ()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SwitchCameraTarget(GameObject newTarget)
    {
        vCam.Follow = newTarget.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
