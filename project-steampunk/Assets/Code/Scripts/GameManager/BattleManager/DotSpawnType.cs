using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotSpawnType : MonoBehaviour
{
   public enum EnemyTypeSpawn
    {
        Ant,
        AntShield,
        Beetle,
        Bee
    }
    public EnemyTypeSpawn enemyTypeSpawn;
}
