using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.VFX;
using UnityEngine.InputSystem.XR;

public class CharacterControllerMove : MonoBehaviour
{
    private Animator animatorPlayer; //animator for change hands anim
    private CharacterController characterController;


    //move data
    [Header("Переменные перемещения")]
    [SerializeField] private float speed;
    [Tooltip(
            "Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
    public float movementSharpnessOnGround = 15;
    [SerializeField] private float mouseSense;
    private Vector3 characterVelocity;
    Vector2 inputMove;
    Vector3 m_GroundNormal;
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

    [SerializeField] private float gravityDownForce = 20f;

    //jump
    [Header("Гравитация и переменные прыжка")]
    private Vector3 gravityVelocity;
    [SerializeField] private float jumpForce;
    float m_LastTimeJumped = 0f;
    const float k_JumpGroundingPreventionTime = 0.2f;

    [Header("Угол наклона камеры Y")]
    [SerializeField] private float yAngle;
    private float xRotation;
    private ActionPrototypePlayer inputActions;
    private bool isGrounded;

    //dash data
    [Header("Dash переменные")]
    [SerializeField] private int layerIgnore; //игнорирование коллизии слоя
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTimeLimit;//время рывка
    [SerializeField] private float dashCooldown;//время перезарядки рывка
    private float dashTimer;
    private bool dashTrue;

    [SerializeField] private float dashForce;


    //vfx effects move
    private VisualEffect effectDash;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //load position
        

        characterController = GetComponent<CharacterController>();
        animatorPlayer = GetComponentInChildren<Animator>();

        inputActions = SingletonActionPlayer.Instance.inputActions;


        //camera and vfx effects move getcomponents
        effectDash = GetComponentInChildren<VisualEffect>();
        camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        animatorCinemachineVirtualCam = GameObject.Find("VirtualCameraAnimator").GetComponent<Animator>();


      

    }
    private void Start()
    {
        GameManagerSingleton.Instance.SaveSystem.LoadData();
        //_currentHp = GameManagerSingleton.Instance.SaveSystem.playerData.health;
        transform.position = GameManagerSingleton.Instance.SaveSystem.playerData.position;
        transform.rotation = Quaternion.Euler(GameManagerSingleton.Instance.SaveSystem.playerData.rotation);
        Physics.SyncTransforms();
    }
    private void OnEnable()
    {
        inputActions.Player.Jump.started += Jump;
        inputActions.Player.Dash.started += Dash;
    }
    private void OnDisable()
    {
        inputActions.Player.Jump.started -= Jump;
        inputActions.Player.Dash.started -= Dash;
    }
    public float GetSpeed()
    {
        return speed;
    }
    private void Update()
    {
        inputMove = inputActions.Player.Move.ReadValue<Vector2>();
        Vector2 inputLook = inputActions.Player.Look.ReadValue<Vector2>();
        Debug.Log(inputMove);
        Move(inputMove);
        Rotation(inputLook);
        IsGrounded();
        EffectsMove(inputMove);
        gravityVelocity.y += Physics.gravity.y * Time.deltaTime;
        if (isGrounded == true)
            gravityVelocity.y = 0f;
        characterController.Move((gravityVelocity * gravityDownForce) * Time.deltaTime);
    }
    //effects move
    public void EffectsMove(Vector2 _inputMove)
    {
        if (isGrounded)
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
    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }
    Vector3 GetCapsuleBottomHemisphere()
    {
        Vector3 centerOfSphere1 = transform.position + Vector3.up *
            (characterController.radius + Physics.defaultContactOffset);
        return centerOfSphere1;//transform.position + (transform.up * characterController.radius);
    }

    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        Vector3 centerOfSphere2 = transform.position + Vector3.up *
            (characterController.height - characterController.radius + Physics.defaultContactOffset);
        return centerOfSphere2;//transform.position + (transform.up * (atHeight - characterController.radius));
    }
    private void Move(Vector2 inputMove)
    {
        Vector3 move = transform.right * inputMove.x + transform.forward * inputMove.y;
        Vector3 targetVelocity = move * speed;
        targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) *
                                     targetVelocity.magnitude;
        characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity,
                       movementSharpnessOnGround * Time.deltaTime);

        //characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
        characterController.Move(characterVelocity * Time.deltaTime);


        //anim and audio
        if ((inputMove != Vector2.zero) && isGrounded != false
              && dashTrue == false)
        {
            AudioManager.InstanceAudio.PlaySfxSound("Move");
        }
        animatorPlayer.SetFloat("ADSpeed", inputMove.x, 0.1f, Time.deltaTime);
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
        float startTime = Time.time;
        AudioManager.InstanceAudio.PlaySfxSound("Dash");
        while (Time.time < startTime + dashTimeLimit)
        {
            effectDash.Play();
            if (inputMove == Vector2.zero)
            {
                characterController.Move((characterVelocity + transform.forward) * speed
                    * dashSpeed * Time.deltaTime);
            }
            else
            {
                characterVelocity *= 10;
                characterVelocity = characterVelocity.normalized;
                characterController.Move(characterVelocity * speed * dashSpeed * Time.deltaTime);
            }

            yield return null;
        }
        effectDash.Stop();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log(doubleJump);
        if (context.started && isGrounded == true)
        {
            // add the jumpSpeed value upwards
            gravityVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * Physics.gravity.y);
            characterController.Move((gravityVelocity * gravityDownForce) * Time.deltaTime);
            // Force grounding to false
            m_GroundNormal = Vector3.up;
            m_LastTimeJumped = Time.time;
            //audio
            AudioManager.InstanceAudio.PlaySfxSound("Jump");
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            animatorPlayer.SetBool("jump", false);
        }
    }

    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= characterController.slopeLimit;
    }
    private void IsGrounded()
    {
        Debug.Log(isGrounded);
        Debug.Log(Vector3.Angle(transform.up, m_GroundNormal));

        if (Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime)
        {
            //isGrounded = Physics.CheckSphere(dotGround.position, 0.4f, groundLayer);
            //detect ground and correct normal 
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(characterController.height),
                   characterController.radius - Physics.defaultContactOffset,
                   Vector3.down, out RaycastHit hit, sphereRadius, groundLayer,
                   QueryTriggerInteraction.Ignore))
            {
                isGrounded = true;
                //Debug.Log(hit.collider);
                float distanceToGround = Vector3.Distance(GetCapsuleBottomHemisphere(), hit.point);
                if (distanceToGround < characterController.height / 2)
                {
                    m_GroundNormal = hit.normal;

                }

                // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                // and if the slope angle is lower than the character controller's limit
                if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                    IsNormalUnderSlopeLimit(m_GroundNormal))
                {
                    
                    // handle snapping to the ground
                    if (hit.distance > characterController.skinWidth)
                    {
                        characterController.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }
        if (!Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(characterController.height),
                  characterController.radius - Physics.defaultContactOffset,
                  Vector3.down, sphereRadius, groundLayer,
                  QueryTriggerInteraction.Ignore))
        {
            isGrounded = false;
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(dotGround.position, sphereRadius);
    }
}
