using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static DotSpawnType;

public class Spawner : MonoBehaviour
{
    public int timeSpawnDelay;
    public int spawnerID;
    public float count;
    public List<DotSpawnType> dotSpawn;
    public Dictionary<int, GameObject> enemies;
    public List<DoorController> doors;
    [SerializeField] private GameObject enemyAntPrefab;
    [SerializeField] private GameObject enemyAntShieldPrefab;
    [SerializeField] private GameObject enemySpiderPrefab;
    [SerializeField] private GameObject enemyBeetlePrefab;
    [SerializeField] private GameObject enemyBeetleShieldPrefab;
    [SerializeField] private GameObject enemyBeetleTurretPrefab;
    [SerializeField] private GameObject enemyBeePrefab;
    [SerializeField] private GameObject enemyBeeDoubleVar1;
    [SerializeField] private GameObject enemyBeeDoubleVar2;
    [SerializeField] private GameObject enemyBossPrefab;
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
            AudioManager.InstanceAudio.PlayMusic("Battle", true);

            foreach (var door in doors)
            {
                door.DoorClose();
            }

            StartCoroutine(SetActiveEnemies());// foreach set active enemies with await in async method

            detectZone.enabled = false;

        }
    }
    private IEnumerator SetActiveEnemies()
    {
        //int i = 0;
        foreach (var enemy in enemies.Values)
        {
            enemy.SetActive(true);
            // i++;
            yield return new WaitForSeconds(timeSpawnDelay / 1000);
           //await Task.Delay(timeSpawnDelay);
        }
    }
    private void Start()
    {
        doors = new List<DoorController>();
        doors.AddRange(GetComponentsInChildren<DoorController>());
        doors.ForEach(door => { door.gameObject.SetActive(false); });
        enemies = new Dictionary<int, GameObject>();
        detectZone = GetComponent<BoxCollider>();
        dotSpawn = new List<DotSpawnType>();
        dotSpawn.AddRange(GetComponentsInChildren<DotSpawnType>());

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
        int j = 0;
        foreach (var dot in dotSpawn)
        {
            GameObject enemy = null;
            //EnemyTypeSpawn dotSpawnType = dot.GetComponent<DotSpawnType>().enemyTypeSpawn;
            switch (dot.enemyTypeSpawn)
            {
                case EnemyTypeSpawn.Ant:
                    enemy = Instantiate(enemyAntPrefab, dot.gameObject.transform);
                    break;
                case EnemyTypeSpawn.AntShield:
                    enemy = Instantiate(enemyAntShieldPrefab, dot.gameObject.transform);
                    break;
                case EnemyTypeSpawn.Spider:
                    enemy = Instantiate(enemySpiderPrefab, dot.gameObject.transform);
                    break;
                case EnemyTypeSpawn.Beetle:
                    enemy = Instantiate(enemyBeetlePrefab, dot.gameObject.transform);
                    break;
                case EnemyTypeSpawn.BeetleShield:
                    enemy = Instantiate(enemyBeetleShieldPrefab, dot.gameObject.transform);
                    break;
                case EnemyTypeSpawn.BeetleTurret:
                   enemy = Instantiate(enemyBeetleTurretPrefab, dot.gameObject.transform);
                    break;
                case EnemyTypeSpawn.Bee:
                    enemy = Instantiate(enemyBeePrefab, dot.gameObject.transform);
                    break;
                case EnemyTypeSpawn.BeeDouble1:
                    enemy = enemy = Instantiate(enemyBeeDoubleVar1, dot.gameObject.transform);
                    break;
                case EnemyTypeSpawn.BeeDouble2:
                    enemy = enemy = Instantiate(enemyBeeDoubleVar2, dot.gameObject.transform);
                    break;
                case EnemyTypeSpawn.LaBoss:
                    enemy = enemy = Instantiate(enemyBossPrefab, dot.gameObject.transform);
                    break;
            }
            enemy.transform.localPosition = Vector3.zero;
            enemies.Add(j, enemy);
            enemies[j].GetComponent<HpEnemy>()._idEnemy = j;
            enemies[j].GetComponent<HpEnemy>().DeleteList += DeleteList;
            enemies[j].SetActive(false);
            j++;
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
                door.dissolve = true;
            }
            AudioManager.InstanceAudio.PlayMusic("Battle", false);
        }
    }
}
