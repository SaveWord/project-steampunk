using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpEnemy : MonoBehaviour
{
    private string state;
    private bool immovable;
    private Vector3 playerPosition;

    private float jumpForce;
    private Animator _animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ParticleSystem deathParticlePrefab;

      private void OnTriggerEnter(Collider other)
      {
          if (other.CompareTag("bullet")||other.CompareTag("killzone"))
          {
              state = other.gameObject.GetComponent<damage_interface>().getstate();
              Debug.Log(state);
              switch (state)
              {
                  case "frozen":
                      playerPosition = transform.position;
                      immovable = true;
                      Debug.Log(false);
                      break;
                  case "jump":
                      jumpForce = other.gameObject.GetComponent<damage_interface>().getDamage();
                      rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                      Debug.Log(true);
                      break;
                  default: break;

              }
          }
      }

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
        _animator = GetComponentInChildren<Animator>();
    }

    private void HandleEnemyDied()
    {
        var deathparticle = Instantiate(deathParticlePrefab, transform.position, transform.rotation);
        //animation of death
        _animator.SetBool("isDead", true);
        Destroy(deathparticle, 4f);
        GameObject.Destroy(this.gameObject,1f);
    }

 

}
