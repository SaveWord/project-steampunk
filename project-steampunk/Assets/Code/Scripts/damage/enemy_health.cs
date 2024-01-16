using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy_health : health_abstract
{
    private string state;
    private bool immovable;
    private Vector3 playerPosition;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enter!");
        Debug.Log(other.tag);
        if (other.CompareTag("bullet")||other.CompareTag("killzone"))
        {
            mineHP -= other.gameObject.GetComponent<damage_interface>().getDamage();
            UpdateHealth();
            Debug.Log(mineHP);
            state = other.gameObject.GetComponent<damage_interface>().getstate();
            float totalTime = other.gameObject.GetComponent<damage_interface>().gettime();
            if(totalTime > 0)
            {
                StartCoroutine(TakePeriodicDamage(totalTime, other.gameObject.GetComponent<damage_interface>().getDamage()));
            }
            switch (state)
            {
                case "frozen":
                    playerPosition = transform.position;
                    immovable = true; 
                    break;
                default: break;

            }
        }

        //if (other.CompareTag("killzone"))
        //{
        //    BlowArea killZone = other.GetComponent<BlowArea>();
        //    if (killZone.isfrozen())
        //    {
        //        initialPosition = transform.position;
        //        frozeen = true;
        //    }
        //    else
        //    {
        //        frozeen = false;
        //    }
        //    if (killZone.liftUp() > 0)
        //    {
        //        wasAgentEnabled = agent.enabled;
        //        agent.enabled = false;
        //        isJumping = true;
        //        initialPosition = transform.position;
        //        Debug.Log("lify!");
        //        jumpSpeed = killZone.liftUp();
        //        jumpHeight = jumpSpeed / 2;

        //    }
        //    if (killZone != null)
        //    {
        //        damageAmount = killZone.getDamdge();
        //    }
        //    Debug.Log(damageAmount);
        //}
    }
    public void TakeDamage(float dama)
    {
        mineHP -= dama;
        UpdateHealth();
        Debug.Log(mineHP);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("killzone"))
        {
            if (immovable)
            {
                transform.position = playerPosition;
            }
        }
    }

    private IEnumerator TakePeriodicDamage(float totalTime, float damage)
    {
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            TakeDamage(damage);
            mineHP -= damage;
            yield return new WaitForSeconds(1f);
            elapsedTime += 3f;
        }
    }
}
