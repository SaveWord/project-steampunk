using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using static IWeapon;

public class Weapon : IWeapon
{
    public float Damage { get; set; } = 20f;
    public float Range { get; set; } = 100.0f;
    public float ReloadSpeed { get; set; } = 5;
    public float Patrons { get; set; } = 6f;
    public WeaponTypeDamage AttackType { get; set; } = WeaponTypeDamage.Physical;
    LayerMask IWeapon.enemyLayer { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool Switch { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public RaycastHit hitLine { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Shoot(InputAction.CallbackContext context)
    {

    }
    public async void Reload(InputAction.CallbackContext context)
    {
       
    }
    public async Task CancelToken() { }
}
