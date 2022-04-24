using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [SerializeField]
    private Vector2 directionToTravel;
    [SerializeField]
    private Room connectedRoom;

    public Vector2 DirectionToTravel
    {
        get { return directionToTravel; }
    }
    public Room ConnectedRoom
    {
        get { return connectedRoom; }
    }
}
