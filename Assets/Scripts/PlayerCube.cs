using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCube : MonoBehaviour
{
    [SerializeField] private float speed, jumpSpeed;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private Renderer rendererBody;
    [SerializeField] private Collider2D colliderBody;

    private PlayerControls playerControls;
    private Rigidbody2D rigidBody;

    private Material materialBody;

    private void Awake()
    {
        playerControls = new PlayerControls();
        Assert.IsNotNull(playerControls, "playerControls is null");

        rigidBody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rigidBody, "rigidBody2D is null");

        Assert.IsNotNull(rendererBody, "rendererBody is null");
        Assert.IsNotNull(colliderBody, "colliderBody is null");

        materialBody = rendererBody.material;
        Assert.IsNotNull(materialBody, "materialBody is null");
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
        materialBody.color = Color.green;

        playerControls.Land.Jump.performed += _ => jump();
    }

    private bool isGrounded()
    {
        Vector2 topLeft = transform.position;
        topLeft.x -= colliderBody.bounds.extents.x;
        topLeft.y += colliderBody.bounds.extents.y;

        Vector2 bottomRight = transform.position;
        bottomRight.x += colliderBody.bounds.extents.x;
        bottomRight.y -= colliderBody.bounds.extents.y;

        return Physics2D.OverlapArea(topLeft, bottomRight, layerGround);
    }

    private void jump()
    {
        Debug.Log("Jump Pressed!");

        if (isGrounded())
        {
            Debug.Log("Grounded True = Jump!");
            rigidBody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);

            Debug.Log("Changing color => Green");
            materialBody.color = Color.blue;
        }
        else
        {
            Debug.Log("Grounded False = No Jump!");
            materialBody.color = Color.red;
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
