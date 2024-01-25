using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using static IWeapon;

public class ParametrsUpdateDecorator : MainDecorator
{
    private IWeapon _weapon;
    private float _updateDamage;
    private float _updateRange;
    private float _updateReload;
    private float _updatePatrons;
    private WeaponTypeDamage _updateWeaponType;
    LayerMask _updateEnemyLayer;

    public ParametrsUpdateDecorator(IWeapon weapon, float updateDamage,
        float updateRange, float updateReload, float updatePatrons,
        IWeapon.WeaponTypeDamage updateWeaponType, LayerMask mask) : base(weapon)
    {
        _weapon = weapon;
        _updateDamage = updateDamage;
        _updateRange = updateRange;
        _updateReload = updateReload;
        _updatePatrons = updatePatrons;
        _updateWeaponType = updateWeaponType;
        maxPatrons = updatePatrons;
    }
    public override float Damage
    {
        get { return _updateDamage; }
        set { }
    }

    public override float Range
    {
        get { return _updateRange; }
        set { }
    }

    public override float ReloadSpeed
    {
        get { return _updateReload; }
        set { }
    }

    public override float Patrons
    {
        get { return _updatePatrons; }
        set { _updatePatrons = value; }
    }

    public override WeaponTypeDamage AttackType
    {
        get { return _updateWeaponType; }
        set { }
    }
    public override LayerMask enemyLayer
    {
        get {return _updateEnemyLayer;}
        set { }
    }
    public override void Shoot(InputAction.CallbackContext context)
    {
        if (context.started && Patrons > 0)
        {
            Patrons--;
            Debug.Log(Patrons);
            Debug.DrawRay(Camera.main.transform.localPosition, Camera.main.transform.forward, Color.blue);
            if (Physics.Raycast(Camera.main.transform.localPosition, Camera.main.transform.forward,
                out RaycastHit hit, Range, enemyLayer))
            {
                Debug.Log("hit");
                hit.collider.TryGetComponent(out IDamageableProps damageableProps);
                damageableProps?.GetDamage(Damage);

                hit.collider.TryGetComponent(out IDamageable damageable);
                damageable?.GetDamage(Damage);

                hit.collider.TryGetComponent(out enemy_health dama);
                dama?.TakeDamage(Damage);
            }
        }
        if (Patrons == 0)
        {
            Reload(context);
        }
    }
    public async override void Reload(InputAction.CallbackContext context)
    {
        if (context.started && Patrons < maxPatrons)
        {
            Debug.Log("Activate");
            await Task.Delay((int)ReloadSpeed * 1000);
            Debug.Log("Deactivate");
            Patrons = maxPatrons;
        }
    }
}
