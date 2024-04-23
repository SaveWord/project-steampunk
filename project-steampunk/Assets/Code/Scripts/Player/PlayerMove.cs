using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.VFX;

public class PlayerMove : MonoBehaviour
{
    private Animator animatorPlayer; //animator for change hands anim


    //move data
    [Header("Переменные перемещения")]
    [SerializeField] private float speed;
    [SerializeField] private float speedSlope;
    [SerializeField] private float mouseSense;

    [SerializeField] private float smoothInputSpeed;
    private Vector2 currentMove;
    private Vector2 smoothInputVelocity;
    public float DashSlider
    {
        get
        {
            float remainingTime = dashTimer > Time.time ? dashTimer - Time.time : 0f;
            float progress = 1f - Mathf.Clamp01(remainingTime / dashCooldown);
            return progress;
        }
        private set { }
    }
    public float MouseSense { get { return mouseSense; } set { mouseSense = value; } }

    [Header("Cinemachine Virtual Cameras")]
    [SerializeField] private CinemachineVirtualCamera cam;
    private Animator animatorCinemachineVirtualCam;// анимация поворота головы при движении на A D
    private CinemachineBasicMultiChannelPerlin camNoise;//шум для иммитация бега вперед, покачивание камеры


    [Header("Переменные детекта ground")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform dotGround;
    [SerializeField] private float sphereRadius;
    [SerializeField] private float verticalDamping = 0.5f;

    //jump
    [Header("Гравитация и переменные прыжка")]
    [SerializeField] private float jumpHeight;
    [Tooltip("Доп сила для двойного прыжка"), SerializeField] private float jumpForceDouble = 1.5f;
    private float jumpForce;
    private bool jumpTrue;
    private int doubleJump;

    private float standartGravityScale = 5;
    [Tooltip("Change physics,gravity"), SerializeField] private float gravityScale = 5;
    [Tooltip("Change physics, gravity"), SerializeField] private float fallingGravityScale = 5;


    [Header("Угол наклона камеры Y")]
    [SerializeField] private float yAngle;
    private float xRotation;
    private Rigidbody rb;
    private ActionPrototypePlayer inputActions;
    private bool isGrounded;




    //state player movement
    //private State state;

    //hook data 
    //[SerializeField] private Transform debugRayHit; //дебаг для хука
    /*[Header("Тайминги хука")]
    [SerializeField] private float distanceHook;
    [SerializeField] private float timeNextHook;//перезарядка крюк кошки
    private Vector3 hookPosition;
    private float timerHook;
    */

    //dash data
    [Header("Dash переменные")]
    [SerializeField] private int layerIgnore; //игнорирование коллизии слоя
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTimeLimit;//время рывка
    [SerializeField] private float dashCooldown;//время перезарядки рывка
    private float dashTimer;
    private bool dashTrue;
    private float standartSpeed;

    [SerializeField] private float dashForce;


    private RaycastHit slopeHit;

    //tackle data
    /*
    [Header("Переменные подката")]
    [SerializeField] private float tackleSpeed;//скорость подката
    [SerializeField] private float tackleSpeedSubtraction; // уменьшение скорости при долгом зажатии подката
    [SerializeField] private float tackleTimeLimit;//время подката
    [SerializeField] private float colliderHeight;//высота коллайдера во время подката
    private CapsuleCollider[] capsuleColliders;
    private bool tackleActive;
    */



    //wallRunning
    /*
    [Header("Переменные бега по стене")]
    [SerializeField] private float wallMoveTime;
    [SerializeField] private float wallRayRange;
    [SerializeField] private LayerMask wallLayer;
    private float wallRunDetect;
    private bool wallRight;
    private bool wallLeft;
    */


    //vfx effects move
    private VisualEffect effectDash;

    /*
    [Header("Звуки мувмента")]
    [SerializeField] private AudioClip moveClip;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip jumpClip;
   */
    // private AudioSource audioSource;

    private void Awake()
    {
        standartSpeed = speed;
        //capsuleColliders = GetComponents<CapsuleCollider>();//change collider in slide
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        //audioSource = GetComponent<AudioSource>();
        animatorPlayer = GetComponentInChildren<Animator>();

        //inputActions = new ActionPrototypePlayer();
        inputActions = SingletonActionPlayer.Instance.inputActions;


        //camera and vfx effects move getcomponents
        effectDash = GetComponentInChildren<VisualEffect>();
        camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        animatorCinemachineVirtualCam = GameObject.Find("VirtualCameraAnimator").GetComponent<Animator>();


    }
    public float GetSpeed()
    {
        return speed;
    }
    private void Update()
    {
        IsGrounded();
    }
    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * (standartGravityScale - 1) * rb.mass); //change gravity

        Vector2 inputMove = inputActions.Player.Move.ReadValue<Vector2>();
        currentMove = Vector2.SmoothDamp(currentMove, inputMove, ref smoothInputVelocity,
            smoothInputSpeed);
        if (inputMove.x == 0)
            currentMove = new Vector2(0, currentMove.y);
        if (inputMove.y == 0)
            currentMove = new Vector2(currentMove.x, 0);
        //Debug.Log(currentMove);
        Vector2 inputLook = inputActions.Player.Look.ReadValue<Vector2>();
        rb.velocity += Vector3.up * Physics.gravity.y * verticalDamping;
        Move(currentMove);
        Rotation(inputLook);

