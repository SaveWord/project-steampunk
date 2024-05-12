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

        private Color groundColor;

        [SerializeField]
        [Range (0f,1f)]
        private float _transparency;

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
            gameObject.SetActive(false);
        }

        public override void Activate(ITarget target, Transform attackSpot)
        {
            foreach (GameObject child in _patternPartsList)
            {
                child.SetActive(true);
                StartCoroutine(ChargeCoroutine(child));
            }
        }

        private IEnumerator ChargeCoroutine(GameObject gameObject)
        {

            gameObject.GetComponent<MeshRenderer>().materials[0] = _materialCharge;
            groundColor = gameObject.GetComponent<MeshRenderer>().materials[1].color;
            groundColor.a = 0f;
            gameObject.GetComponent<MeshRenderer>().materials[1].color = groundColor;
            yield return new WaitForSeconds(_chargeTime/5);
            groundColor.a = 0.2f;
            gameObject.GetComponent<MeshRenderer>().materials[1].color = groundColor;

            yield return new WaitForSeconds(_chargeTime / 5);
            groundColor.a = 0.4f;
            gameObject.GetComponent<MeshRenderer>().materials[1].color = groundColor;

            yield return new WaitForSeconds(_chargeTime / 5);
            groundColor.a = 0.6f;
            gameObject.GetComponent<MeshRenderer>().materials[1].color = groundColor;

            yield return new WaitForSeconds(_chargeTime / 5);
            groundColor.a = 0.8f;
            gameObject.GetComponent<MeshRenderer>().materials[1].color = groundColor;

            yield return new WaitForSeconds(_chargeTime / 5);
            groundColor.a = 1f;
            gameObject.GetComponent<MeshRenderer>().materials[1].color = groundColor;



            gameObject.GetComponent<GroundComponent>().isDamaging = true;

            gameObject.GetComponent<MeshRenderer>().materials[0] = _materialDamage;
            
            // damage dealing time frame
            yield return new WaitForSeconds(_damageTime);

            gameObject.GetComponent<GroundComponent>().isDamaging = false;

            gameObject.GetComponent<MeshRenderer>().materials[0] = _materialNone;
            
            gameObject.SetActive(false);
           
            //StartCoroutine(CooldownCoroutine());
        }

        private IEnumerator CooldownCoroutine()
        {
            yield return new WaitForSeconds(_chargeTime);
           //PulseCycle();
        }
    }
}

