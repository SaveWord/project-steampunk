using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Material material;
    private Color newColor;
   [SerializeField] private float fadeDuration = 2.0f;
    private float elapsedTime;
    public bool dissolve;
    private void Start()
    {
        material = GetComponent<Renderer>().material;
        newColor = material.color;
    }
    private void Update()
    {
        DoorOpen();
    }
    public void DoorOpen()
    {
        if (dissolve != false)
        {
           
            elapsedTime+= Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / fadeDuration);
            float currentAlpha = Mathf.Lerp(1, 0, progress);
            newColor.a = currentAlpha;
            material.color = newColor;
            if(newColor.a == 0)
                gameObject.SetActive(false);
        }
    }
    public void DoorClose()
    {
        gameObject.SetActive(true);
        Debug.Log("DoorClose");
    }
}
