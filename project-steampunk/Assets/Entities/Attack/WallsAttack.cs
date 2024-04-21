using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies.Bullets;
namespace Enemies.Attacks.Attacks
{
    public class WallsAttack : AttackBaseClass
    {
       
        
        [Header("insta spawning")]
        [SerializeField] private bool makeChildren = false;
        [SerializeField] private bool destroyChildren = false;
        [SerializeField] private bool clear = false;
        [SerializeField] private float shotdelay;
        private bool lookAtTarget = false;
        [SerializeField] private List<Pair<BulletSpot, GameObject>> _shotQueue;

        [Header("attack params")]
        public float _wallDamage;
        [SerializeField] private int wallCount = 40;
        [SerializeField] private float largeRadius = 10f;
        [SerializeField] private string axis = "y";
        [SerializeField] private List<GameObject> bullets = new List<GameObject>();
        public float _projectileSpeed=1;

        [SerializeField] private float _attackTime = 10f;
        public bool Activated;
        [SerializeField] private GameObject bullet;

        void Awake()
        {
            for (int i = 0; i < wallCount; i++)
            {

                float angle = (Mathf.PI * 2 * i) / wallCount;

                float sin = Mathf.Sin(angle) * largeRadius;
                float cos = Mathf.Cos(angle) * largeRadius;
                float sin100 = Mathf.Sin(angle) * largeRadius * 100;
                float cos100 = Mathf.Cos(angle) * largeRadius * 100;

                var sp = Instantiate(Resources.Load<GameObject>("WallBullet"));
                sp.transform.parent = gameObject.transform;
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
                var SpotPoint = sp.transform.position;
                var ShotDirection = sp.transform.position - transform.position;
                sp.transform.LookAt(-ShotDirection.normalized);
                sp.transform.rotation = Quaternion.Euler(sp.transform.eulerAngles.x, sp.transform.eulerAngles.y + 90, sp.transform.eulerAngles.z);
                bullets.Add(sp);
            }
        }

        
        public override void Activate(ITarget target, Transform attackSpot)
        {
            patternSpawnPoint = attackSpot;
            
            MakeShots();
        }
        //del
        private void MakeShot(ITarget target, GameObject bullet, BulletSpot bulletSpot, Transform attackSpot)
        {
            var projectile = Instantiate(bullet);

            projectile.transform.position = attackSpot.position + bulletSpot.SpotPoint;
            projectile.transform.LookAt(bulletSpot.ShotDirection);
            bullets.Add(projectile);
        }

        private void MakeShots()
        {
            foreach(GameObject sp in bullets)
            {
                var wall = sp.GetComponent<EnergyWall>();
                wall.gameObject.SetActive(true);
                wall.Activate(_wallDamage, _projectileSpeed, _attackTime);
                
            }

            StartCoroutine(Duration());
        }

        private IEnumerator Duration()
        {

            yield return new WaitForSeconds(_attackTime);
            //gameObject.SetActive(false);
        }

        // for tuning
        void Update()
        {
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

                for (int i = 0; i < wallCount; i++)
                {

                    float angle = (Mathf.PI * 2 * i) / wallCount;

                    float sin = Mathf.Sin(angle) * largeRadius;
                    float cos = Mathf.Cos(angle) * largeRadius;
                    float sin100 = Mathf.Sin(angle) * largeRadius * 100;
                    float cos100 = Mathf.Cos(angle) * largeRadius * 100;

                    var sp = Instantiate(Resources.Load<GameObject>("WallBullet"));

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
                    _shotQueue.Add(new Pair<BulletSpot, GameObject>());
                    var SpotPoint = sp.transform.position;
                    var ShotDirection = sp.transform.position - transform.position;

                    var bulletspot = new BulletSpot();
                    bulletspot.LookAtTarget = lookAtTarget;
                    bulletspot.ShotDirection = ShotDirection.normalized;
                    sp.transform.LookAt(-ShotDirection.normalized);
                    bullets.Add(sp);
                    // sp.transform.LookAt(transform);
                    // sp.transform.LookAt(Quaternion.Inverse(sp.transform));
                    // bulletspot.ShotDirection = Quaternion.Inverse(sp.transform.rotation);
                    bulletspot.SpotPoint = SpotPoint;
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
    }

}

