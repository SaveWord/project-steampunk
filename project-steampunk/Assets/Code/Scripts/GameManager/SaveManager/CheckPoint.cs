using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    public int idCheckPoint;
    public List<Spawner> spawners = new List<Spawner>();
    private Collider colliderCheckPoint;
    private void Start()
    {
        colliderCheckPoint = GetComponent<Collider>();
        GameManagerSingleton.Instance.SaveSystem.LoadCheckPoint();
        if (idCheckPoint < GameManagerSingleton.Instance.SaveSystem.checkPointData.disablePoint.Count)
        {
            if (GameManagerSingleton.Instance.SaveSystem.checkPointData.disablePoint[idCheckPoint] == 1)
            {
                colliderCheckPoint.enabled = false;
            }
        }
        spawners.AddRange(GetComponentsInChildren<Spawner>());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out IHealth health);
           int sceneId = SceneManager.GetActiveScene().buildIndex;
            //float hp = health.CurrentHp;
            Vector3 position = this.transform.position;
            GameManagerSingleton.Instance.SaveSystem.SaveData( position,sceneId);
            GameManagerSingleton.Instance.SaveSystem.SaveCheckPoint(1);
            foreach (var spawner in spawners)
            {
                spawner.SaveStaySpawner();
            }

            colliderCheckPoint.enabled = false;

        }
    }
}