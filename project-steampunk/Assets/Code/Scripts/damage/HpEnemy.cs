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
    [SerializeField] private Rigidbody rb;

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
    internal void TakeDamage(int amount)
    {
        GetComponent<IHealth>().TakeDamage(amount);
        Debug.Log(GetComponent<IHealth>().CurrentHp);
    }

}
