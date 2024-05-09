using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonActionPlayer : MonoBehaviour
{
    public GameObject player;
    public static SingletonActionPlayer Instance { get; private set; }
    public ActionPrototypePlayer inputActions;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        inputActions = new ActionPrototypePlayer();
        inputActions.Enable();
        if (SceneManager.GetActiveScene().buildIndex != 0)
            player.SetActive(true);
    }
    private void OnDestroy()
    {
        inputActions.Disable();
    }
}
