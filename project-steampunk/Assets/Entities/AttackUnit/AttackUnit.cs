using UnityEngine;

namespace Enemies.Attacks.AttackUnits
{
    [CreateAssetMenu(menuName = "AttackUnits/AttackUnit")]
    public class AttackUnit : ScriptableObject
    {
        public int BlockedAttacksCount { get; private set; }
        public Transform AttackSpawnPoint;

        [SerializeField] private Bullet _bullet;
        [SerializeField] private float _allowableBlockTime = 0.5f;

        public void Attack(ITarget target)
        {
            var bullet = Instantiate(_bullet);

            bullet.transform.position = AttackSpawnPoint.position;
            bullet.OnDestroy.AddListener(GetAttackStatistic);
            bullet.StartFly(target.GetPosition());
        }

        private void GetAttackStatistic(float flyTime)
        {
            if(flyTime < _allowableBlockTime)
                BlockedAttacksCount++;
            else
                BlockedAttacksCount = 0;
            Debug.Log("BlockedAttackCount = " + BlockedAttacksCount);
        }

    }
}
