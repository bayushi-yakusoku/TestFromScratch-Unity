using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, jumpSpeed;
    [SerializeField] private LayerMask ground;

    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerControls.Land.Jump.performed += _ => jump();
    }

    private bool isGrounded()
    {
        Vector2 topLeft = transform.position;
        topLeft.x -= col.bounds.extents.x;
        topLeft.y += col.bounds.extents.y;

        Vector2 bottomRight = transform.position;
        bottomRight.x += col.bounds.extents.x;
        bottomRight.y -= col.bounds.extents.y;

        return Physics2D.OverlapArea(topLeft, bottomRight, ground);
    }

    private void jump()
    {
        Debug.Log("Jump Pressed!");

        if (isGrounded())
        {
            Debug.Log("Grounded True = Jump!");
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Grounded False = No Jump!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Read the movement value
        float movementInput = playerControls.Land.Move.ReadValue<float>();

        // Move the player
        Vector3 currentPosition = transform.position;
        currentPosition.x += movementInput * speed * Time.deltaTime;

        transform.position = currentPosition;
    }
}
