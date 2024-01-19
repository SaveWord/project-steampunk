using UnityEngine;
using Enemies.Bullets;
using Unity.VisualScripting;

namespace Enemies.Attacks.AttackUnits
{
    [CreateAssetMenu(menuName = "AttackUnits/AttackUnit")]
    public class AttackUnit : ScriptableObject
    {
        public int BlockedAttacksCount { get; private set; }
        public Transform AttackSpawnPoint;

        [SerializeField] private Bullet _bullet;

        public void Attack(ITarget target)
        {
            var bullet = Instantiate(_bullet);

            bullet.transform.position = AttackSpawnPoint.position;
            bullet.StartFly(target.GetPosition());
        }
    }
}
