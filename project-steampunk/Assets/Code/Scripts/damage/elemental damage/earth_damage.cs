using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class earth_damage : MonoBehaviour, damage_interface
{
    [SerializeField] private float _damage = 0.1f;
    [SerializeField] private float timerR = 20f;
    [SerializeField] private float damageInterval = 2f;
    private float damageTimer = 0f;

    [SerializeField] private GameObject LavaA;
    [SerializeField] private GameObject GlassA;

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
            fire_damage fire = other.gameObject.GetComponent<fire_damage>();

            if (fire != null)
            {
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = -1.75f;
                Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);
                transform.rotation = spawnRotation;

                GameObject spawnedObject = Instantiate(LavaA, spawnPosition, spawnRotation);
                Destroy(gameObject);
            }

            electric_damage electro = other.gameObject.GetComponent<electric_damage>();

            if (electro != null)
            {
                Vector3 spawnPosition = transform.position;
                Quaternion spawnRotation = transform.rotation;

                GameObject spawnedObject = Instantiate(GlassA, spawnPosition, spawnRotation);
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

}
