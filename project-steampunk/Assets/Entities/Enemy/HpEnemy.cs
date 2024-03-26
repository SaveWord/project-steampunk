using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class HpEnemy : MonoBehaviour
{
    private string state;
    private bool immovable;
    private Vector3 playerPosition;

    private float jumpForce;
    private Animator _animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject deathParticlePrefab;
    [SerializeField] private GameObject healDropPrefab;

      private void OnTriggerStay(Collider other)
      {
          if (immovable)
          {
              transform.position = playerPosition;
          }
      } 

    private void Start()
    {
        GetComponent<IHealth>().OnDied += HandleEnemyDied;
        //_animator = GetComponentInChildren<Animator>();
        healDropPrefab = Resources.Load<GameObject>("HealDrop");

    }

    private void HandleEnemyDied()
    {
        var deathparticle = Instantiate(deathParticlePrefab, transform.position, transform.rotation);
        // drop the heals
        var healCount = Random.Range(0, 2);
        Debug.Log("Healed num " +healCount);
        for (int i=0; i <= healCount; i++)
        {
            var position = new Vector3(transform.position.x+ Random.Range(-10, 10), 0, transform.position.z+ Random.Range(-10, 10));

            Instantiate(healDropPrefab, position, Quaternion.identity);
        }
        //animation of death
       // _animator.SetBool("isDead", true);
        Destroy(deathparticle, 1f);
        GameObject.Destroy(this.gameObject);
    }

 

}
