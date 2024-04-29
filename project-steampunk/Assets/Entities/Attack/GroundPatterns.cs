using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies.Attacks.Attacks;

namespace Enemies.Bullets
{
    public class GroundPatterns : AttackBaseClass
    {
        public List<GameObject> _patternPartsList;

        //Instructions: add children colliders to this object with no render,and for each collider a child object for visual representation

        [Header("Materials")]
        [SerializeField]
        private Material _materialNone;
        [SerializeField]
        private Material _materialCharge;
        [SerializeField]
        private Material _materialDamage;

        [Header("No need")]
        public float _damageTime;
        public float _chargeTime;
        public float _damage;

        private void Awake()
        {
            foreach (Transform segment in gameObject.transform)
            {
                _patternPartsList.Add(segment.gameObject);
            }
            foreach (GameObject child in _patternPartsList)
            {
                child.AddComponent<GroundComponent>();
                child.GetComponent<GroundComponent>()._damage = _damage;
                child.SetActive(false);
            }
        }

        public void Activate(ITarget target, Transform attackSpot)
        {
            foreach (GameObject child in _patternPartsList)
            {
                child.SetActive(true);
                StartCoroutine(ChargeCoroutine(child));
            }
        }

        private IEnumerator ChargeCoroutine(GameObject gameObject)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.GetComponent<MeshRenderer>().material = _materialCharge;
            }
            yield return new WaitForSeconds(_chargeTime);


            gameObject.GetComponent<GroundComponent>().isDamaging = true;
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.GetComponent<MeshRenderer>().material = _materialDamage;
            }
           
            // damage dealing time frame
            yield return new WaitForSeconds(_damageTime);


            gameObject.GetComponent<GroundComponent>().isDamaging = false;
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.GetComponent<MeshRenderer>().material = _materialNone;
            }
            gameObject.SetActive(false);
           
            //StartCoroutine(CooldownCoroutine());
        }

        private IEnumerator CooldownCoroutine()
        {
            yield return new WaitForSeconds(_chargeTime);
           //PulseCycle();
            //SelfDestroy();
        }

        private void SelfDestroy()
        {
           // Destroy(gameObject);
        }

    }
}

