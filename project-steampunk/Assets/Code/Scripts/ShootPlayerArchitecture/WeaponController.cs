using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    private IWeapon weapon;
    private ActionPrototypePlayer inputShoot;
    [SerializeField] private WeaponTypeScriptableObj[] weaponParametrs;
    [SerializeField]private WeaponType weaponType;
    enum WeaponType
    {
        Revolver,
        Shotgun
    }
    private void OnEnable()
    {
        inputShoot = new ActionPrototypePlayer();
        inputShoot.Enable();
        inputShoot.Player.Shoot.started += context => Shoot(context);
        //inputShoot.Player.Shoot.canceled += context => Shoot(context);
        inputShoot.Player.Reload.started += context => Reload(context);
    }
    private void OnDisable()
    {
        inputShoot.Disable();
        inputShoot.Player.Shoot.started -= context => Shoot(context);
        //inputShoot.Player.Shoot.canceled -= context => Shoot(context);
        inputShoot.Player.Reload.started -= context => Reload(context);
    }
    private void Start()
    {
        switch (weaponType)
        {
            case WeaponType.Revolver:
                weapon = new ParametrsUpdateDecorator(weapon, weaponParametrs[0].damage,
                    weaponParametrs[0].range, weaponParametrs[0].reloadSpeed, 
                    weaponParametrs[0].patrons, weaponParametrs[0].attackType, 
                    weaponParametrs[0].enemyLayer);
                break;
            case WeaponType.Shotgun:

                break;
        }
    }
    public void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log(weapon.Patrons);
        weapon.Shoot(context);
    }
    public async void Reload(InputAction.CallbackContext context)
    {
        weapon.Reload(context);
    }
}
