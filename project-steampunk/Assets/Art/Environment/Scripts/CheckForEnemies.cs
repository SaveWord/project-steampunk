using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForEnemies : MonoBehaviour
{
    public bool EnemiesDead;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer==6)
        {
            EnemiesDead = false;
        }
    }
}
