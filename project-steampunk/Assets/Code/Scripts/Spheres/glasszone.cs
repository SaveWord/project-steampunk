using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glasszone : MonoBehaviour, BlowArea
{
    [SerializeField] private float damageAmount = 0.0001f;
    [SerializeField] private float timerR = 15f;
    [SerializeField] private bool frozenStatus = true;
    [SerializeField] private float liftForce = 0f;
    //public float DamageAmount => damageAmount;

    void Update()
    {
        timerR -= Time.deltaTime;
        if (timerR < 0)
        {
            Destroy(gameObject);
        }
    }
    public float getDamdge()
    {
        return damageAmount;
    }
    public bool isfrozen()
    {
        return frozenStatus;
    }
    public float liftUp()
    {
        return liftForce;
    }
}
