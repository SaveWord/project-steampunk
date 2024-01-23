using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IWeapon
{
    float Damage { get; set; }
    float Range { get; set; }
    float ReloadSpeed { get; set; }
    float Patrons { get; set; }
    WeaponTypeDamage AttackType { get; set; }
    LayerMask enemyLayer { get; set; }

    enum WeaponTypeDamage
    {
        Physical
    }
    public void Shoot(InputAction.CallbackContext context)
    {

    }
    public void Reload(InputAction.CallbackContext context)
    {

    }
    IEnumerator ReloadCoroutine()
    {

        yield return new WaitForSeconds(ReloadSpeed);
    }
}
