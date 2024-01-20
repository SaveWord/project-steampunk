using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, ITarget, IDamageable
{
    public int GetTargetID()
    {
        return gameObject.GetInstanceID();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void GetDamage(float damage)
    {
        
    }

    public void Status(string state)
    {
        throw new System.NotImplementedException();
    }
}
