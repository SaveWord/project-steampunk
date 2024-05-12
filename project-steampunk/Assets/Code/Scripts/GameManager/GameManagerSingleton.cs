using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManagerSingleton : MonoBehaviour
{
    public static GameManagerSingleton Instance { get; private set; }
    public List<CheckPoint> checkPointsID = new List<CheckPoint>();
    public List<Spawner> spawnerID = new List<Spawner>();
    public SaveSystem SaveSystem { get; private set; }
    public AudioMixer _mixer;
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
        _mixer.SetFloat("MuteParam", Mathf.Log10(1) * 20);
        Time.timeScale = 0f;
    }
    public void UnPauseGame()
    {
        SingletonActionPlayer.Instance.inputActions.Player.Enable();
        _mixer.SetFloat("MuteParam", Mathf.Log10(0) * 20);
        //SingletonActionPlayer.Instance.inputActions.UICustom.Disable();
        Time.timeScale = 1f;
    }

    public bool CheckpointExists()
    {
        return SaveSystem.CheckpointExists();
    }
}
