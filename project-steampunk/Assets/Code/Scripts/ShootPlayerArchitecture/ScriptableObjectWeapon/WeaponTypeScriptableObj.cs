using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IWeapon;


[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/WeaponTypeAndParametrs", order = 1)]
public class WeaponTypeScriptableObj : ScriptableObject
{
    public float fireRate;
    public float damage;
    public float range;
    public float reloadSpeed;
    public float patrons;
    public WeaponTypeDamage attackType;
    public LayerMask enemyLayer;
}
