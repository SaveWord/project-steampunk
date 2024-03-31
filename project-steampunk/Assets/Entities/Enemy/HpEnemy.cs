using System;
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

    //delete ListSpawner and check door
    public int _idEnemy;
    public event Action<int> DeleteList;

    private void OnTriggerEnter(Collider other)
      {
          //if (other.CompareTag("bullet")||other.CompareTag("killzone"))
          //{
          //    state = other.gameObject.GetComponent<damage_interface>().getstate();
          //    Debug.Log(state);
          //    switch (state)
          //    {
          //        case "frozen":
          //            playerPosition = transform.position;
          //            immovable = true;
          //            Debug.Log(false);
          //            break;
          //        case "jump":
          //            jumpForce = other.gameObject.GetComponent<damage_interface>().getDamage();
          //            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
          //            Debug.Log(true);
          //            break;
          //        default: break;

          //    }
          //}
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
        healDropPrefab = Resources.Load<GameObject>("HealDrop");
    }

    private void HandleEnemyDied()
    {
        var deathparticle = Instantiate(deathParticlePrefab, transform.position, transform.rotation);
        // drop the heals
        var healCount = UnityEngine.Random.Range(0, 2);
        Debug.Log("Healed num " +healCount);
        for (int i=0; i <= healCount; i++)
        {
            var position = new Vector3(transform.position.x+ UnityEngine.Random.Range(-10, 10), 0, transform.position.z+ UnityEngine.Random.Range(-10, 10));

            Instantiate(healDropPrefab, position, Quaternion.identity);
        }
        //animation of death
        _animator.SetBool("isDead", true);
        Destroy(deathparticle, 1f);
        GameObject.Destroy(this.gameObject,1f);
        DeleteList(_idEnemy);
    }
}
