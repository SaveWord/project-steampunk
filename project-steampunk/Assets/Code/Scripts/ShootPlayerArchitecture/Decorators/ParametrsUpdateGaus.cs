using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static IWeapon;

public class ParametrsUpdateGaus : ParametrsUpdateDecorator
{
    private Transform _distanceTarget;
    private DistanceAndDamage[] _distanceAndDamage;
    public ParametrsUpdateGaus(Transform distanceTarget, IWeapon weapon, float updateFireRate,
         DistanceAndDamage[] distanceAndDamage
         , float updateReload, float updatePatrons,
         IWeapon.WeaponTypeDamage updateWeaponType, LayerMask mask,
         ParticleSystem vfxShootPrefab, ParticleSystem vfxImpactMetalProps, ParticleSystem vfxImpactOtherProps,
         TextMeshProUGUI patronsText,
         Animator animator, Animator animatorWeapon, CinemachineImpulseSource recoil)
         : base(weapon, updateFireRate, distanceAndDamage, updateReload, updatePatrons, updateWeaponType,
             mask, vfxShootPrefab, vfxImpactMetalProps, vfxImpactOtherProps, patronsText, animator, animator, recoil)
    {
        _updateFireRate = updateFireRate;

        _weapon = weapon;
        _updateDamage = distanceAndDamage.Last().damage;
        _distanceAndDamage = distanceAndDamage;
        _updateMaxRange = distanceAndDamage.Last().range;
        _updateReload = updateReload;
        _updatePatrons = updatePatrons;
        _updateWeaponType = updateWeaponType;
        _updateEnemyLayer = mask;
        maxPatrons = updatePatrons;
        _distanceTarget = distanceTarget;

        //effect parametrs
        _vfxShootPrefab = vfxShootPrefab;
        _vfxImpactMetalProps = vfxImpactMetalProps;
        _vfxImpactOtherProps = vfxImpactOtherProps;
        _patronsText = patronsText;
        _animator = animator;
        _animatorWeapon = animatorWeapon;
        _recoil = recoil;
    }

    //properties
    public override float Damage
    {
        get { return _updateDamage; }
        set { _updateDamage = value; }
    }

    public override float Range
    {
        get { return _updateMaxRange; }
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
        get { return _updateEnemyLayer; }
        set { }
    }

    //methods decorator shoot and reload logic

    public override void Shoot(InputAction.CallbackContext context)
    {
        float currentTime = Time.time;
        float timeDifference = currentTime - _updateLastShoot;

        if ((context.started || context.performed) && Patrons > 0 && timeDifference >= _updateFireRate)
        {
            _updateLastShoot = currentTime;
            Patrons--;

            //vfx and animation and ui
            ShowAnimatorAndInternalImpact();
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
                out RaycastHit hit, Range, enemyLayer, QueryTriggerInteraction.Ignore))
            {
                float rangeBetween = Vector3.Distance(hit.point, _distanceTarget.position);

                for (int i = 0; i <= _distanceAndDamage.Length - 1; i++)
                {
                    if (rangeBetween <= _distanceAndDamage[i].range)
                    {
                        Damage = _distanceAndDamage[i].damage;
                        break;
                    }
                }
                hit.collider.TryGetComponent(out IShield destroyShield);
                destroyShield?.ShieldDestroy();


                hit.collider.TryGetComponent(out IDamageableProps damageableProps);
                damageableProps?.GetDamage(Damage);

                hit.collider.TryGetComponent(out IHealth damageable);
                damageable?.TakeDamage(Damage);



                ShowVFXImpact(hit);
            }
        }
    }

    private void ShowAnimatorAndInternalImpact()
    {
        _animator.SetBool("shoot", true);
        _animatorWeapon.SetBool("shoot", true);
        _recoil.GenerateImpulse();
        _vfxShootPrefab.Stop();
        _vfxShootPrefab.Play();
        _patronsText.text = Patrons.ToString();
    }
    private void ShowVFXImpact(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == 25)
        {
            Instantiate(_vfxImpactMetalProps, hit.point,
                Quaternion.FromToRotation(Vector3.forward, hit.normal));
        }
        else
        {
            Instantiate(_vfxImpactOtherProps, hit.point,
                Quaternion.FromToRotation(Vector3.forward, hit.normal));
        }
    }

    public async override void Reload(InputAction.CallbackContext context)
    {
        if (Patrons < maxPatrons)
        {
            Patrons++;
            _patronsText.text = Patrons.ToString();
        }
    }
}
