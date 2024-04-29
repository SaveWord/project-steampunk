using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonActionPlayer: MonoBehaviour
{
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
        inputActions.Player.Disable();
        inputActions.UI.Disable();
        inputActions.UICustom.Enable();
    }
    private void OnDestroy()
    {
        inputActions.Disable();
    }
}
