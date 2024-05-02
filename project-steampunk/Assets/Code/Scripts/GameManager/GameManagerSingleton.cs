using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSingleton : MonoBehaviour
{
    public static GameManagerSingleton Instance { get; private set; }
    public List<CheckPoint> checkPointsID = new List<CheckPoint>();
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
        foreach (var checkPoint in checkPointsID)
        {
            checkPoint.idCheckPoint = j;
            j++;
        }
        
    }
    public void PauseGame()
    {
        SingletonActionPlayer.Instance.inputActions.Player.Disable();
        SingletonActionPlayer.Instance.inputActions.UICustom.Enable();
        Time.timeScale = 0f;
    }
    public void UnPauseGame()
    {
        SingletonActionPlayer.Instance.inputActions.Player.Enable();
        //SingletonActionPlayer.Instance.inputActions.UICustom.Disable();
        Time.timeScale = 1f;
    }
}
