using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water_damage : MonoBehaviour, damage_interface
{
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float timerR = 20f;
    [SerializeField] private GameObject electoA;
    [SerializeField] private GameObject steamA;

    public float getDamage()
    {
        return _damage;
    }
    public string getstate()
    {
        return "";
    }
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
    public float gettime()
    {
        return timerR;
    }
}
