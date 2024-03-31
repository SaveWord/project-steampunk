using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    protected IWeapon weapon;
    protected ActionPrototypePlayer inputShoot;
    protected bool isPressed;
    protected InputAction.CallbackContext isPressedContext;
    protected TextMeshProUGUI patronsText;
    [SerializeField] protected WeaponTypeScriptableObj weaponParametrs;

    //visual
    protected ParticleSystem vfxShootPrefab;

    protected CinemachineImpulseSource recoilCinemachine;
    protected Animator animatorArms;
    protected Animator animatorWeapon;

   protected virtual void SubscriptionInput()
    {
        inputShoot.Player.Shoot.started += context => Shoot(context);
        inputShoot.Player.Shoot.performed += context => Shoot(context);
        inputShoot.Player.Shoot.canceled += context => Shoot(context);
        inputShoot.Player.Reload.started += context => Reload(context);
    }
    protected virtual void UnSubscribeInput()
    {
       
        inputShoot.Player.Shoot.started -= context => Shoot(context);
        inputShoot.Player.Shoot.performed -= context => Shoot(context);
        inputShoot.Player.Shoot.canceled -= context => Shoot(context);
        inputShoot.Player.Reload.started -= context => Reload(context);
    }
    protected void OnEnable()
    {
        recoilCinemachine = transform.root.GetComponentInChildren<CinemachineImpulseSource>();
        vfxShootPrefab = GetComponentInChildren<ParticleSystem>();
        animatorArms = transform.root.GetComponentInChildren<Animator>();
        animatorWeapon = GetComponent<Animator>();
        //inputShoot = SingletonActionPlayer.Instance.inputActions;
        inputShoot = new ActionPrototypePlayer();
        inputShoot.Enable();
        SubscriptionInput();
        
    }
    protected void OnDisable()
    {
        //inputShoot.Disable();
        UnSubscribeInput();  
    }
    protected virtual void Update()
    {
        if (isPressed)
        {
            weapon.Shoot(isPressedContext);
        }
    }
    protected virtual void Start()
    {
        //use parametrs for shoot and weapon
        patronsText = GetComponentInChildren<TextMeshProUGUI>();

        weapon = new ParametrsUpdateDecorator(weapon,
            weaponParametrs.fireRate,
            weaponParametrs.distanceAndDamages
            , weaponParametrs.reloadSpeed,
            weaponParametrs.patrons, weaponParametrs.attackType,
            weaponParametrs.enemyLayer,
            vfxShootPrefab, weaponParametrs.vfxImpactMetalProps, weaponParametrs.vfxImpactOtherProps,
            patronsText, animatorArms, animatorWeapon, recoilCinemachine);
    }


    public virtual void Shoot(InputAction.CallbackContext context)
    {
        //One Shoot
        if (context.started)
            weapon.Shoot(context);
        //Shoot Pressed
        if (context.performed)
        {
            isPressedContext = context;
            isPressed = true;
        }
        //Cancel action
        if (context.canceled)
        {
            isPressedContext = context;
            isPressed = false;
            animatorArms.SetBool("shoot", false);
            animatorWeapon.SetBool("shoot", false);
        }
    }
    //reload func
    public virtual async void Reload(InputAction.CallbackContext context)
    {
        weapon.Reload(context);
    }
}
