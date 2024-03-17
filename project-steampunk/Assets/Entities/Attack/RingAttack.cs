using Enemies.Bullets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Attacks.Attacks
{
    public class RingAttack :  MonoBehaviour, IAttack
    {
        public bool Activated { get; private set; }

        [SerializeField] private List<Pair<BulletSpot, Ring>> _shotQueue;

        public void Activate(ITarget target, Transform attackSpot)
        {
            Activated = true;
            StartCoroutine(MakeShots(target, attackSpot));
        }

        private void MakeShot(ITarget target, Ring bullet, BulletSpot bulletSpot, Transform attackSpot)
        {
            var projectile = Instantiate(bullet);

            projectile.Target = target;
            projectile.transform.position = attackSpot.position + bulletSpot.SpotPoint;

            if(bulletSpot.LookAtTarget) 
                projectile.StartFly(target.GetPosition() + bulletSpot.ShotDirection);
            else 
                projectile.StartFly(bulletSpot.ShotDirection);
        }

        private IEnumerator MakeShots(ITarget target, Transform attackSpot)
        {
            foreach (var shot in _shotQueue)
            {
                yield return new WaitForSeconds(shot.Key.ShotDelay);
                MakeShot(target, shot.Value, shot.Key, attackSpot);
            }
            Activated = false;
        }
    }
}

