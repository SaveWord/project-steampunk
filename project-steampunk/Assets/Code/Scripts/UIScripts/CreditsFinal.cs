using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsFinal : MonoBehaviour
{
    public static event Action CreditsFinalEnd;
    public void CreditsEndAnimInvoke()
    {
        CreditsFinalEnd?.Invoke();
    }
}
