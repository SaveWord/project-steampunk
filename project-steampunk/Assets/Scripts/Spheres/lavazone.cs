using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavazone : MonoBehaviour, BlowArea
{
    [SerializeField] private float damageAmount = 0.4f;
    [SerializeField] private float timerR = 7f;
    [SerializeField] private bool frozenStatus = false;
    [SerializeField] private float liftForce = 0f;

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
