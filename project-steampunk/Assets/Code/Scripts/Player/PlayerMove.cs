using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.VFX;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviour
{
    private Animator animatorPlayer; //animator for change hands anim

    //move data
    [Header("���������� �����������")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float mouseSense;
    public float MouseSense { get { return mouseSense; } set { mouseSense = value; } }

    [Header("Cinemachine Virtual Cameras")]
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private CinemachineVirtualCamera camTackle;
    private Animator animatorCinemachineVirtualCam;// �������� �������� ������ ��� �������� �� A D
    private CinemachineBasicMultiChannelPerlin camNoise;//��� ��� ��������� ���� ������, ����������� ������


    [Header("���������� ������� ground")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform dotGround;
    [SerializeField] private float verticalDamping = 0.5f;



    private float xRotation;
    private Rigidbody rb;
    private ActionPrototypePlayer inputActions;
    private bool isGrounded;

    //state player movement
    //private State state;

    //hook data 
    //[SerializeField] private Transform debugRayHit; //����� ��� ����
    /*[Header("�������� ����")]
    [SerializeField] private float distanceHook;
    [SerializeField] private float timeNextHook;//����������� ���� �����
    private Vector3 hookPosition;
    private float timerHook;
    */

    //dash data
    [Header("Dash ����������")]
    [SerializeField] private int layerIgnore; //������������� �������� ����
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTimeLimit;//����� �����
    [SerializeField] private float dashCooldown;//����� ����������� �����
    private float dashTimer;


    //tackle data
    [Header("���������� �������")]
    [SerializeField] private float tackleSpeed;//�������� �������
    [SerializeField] private float tackleSpeedSubtraction; // ���������� �������� ��� ������ ������� �������
    [SerializeField] private float tackleTimeLimit;//����� �������
    [SerializeField] private float colliderHeight;//������ ���������� �� ����� �������
    private CapsuleCollider[] capsuleColliders;
    private bool tackleActive;
    private float oldSpeed;

    private int doubleJump;

    //wallRunning
    /*
    [Header("���������� ���� �� �����")]
    [SerializeField] private float wallMoveTime;
    [SerializeField] private float wallRayRange;
    [SerializeField] private LayerMask wallLayer;
    private float wallRunDetect;
    private bool wallRight;
    private bool wallLeft;
    */

    //kick
    [Header("���������� �����")]
    [SerializeField] private float maxDistanceKick;
    [SerializeField] private float kickForce;
    [SerializeField] private LayerMask enemyLayer;

    //vfx effects move
    private VisualEffect effectDash;

    private void Awake()
    {
        
        Physics.gravity = new Vector3(0, -8.61f, 0); //change gravity
        capsuleColliders = GetComponents<CapsuleCollider>();//change collider in slide
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        animatorPlayer = GetComponentInChildren<Animator>();
        inputActions = new ActionPrototypePlayer();
        inputActions.Player.Enable();

        //camera and vfx effects move getcomponents
        effectDash = GetComponentInChildren<VisualEffect>();
        camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        animatorCinemachineVirtualCam = GameObject.Find("VirtualCameres").GetComponent<Animator>();


    }
    public float GetSpeed()
    {
        return speed;
    }
    private void Update()
    {
        OnDrawGizmosSelected();
        IsGrounded();


    }
    private void FixedUpdate()
    {
        Vector2 inputMove = inputActions.Player.Move.ReadValue<Vector2>();
        Vector2 inputLook = inputActions.Player.Look.ReadValue<Vector2>();
        rb.velocity += Vector3.up * Physics.gravity.y * verticalDamping;
        Move(inputMove);
        Rotation(inputLook);

        Debug.Log(inputMove);
        if ((inputMove.y < 0 || inputMove.x != 0) && tackleActive == true)
        {
            rb.velocity = new Vector3((inputMove.y < 0) ?
            -rb.velocity.x : rb.velocity.x, rb.velocity.y, 0);
        }
        EffectsMove(inputMove);
    }
    //effects move
    public void EffectsMove(Vector2 _inputMove)
    {
        if (IsGrounded())
        { camNoise.m_AmplitudeGain = _inputMove.magnitude * 2; }
        else { camNoise.m_AmplitudeGain = 0; }
        animatorCinemachineVirtualCam.SetFloat("rotateCam", _inputMove.x, 0.1f, Time.deltaTime);
    }
    //Movement
    private void Rotation(Vector2 inputLook)
    {
        float xLook = inputLook.x * mouseSense;
        float yLook = inputLook.y * mouseSense;

        xRotation -= yLook;
        xRotation = Mathf.Clamp(xRotation, -55.5f, 55.5f);


        if (tackleActive)
            camTackle.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        else
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * xLook);
    }
    private void Move(Vector2 inputMove)
    {
        Vector3 move = transform.right * inputMove.x + transform.forward * inputMove.y;
        rb.velocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);

        animatorPlayer.SetFloat("speed", inputMove.magnitude, 0.1f, Time.deltaTime);
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (dashTimer <= Time.time)
            {
                dashTimer = Time.time + dashCooldown;
                StartCoroutine(DashCoroutine());
            }
        }
    }
    IEnumerator DashCoroutine()
    {

        oldSpeed = speed;
        speed = dashSpeed;
        Physics.IgnoreLayerCollision(gameObject.layer, layerIgnore, true);
        effectDash.Play();
        yield return new WaitForSeconds(dashTimeLimit);
        effectDash.Stop();
        Physics.IgnoreLayerCollision(gameObject.layer, layerIgnore, false);
        speed = oldSpeed;

    }

    //������
    /*
    public void Tackle(InputAction.CallbackContext context)
    {
        if (inputActions.Player.Move.ReadValue<Vector2>().y > 0)
        {
            StartCoroutine(TackleCoroutine(context));
        }
    }

    IEnumerator TackleCoroutine(InputAction.CallbackContext contextCoroutine)
    {
        float oldSpeed;
        float oldScale;
        oldSpeed = speed;
        oldScale = capsuleColliders[0].height;
        speed = tackleSpeed;
        while (contextCoroutine.performed)
        {
            animatorPlayer.SetBool("slide", true);


            tackleActive = true;
            speed -= (speed > 0) ? tackleSpeedSubtraction : 0f;
            capsuleColliders[0].height = colliderHeight;
            capsuleColliders[1].height = capsuleColliders[0].height + 0.25f;
            cam.enabled = false;
            camTackle.enabled = true;
            yield return new WaitForSeconds(tackleTimeLimit);
        }
        speed = oldSpeed;
        capsuleColliders[0].height = oldScale;
        capsuleColliders[1].height = capsuleColliders[0].height + 0.25f;
        tackleActive = false;
        cam.enabled = true;
        camTackle.enabled = false;


        animatorPlayer.SetBool("slide", false);
    }
      */

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log(doubleJump);
        if (context.phase == InputActionPhase.Started && IsGrounded() == true
            && tackleActive == false)
        {
            animatorPlayer.SetBool("jump", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if (doubleJump == 1 && context.phase == InputActionPhase.Started)
        {
            animatorPlayer.SetBool("jump", true);
            // rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            StartCoroutine(JumpCoroutineUpSpeed());
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doubleJump = 0;
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            animatorPlayer.SetBool("jump", false);
        }
    }
  

    IEnumerator JumpCoroutineUpSpeed()
    {
        speed = speed * 1.3f;
        yield return new WaitForSeconds(0.1f);
        speed = speed / 1.3f;
    }
    private bool IsGrounded()
    {
        float sphereRadius = 1f;
        isGrounded = Physics.CheckSphere(dotGround.position, sphereRadius, groundLayer);
        
        if (isGrounded == true) { doubleJump = 1; }
        return isGrounded;
    }
    void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(dotGround.position, 1f);
    }
    //hookShot
    /*
    public void HandleHookShot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && timerHook <= Time.time)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, distanceHook))
            {
                timerHook = Time.time + timeNextHook;
                debugRayHit.position = hit.point;
                hookPosition = hit.point;
                state = State.HookShotFly;
            }

        }

    }
    private void HandleHookMovement()
    {
        float hookSpeedMin = 10f;
        float hookSpeedMax = 40f;
        float speedHook = Mathf.Clamp(Vector3.Distance(transform.position, hookPosition), hookSpeedMin,
            hookSpeedMax);
        float speedHookMultiplie = 2f;
        Vector3 hookShootDir = (hookPosition - transform.position).normalized;
        rb.velocity = hookShootDir * speedHook * speedHookMultiplie;
        if (Vector3.Distance(transform.position, hookPosition) < 1f)
        {
            state = State.Normal;
        }
        if (inputActions.Player.HookShot.phase == InputActionPhase.Started)
        {
            state = State.Normal;
            rb.velocity = Vector3.zero;
        }
    }
    */


    //ground check

    //wall run
    /* private void WallRunningState()
     {
         wallRight = Physics.Raycast(transform.position, transform.right, wallRayRange, wallLayer);
         wallLeft = Physics.Raycast(transform.position, -transform.right, wallRayRange, wallLayer);
         if ((wallRight || wallLeft) && rb.velocity.y > 0f && !IsGrounded())
         {
             state = State.WallRuning;
             wallRunDetect = Time.time + wallMoveTime;
         }
     }
     private void WallRunningMovement()
     {
         if (wallRunDetect > Time.time)
         {

             Vector2 inputWallMove = inputActions.Player.WallRun.ReadValue<Vector2>();
             Vector3 moveWall = transform.right * inputWallMove.x + transform.forward * inputWallMove.y;
             rb.velocity = new Vector3(moveWall.x * speed, 0, moveWall.z * speed);
             rb.useGravity = false;
         }
         if (wallRunDetect < Time.time || (!wallLeft && !wallRight) ||
             rb.velocity.x == 0)
         {
             state = State.Normal;
             rb.useGravity = true;
         }

     }
    */

    //hit leg (kick)
    public void Kick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward,
                out RaycastHit hit, maxDistanceKick, enemyLayer))
            {
                hit.collider.GetComponent<Rigidbody>().
                    AddForce(((hit.collider.transform.position - transform.position).normalized)
                    * kickForce, ForceMode.Impulse);
            }
        }
    }

}
