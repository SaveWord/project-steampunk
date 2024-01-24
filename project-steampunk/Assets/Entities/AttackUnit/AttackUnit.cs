using UnityEngine;
using Enemies.Bullets;

namespace Enemies.Attacks.AttackUnits
{
    public class AttackUnit : MonoBehaviour
    {
        public Transform AttackSpawnPoint;
        [SerializeField] private Bullet _bullet;

        public void Attack(ITarget target)
        {
            var bullet = Instantiate(_bullet);

            bullet.Target = target;
            bullet.transform.position = AttackSpawnPoint.position;
            bullet.StartFly(target.GetPosition());
        }
    }
}
