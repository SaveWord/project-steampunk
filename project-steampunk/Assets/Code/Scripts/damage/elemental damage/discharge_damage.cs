using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class discharge_damage : MonoBehaviour, damage_interface
{
    [SerializeField] private float _damage = 0.3f;
[SerializeField] private float timerR = 20f;
[SerializeField] private float damageInterval = 2f;
private float damageTimer = 0f;

public float getDamage()
{
    return _damage;
}
public string getstate()
{
    return "frozen";
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
