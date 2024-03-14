using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Doors0Open : MonoBehaviour
{
    [SerializeField] GameObject ArenaArea;
    [SerializeField] GameObject Doors;
    [SerializeField] float speed;
    Animator animator;
    bool open;
    private void Awake()
    {
        animator = Doors.GetComponent<Animator>();
        animator.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&!open)
        {
            open = true;
            foreach (GameObject enemy in ArenaArea.GetComponent<CheckForEnemies>().enemiesInArena)
            {
                if (!enemy.IsDestroyed())
                    open = false;

            }
            if(open)
            {
                animator.enabled = true;
                animator.Play("Doors0Open",0);
                animator.speed = speed;
                Destroy(ArenaArea);
            }
        }
    }
}
