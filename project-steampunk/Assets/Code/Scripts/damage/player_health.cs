using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player_health : health_abstract
{
    //[SerializeField] private Transform playerTransform;
    private Vector3 playerPosition;
    private bool isDead;

    void Update()
    {
        if (isDead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("boom!");
        //Debug.Log(other.tag);
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            mineHP -= other.gameObject.GetComponent<damage_interface>().getDamage();
            UpdateHealth();
            //Debug.Log(mineHP);
        }

    }

    public override void Die()
    {
        Console.WriteLine("Implementation in the derived class.");
        playerPosition = transform.position;
        isDead = true;
    }

    
}
