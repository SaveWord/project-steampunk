using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class hitMarkEnemy : MonoBehaviour
{
    [SerializeField] private GameObject standardCross;
    [SerializeField] private GameObject redCross;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private LayerMask transparentLayer;
    [SerializeField] private WeaponTypeScriptableObj weaponParametrs;
    public float changeRadius;
    void Start()
    {
        standardCross.gameObject.SetActive(true);
        redCross.gameObject.SetActive(false);
    }
    void Update()
    {
        /*
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
                out RaycastHit hit, weaponParametrs.distanceAndDamages.Last().range,enemyLayer, QueryTriggerInteraction.Ignore))
        {
                redCross.gameObject.SetActive(true);
                standardCross.gameObject.SetActive(false);      
        }
        else
        {
            redCross.gameObject.SetActive(false);
            standardCross.gameObject.SetActive(true);
        }
        */

        if (Physics.SphereCast(Camera.main.transform.position, 1f, Camera.main.transform.forward,
                 out RaycastHit hit, weaponParametrs.distanceAndDamages.Last().range, enemyLayer, QueryTriggerInteraction.Ignore))
        {
           for(int i = 0; i <=weaponParametrs.distanceAndDamages.Length-1; i++)
           {
                if (weaponParametrs.distanceAndDamages[i].range > hit.distance)
                {
                    changeRadius = weaponParametrs.distanceAndDamages[i].radiusRay;
                    break;
                }
           }
        }

        var hits = Physics.SphereCastAll(Camera.main.transform.position, changeRadius, Camera.main.transform.forward, weaponParametrs.distanceAndDamages.Last().range, ~transparentLayer, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.yellow);
        if (hits.Length != 0)
        {
            var closestHit = hits[0];

            for (int i = 1; i < hits.Length; i++)
            {
                if (hits[i].distance < closestHit.distance)
                {
                    closestHit = hits[i];

                }
            }
            if (closestHit.collider.gameObject.layer == 6)
            { 
                redCross.gameObject.SetActive(true);
                standardCross.gameObject.SetActive(false);
            }
            else
            {
                redCross.gameObject.SetActive(false);
                standardCross.gameObject.SetActive(true);
            }
        }

        /*if (Physics.SphereCast(Camera.main.transform.position, changeRadius, Camera.main.transform.forward,
                out RaycastHit hit_1, weaponParametrs.distanceAndDamages.Last().range, enemyLayer, QueryTriggerInteraction.Ignore))
        {
            redCross.gameObject.SetActive(true);
            standardCross.gameObject.SetActive(false);
        }
        else
        {
            redCross.gameObject.SetActive(false);
            standardCross.gameObject.SetActive(true);
        }*/
    }
}