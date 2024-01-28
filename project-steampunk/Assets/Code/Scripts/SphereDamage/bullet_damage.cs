using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_damage : MonoBehaviour, damage_interface
{
    [SerializeField] private float _damage = 1f;

    public float getDamage()
    {
        return _damage;
    }
    public string getstate()
    {
        return "";
    }
    public float gettime()
    {
        return 0;
    }
}
