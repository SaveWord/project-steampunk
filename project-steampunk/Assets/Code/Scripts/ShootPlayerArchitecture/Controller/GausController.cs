using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GausController : WeaponController
{
    InputAction.CallbackContext context;//null context
    [SerializeField] private List<Image> gausePatronsImages;
    private ParticleSystem afterFireSmoke;

    int lastShootAnim;
    protected override void SubscriptionInput()
    {
        inputShoot.Player.Shoot.started += Shoot;
        inputShoot.Player.Shoot.performed += Shoot;
        inputShoot.Player.Shoot.canceled += Shoot;
    }
    protected override void UnSubscribeInput()
    {
        inputShoot.Player.Shoot.started -= Shoot;
        inputShoot.Player.Shoot.performed -= Shoot;
        inputShoot.Player.Shoot.canceled -= Shoot;
    }
    protected override void Start()
    {
        ParticleSystem[] particle = GetComponentsInChildren<ParticleSystem>();
        afterFireSmoke = particle[particle.Length - 2];


        patronsText = GetComponentInChildren<TextMeshProUGUI>();

        weapon = new ParametrsUpdateGaus(transform, weapon,
            weaponParametrs.fireRate,
            weaponParametrs.distanceAndDamages
            , weaponParametrs.reloadSpeed,
            weaponParametrs.patrons, weaponParametrs.attackType,
            weaponParametrs.enemyLayer,
            vfxShootPrefab, weaponParametrs.vfxImpactMetalProps, weaponParametrs.vfxImpactOtherProps,
            patronsText, gausePatronsImages,
            animatorArms, animatorWeapon, recoilCinemachine, afterFireSmoke);
        weapon.Switch = false;
        startSwitchInisialise = true;
    }
    protected override void Update()
    {
        if (isPressed)
        {
            lastShootAnim = (int)weapon.Patrons;
            weapon.Shoot(isPressedContext);
            if (lastShootAnim == 0)
            {
                isPressed = false;
                animatorArms.SetBool("shoot", false);
                animatorWeapon.SetBool("shoot", false);
            }
        }
        if(weapon.Patrons == 0) animatorArms.SetBool("reload", true); 
        else animatorArms.SetBool("reload", false);
    }
    public override void Shoot(InputAction.CallbackContext context)
    {
        //One Shoot
        if (context.started)
        {
            //weapon.Shoot(context);
            //CancelInvoke("ReloadInvoke");
        }
        //Shoot Pressed
        if (context.performed)
        {
            //Debug.Log(this.GetType().Name);
            CancelInvoke("ReloadInvoke");
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
            InvokeRepeating("ReloadInvoke", 1f, weapon.ReloadSpeed);
        }
    }
    private void ReloadInvoke()
    {
        Reload(context);
    }
    public override void Reload(InputAction.CallbackContext context)
    {
        weapon.Reload(context);
    }
}
