using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    private IWeapon weapon;
    private ActionPrototypePlayer inputShoot;
    private bool isPressed;
    private InputAction.CallbackContext isPressedContext;
    private TextMeshProUGUI patronsText;
    [SerializeField] private WeaponTypeScriptableObj[] weaponParametrs;
    [SerializeField] private WeaponType weaponType;

    //visual
    private ParticleSystem vfxShootPrefab;
    private CinemachineImpulseSource recoilCinemachine;
    private Animator animatorArms;
    private Animator animatorWeapon;
    enum WeaponType
    {
        Revolver,
        Shotgun
    }
    private void OnEnable()
    {
        recoilCinemachine = transform.root.GetComponentInChildren<CinemachineImpulseSource>();
        vfxShootPrefab = GetComponentInChildren<ParticleSystem>();
        animatorArms = transform.root.GetComponentInChildren<Animator>();
        animatorWeapon = GetComponent<Animator>();
        inputShoot = new ActionPrototypePlayer();
        inputShoot.Enable();
        inputShoot.Player.Shoot.started += context => Shoot(context);
        inputShoot.Player.Shoot.performed += context => Shoot(context);
        inputShoot.Player.Shoot.canceled += context => Shoot(context);
        inputShoot.Player.Reload.started += context => Reload(context);
    }
    private void OnDisable()
    {
        inputShoot.Disable();
        inputShoot.Player.Shoot.started -= context => Shoot(context);
        inputShoot.Player.Shoot.performed -= context => Shoot(context);
        inputShoot.Player.Shoot.canceled -= context => Shoot(context);
        inputShoot.Player.Reload.started -= context => Reload(context);
    }
    private void Update()
    {
        if(isPressed)
        {
            weapon.Shoot(isPressedContext);
        }
    }
    private void Start()
    {
        //use parametrs for shoot and weapon
        patronsText = transform.root.GetComponentInChildren<TextMeshProUGUI>();
        switch (weaponType)
        {
            case WeaponType.Revolver:
                weapon = new ParametrsUpdateDecorator(weapon, 
                    weaponParametrs[0].fireRate,
                    weaponParametrs[0].damage,
                    weaponParametrs[0].range, weaponParametrs[0].reloadSpeed,
                    weaponParametrs[0].patrons, weaponParametrs[0].attackType,
                    weaponParametrs[0].enemyLayer,
                    vfxShootPrefab,patronsText, animatorArms, animatorWeapon,recoilCinemachine);
                break;
            case WeaponType.Shotgun:

                break;
        }

    }
    public void Shoot(InputAction.CallbackContext context)
    {
        //One Shoot
        if (context.started)
            weapon.Shoot(context);
        //Shoot Pressed
        if (context.performed)
        {
            isPressedContext = context;
            isPressed = true;
        }
        //Cancel action
        if(context.canceled) 
        {
            isPressedContext = context;
            isPressed = false;
            animatorArms.SetBool("shoot", false);
            animatorWeapon.SetBool("shoot", false);
        }
    }
    /*
    IEnumerator ShootCoroutine(InputAction.CallbackContext context)
    {
        while (context.performed)
        {
            yield return new WaitForSeconds(0.6f);
            weapon.Shoot(context);
            Debug.Log(context);
        }
    }
    */
    //reload func
    public async void Reload(InputAction.CallbackContext context)
    {
        weapon.Reload(context);
    }
}
