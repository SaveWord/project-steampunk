using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static IWeapon;

public abstract class MainDecorator : MonoBehaviour,IWeapon
{
    protected IWeapon weapon;
    protected float maxPatrons;


    public MainDecorator(IWeapon MainDecorator)
    {
        MainDecorator = weapon;
    }
    public virtual float Damage
    {
        get { return weapon.Damage; }
        set { }
    }
    public virtual float Range
    {
        get { return weapon.Range; }
        set { }
    }
    public virtual float ReloadSpeed
    {
        get { return weapon.ReloadSpeed; }
        set { }
    }
    public virtual float Patrons
    {
        get { return weapon.Patrons; }
        set { }
    }
    public virtual WeaponTypeDamage AttackType
    {
        get { return weapon.AttackType; }
        set { }
    }
    public virtual LayerMask enemyLayer
    {
        get { return weapon.enemyLayer; }
        set { }
    }
    public virtual void Shoot(InputAction.CallbackContext context)
    {
        weapon.Shoot(context);
    }
    public async  virtual void Reload(InputAction.CallbackContext context)
    {
        weapon.Reload(context);
    }
}
