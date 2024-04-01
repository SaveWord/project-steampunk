using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int spawnerID;
    public float count;
    public List<Transform> dotSpawn;
    public Dictionary<int, GameObject> enemies;
    public List<DoorController> doors;
    [SerializeField] private GameObject enemyAntPrefab;
    //[SerializeField] private int enemiesCount;
    BoxCollider detectZone;

    private void Update()
    {
        count = enemies.Count;
    }
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
                enemies[j].GetComponent<HpEnemy>()._idEnemy = j;
                enemies[j].GetComponent<HpEnemy>().DeleteList += DeleteList;
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
        dotSpawn = transform.Cast<Transform>().ToList();

        //load data
        GameManagerSingleton.Instance.SaveSystem.LoadSpawnerData();

        //if load disable spawner and open door
        if (spawnerID < GameManagerSingleton.Instance.SaveSystem.spawnerData.disableSpawner.Count)
        {
            if (GameManagerSingleton.Instance.SaveSystem.spawnerData.disableSpawner[spawnerID] == 1)
            {
                detectZone.enabled = false;
                DoorCheck();
            }
        }

    }
    public void SaveStaySpawner()
    {
        GameManagerSingleton.Instance.SaveSystem.SaveSpawnerData(1);
    }
    private void DeleteList(int id)
    {
        enemies[id].GetComponent<HpEnemy>().DeleteList -= DeleteList;
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
        }
    }
}
