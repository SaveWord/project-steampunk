using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ateamzone : MonoBehaviour, BlowArea
{
    [SerializeField] private float damageAmount = 0.0001f;
    [SerializeField] private float timerR = 1f;
    [SerializeField] private bool frozenStatus = false;
    [SerializeField] private float liftForce = 15;
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
