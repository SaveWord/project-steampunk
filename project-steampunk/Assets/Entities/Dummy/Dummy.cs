using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IAttackTarget
{
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
