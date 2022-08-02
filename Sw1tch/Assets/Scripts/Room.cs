using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera roomCamera;
    [SerializeField]
    private List<WarpPoint> warps;
    [SerializeField]
    private string spawnRoomOfCharacter;
    [SerializeField]
    private CompositeCollider2D inBoundsCol;

    public CompositeCollider2D InBoundsCol()
    {
        return inBoundsCol;
    }


    public string SpawnRoomOfCharacter()
    {
        return spawnRoomOfCharacter;
    }
    public List<WarpPoint> Warps()
    {
        return warps;
    }

    public CinemachineVirtualCamera RoomCamera()
    {
        return roomCamera;
    }

    //THINGS A ROOM MUST DO
    //Contain players (make them children of the room for easy switching)
    //Contain a list of doors?
    //need a tag determining how to use the doors
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableRoom()
    {
        gameObject.SetActive(false);
    }

    public void EnableRoom()
    {
        gameObject.SetActive(true);
    }
}
