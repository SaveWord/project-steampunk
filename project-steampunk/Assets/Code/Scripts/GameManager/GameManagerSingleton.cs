using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSingleton : MonoBehaviour
{
    public static GameManagerSingleton Instance { get; private set; }
    public List<Spawner> spawnerID = new List<Spawner>();
    public SaveSystem SaveSystem { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        SaveSystem = GetComponentInChildren<SaveSystem>();
        spawnerID.AddRange(GetComponentsInChildren<Spawner>());
        int j = 0;
        foreach (var spawner in spawnerID)
        {
            spawner.spawnerID = j;
            j++;
        }
    }
}
