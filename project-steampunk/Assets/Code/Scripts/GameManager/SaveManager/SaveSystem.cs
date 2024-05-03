using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public PlayerData playerData;
    public CheckPointData checkPointData;
    public SpawnerData spawnerData;
    private string saveDataPath;
    private string saveDataPathCheckPoint;
    private string saveDataPathSpawner;

    private void OnEnable()
    {
        playerData = new PlayerData();
        checkPointData = new CheckPointData();
        spawnerData = new SpawnerData();
        saveDataPath = Application.persistentDataPath + "/PlayerData.json";
        saveDataPathCheckPoint = Application.persistentDataPath + "/CheckPoint.json";
        saveDataPathSpawner = Application.persistentDataPath + "/SpawnerData.json";

    }
    public void SaveSwitchWeapon(int switchWp)
    {
        playerData.switchWeapon = switchWp;
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveDataPath, savePlayerData);
    }
    public void SaveScene(int idScene)
    {
        playerData.sceneID = idScene;
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveDataPath, savePlayerData);
    }
    public void SaveData(Vector3 pos, int idScene, Vector3 rot)
    {
        //playerData.health = hp;
        playerData.position = pos;
        playerData.sceneID = idScene;
        playerData.rotation = rot;
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveDataPath, savePlayerData);
        Debug.Log("FileSave");
    }
    public void SaveSpawnerData(int disable)
    {
        spawnerData.disableSpawner.Add(disable);
        string saveDataSpawner = JsonUtility.ToJson(spawnerData);
        File.WriteAllText(saveDataPathSpawner, saveDataSpawner);
        Debug.Log("FileSpawnerSave");
    }
    public void SaveCheckPoint(int disableCheck)
    {
        checkPointData.disablePoint.Add(disableCheck);
        string saveCheckPointData = JsonUtility.ToJson(checkPointData);
        File.WriteAllText(saveDataPathCheckPoint, saveCheckPointData);
        Debug.Log("FileSavePoint");
    }
    public void LoadData()
    {
        if (File.Exists(saveDataPath))
        {
            string loadPlayerData = File.ReadAllText(saveDataPath);
            playerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
            Debug.Log("FileLoad");
        }
        else
        {
            //playerData.health = 100;
            playerData.sceneID = 1;
            if (SceneManager.GetActiveScene().buildIndex != 0)
                playerData.position = GetComponentInChildren<CheckPoint>().gameObject.transform.position;
            Debug.Log("File not found");
        }
    }
    public void LoadSpawnerData()
    {
        if (File.Exists(saveDataPathSpawner))
        {
            string loadSpawnerData = File.ReadAllText(saveDataPathSpawner);
            spawnerData = JsonUtility.FromJson<SpawnerData>(loadSpawnerData);
            Debug.Log("FileSpawnLoad");
        }
    }
    public void LoadCheckPoint()
    {
        if (File.Exists(saveDataPathCheckPoint))
        {
            string loadCheckPointData = File.ReadAllText(saveDataPathCheckPoint);
            checkPointData = JsonUtility.FromJson<CheckPointData>(loadCheckPointData);
            Debug.Log("FileLoadPoint");
        }
        else { Debug.Log("FilePoint not found"); }
    }

    public void DeleteAllSave()
    {
        File.Delete(saveDataPath);
        File.Delete(saveDataPathSpawner);
        File.Delete(saveDataPathCheckPoint);
    }
}

public class CheckPointData
{
    public List<int> disablePoint = new List<int>();
}
public class PlayerData
{
    //public float health;
    public Vector3 position;
    public Vector3 rotation;
    public int switchWeapon;
    public int sceneID;
}
public class SpawnerData
{
    public List<int> disableSpawner = new List<int>();
}
