using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int spawnerID;
    public List<Transform> dotSpawn;
    public Dictionary<int, GameObject> enemies;
    public List<DoorController> doors;
    [SerializeField] private GameObject enemyAntPrefab;
    //[SerializeField] private int enemiesCount;
    BoxCollider detectZone;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int j = 0;
            foreach (var dot in dotSpawn)
            {

                var enemy = Instantiate(enemyAntPrefab, dot);
                enemy.transform.localPosition = Vector3.zero;
                enemies.Add(j, enemy);
                enemies[j].GetComponent<HpHandler>()._idEnemy = j;
                enemies[j].GetComponent<HpHandler>().DeleteList += DeleteList;
                j++;
            }
            foreach (var door in doors)
            {
                door.DoorClose();
            }
            detectZone.enabled = false;

        }
    }
    private void Start()
    {
        doors = new List<DoorController>();
        enemies = new Dictionary<int, GameObject>();
        doors.AddRange(GetComponentsInChildren<DoorController>());
        detectZone = GetComponent<BoxCollider>();
        GameManagerSingleton.Instance.SaveSystem.LoadSpawnerData();
        if (spawnerID < GameManagerSingleton.Instance.SaveSystem.spawnerData.disableSpawner.Count)
        {
            if (GameManagerSingleton.Instance.SaveSystem.spawnerData.disableSpawner[spawnerID] == 1)
            {
                DoorCheck();
                detectZone.enabled = false;
            }
        }

    }
    private void DeleteList(int id)
    {
        enemies[id].GetComponent<HpHandler>().DeleteList -= DeleteList;
        enemies.Remove(id);
        DoorCheck();

    }
    private void DoorCheck()
    {
        if (enemies.Count == 0)
        {
            Debug.Log("ENEMYES Empty");
            foreach (var door in doors)
            {
                door.DoorOpen();
            }
            GameManagerSingleton.Instance.SaveSystem.SaveSpawnerData(spawnerID, 1);
        }
    }
}
