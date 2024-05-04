using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static IWeapon;

public class ParametrsUpdateGaus : ParametrsUpdateDecorator
{
    private ParticleSystem _afterFireParticle;
    private List<Image> _updateGausePatronsImage;
    private List<GameObject> _poolTrail;
    private int indexPatron;

    public ParametrsUpdateGaus(Transform distanceTarget, IWeapon weapon, float updateFireRate,
         DistanceAndDamage[] distanceAndDamage
         , float updateReload, float updatePatrons,
         IWeapon.WeaponTypeDamage updateWeaponType, LayerMask mask,
         ParticleSystem vfxShootPrefab, ParticleSystem vfxImpactMetalProps, ParticleSystem vfxImpactOtherProps,
         TextMeshProUGUI patronsText, List<Image> updateGausePatronsImage,
         Animator animator, Animator animatorWeapon, CinemachineImpulseSource recoil,
         List<GameObject> poolTrail, ParticleSystem afterFireParticle)
         : base(distanceTarget,weapon, updateFireRate, distanceAndDamage, updateReload, updatePatrons, updateWeaponType,
             mask, vfxShootPrefab, vfxImpactMetalProps, vfxImpactOtherProps, patronsText, animator, animator, recoil)
    {
        _updateFireRate = updateFireRate;


        _weapon = weapon;
        _updateDamage = distanceAndDamage.Last().damage;
        _distanceAndDamage = distanceAndDamage;
        _updateMaxRange = distanceAndDamage.Last().range;
        _updateReload = updateReload;
        _updatePatrons = updatePatrons;
        _updateGausePatronsImage = updateGausePatronsImage;
        maxPatrons = updatePatrons;
        _updateWeaponType = updateWeaponType;
        _updateEnemyLayer = mask;
        _distanceTarget = distanceTarget;

        //effect parametrs
        _vfxShootPrefab = vfxShootPrefab;
        _vfxImpactMetalProps = vfxImpactMetalProps;
        _vfxImpactOtherProps = vfxImpactOtherProps;
        _patronsText = patronsText;
        _animator = animator;
        _animatorWeapon = animatorWeapon;
        _recoil = recoil;
        _poolTrail = poolTrail;
        _afterFireParticle = afterFireParticle;
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
        _afterFireParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if ((context.started || context.performed) && Patrons > 0 && timeDifference >= _updateFireRate)
        {
            _updateLastShoot = currentTime;
            Patrons--;


            //vfx and animation and ui
            ShowAnimatorAndInternalImpact();
            AudioManager.InstanceAudio.PlaySfxWeapon("GaussShoot");
            //TrailPoolSpawn();
            //aim assist, change radius sphere cast from distance

            if (Physics.SphereCast(Camera.main.transform.position, 2f, Camera.main.transform.forward,
                out RaycastHit hit1, Range, enemyLayer, QueryTriggerInteraction.Ignore))
            {
                for (int i = 0; i <= _distanceAndDamage.Length - 1; i++)
                {
                    if (_distanceAndDamage[i].range > hit1.distance)
                    {
                        changeRadius = _distanceAndDamage[i].radiusRay;
                        break;
                    }
                }
            }

            //shoot logic
            if (Physics.SphereCast(Camera.main.transform.position, changeRadius, Camera.main.transform.forward,
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

                if (damageable != null || destroyShield!=null || damageableProps!=null)
                    ShowDamage(Damage + "", Color.cyan);

                //vfx and animator stop
                ShowVFXImpact(hit);

                //if (Patrons <= 0)
                //{
                //    _animator.SetBool("shoot", false);
                //    _animatorWeapon.SetBool("shoot", false);
                //    _afterFireParticle.Play();

                //}

            }
        }
        indexPatron = Mathf.Clamp((int)Patrons, 0, _updateGausePatronsImage.Count - 1);
        _updateGausePatronsImage[indexPatron].enabled = false;
    }
    private void TrailPoolSpawn()
    {
        for (int i = 0; i < _poolTrail.Count; i++)
        {
            if (!_poolTrail[i].activeInHierarchy)
            {
                _poolTrail[i].transform.position = _vfxShootPrefab.transform.position;
                _poolTrail[i].transform.rotation = _vfxShootPrefab.transform.rotation;
                _poolTrail[i].SetActive(true);
                break;
            }
        }
    }
    protected override void ShowAnimatorAndInternalImpact()
    {
        _animator.SetBool("shoot", true);
        _animatorWeapon.SetBool("shoot", true);
        _recoil.GenerateImpulse();
        _vfxShootPrefab.Stop();
        _vfxShootPrefab.Play();
    }
    public override void ReloadSound()
    {
        AudioManager.InstanceAudio.PlaySfxWeapon("GaussReload");
    }
    public async override void Reload(InputAction.CallbackContext context)
    {
        if (Patrons < maxPatrons && isReload != true)
        {
            //if (Patrons <= 0)
            ReloadSound();
            indexPatron = Mathf.Clamp((int)Patrons, 0, _updateGausePatronsImage.Count - 1);
            _updateGausePatronsImage[indexPatron].enabled = true;
            Patrons++;
            //_patronsText.text = Patrons.ToString();
        }
    }
}
