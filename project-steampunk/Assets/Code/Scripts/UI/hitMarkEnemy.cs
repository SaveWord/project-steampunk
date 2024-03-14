using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class hitMarkEnemy : MonoBehaviour
{
    [SerializeField] private GameObject standardCross;
    [SerializeField] private GameObject redCross;
    [SerializeField] private LayerMask enemyLayer;
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
        if (Physics.SphereCast(Camera.main.transform.position, 2f, Camera.main.transform.forward,
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



        if (Physics.SphereCast(Camera.main.transform.position, changeRadius, Camera.main.transform.forward,
                out RaycastHit hit_1, weaponParametrs.distanceAndDamages.Last().range, enemyLayer, QueryTriggerInteraction.Ignore))
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
}