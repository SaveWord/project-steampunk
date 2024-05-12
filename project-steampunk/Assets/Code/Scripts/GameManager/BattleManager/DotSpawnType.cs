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
        BeeDouble1,
        BeeDouble2,
        LaBoss
    }
    public EnemyTypeSpawn enemyTypeSpawn;
}
