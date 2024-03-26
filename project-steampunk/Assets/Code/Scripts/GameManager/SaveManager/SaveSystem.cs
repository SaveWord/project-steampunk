using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public PlayerData playerData;
    public CheckPointData checkPointData;
    private string saveDataPath;
    private string saveDataPathCheckPoint;

    private void OnEnable()
    {
        playerData = new PlayerData();
        checkPointData = new CheckPointData();
        saveDataPath = Application.persistentDataPath + "/PlayerData.json";
        saveDataPathCheckPoint = Application.persistentDataPath + "/CheckPoint.json";
    }
    public void SaveData(float hp, Vector3 pos )
    {
        playerData.health = hp;
        playerData.position = pos;
        string savePlayerData = JsonUtility.ToJson( playerData );
        File.WriteAllText( saveDataPath, savePlayerData );
        Debug.Log("FileSave");
    }

    public void LoadData()
    {
        if (File.Exists(saveDataPath))
        {
            string loadPlayerData = File.ReadAllText(saveDataPath);
            playerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
            Debug.Log("FileLoad");
        }
        else { Debug.Log("File not found"); }
    }
    public void SaveCheckPoint(int disableCheck)
    {
        checkPointData.disablePoint.Add(disableCheck);
        string saveCheckPointData = JsonUtility.ToJson(checkPointData);
        File.WriteAllText(saveDataPathCheckPoint, saveCheckPointData);
        Debug.Log("FileSavePoint");
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
}
public class CheckPointData
{
    public List<int> disablePoint;
}
public class PlayerData
{
    public float health;
    public Vector3 position;
}
