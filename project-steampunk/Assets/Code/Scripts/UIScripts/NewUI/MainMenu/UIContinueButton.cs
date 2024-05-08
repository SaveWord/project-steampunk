using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContinueButton : MonoBehaviour
{
    private void Awake()
    {
        if (GameManagerSingleton.Instance.CheckpointExists())
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}
