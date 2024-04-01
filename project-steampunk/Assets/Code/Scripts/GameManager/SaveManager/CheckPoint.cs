using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private int idCheckPoint;
    public List<Spawner> spawners = new List<Spawner>();
    private void Start()
    {
        GameManagerSingleton.Instance.SaveSystem.LoadCheckPoint();
        if (idCheckPoint < GameManagerSingleton.Instance.SaveSystem.checkPointData.disablePoint.Count)
        {
            if (GameManagerSingleton.Instance.SaveSystem.checkPointData.disablePoint[idCheckPoint] == 1)
            {
                gameObject.SetActive(false);
            }
        }
       spawners.AddRange(GetComponentsInChildren<Spawner>());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out IHealth health);
            float hp = health.CurrentHp;
            Vector3 position = other.transform.position;
            GameManagerSingleton.Instance.SaveSystem.SaveData(hp, position);
            GameManagerSingleton.Instance.SaveSystem.SaveCheckPoint(1);
                foreach (var spawner in spawners)
                {
                    spawner.SaveStaySpawner();
                }
                   
            gameObject.SetActive(false);

        }
    }
}
