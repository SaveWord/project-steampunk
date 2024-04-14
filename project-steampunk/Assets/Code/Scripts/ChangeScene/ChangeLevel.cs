using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] private int sceneName;
    private void OnTriggerEnter(Collider other)
    {
        GameManagerSingleton.Instance.SaveSystem.DeleteAllSave();
        SceneManager.LoadSceneAsync(sceneName);
    }
}
