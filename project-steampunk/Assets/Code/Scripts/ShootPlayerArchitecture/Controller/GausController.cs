using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GausController : WeaponController
{
    InputAction.CallbackContext context;//null context
    protected override void SubscriptionInput()
    {
        inputShoot.Player.Shoot.started += context => Shoot(context);
        inputShoot.Player.Shoot.performed += context => Shoot(context);
        inputShoot.Player.Shoot.canceled += context => Shoot(context);
    }
    protected override void UnSubscribeInput()
    {
        inputShoot.Player.Shoot.started -= context => Shoot(context);
        inputShoot.Player.Shoot.performed -= context => Shoot(context);
        inputShoot.Player.Shoot.canceled -= context => Shoot(context);
    }
    protected override void Start()
    {
        patronsText = transform.root.GetComponentInChildren<TextMeshProUGUI>();

        weapon = new ParametrsUpdateGaus(transform,weapon,
            weaponParametrs.fireRate,
            weaponParametrs.distanceAndDamages
            ,weaponParametrs.reloadSpeed,
            weaponParametrs.patrons, weaponParametrs.attackType,
            weaponParametrs.enemyLayer,
            vfxShootPrefab, weaponParametrs.vfxImpactMetalProps, weaponParametrs.vfxImpactOtherProps,
            patronsText, animatorArms, animatorWeapon, recoilCinemachine);
    }
    public override void Shoot(InputAction.CallbackContext context)
    {
        //One Shoot
        if (context.started)
        {
            weapon.Shoot(context);
            CancelInvoke("ReloadInvoke");
        }
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
