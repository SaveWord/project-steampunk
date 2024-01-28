using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class steam_damage : MonoBehaviour, damage_interface
{
    [SerializeField] private float _liftForce = 90f;
    [SerializeField] private float timerR = 1f;
    private float damageTimer = 0f;
    void Update()
    {
        timerR -= Time.deltaTime;
        if (timerR < 0)
        {
            Destroy(gameObject);
        }
    }
    public float getDamage()
    {
        return _liftForce;
    }
    public string getstate()
    {
        return "jump";
    }
    
}
