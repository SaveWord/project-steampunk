using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, ITarget
{
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public int GetTargetID()
    {
        return gameObject.GetInstanceID();
    }
}
