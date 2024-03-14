using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors0Event : MonoBehaviour
{
    void stopAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}
