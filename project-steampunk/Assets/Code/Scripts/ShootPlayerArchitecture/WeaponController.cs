using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    private IWeapon weapon;
    private ActionPrototypePlayer inputShoot;
    private TextMeshProUGUI patronsText;
    [SerializeField] private WeaponTypeScriptableObj[] weaponParametrs;
    [SerializeField] private WeaponType weaponType;
    private Animator animatorArms;
    private Animator animatorWeapon;
    enum WeaponType
    {
        Revolver,
        Shotgun
    }
    private void OnEnable()
    {
        animatorArms = transform.root.GetComponentInChildren<Animator>();
        animatorWeapon = GetComponent<Animator>();
        inputShoot = new ActionPrototypePlayer();
        inputShoot.Enable();
        inputShoot.Player.Shoot.started += context => Shoot(context);
        inputShoot.Player.Shoot.performed += context => Shoot(context);
        inputShoot.Player.Shoot.canceled += context => Shoot(context);
        inputShoot.Player.Reload.started += context => Reload(context);
    }
    private void OnDisable()
    {
        inputShoot.Disable();
        inputShoot.Player.Shoot.started -= context => Shoot(context);
        inputShoot.Player.Shoot.performed -= context => Shoot(context);
        inputShoot.Player.Shoot.canceled -= context => Shoot(context);
        inputShoot.Player.Reload.started -= context => Reload(context);
    }
    private void Start()
    {
        patronsText = transform.root.GetComponentInChildren<TextMeshProUGUI>();
        switch (weaponType)
        {
            case WeaponType.Revolver:
                weapon = new ParametrsUpdateDecorator(weapon, weaponParametrs[0].damage,
                    weaponParametrs[0].range, weaponParametrs[0].reloadSpeed,
                    weaponParametrs[0].patrons, weaponParametrs[0].attackType,
                    weaponParametrs[0].enemyLayer,
                    weaponParametrs[0].vfxShootPrefab, patronsText, animatorArms, animatorWeapon);
                break;
            case WeaponType.Shotgun:

                break;
        }

    }
    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.started)
            weapon.Shoot(context);
        if (context.performed)
        {
            StartCoroutine(ShootCoroutine(context));

        }
        if(context.canceled) 
        {
            animatorArms.SetBool("shoot", false);
            animatorWeapon.SetBool("shoot", false);
        }
    }
    IEnumerator ShootCoroutine(InputAction.CallbackContext context)
    {
        while (context.performed)
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log(context);
            weapon.Shoot(context);
        }
    }
    public async void Reload(InputAction.CallbackContext context)
    {
        weapon.Reload(context);
    }
}
