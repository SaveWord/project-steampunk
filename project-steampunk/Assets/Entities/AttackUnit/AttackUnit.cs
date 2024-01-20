using UnityEngine;
using Enemies.Bullets;

namespace Enemies.Attacks.AttackUnits
{
    [CreateAssetMenu(menuName = "AttackUnits/AttackUnit")]
    public class AttackUnit : ScriptableObject
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
