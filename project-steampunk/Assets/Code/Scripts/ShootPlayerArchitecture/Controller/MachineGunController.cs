using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MachineGunController : WeaponController
{
    private RecoilMachineGun recoil;

    Vector3 target;
    Vector3 current;
    protected override void Start()
    {
        lineRenderers = new List<LineRenderer>();
        LineRenderer tmp;
        for (int i = 0; i < 10; i++)
        {
            tmp = Instantiate(lineRenderer);
            tmp.enabled = false;
            lineRenderers.Add(tmp);
        }
        //use parametrs for shoot and weapon
        patronsText = GetComponentInChildren<TextMeshProUGUI>();
        recoil = transform.root.GetComponentInChildren<RecoilMachineGun>();
        weapon = new ParametrsUpdateMachineGun(transform,weapon, weaponParametrs.fireRate,
            weaponParametrs.distanceAndDamages
            , weaponParametrs.reloadSpeed,
            weaponParametrs.patrons, weaponParametrs.attackType,
            weaponParametrs.enemyLayer,
            vfxShootPrefab, weaponParametrs.vfxImpactMetalProps, weaponParametrs.vfxImpactOtherProps,
            patronsText, animatorArms, animatorWeapon, recoilCinemachine,recoil, lineRenderers,dotLine);
        weapon.Switch = false;
        startSwitchInisialise = true;
    }
}
