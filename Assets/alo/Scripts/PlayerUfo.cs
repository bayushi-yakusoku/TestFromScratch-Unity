using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

// Helicopter Movements:
// - Pitch    : Inclinaison Avant/Arriere
// - Roll     : Inclinaison Gauche/Droite
// - Yaw      : Rotation sur l'Axe vertical
// - Throttle : Translation Verticale (Haut/Bas)

// Helicopter Controls:
// - Cyclic (Manche)                  : Pitch & Roll
// - Collective (Levier)              : Throttle
// - Anti-Torque (Pedales)            : Yaw
// - Throttle (Poignee sur le levier) : Throttle

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerUfo : MonoBehaviour
{
    [SerializeField] private Renderer rendererBody;
    [SerializeField] private Collider2D colliderBody;
    [SerializeField] private float enginePower;
    [SerializeField] private float rotateMultiplicator;
    [SerializeField] private bool deltaModeDirection = true;

    private float targetPitch;
    [Range(0, 90)] [SerializeField] private int maxPitch;
    [Range(0, 10)] [SerializeField] private int correctionAngle;

    [Space(10)]
    [SerializeField] private PlayerUfoInfo runTimeInfo;

    private PlayerControls playerControls;
    private Rigidbody2D rigidBody;
    private Material materialBody;
    private PlayerInput playerInput;

    private float collective = 0f;
    public float Collective { get => collective; }

    private float cyclic = 0f;
    public float Cyclic { get => cyclic; }

    public string PlayerName { get; set; } = "Player1";

    private Vector3 respawnPoint;

    private void Awake()
    {
        InitAndGetComponents();

        ChecksAndAsserts();

        BindInputActionWithMethods();
    }

    private void InitAndGetComponents()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");

        playerControls = new PlayerControls();

        rigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        materialBody = rendererBody.material;

        respawnPoint = transform.position;
    }

    private void ChecksAndAsserts()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");

        Assert.IsNotNull(playerControls, "playerControls is null");
        Assert.IsNotNull(rigidBody, "rigidBody is null");
        Assert.IsNotNull(playerInput, "playerInput is null");
        Assert.IsNotNull(materialBody, "materialBody is null");
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
        UpdateDirection();

        rigidBody.AddRelativeForce(new Vector2(0, collective * enginePower));
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void BindInputActionWithMethods()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");

        playerControls.Ufo.Fire.performed += FirePerformed;

        playerControls.Ufo.Klaxxon.performed += KlaxxonPerformed;

        playerControls.Ufo.Profile.performed += ProfilePerformed;

        playerControls.Ufo.Throttle.performed += ThrottlePerformed;
        playerControls.Ufo.Throttle.canceled += ThrottleCanceled; ;

        playerControls.Ufo.Rotate.performed += RotatePerformed;
        playerControls.Ufo.Rotate.canceled += RotateCanceled;
    }

    private void ProfilePerformed(InputAction.CallbackContext obj)
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ... ");
    }

    private void KlaxxonPerformed(InputAction.CallbackContext context)
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ... ");
    }

    private void RotatePerformed(InputAction.CallbackContext context)
    {
        cyclic = context.ReadValue<float>();
        targetPitch = (int) (- cyclic * maxPitch);
        //Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Direction: " + direction);
    }

    private void RotateCanceled(InputAction.CallbackContext context)
    {
        cyclic = 0f;
        targetPitch = 0f;

        //Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Direction: " + direction);
    }

    private void ThrottlePerformed(InputAction.CallbackContext context)
    {
        collective = context.ReadValue<float>();
        //Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Read Value " + throttle);
    }

    private void ThrottleCanceled(InputAction.CallbackContext context)
    {
        collective = 0f;
        //Debug.Log(MethodBase.GetCurrentMethod().Name + "(): Read Value " + throttle);
    }

    private void FirePerformed(InputAction.CallbackContext context)
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): " + PlayerName + " Open Fire!!!!");
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
        runTimeInfo.cyclic = cyclic;
        runTimeInfo.collective = collective;
        runTimeInfo.targetPitch = targetPitch;
    }

    private void UpdateDirection()
    {
        if (deltaModeDirection)
        {
            DeltaModeDirection();
        }
        else
        {
            DirectModeDirection();
        }
    }

    private float GetPitch()
    {
        float pitch;
        float zRotation = transform.rotation.eulerAngles.z;

        if (zRotation < 180)
        {
            pitch = zRotation;
        }
        else
        {
            pitch = -(360 - zRotation);
        }

        return (int) pitch;
    }

    private void AutoCorrectPitch(int target, int correctionAngle)
    {
        if (cyclic != 0f)
        {
            runTimeInfo.correctionPitchActivated = false;
            runTimeInfo.correctionPitchTarget = 999999999;
            return;
        }

        float pitch = GetPitch();

        if (Mathf.Abs(pitch) > target)
        {
            runTimeInfo.correctionPitchActivated = true;
            runTimeInfo.correctionPitchTarget = target;

            Vector3 correction = new Vector3(0, 0, - Mathf.Sign(pitch) * correctionAngle);
            transform.Rotate(correction);
        }
        else
        {
            runTimeInfo.correctionPitchActivated = false;
            runTimeInfo.correctionPitchTarget = 999999999;
        }
    }

    private void CorrectPitch(float target, float correctionAngle)
    {
        float pitch = GetPitch();
        float delta = target - pitch;

        if (pitch != target)
        {
            if (Mathf.Abs(delta) < correctionAngle)
            {
                
                transform.Rotate(new Vector3(0, 0, delta));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, Mathf.Sign(delta) * correctionAngle));
            }
        }
    }

    private void DeltaModeDirection()
    {
        float pitch = GetPitch();

        if (Mathf.Abs(pitch) <= maxPitch || 
            Mathf.Sign(targetPitch) != Mathf.Sign(pitch))
        {
            float angle = -cyclic * rotateMultiplicator;

            transform.Rotate(new Vector3(0, 0, angle));
        }
        else
        {
            AutoCorrectPitch(maxPitch, correctionAngle);
        }

        runTimeInfo.pitch = GetPitch();
    }

    private void DirectModeDirection()
    {
        float pitch = GetPitch();

        if (Mathf.Abs(pitch) <= maxPitch ||
            Mathf.Abs(targetPitch) < Mathf.Abs(pitch))
        {
            CorrectPitch(targetPitch, rotateMultiplicator);
        }
        else
        {
            AutoCorrectPitch(0, correctionAngle);
        }

        runTimeInfo.pitch = GetPitch();
    }
}

[System.Serializable]
class PlayerUfoInfo
{
    [ReadOnly] public float collective;
    [ReadOnly] public float cyclic;
    [ReadOnly] public float targetPitch;
    [ReadOnly] public float pitch;
    [ReadOnly] public bool correctionPitchActivated;
    [ReadOnly] public float correctionPitchTarget;
}
