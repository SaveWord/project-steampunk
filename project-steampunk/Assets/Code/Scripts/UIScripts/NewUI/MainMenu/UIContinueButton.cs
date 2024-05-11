using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContinueButton : MonoBehaviour
{
    private void Start()
    {
        if (GameManagerSingleton.Instance.CheckpointExists())
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}
