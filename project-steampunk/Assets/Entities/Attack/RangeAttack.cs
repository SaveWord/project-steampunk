using Enemies.Bullets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Attacks.Attacks
{
    public class RangeAttack : AttackBaseClass //rename bullet attack
    {
        public bool Activated { get; set; }

        [SerializeField] private List<Pair<BulletSpot, Bullet>> _shotQueue;
        [SerializeField] private Bullet bullet;
        [SerializeField] private bool makeChildren = false;
        [SerializeField] private bool destroyChildren = false;
        [SerializeField] private bool clear = false;
        [SerializeField] private int sphereCount = 40;
        [SerializeField] private float shotdelay;
        [SerializeField] private float largeRadius = 10f;
        [SerializeField] private float smallRadius = 3f;
        [SerializeField] private string axis="y";
        private bool instanciated = false;
        public List<Bullet> _listOfAllBullets = new List<Bullet>();

        [SerializeField] private EnemyAudioCollection _audioSource;
        
        void Update()
        {
            //this is a script to spawn circles of bullets in editor (makeChildren always false)
            if (makeChildren)
            {
                if (destroyChildren)
                {   
                    var count = 0;
                    foreach (Transform child in gameObject.transform)
                    {
                        Destroy(child.gameObject);
                        count++;
                    }
                }
                if (clear)
                    {
                        _shotQueue.Clear();
                        _shotQueue.TrimExcess();
                    }

                for (int i = 0; i < sphereCount; i++)
                {
                    float angle = (Mathf.PI * 2 * i) / sphereCount;

                    float sin = Mathf.Sin(angle) * largeRadius;
                    float cos = Mathf.Cos(angle) * largeRadius;
                    float sin100 = Mathf.Sin(angle) * largeRadius*100;
                    float cos100 = Mathf.Cos(angle) * largeRadius*100;

                    var sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sp.transform.parent = gameObject.transform;
                    sp.transform.localScale = new Vector3(smallRadius, smallRadius, smallRadius);
                    Vector3 targetPos = new Vector3(0,0,0);

                    if (axis == "z")
                    {
                        sp.transform.position = new Vector3(sin, cos, 0);
                       // targetPos = new Vector3(sin100, cos100, 0);
                    }
                    else if (axis == "x")
                    {
                        // around the X axis
                        sp.transform.position = new Vector3(0, sin, cos);
                      //  targetPos = new Vector3(0, sin100, cos100);
                    }
                    else if (axis == "y")
                    // around the Y axis
                    {
                        sp.transform.position = new Vector3(sin, 0, cos);
                       // targetPos = new Vector3(sin100, 0, cos100);
                    }
                    _shotQueue.Add(new Pair<BulletSpot, Bullet>());
                    var SpotPoint = sp.transform.position;
                    var ShotDirection = sp.transform.forward;

                    var bulletspot = new BulletSpot(); bulletspot.LookAtTarget = true; 
                    
                    bulletspot.SpotPoint=SpotPoint;
                    bulletspot.ShotDelay = shotdelay;
                    //bulletspot.ShotDirection=ShotDirection;
                    _shotQueue[i].Key = bulletspot;
                    _shotQueue[i].Value = bullet;
                    if (destroyChildren)
                         Destroy(sp);
                }
                makeChildren = false;
            }
        }

        public override void Activate(ITarget target, Transform attackSpot)
        {
           
            patternSpawnPoint = attackSpot;
            Activated = true;
            StartCoroutine(MakeShots(target, attackSpot));
        }

        private void MakeShot(ITarget target, Bullet bullet, BulletSpot bulletSpot, Transform attackSpot, int bulletIndex)
        {
            var attackBullet = _listOfAllBullets[bulletIndex];
            attackBullet.gameObject.SetActive(false);
            attackBullet.Target = target;
            attackBullet.transform.position = attackSpot.position + bulletSpot.SpotPoint;
            attackBullet.gameObject.SetActive(true);
            
            if (bulletSpot.LookAtTarget)
                attackBullet.StartFly(target.GetPosition() + bulletSpot.ShotDirection);
            else
                attackBullet.StartFly(bulletSpot.ShotDirection);
        }

        private IEnumerator MakeShots(ITarget target, Transform attackSpot)
        {
            if (!instanciated)
            {
                instanciated = true;
                _audioSource = transform.parent.gameObject.transform.parent.GetComponentInChildren<EnemyAudioCollection>();
                foreach (var shot in _shotQueue)
                {
                    var bulletSpawned = Instantiate(shot.Value);
                    _listOfAllBullets.Add(bulletSpawned);
                }
            }

            _audioSource.PlaySfxEnemy("EnemyAttackBullet");
            foreach (var shot in _shotQueue)
            {
                yield return new WaitForSeconds(shot.Key.ShotDelay);
                transform.LookAt(shot.Key.ShotDirection);
                MakeShot(target, shot.Value, shot.Key, attackSpot, _shotQueue.IndexOf(shot));
            }
            Activated = false;
        }
    }
}

