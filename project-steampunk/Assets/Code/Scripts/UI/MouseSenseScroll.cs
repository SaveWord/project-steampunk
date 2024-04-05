using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MouseSenseScroll : MonoBehaviour
{
    [SerializeField] private GameObject testMenu;
    [SerializeField] private GameObject testMenuDie;
    [SerializeField] private Slider sliderSense;
    public string optionsFileName;
    private string filePath;
    private PlayerMove player;
    private ActionPrototypePlayer inputActionsUI;
    private bool activeSlider = false;
    private bool activeDieMenu =false;
    private void OnEnable()
    {
        Player.dieMenuEvent += ContinueDie;
        Time.timeScale = 1;
        player = transform.root.GetComponent<PlayerMove>();
        inputActionsUI = SingletonActionPlayer.Instance.inputActions;
        //inputActionsUI = new ActionPrototypePlayer();
        //inputActionsUI.Enable();
        inputActionsUI.UICustom.SenseESCBuild.started += context => ActiveSlider(context);
        filePath = Application.dataPath + "/" + optionsFileName;
        LoadSense();
    }
    private void OnDisable()
    {
        Player.dieMenuEvent -= ContinueDie;
        player.MouseSense = sliderSense.value;
        inputActionsUI.UICustom.SenseESCBuild.started -= context => ActiveSlider(context);
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
        if (activeDieMenu == false)
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
    }
    public void ContinueButton()
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
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManagerSingleton.Instance.SaveSystem.DeleteAllSave();
    }
    public void ContinueDie()
    {
        activeDieMenu = true;
        inputActionsUI.Player.Disable();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        testMenuDie.SetActive(true);
    }
    public void ControllPointLoad()
    {
        Time.timeScale = 1;
        inputActionsUI.Player.Enable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
