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
        //TODO: restore later
        //_animator = GetComponentInChildren<Animator>();
    }

    private void HandleEnemyDied()
    {
        var deathparticle = Instantiate(deathParticlePrefab, transform.position, transform.rotation);
        //animation of death
       // _animator.SetBool("isDead", true);
        Destroy(deathparticle, 1f);
        GameObject.Destroy(this.gameObject);
    }

 

}
