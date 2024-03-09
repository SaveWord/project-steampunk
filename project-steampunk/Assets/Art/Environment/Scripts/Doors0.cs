using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors0 : MonoBehaviour
{
    [SerializeField] GameObject ArenaArea;
    bool check;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            check = ArenaArea.GetComponent<CheckForEnemies>().EnemiesDead;
            if(check)
            {
                ArenaArea.GetComponent<Animation>().Play();
            }
        }


    }
}
