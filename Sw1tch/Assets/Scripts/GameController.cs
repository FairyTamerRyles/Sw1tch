using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using Pathfinding;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject circe;
    [SerializeField]
    private GameObject boe;
    [SerializeField]
    private GameObject triane;

    [SerializeField]
    private GameObject currentPlayer;
    public List<GameObject> liveCharacters;

    [SerializeField]
    private bool paused = false;
    private RoomRandomizer roomRandomizer;
    [SerializeField]
    private List<GameObject> rooms;
    [SerializeField]
    private AStarController aStarController;


    public void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        List<Room> roomsForRandomizing = new List<Room>();
        string startingRoom = "";
        foreach(GameObject r in rooms)
        {
            GameObject rInstance = Instantiate(r, new Vector3(0, 0.5f, 0), Quaternion.identity);
            Room rRoom = rInstance.GetComponent<Room>();
            roomsForRandomizing.Add(rRoom);

            if(currentPlayer.name != rRoom.SpawnRoomOfCharacter())
            {
                CameraController.Instance.SetCameraPriority(rRoom.RoomCamera(), 0);
            }
            else
            {
                CameraController.Instance.SetCameraPriority(rRoom.RoomCamera(), 1);
                startingRoom = rInstance.name;
            }
            if(rRoom.SpawnRoomOfCharacter() == "Boe")
            {
                CameraController.Instance.SetCameraTarget(rRoom.RoomCamera(), boe);
                boe.transform.parent = rInstance.transform;
            }
            else if(rRoom.SpawnRoomOfCharacter() == "Triane")
            {
                CameraController.Instance.SetCameraTarget(rRoom.RoomCamera(), triane);
                triane.transform.parent = rInstance.transform;
            }
            else if(rRoom.SpawnRoomOfCharacter() == "Circe")
            {
                CameraController.Instance.SetCameraTarget(rRoom.RoomCamera(), circe);
                circe.transform.parent = rInstance.transform;
            }
            else
            {
                CameraController.Instance.SetCameraTarget(rRoom.RoomCamera(), currentPlayer);
            }
            AstarPath.FindAstarPath();
            AstarPath.active.Scan();
        }



        //Cursor.visible = false;
        //InvokeRepeating("randomlyChangePlayer", 5.0f, 5.0f);
        liveCharacters.Add(circe);
        liveCharacters.Add(boe);
        liveCharacters.Add(triane);
        roomRandomizer = new RoomRandomizer();
        roomRandomizer.RandomizeRooms(roomsForRandomizing, startingRoom);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool Paused()
    {
        return paused;
    }

    public void SetPaused(bool value)
    {
        paused = value;
    }

    private void changeCurrentPlayer(GameObject newCurrentPlayer)
    {
        if(!paused)
        {
            if(newCurrentPlayer != currentPlayer)
            {
                //currentPlayer.GetComponent<IPlayable>().Reset(); *****YOU NEED THIS TO RESET THE CHARACTERS
                currentPlayer.SetActive(false);
                if(currentPlayer == circe)
                {
                    GameObject[] circeProjectiles = GameObject.FindGameObjectsWithTag("Circe");
                    foreach(GameObject projectile in circeProjectiles)
                    {
                        projectile.SetActive(false);
                    }
                }
                else if(currentPlayer == boe)
                {
                    GameObject[] boeProjectiles = GameObject.FindGameObjectsWithTag("Boe");
                    foreach(GameObject projectile in boeProjectiles)
                    {
                        projectile.SetActive(false);
                    }
                }
                else if(currentPlayer == triane)
                {
                    GameObject[] trianeProjectiles = GameObject.FindGameObjectsWithTag("Triane");
                    foreach(GameObject projectile in trianeProjectiles)
                    {
                        projectile.SetActive(false);
                    }
                }
                newCurrentPlayer.SetActive(true);
                if(newCurrentPlayer == circe)
                {
                    GameObject[] circeProjectiles = GameObject.FindGameObjectsWithTag("Circe");
                    foreach(GameObject projectile in circeProjectiles)
                    {
                        projectile.SetActive(true);
                    }
                    circe.GetComponent<PlayerChar>().switchOdds = 1;
                    triane.GetComponent<PlayerChar>().switchOdds++;
                    boe.GetComponent<PlayerChar>().switchOdds++;
                }
                else if(newCurrentPlayer == boe)
                {
                    GameObject[] boeProjectiles = GameObject.FindGameObjectsWithTag("Boe");
                    foreach(GameObject projectile in boeProjectiles)
                    {
                        projectile.SetActive(true);
                    }
                    circe.GetComponent<PlayerChar>().switchOdds++;
                    triane.GetComponent<PlayerChar>().switchOdds++;
                    boe.GetComponent<PlayerChar>().switchOdds = 1;
                }
                else if(newCurrentPlayer == triane)
                {
                    GameObject[] trianeProjectiles = GameObject.FindGameObjectsWithTag("Triane");
                    foreach(GameObject projectile in trianeProjectiles)
                    {
                        projectile.SetActive(true);
                    }
                    circe.GetComponent<PlayerChar>().switchOdds++;
                    triane.GetComponent<PlayerChar>().switchOdds = 1;
                    boe.GetComponent<PlayerChar>().switchOdds++;
                }
                CameraController.Instance.SwitchCameraTarget(newCurrentPlayer, currentPlayer.transform.parent.GetComponent<Room>());
                currentPlayer = newCurrentPlayer;

                //activeEnemiesTargetPlayer();
            }
        }
    }

    void randomlyChangePlayer()
    {
        GameObject chosenOne = currentPlayer;
        List<GameObject> choosableChars = new List<GameObject>();
        foreach(GameObject character in liveCharacters)
        {
            if(character != currentPlayer)
            {
                choosableChars.Add(character);
            }
        }
        //Debug.Log("Choosable characters count: " + choosableChars.Count);
        if(choosableChars.Count > 1)
        {
            GameObject currentFavorite = choosableChars[0];
            foreach(GameObject option in choosableChars)
            {
                if(option.GetComponent<PlayerChar>().switchOdds >= currentFavorite.GetComponent<PlayerChar>().switchOdds)
                {
                    float rand = Random.Range(0f, 7f);
                    if(rand < option.GetComponent<PlayerChar>().switchOdds)
                    {
                        currentFavorite = option;
                    }
                }
            }
            chosenOne = currentFavorite;
        }
        else if (choosableChars.Count == 1)
        {
            chosenOne = choosableChars[0];
        }
        if(chosenOne != currentPlayer)
        {
            changeCurrentPlayer(chosenOne);
        }
    }

    public void changeRooms(Room newRoom, Transform spawnPoint)
    {
        Room previousRoom = currentPlayer.transform.parent.gameObject.GetComponent<Room>();
        
        //first we make the player a child of the new room
        currentPlayer.transform.parent = newRoom.gameObject.transform;
        //next, disable the previous room
        previousRoom.DisableRoom();
        //now we activate the new room
        newRoom.EnableRoom();
        //move the player to the appropriate Spawn
        currentPlayer.transform.position = spawnPoint.position;
        //change the camera
        newRoom.RoomCamera().Priority = 1;
        previousRoom.RoomCamera().Priority = 0;

        //play the enter room animations, which will unpause the swapping
    }
}
