using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitMarkEnemy : MonoBehaviour
{
    [SerializeField] private GameObject standardCross;
    [SerializeField] private GameObject redCross;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private WeaponTypeScriptableObj weaponParametrs;

    // Start is called before the first frame update
    void Start()
    {
        standardCross.gameObject.SetActive(true);
        redCross.gameObject.SetActive(false);
    }
    void Update()
    {
        //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
        //        out RaycastHit hit,weaponParametrs.range,enemyLayer, QueryTriggerInteraction.Ignore))
        //{
        //        redCross.gameObject.SetActive(true);
        //        standardCross.gameObject.SetActive(false);      
        //}
        //else
        //{
        //    redCross.gameObject.SetActive(false);
        //    standardCross.gameObject.SetActive(true);
        //}
        standardCross.gameObject.SetActive(true);
    }
}