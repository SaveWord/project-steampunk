using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static IWeapon;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.UIElements;
using System.Linq;

public class ParametrsUpdateDecorator : MainDecorator
{
    protected IWeapon _weapon;
    //parametrs weapon
    protected float _updateLastShoot;
    protected float _updateFireRate;
    protected float _updateDamage;
    protected float _updateMaxRange;
    protected float _updateReload;
    protected float _updatePatrons;
    protected WeaponTypeDamage _updateWeaponType;
    protected LayerMask _updateEnemyLayer;


    //effects and ui value
    protected CinemachineImpulseSource _recoil;
    protected ParticleSystem _vfxShootPrefab;
    protected ParticleSystem _vfxImpactMetalProps;
    protected ParticleSystem _vfxImpactOtherProps;
    protected TextMeshProUGUI _patronsText;
    protected bool isReload;
    protected Animator _animator;
    protected Animator _animatorWeapon;


    //constructor
    public ParametrsUpdateDecorator(IWeapon weapon, float updateFireRate,
        DistanceAndDamage[] updateDamage, float updateReload, float updatePatrons,
        IWeapon.WeaponTypeDamage updateWeaponType, LayerMask mask,
        ParticleSystem vfxShootPrefab, ParticleSystem vfxImpactMetalProps, ParticleSystem vfxImpactOtherProps,
        TextMeshProUGUI patronsText,
        Animator animator, Animator animatorWeapon, CinemachineImpulseSource recoil) : base(weapon)
    {

        _updateFireRate = updateFireRate;

        _weapon = weapon;
        _updateDamage = updateDamage.Last().damage;
        _updateMaxRange = updateDamage.Last().range;
        _updateReload = updateReload;
        _updatePatrons = updatePatrons;
        _updateWeaponType = updateWeaponType;
        _updateEnemyLayer = mask;
        maxPatrons = updatePatrons;

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

                hit.collider.TryGetComponent(out IDamageableProps damageableProps);
                damageableProps?.GetDamage(Damage);

                hit.collider.TryGetComponent(out IHealth damageable);
                damageable?.TakeDamage(Damage);


                ShowVFXImpact(hit);
            }
        }
        if (Patrons == 0 && isReload == false)
        {
            _animator.SetBool("shoot", false);
            _animatorWeapon.SetBool("shoot", false);
            Reload(context);
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
        if(hit.collider.gameObject.layer == 25)
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
        if ((context.started || context.performed) && Patrons < maxPatrons)
        {
            Debug.Log("Activate");
            isReload = true;
            _animator.SetBool("reload", true);
            _animatorWeapon.SetBool("reload", true);
            await Task.Delay((int)ReloadSpeed * 1000);
            isReload = false;
            _animator.SetBool("reload", false);
            _animatorWeapon.SetBool("reload", false);
            Debug.Log("Deactivate");
            Patrons = maxPatrons;
            _patronsText.text = Patrons.ToString();
        }
    }
}
