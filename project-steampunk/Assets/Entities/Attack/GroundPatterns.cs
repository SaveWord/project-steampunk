using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies.Attacks.Attacks;

namespace Enemies.Bullets
{
    public class GroundPatterns : AttackBaseClass
    {
        public List<GameObject> _patternPartsList;
        public List<GroundComponent> _collidersList;
        public bool Activated = false;

        //Instructions: add children parts to this object with collider somewhere in each. use flipangle to kill player

        [Header("Visuals")]
        [SerializeField]
        private Animator _bossAnimator;
        [SerializeField]
        private EnemyAudioCollection _audioSource;
        [SerializeField]
        private GameObject _bossObject;

        [Header("Parametres")]
        public float _damageTime;
        public float _chargeTime;
        public float _damage;
        [SerializeField]
        private float _flipAngle = 0f;

        private bool _setUp=false;
        private void Awake()
        {
            _bossObject = transform.parent.gameObject.transform.parent.gameObject;
            _audioSource = _bossObject.GetComponentInChildren<EnemyAudioCollection>();
            _bossObject = _bossObject.GetComponentInChildren<Boss>().gameObject;
            _bossAnimator = GameObject.FindGameObjectWithTag("animated").GetComponent<Animator>();
            SetUpAttack();
        }

        private void SetUpAttack()
        {
            //each zone
            foreach (Transform segment in gameObject.transform)
            {
                _patternPartsList.Add(segment.gameObject);

            }

            //searching for collider
            foreach (GameObject patternPart in _patternPartsList)
            {
                foreach (Transform effect in patternPart.transform)
                {
                    var meshCollider = effect.gameObject.GetComponent<MeshCollider>();
                    if (meshCollider != null)
                    {
                        effect.gameObject.AddComponent<GroundComponent>();
                        _collidersList.Add(effect.gameObject.GetComponent<GroundComponent>());
                        effect.gameObject.GetComponent<GroundComponent>()._damage = _damage;
                       // patternPart.SetActive(false);
                    }
                }
            }
            _setUp = true;
            Activated = false;
            gameObject.SetActive(false);
        }

        public override void Activate(ITarget target, Transform attackSpot)
        {
            
            //play animator
            //_audioSource.PlaySfxEnemy("EnemyGroundPattern");
            if (_setUp)
                StartCoroutine(ChargeCoroutine());
            else SetUpAttack();
        }

        private IEnumerator ChargeCoroutine()
        {
            if (!Activated)
            {
                Activated = true;
                gameObject.transform.rotation = Quaternion.Euler(0, _bossObject.transform.rotation.y - _flipAngle, 0);
                foreach (GameObject partPattern in _patternPartsList)
                   // partPattern.SetActive(true);
                _bossAnimator.SetBool("isGroundPattern", true);
                yield return new WaitForSeconds(_chargeTime);

                _bossAnimator.SetBool("isGroundPattern", false);
                foreach (GroundComponent partPattern in _collidersList)
                    partPattern.isDamaging = true;
                
                // damage dealing time frame
                yield return new WaitForSeconds(_damageTime);
                foreach (GroundComponent partPattern in _collidersList)
                {
                    partPattern.isDamaging = false;
                    //partPattern.SetActive(false);
                }
                Activated = false;
                gameObject.SetActive(false);
                //StartCoroutine(CooldownCoroutine());
        
            }
        }

        private IEnumerator CooldownCoroutine()
        {
            yield return new WaitForSeconds(_chargeTime);
           //PulseCycle();
        }
    }
}

