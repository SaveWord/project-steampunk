using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    public float rotateSpeed;
    void Update()
    {
        transform.Rotate(0,Time.deltaTime * rotateSpeed,0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<SwitchWeapon>().WeaponUnlockMethod();
            gameObject.SetActive(false);
        }
    }
}
