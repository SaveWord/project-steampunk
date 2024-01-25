using System.Collections;
using System.Threading.Tasks;
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
    public async void Reload(InputAction.CallbackContext context)
    {
        await Task.Delay((int)ReloadSpeed * 1000);
    }
}
