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

    public void SwitchCameraTarget(GameObject newTarget, Room currentRoom)
    {
        //vCam.Follow = newTarget.transform;
        if(newTarget.transform.parent.GetComponent<Room>() != null)
        {
            Room newRoom = newTarget.transform.parent.GetComponent<Room>();
            if(newRoom != currentRoom)
            {
                currentRoom.RoomCamera().Priority = 0;
                newRoom.RoomCamera().Priority = 1;
            }
        }
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
