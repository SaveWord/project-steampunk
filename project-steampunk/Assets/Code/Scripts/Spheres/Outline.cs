using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField] private GameObject spheres;
    void Start()
    {
        spheres.SetActive(false);
    }

    // Update is called once per frame
    public void Turnoff()
    {
        spheres.SetActive(false);
    }
    public void Turnon()
    {
        spheres.SetActive(true);
    }
}
