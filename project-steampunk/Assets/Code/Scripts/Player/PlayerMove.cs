using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Animator animatorPlayer;

    //move data
    [Header("���������� �����������")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float mouseSense;


    [Header("���������� ������� ground")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform dotGround;
    [SerializeField] private float verticalDamping = 0.5f;


    private float xRotation;
    private Camera cam;
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
    [SerializeField] private float tackleSpeed;
    [SerializeField] private float tackleTimeLimit;//����� �������
    [SerializeField] private float tackleCooldown;//����� ����������� �������
    [SerializeField] private float scaleY;//��������� transformPlayer, ���������� �� ����������
    private CapsuleCollider [] capsuleColliders;
   
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

    [SerializeField] private GameObject spheres;

   /* private enum State
    {
        Normal,
        //Sprint,
        //HookShotFly,
        //WallRuning

    }
   */
    private void Awake()
    {
        capsuleColliders = GetComponents<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        animatorPlayer = GetComponent<Animator>();
        inputActions = new ActionPrototypePlayer();
        inputActions.Player.Enable();
        cam = GameObject.FindAnyObjectByType<Camera>();
        //state = State.Normal;
    }
    private void Update()
    {
        //Debug.Log(IsGrounded());
        IsGrounded();
        //Debug.Log(sprintTimer); 
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistanceKick,
                Color.red);
        Vector2 inputMove = inputActions.Player.Move.ReadValue<Vector2>();
        Vector2 inputLook = inputActions.Player.Look.ReadValue<Vector2>();
        rb.velocity += Vector3.up * Physics.gravity.y * verticalDamping;
        Rotation(inputLook);
        Move(inputMove);
        //WallRunningState();
       /* switch (state)
        {
            default:
            case State.Normal:
                
                break;
            case State.HookShotFly:
                HandleHookMovement();
                Rotation(inputLook);
                break;
            case State.WallRuning:
                WallRunningMovement();
                Rotation(inputLook);
                break;
        }
       */
    }

    //Movement
    private void Move(Vector2 inputMove)
    {
        Vector3 move = transform.right * inputMove.x + transform.forward * inputMove.y;
        rb.velocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);
        
    }
    private void Rotation(Vector2 inputLook)
    {
        float xLook = inputLook.x * mouseSense;
        float yLook = inputLook.y * mouseSense;

        xRotation -= yLook;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * xLook);
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
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
        float oldSpeed;
        oldSpeed = speed;
        speed = dashSpeed;
        Physics.IgnoreLayerCollision(gameObject.layer, layerIgnore, true);
        yield return new WaitForSeconds(dashTimeLimit);
        Physics.IgnoreLayerCollision(gameObject.layer, layerIgnore, false);
        speed = oldSpeed;

    }

    //������
    public void Tackle(InputAction.CallbackContext context) 
    {
        if(context.phase == InputActionPhase.Started && (rb.velocity.x > 0 || rb.velocity.z >0.01))
        {
            StartCoroutine(TackleCoroutine());
        }
    }
    IEnumerator TackleCoroutine()
    {
        float oldSpeed;
        float oldScale;
        oldSpeed = speed;
        oldScale = capsuleColliders[0].height;
        speed = tackleSpeed;
        capsuleColliders[0].height = 1.5f;
        capsuleColliders[1].height = capsuleColliders[0].height + 0.25f;
        animatorPlayer.SetBool("tackle",true);
        yield return new WaitUntil(() => animatorPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        capsuleColliders[0].height = oldScale;
        capsuleColliders[1].height = capsuleColliders[0].height + 0.25f;
        animatorPlayer.SetBool("tackle", false);
        speed = oldSpeed;


        //oldScale = transform.localScale.y;
        //transform.localScale = new Vector3(transform.localScale.x,scaleY,transform.localScale.z);
        //yield return new WaitForSeconds(tackleTimeLimit);
        //transform.localScale = new Vector3(transform.localScale.x, oldScale, transform.localScale.z);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log(doubleJump);
        if (context.phase == InputActionPhase.Started && IsGrounded() == true)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if (doubleJump == 1 && context.phase == InputActionPhase.Started)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doubleJump = 0;
        }
    }

    private bool IsGrounded()
    {
        float sphereRadius = 1f;
        isGrounded = Physics.CheckSphere(dotGround.position, sphereRadius, groundLayer);
        if(isGrounded == true) { doubleJump = 1; }
        return isGrounded;
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
                    * kickForce,ForceMode.Impulse);
            }
        }
    }

}
