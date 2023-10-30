using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    PlayerStats player;
    // Start is called before the first frame update
    [HideInInspector]
     public Vector2 moveVector;
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 lastMoveVector;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        lastMoveVector = new Vector2(1, 0f); //set the move direction of the weapon to the right when the game start
        player = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(moveVector.x * player.currentMoveSpeed, moveVector.y * player.currentMoveSpeed);
    }

    public void move(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();

        if (moveVector.x != 0)
        {
            lastHorizontalVector = moveVector.x;
            lastMoveVector = new Vector2(lastHorizontalVector, 0f);//Last move x
        }

        if (moveVector.y != 0)
        {
            lastVerticalVector = moveVector.y;
            lastMoveVector = new Vector2(0f, lastVerticalVector);//Last move y
        }

        if(moveVector.x != 0 && moveVector.y != 0)
        {
            lastMoveVector = new Vector2(lastHorizontalVector, lastVerticalVector); //while moving
        }
    }
}
