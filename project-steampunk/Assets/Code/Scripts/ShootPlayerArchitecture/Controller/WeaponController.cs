using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    protected CancellationTokenSource cancellationTokenSource; //async reload token cancel
    protected bool startSwitchInisialise;
    protected IWeapon weapon;
    protected ActionPrototypePlayer inputShoot;
    protected bool isPressed;
    protected InputAction.CallbackContext isPressedContext;
    protected TextMeshProUGUI patronsText;
    [SerializeField] protected WeaponTypeScriptableObj weaponParametrs;

    //visual
    protected ParticleSystem vfxShootPrefab;
    protected List<LineRenderer> lineRenderers;
    [SerializeField]protected LineRenderer lineRenderer;

    protected CinemachineImpulseSource recoilCinemachine;
    protected Animator animatorArms;
    protected Animator animatorWeapon;

    protected virtual void SubscriptionInput()
    {
        inputShoot.Player.Shoot.started += Shoot;
        inputShoot.Player.Shoot.performed += Shoot;
        inputShoot.Player.Shoot.canceled += Shoot;
        inputShoot.Player.Reload.started += Reload;
    }
    protected virtual void UnSubscribeInput()
    {
        inputShoot.Player.Shoot.started -= Shoot;
        inputShoot.Player.Shoot.performed -= Shoot;
        inputShoot.Player.Shoot.canceled -= Shoot;
        inputShoot.Player.Reload.started -= Reload;
    }
    protected void OnEnable()
    {
        recoilCinemachine = transform.root.GetComponentInChildren<CinemachineImpulseSource>();
        vfxShootPrefab = GetComponentInChildren<ParticleSystem>();
        animatorArms = transform.root.GetComponentInChildren<Animator>();
        animatorWeapon = GetComponent<Animator>();
        inputShoot = SingletonActionPlayer.Instance.inputActions;
        //inputShoot = new ActionPrototypePlayer();
        //inputShoot.Enable();
        if (startSwitchInisialise)
        {
            weapon.Switch = false;
        }
        SubscriptionInput();

    }
    protected void OnDisable()
    {
        inputShoot.Player.Shoot.Reset();
        inputShoot.Player.Reload.Reset();
        animatorWeapon.SetBool("reload", false);
        animatorArms.SetBool("reload", false);
        animatorArms.SetBool("shoot", false);
        animatorWeapon.SetBool("shoot", false);
        weapon.Switch = true;
        weapon.CancelToken();
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
        lineRenderers = new List<LineRenderer>();
        LineRenderer tmp;
        for (int i = 0; i < 10; i++)
        {
            tmp = Instantiate(lineRenderer);
            tmp.enabled = false;
            lineRenderers.Add(tmp);
        }

        //use parametrs for shoot and weapon
        patronsText = GetComponentInChildren<TextMeshProUGUI>();

        weapon = new ParametrsUpdateDecorator(transform, weapon,
            weaponParametrs.fireRate,
            weaponParametrs.distanceAndDamages
            , weaponParametrs.reloadSpeed,
            weaponParametrs.patrons, weaponParametrs.attackType,
            weaponParametrs.enemyLayer,
            vfxShootPrefab, weaponParametrs.vfxImpactMetalProps, weaponParametrs.vfxImpactOtherProps,
            patronsText, animatorArms, animatorWeapon, recoilCinemachine,lineRenderers);
        weapon.Switch = false;
        startSwitchInisialise = true;
    }


    public virtual void Shoot(InputAction.CallbackContext context)
    {
        //One Shoot
        //if (context.started)
            //weapon.Shoot(context); PoolActive();
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
