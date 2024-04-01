using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonActionPlayer: MonoBehaviour
{
    public static SingletonActionPlayer Instance { get; private set; }
    public ActionPrototypePlayer inputActions;
    [SerializeField] private GameObject player;
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
        player.SetActive(true);
    }
    private void OnDestroy()
    {
        inputActions.Disable();
    }
}
