using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water_damage : MonoBehaviour, damage_interface
{
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float timerR = 20f;
    [SerializeField] private float damageInterval = 2f;
    [SerializeField] private GameObject electoA;
    [SerializeField] private GameObject steamA;
    private float damageTimer = 0f;

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
            electric_damage electo = other.gameObject.GetComponent<electric_damage>();

            if (electo != null)
            {
                Debug.Log("electro!");
                Vector3 spawnPosition = transform.position;
                Quaternion spawnRotation = transform.rotation;

                GameObject spawnedObject = Instantiate(electoA, spawnPosition, spawnRotation);
                Destroy(gameObject);
            }

            fire_damage fire = other.gameObject.GetComponent<fire_damage>();

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

    private void OnTriggerStay(Collider other)
    {
        other.gameObject.TryGetComponent(out health_abstract health);
        damageTimer += Time.deltaTime;
        if (damageTimer >= damageInterval)
        {
            health?.TakeDamage(_damage);
            damageTimer = 0f;
        }
    }
    public float gettime()
    {
        return timerR;
    }
}
