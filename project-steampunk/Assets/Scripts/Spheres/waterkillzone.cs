using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterkillzone : MonoBehaviour, BlowArea
{
    [SerializeField] private float damageAmount = 0.0001f;
    [SerializeField] private float timerR = 20f;
    [SerializeField] private bool frozenStatus = false;
    [SerializeField] private GameObject electoA;
    [SerializeField] private GameObject steamA;
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
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter!");
        if (other.CompareTag("killzone"))
        {
            electricArea electo = other.gameObject.GetComponent<electricArea>();

            if (electo != null)
            {
                Debug.Log("electro!");
                Vector3 spawnPosition = transform.position;
                Quaternion spawnRotation = transform.rotation;

                GameObject spawnedObject = Instantiate(electoA, spawnPosition, spawnRotation);
                Destroy(gameObject);
            }

            fireArea fire = other.gameObject.GetComponent<fireArea>();

            if (fire != null)
            {
                Debug.Log("stem!");
                Vector3 spawnPosition = transform.position;
                Quaternion spawnRotation = transform.rotation;

                GameObject spawnedObject = Instantiate(steamA, spawnPosition, spawnRotation);
                Destroy(gameObject);
            }
        }
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
