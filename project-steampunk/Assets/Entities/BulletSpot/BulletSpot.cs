using System;
using UnityEngine;

[Serializable]
public class BulletSpot
{
    public Vector3 SpotPoint;
    public Vector3 ShotDirection;
    public float ShotDelay;
    public bool LookAtTarget = true;  
}
