using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHealth : MonoBehaviour, IShield
{ 
    public void ShieldDestroy()
    {
        Destroy(gameObject);
    }
}
