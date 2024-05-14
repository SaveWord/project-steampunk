using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsFinal : MonoBehaviour
{
    public static event Action CreditsFinalEnd;
    private void OnEnable()
    {
        AudioManager.InstanceAudio.PlayMusic("Credits", true);
    }
    public void CreditsEndAnimInvoke()
    {
        CreditsFinalEnd?.Invoke();
    }
}
