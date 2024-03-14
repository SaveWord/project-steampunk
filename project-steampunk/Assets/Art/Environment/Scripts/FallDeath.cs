using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class FallDeath : MonoBehaviour
{
    HpHandler playerHP;
    public AudioSource source;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {       
        if(other.CompareTag("Player"))
        {
            source.Play();
            StartCoroutine(waitForSound(other));
            
        }

    }
    IEnumerator waitForSound(Collider other)
    {
        while (source.isPlaying)
        {
            yield return null;
        }
        other.GetComponent<HpHandler>().TakeDamage(1000f);
    }
}
