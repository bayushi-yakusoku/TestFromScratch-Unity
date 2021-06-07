using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerUfo : MonoBehaviour
{
    [SerializeField] private Renderer rendererBody;
    [SerializeField] private Collider2D colliderBody;
    [SerializeField] private float rotor;
    [SerializeField] private float rotateSpeed;

    private PlayerControls playerControls;
    private Rigidbody2D rigidBody;
    private Material materialBody;
    private PlayerInput playerInput;

    private float _thrust = 0f;
    private float _direction = 0f;

    private Vector3 respawnPoint;

    private void Awake()
    {
        playerControls = new PlayerControls();
        
        rigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        materialBody = rendererBody.material;

        Assert.IsNotNull(playerControls, "playerControls is null");
        Assert.IsNotNull(rigidBody, "rigidBody is null");
        Assert.IsNotNull(playerInput, "playerInput is null");
        Assert.IsNotNull(materialBody, "materialBody is null");

        setupActionEvents();

        respawnPoint = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        Vector3 rotate = new Vector3(0, 0, -_direction);

        transform.Rotate(rotate * rotateSpeed);

        rigidBody.AddRelativeForce(new Vector2(0, 1) * _thrust * rotor);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void setupActionEvents()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");

        playerControls.Ufo.Fire.performed += Fire_performed;

        playerControls.Ufo.Thrust.performed += Thrust_performed;
        playerControls.Ufo.Thrust.canceled += Thrust_canceled; ;

        playerControls.Ufo.Rotate.performed += Rotate_performed;
        playerControls.Ufo.Rotate.canceled += Rotate_canceled;
    }

    private void Rotate_canceled(InputAction.CallbackContext context)
    {
        _direction = 0f;
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Direction: " + _direction);
    }

    private void Thrust_canceled(InputAction.CallbackContext context)
    {
        _thrust = 0f;
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Read Value" + _thrust);
    }

    private void Rotate_performed(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<float>();
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Direction: " + _direction);
    }

    private void Thrust_performed(InputAction.CallbackContext context)
    {
        _thrust = context.ReadValue<float>();
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Read Value" + _thrust);
    }

    private void Fire_performed(InputAction.CallbackContext context)
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");
    }

    public float thrust
    {
        get { return _thrust; } 
    }

    public float direction
    {
        get { return _direction; }
    }

    public void respawn()
    {
        Debug.Log("Respawn Ufo!");
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = 0f;

        transform.position = respawnPoint;
        transform.forward = Vector3.zero;
    }
}
