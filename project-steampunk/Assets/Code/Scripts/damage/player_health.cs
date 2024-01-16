using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_health : health_abstract
{
    //[SerializeField] private Transform playerTransform;
    private Vector3 playerPosition;
    private bool isDead;

    void Update()
    {
        if (isDead)
        {
            transform.position = playerPosition;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enter!");
        //Debug.Log(other.tag);
        if (other.CompareTag("bullet"))
        {
            mineHP -= other.gameObject.GetComponent<damage_interface>().getDamage();
            UpdateHealth();
            //Debug.Log(mineHP);
        }

    }

    protected override void Die()
    {
        Console.WriteLine("Implementation in the derived class.");
        playerPosition = transform.position;
        isDead = true;
    }

    
}
