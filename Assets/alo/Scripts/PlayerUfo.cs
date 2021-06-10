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

    [Space(10)]
    [SerializeField] private PlayerUfoInfo runTimeInfo;

    private PlayerControls playerControls;
    private Rigidbody2D rigidBody;
    private Material materialBody;
    private PlayerInput playerInput;

    private float _thrust = 0f;
    private float _direction = 0f;

    private string _playerName = "Player1";

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

        SetupActionEvents();

        respawnPoint = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfo();
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

    private void SetupActionEvents()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");

        playerControls.Ufo.Fire.performed += FirePerformed;

        playerControls.Ufo.Thrust.performed += ThrustPerformed;
        playerControls.Ufo.Thrust.canceled += ThrustCanceled; ;

        playerControls.Ufo.Rotate.performed += RotatePerformed;
        playerControls.Ufo.Rotate.canceled += RotateCanceled;
    }

    private void RotateCanceled(InputAction.CallbackContext context)
    {
        _direction = 0f;
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Direction: " + _direction);
    }

    private void ThrustCanceled(InputAction.CallbackContext context)
    {
        _thrust = 0f;
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Read Value" + _thrust);
    }

    private void RotatePerformed(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<float>();
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Direction: " + _direction);
    }

    private void ThrustPerformed(InputAction.CallbackContext context)
    {
        _thrust = context.ReadValue<float>();
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Read Value" + _thrust);
    }

    private void FirePerformed(InputAction.CallbackContext context)
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): " + _playerName + " Open Fire!!!!");
    }

    public float thrust
    {
        get { return _thrust; } 
    }

    public float direction
    {
        get { return _direction; }
    }

    public string playerName
    {
        get => _playerName;
        set => _playerName = value;
    }

    public void Respawn()
    {
        Debug.Log("Respawn Ufo!");
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = 0f;

        transform.position = respawnPoint;
        transform.forward = Vector3.zero;
    }

    private void UpdateInfo()
    {
        runTimeInfo.direction = _direction;
        runTimeInfo.thrust = _thrust;
    }
}

[System.Serializable]
class PlayerUfoInfo
{
    [ReadOnly]
    public float thrust;
    [ReadOnly]
    public float direction;
}