        //if ((inputMove.y < 0 || inputMove.x != 0) && tackleActive == true)
        //{
        //    rb.velocity = new Vector3((inputMove.y < 0) ?
        //    -rb.velocity.x : rb.velocity.x, rb.velocity.y, 0);
        //}
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
        xRotation = Mathf.Clamp(xRotation, -yAngle, yAngle);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * xLook);
    }
    private void Move(Vector2 inputMove)
    {
        Physics.SphereCast(transform.position + Vector3.up * 2, 1f, Vector3.down, out slopeHit, 10, groundLayer);
        Vector3 move = transform.right * inputMove.x + transform.forward * inputMove.y;
        Vector3 projectedMove = Vector3.ProjectOnPlane(move, slopeHit.normal).normalized;
        float angle = Vector3.Angle(slopeHit.normal, Vector3.up);
        if (dashTrue != true)
        {


            if ((inputMove != Vector2.zero) && jumpTrue == false
                && dashTrue == false)
            {
                AudioManager.InstanceAudio.PlaySfxSound("Move");
                //audioSource.clip = moveClip;
                //audioSource.Play();
            }
            // else if ((inputMove == Vector2.zero) && jumpTrue == false)
            //audioSource.Stop();



            //Debug.Log(rb.velocity);
            //Debug.Log(projectedMove);
            //Debug.Log(angle);
            //if (inputMove.magnitude == 0) // moment stop if movement in keyboard stop
            // rb.velocity = new Vector3(0, rb.velocity.y, 0);

            //more powerfull gravity if in air
            if (rb.velocity.y >= 0)
                standartGravityScale = gravityScale;
            else if (rb.velocity.y < 0)
                standartGravityScale = fallingGravityScale;


            if (projectedMove.magnitude == 0 && jumpTrue == false)// stop in slope floor
                rb.velocity = new Vector3(0, -50, 0);

            if ((angle != 0) && jumpTrue == false)// slope movement
            {
                if (rb.velocity.y > 0)
                {
                    rb.velocity = new Vector3(projectedMove.x * speed * speedSlope, -50,
                        projectedMove.z * speed * speedSlope);
                }
                rb.velocity = new Vector3(projectedMove.x * speed, projectedMove.y * speed,
                  projectedMove.z * speed);
            }

            else
            {
                rb.velocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);
            }
            if ((rb.velocity.y > 0) && jumpTrue != true)
                rb.AddForce(Physics.gravity * 40);

            animatorPlayer.SetFloat("ADSpeed",inputMove.x, 0.1f, Time.deltaTime);
            animatorPlayer.SetFloat("speed", inputMove.magnitude, 0.1f, Time.deltaTime);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (dashTimer <= Time.time)
            {

                dashTimer = Time.time + dashCooldown;
                //Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
                //input = transform.right * input.x + transform.forward * input.y;
                //if (input == Vector2.zero) input = Vector2.down;
                //rb.velocity = rb.velocity;
                //rb.AddForce(input.x * dashForce, 0,
                //    input.y * dashForce, ForceMode.Impulse);
                StartCoroutine(DashCoroutine());
            }

        }
    }
    IEnumerator DashCoroutine()
    {
        Vector3 input = inputActions.Player.Move.ReadValue<Vector2>();
        input = transform.right * input.x + transform.forward * input.y;
        if (input == Vector3.zero) input = transform.forward;
        Debug.Log(input);
        rb.velocity = rb.velocity;
        rb.AddForce(input.x * dashForce, 0,
            input.z * dashForce, ForceMode.Impulse);
        //speed = dashSpeed;
        dashTrue = true;
        Physics.IgnoreLayerCollision(gameObject.layer, layerIgnore, true);
        //effects and audio
        AudioManager.InstanceAudio.PlaySfxSound("Dash");
        //audioSource.Stop();
        // audioSource.PlayOneShot(dashClip);
        effectDash.Play();
        yield return new WaitForSeconds(dashTimeLimit);
        effectDash.Stop();
        Physics.IgnoreLayerCollision(gameObject.layer, layerIgnore, false);
        dashTrue = false;
        speed = standartSpeed;

    }

    //подкат
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
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y * standartGravityScale));
        //Debug.Log(doubleJump);
        if (context.phase == InputActionPhase.Started && IsGrounded() == true)
        {
            jumpTrue = true;
            AudioManager.InstanceAudio.PlaySfxSound("Jump");
            StartCoroutine(JumpCoroutineUpSpeed());
            animatorPlayer.SetBool("jump", true);
            if (rb.velocity.y > 0)
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        /*
        else if (doubleJump == 1 && context.phase == InputActionPhase.Started)
        {
            jumpTrue = true;
            AudioManager.InstanceAudio.PlaySfxSound("Jump");
            animatorPlayer.SetBool("jump", true);
            StartCoroutine(JumpCoroutineUpSpeed());
            if (rb.velocity.y > 0)
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce * jumpForceDouble, ForceMode.Impulse);
            doubleJump = 0;
        }
        */
        if (context.phase == InputActionPhase.Canceled)
        {
            animatorPlayer.SetBool("jump", false);
        }
    }


    IEnumerator JumpCoroutineUpSpeed()
    {
        speed = 65;
        //audio
        //audioSource.Stop();
        // audioSource.PlayOneShot(jumpClip);

        yield return new WaitForSeconds(0.1f);
        speed = standartSpeed;
    }
    private bool IsGrounded()
    {
        isGrounded = Physics.CheckSphere(dotGround.position, sphereRadius, groundLayer);
        if (isGrounded == true) doubleJump = 1;
        return isGrounded;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
        {
            //animatorPlayer.SetBool("isGround", true);
            jumpTrue = false;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(dotGround.position, sphereRadius);


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

}
