using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CheckForEnemies : MonoBehaviour
{
    public List<GameObject> enemiesInArena;
    private void Start()
    {
        enemiesInArena = new List<GameObject>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (!enemiesInArena.Contains(other.gameObject))
                enemiesInArena.Add(other.gameObject);
        }
    }
}
