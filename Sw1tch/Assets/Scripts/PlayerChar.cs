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
    private GameObject sprite;
    private Vector2 moveInput;
    
    public float lerpVar;

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
        moveInput = playerInput.Player.Movement.ReadValue<Vector2>();
    }

    private void Move()
    {
        float speedX = maxRunSpeed * moveInput.x;
        float speedY = maxRunSpeed * moveInput.y;
        speedX = Mathf.Lerp(rb.velocity.x, speedX, lerpVar);
        speedY = Mathf.Lerp(rb.velocity.y, speedY, lerpVar);

        rb.velocity = new Vector2(speedX, speedY);

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
}
