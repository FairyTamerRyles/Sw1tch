using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject circe;
    [SerializeField]
    private GameObject boe;
    [SerializeField]
    private GameObject trian;

    [SerializeField]
    private GameObject currentPlayer;
    public List<GameObject> liveCharacters;

    [SerializeField]
    private CameraController camController;
    [SerializeField]
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("randomlyChangePlayer", 5.0f, 5.0f);
        liveCharacters.Add(circe);
        liveCharacters.Add(boe);
        liveCharacters.Add(trian);
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
                else if(currentPlayer == trian)
                {
                    GameObject[] trianProjectiles = GameObject.FindGameObjectsWithTag("Triane");
                    foreach(GameObject projectile in trianProjectiles)
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
                    trian.GetComponent<PlayerChar>().switchOdds++;
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
                    trian.GetComponent<PlayerChar>().switchOdds++;
                    boe.GetComponent<PlayerChar>().switchOdds = 1;
                }
                else if(newCurrentPlayer == trian)
                {
                    GameObject[] trianProjectiles = GameObject.FindGameObjectsWithTag("Triane");
                    foreach(GameObject projectile in trianProjectiles)
                    {
                        projectile.SetActive(true);
                    }
                    circe.GetComponent<PlayerChar>().switchOdds++;
                    trian.GetComponent<PlayerChar>().switchOdds = 1;
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
