using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseSenseScroll : MonoBehaviour
{
    private PlayerMove player;
    [SerializeField] private GameObject mouseSenseSlider;
    private ActionPrototypePlayer inputActionsUI;
    private bool activeSlider = false;
    private void Start()
    {
        player = transform.root.GetComponent<PlayerMove>();
        inputActionsUI = new ActionPrototypePlayer();
        inputActionsUI.Enable();
        inputActionsUI.UI.SenseESCBuild.started += context => ActiveSlider(context);
    }
    public void ActiveSlider(InputAction.CallbackContext context)
    {
        if (activeSlider == false)
        {
            activeSlider = true;
            mouseSenseSlider.SetActive(activeSlider);
            Cursor.lockState = CursorLockMode.Confined;         
            Cursor.visible = true;
        }
        else if (activeSlider == true)
        {
            activeSlider = false;
            mouseSenseSlider.SetActive(activeSlider);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void OnValueChanged(float sense)
    {
        player.MouseSense = sense;
    }
}
