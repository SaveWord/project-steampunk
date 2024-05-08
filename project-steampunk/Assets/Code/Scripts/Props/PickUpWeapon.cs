using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    public float rotateSpeed;
    public int idPickUp;

    private void Start()
    {
        GameManagerSingleton.Instance.SaveSystem.LoadData();
        if (idPickUp < GameManagerSingleton.Instance.SaveSystem.playerData.switchWeapon)
        {
            gameObject.SetActive(false);
        }
    }
    void Update()
    {
        transform.Rotate(0,Time.deltaTime * rotateSpeed,0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<SwitchWeapon>().WeaponUnlockMethod();
            AudioManager.InstanceAudio.PlaySfxWeapon((idPickUp == 0) ? "MachineGunPickUp" : "GaussPickUp");
            gameObject.SetActive(false);
        }
    }
}
