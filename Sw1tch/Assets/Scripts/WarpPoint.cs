using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    [SerializeField]
    private bool locked;
    [SerializeField]
    private Room parentRoom;
    [SerializeField]
    private WarpPoint adjWarp;
    [SerializeField]
    private Transform spawnPoint;


    public Transform SpawnPoint()
    {
        return spawnPoint;
    }
    public WarpPoint AdjWarp()
    {
        return adjWarp;
    }
    public Room ParentRoom()
    {
        return parentRoom;
    }
    public void SetAdjWarp(WarpPoint a)
    {
        adjWarp = a;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.GetComponent<PlayerChar>() != null)
        {
            //play warp animation 
        }
    }



}
