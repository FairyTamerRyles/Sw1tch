using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    public Vector2 mousePos;
    private InputMaster playerInput;
    [SerializeField]
    private float maxRunSpeed;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject reticle;
    [SerializeField]
    private GameObject sprite;
    private Vector2 moveInput;
    [SerializeField]
    private bool beingMoved;
    [SerializeField]
    private float switchSpeed;
    [SerializeField]
    private float roomSwitchDistance;
    [SerializeField]
    private Vector2 targetPos;
    [SerializeField]
    private bool switchingRooms;

    //private IAttack attack;
    //private IAlt alt;
    //private IUlt ult;
    
    public float lerpVar;

    public float switchOdds = 1;

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    void Awake()
    {
        if(playerInput == null)
        {
            playerInput = new InputMaster();
        }
        //playerInput.Player.Movement.performed += context => Move();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(switchingRooms)
        {
            if(Vector2.Distance(transform.position, targetPos) > 1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, switchSpeed * Time.deltaTime);
            }
            Debug.Log("Distance: " + Vector2.Distance(transform.position, targetPos));
            Debug.Log(targetPos);
            if(Vector2.Distance(transform.position, targetPos) <= 1f)
            {
                Debug.Log("All Good");
                playerInput.Enable();
                switchingRooms = false;
            }
        }
        else
        {
            moveInput = playerInput.Player.Movement.ReadValue<Vector2>();
        }
    }

    private void Move()
    {
        float speedX = maxRunSpeed * moveInput.x;
        float speedY = maxRunSpeed * moveInput.y;
        speedX = Mathf.Lerp(rb.velocity.x, speedX, lerpVar);
        speedY = Mathf.Lerp(rb.velocity.y, speedY, lerpVar);

        rb.velocity = new Vector2(speedX, speedY);
        //For some reason, while this code is being executed, it does not however change the rotation if you do not actually move the mouse
        Vector2 mouseScreenPosition = playerInput.Player.MousePosition.ReadValue<Vector2>();
        mousePos = cam.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        reticle.transform.eulerAngles = new Vector3(0, 0, angle);
        sprite.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void FixedUpdate()
    {
        Move();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "WarpPoint")
        {
            //play room change animation
            GameObject.Find("GameController").GetComponent<GameController>().changeRooms(col.gameObject.GetComponent<WarpPoint>().AdjWarp().ParentRoom(), col.gameObject.GetComponent<WarpPoint>().AdjWarp().SpawnPoint());
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Connector" && !switchingRooms)
        {
            Connector c = col.gameObject.GetComponent<Connector>();
            ChangeRooms(c.ConnectedRoom, c.DirectionToTravel);
        }
    }

    public void ChangeRooms(Room newRoom, Vector2 dirToTravel)
    {
        Room previousRoom = transform.parent.gameObject.GetComponent<Room>();
        playerInput.Disable();
        switchingRooms = true;
        
        //first we make the player a child of the new room
        transform.parent = newRoom.gameObject.transform;
        float x = dirToTravel.x;
        float y = dirToTravel.y;

        x *= roomSwitchDistance;
        y *= roomSwitchDistance;

        Debug.Log("Distances to add: " + x + "," + y);

        Vector2 currentPos = transform.position;
        targetPos = new Vector2(currentPos.x + x, currentPos.y + y);
        Debug.Log(currentPos);
        Debug.Log(targetPos);

        //change the camera
        newRoom.RoomCamera().Priority = 1;
        previousRoom.RoomCamera().Priority = 0;

        //play the enter room animations, which will unpause the swapping
    }
}
