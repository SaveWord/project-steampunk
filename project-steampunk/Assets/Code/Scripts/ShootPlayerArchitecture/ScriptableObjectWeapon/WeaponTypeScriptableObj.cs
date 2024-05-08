using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static IWeapon;


[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/WeaponTypeAndParametrs", order = 1)]
public class WeaponTypeScriptableObj : ScriptableObject
{
    public float fireRate;
    public float reloadSpeed;
    public float patrons;
    public DistanceAndDamage[] distanceAndDamages;
    
    public WeaponTypeDamage attackType;
    public LayerMask enemyLayer;

    //Impact in object shoot
    public ParticleSystem vfxImpactMetalProps;
    public ParticleSystem vfxImpactOtherProps;
}
[Serializable]
public class DistanceAndDamage
{
    public float damage;
    public float range;
    public float radiusRay;
}