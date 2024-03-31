using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseSenseScroll : MonoBehaviour
{
    [SerializeField] private GameObject testMenu;
    [SerializeField] private Slider sliderSense;
    public string optionsFileName;
    private string filePath;
    private PlayerMove player;
    private ActionPrototypePlayer inputActionsUI;
    private bool activeSlider = false;
    private void OnEnable()
    {
        Time.timeScale = 1;
        player = transform.root.GetComponent<PlayerMove>();
        //inputActionsUI = new ActionPrototypePlayer();
        inputActionsUI = SingletonActionPlayer.Instance.inputActions;
        inputActionsUI.Enable();
        inputActionsUI.UICustom.SenseESCBuild.started += context => ActiveSlider(context);
        filePath = Application.dataPath + "/" + optionsFileName;
        LoadSense();
    }
    private void OnDisable()
    {
        player.MouseSense = sliderSense.value;
        SaveSense();

    }
    private void SaveSense()
    {
        string senseString = sliderSense.value.ToString();  
        File.WriteAllText(filePath, senseString);
    }
    private void LoadSense()
    {
        if (File.Exists(filePath))
        {
            string senseString = File.ReadAllText(filePath);
            sliderSense.value = float.Parse(senseString);
            player.MouseSense = float.Parse(senseString);
        }
        else
            Debug.Log("file error");
    }
    public void ActiveSlider(InputAction.CallbackContext context)
    {
        if (activeSlider == false)
        {
            inputActionsUI.Player.Disable();
            Time.timeScale = 0;
            activeSlider = true;
            testMenu.SetActive(activeSlider);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (activeSlider == true)
        {
            inputActionsUI.Player.Enable();
            Time.timeScale = 1;
            activeSlider = false;
            testMenu.SetActive(activeSlider);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void ContinueButton()
    {
        //if (activeSlider == false)
        //{
        //    Time.timeScale = 0;
           
        //    activeSlider = true;
        //    testMenu.SetActive(activeSlider);
        //    Cursor.lockState = CursorLockMode.Confined;
        //    Cursor.visible = true;
        //}
        if (activeSlider == true)
        {
            Time.timeScale = 1;
            inputActionsUI.Player.Enable();
            activeSlider = false;
            testMenu.SetActive(activeSlider);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void OnValueChanged(float sense)
    {
        player.MouseSense = sense;
    }
}
