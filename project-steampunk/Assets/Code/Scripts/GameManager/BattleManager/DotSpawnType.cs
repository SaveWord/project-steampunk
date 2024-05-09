using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotSpawnType : MonoBehaviour
{
    public enum EnemyTypeSpawn
    {
        Ant,
        AntShield,
        Spider,
        Beetle,
        BeetleShield,
        BeetleTurret,
        Bee,
        BeeVlad,
        LaBoss
    }
    public EnemyTypeSpawn enemyTypeSpawn;
}
