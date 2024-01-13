using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electricArea : MonoBehaviour, BlowArea
{
    [SerializeField] private float damageAmount = 0.1f;
    [SerializeField] private float timerR = 6f;
    [SerializeField] private bool frozenStatus = false;
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("killzone"))
        {
            waterkillzone water = other.gameObject.GetComponent<waterkillzone>();

            if (water != null)
            {
                Destroy(gameObject);
            }

            earthArea earth = other.gameObject.GetComponent<earthArea>();

            if (earth != null)
            {
                Destroy(gameObject);
            }
        }
    }
    public float liftUp()
    {
        return liftForce;
    }
}
