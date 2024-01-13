using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earthArea : MonoBehaviour, BlowArea
{
    [SerializeField] private float damageAmount = 0.01f;
    [SerializeField] private float timerR = 20f;
    [SerializeField] private bool frozenStatus = false;
    [SerializeField] private GameObject LavaA;
    [SerializeField] private GameObject GlassA;
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
        Debug.Log("enter!");
        if (other.CompareTag("killzone"))
        {
            fireArea fire = other.gameObject.GetComponent<fireArea>();

            if (fire != null)
            {
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = -1.75f; 
                Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);
                transform.rotation = spawnRotation;

                GameObject spawnedObject = Instantiate(LavaA, spawnPosition, spawnRotation);
                Destroy(gameObject);
            }

            electricArea electro = other.gameObject.GetComponent<electricArea>();

            if (electro != null)
            {
                Vector3 spawnPosition = transform.position;
                Quaternion spawnRotation = transform.rotation;

                GameObject spawnedObject = Instantiate(GlassA, spawnPosition, spawnRotation);
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