using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParametrsUpdateMachineGun : ParametrsUpdateDecorator
{
    private RecoilMachineGun _recoilMachineGun;
    public ParametrsUpdateMachineGun(IWeapon weapon, float updateFireRate,
        DistanceAndDamage[] updateDamage, float updateReload, float updatePatrons,
        IWeapon.WeaponTypeDamage updateWeaponType, LayerMask mask,
        ParticleSystem vfxShootPrefab, ParticleSystem vfxImpactMetalProps, ParticleSystem vfxImpactOtherProps,
        TextMeshProUGUI patronsText, Animator animator, Animator animatorWeapon,
        CinemachineImpulseSource recoil, RecoilMachineGun recoilMachineGun)
        : base(weapon, updateFireRate, updateDamage, updateReload, updatePatrons, updateWeaponType,
          mask, vfxShootPrefab, vfxImpactMetalProps, vfxImpactOtherProps, patronsText, animator, animatorWeapon, recoil)
    {
        _updateFireRate = updateFireRate;

        _distanceAndDamage = updateDamage;

        _weapon = weapon;
        _updateDamage = updateDamage.Last().damage;
        _updateMaxRange = updateDamage.Last().range;
        _updateReload = updateReload;
        _updatePatrons = updatePatrons;
        _updateWeaponType = updateWeaponType;
        _updateEnemyLayer = mask;
        maxPatrons = updatePatrons;

        _recoilMachineGun = recoilMachineGun;

        //effect parametrs
        _vfxShootPrefab = vfxShootPrefab;
        _vfxImpactMetalProps = vfxImpactMetalProps;
        _vfxImpactOtherProps = vfxImpactOtherProps;
        _patronsText = patronsText;
        _animator = animator;
        _animatorWeapon = animatorWeapon;
    }
    public override void Shoot(InputAction.CallbackContext context)
    {
        float currentTime = Time.time;
        float timeDifference = currentTime - _updateLastShoot;

        if (((context.started || context.performed) && Patrons > 0 && timeDifference >= _updateFireRate)
             && isReload == false)
        {
            Vector3 posRay = new Vector3(Camera.main.transform.position.x + Random.Range(-1f,1f), 
                Camera.main.transform.position.y +Random.Range(-1f,1f),0);
            posRay = new Vector3(posRay.x, posRay.y, Camera.main.transform.position.z);

            _recoilMachineGun.Recoil();
            _updateLastShoot = currentTime;
            Patrons--;

            //vfx and animation and ui
            ShowAnimatorAndInternalImpact();

            //shoot logic
            if (Physics.Raycast(posRay, Camera.main.transform.forward,
                   out RaycastHit hit, Range, enemyLayer, QueryTriggerInteraction.Ignore))
            {
                hit.collider.TryGetComponent(out IShield impulseShield);
                impulseShield?.ShieldImpulse();

                hit.collider.TryGetComponent(out IDamageableProps damageableProps);
                damageableProps?.GetDamage(Damage);

                hit.collider.TryGetComponent(out IHealth damageable);
                damageable?.TakeDamage(Damage);


                ShowVFXImpact(hit);
            }
            if (Patrons == 0 && isReload == false)
            {
                _animator.SetBool("shoot", false);
                _animatorWeapon.SetBool("shoot", false);
                Reload(context);
            }

        }
    }
    protected override void ShowAnimatorAndInternalImpact()
    {
        _animator.SetBool("shoot", true);
        _animatorWeapon.SetBool("shoot", true);

        _vfxShootPrefab.Stop();
        _vfxShootPrefab.Play();
        _patronsText.text = Patrons.ToString();
    }
}
