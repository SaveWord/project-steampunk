using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] GameObject ArenaArea;
    [SerializeField] GameObject Platform;
    [SerializeField] float speed;
    Animator animator;
    bool open;
    private void Awake()
    {
        animator = Platform.GetComponent<Animator>();
        animator.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player")&&!open)
        {
            open = true;
            //foreach (GameObject enemy in ArenaArea.GetComponent<CheckForEnemies>().enemiesInArena)
            //{
            //    if (!enemy.IsDestroyed())
            //        open = false;

            //}
            if (open)
            {
                animator.enabled = true;
                animator.Play("Platform0", 0);
                animator.speed = speed;
                Destroy(ArenaArea);
            }
        }
    }
}
