using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemys : MonoBehaviour
{
    [SerializeField] private GameObject[] dotSpawn;
    [SerializeField] private GameObject enemyPrefabStandart;
    [SerializeField] private GameObject enemyPrefabFast;
    [SerializeField] private GameObject enemyPrefabShoot;
    [SerializeField] private float interval;
    private float counter;
    private void FixedUpdate()
    {
        Debug.Log(counter);
        if (counter < Time.time)
        {
            counter = Time.time + interval;
            foreach (var item in dotSpawn)
            {
                int random = Random.Range(1, 4);
                switch (random)
                {
                    case 1:
                        Instantiate(enemyPrefabStandart, item.transform.position,
                            item.transform.rotation);
                        break;
                    case 2:
                        Instantiate(enemyPrefabFast, item.transform.position,
                           item.transform.rotation);
                        break;
                    case 3:
                        Instantiate(enemyPrefabShoot, item.transform.position,
                           item.transform.rotation);
                        break;
                }


            }
        }
    }
}
