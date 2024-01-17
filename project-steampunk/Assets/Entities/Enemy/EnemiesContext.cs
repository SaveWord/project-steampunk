using Enemies.Attacks;
using UnityEngine;

namespace Enemies
{
    public class EnemiesContext : MonoBehaviour
    {
        public static EnemiesContext Instance;
        public Bullet Bullet;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
    }
}
