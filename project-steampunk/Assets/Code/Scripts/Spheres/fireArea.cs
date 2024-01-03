using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireArea : MonoBehaviour, BlowArea
{
    [SerializeField] private float damageAmount = 0.2f;
    [SerializeField] private float timerR = 3f;
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("killzone"))
        {
            earthArea earth = other.gameObject.GetComponent<earthArea>();

            if (earth != null)
            {
                Destroy(gameObject);
            }
            waterkillzone water = other.gameObject.GetComponent<waterkillzone>();

            if (water != null)
            {
                Destroy(gameObject);
            }
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
