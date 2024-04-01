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
    [SerializeField] private Slider sliderSense;
    [SerializeField] private Image cheatsImage;
    public string optionsFileName;
    private string filePath;
    private PlayerMove player;
    private HpHandler playerHpHandler;
    private GameObject playerObject;
    private ActionPrototypePlayer inputActionsUI;
    private bool activeSlider = false;
    private void OnEnable()
    {
        Time.timeScale = 1;
        player = transform.root.GetComponent<PlayerMove>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerHpHandler = playerObject.GetComponent<HpHandler>();
        //inputActionsUI = SingletonActionPlayer.Instance.inputActions;
        inputActionsUI = new ActionPrototypePlayer();
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
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManagerSingleton.Instance.SaveSystem.DeleteAllSave();
    }
    public void Cheats()
    {
        playerHpHandler._invulnerable = !playerHpHandler._invulnerable;
        if(playerHpHandler._invulnerable)
            cheatsImage.color = new Color32(0, 255, 0, 255);
        else
            cheatsImage.color = new Color32(255, 0, 0, 255);

    }
    public void ToBoss()
    {
        playerObject.transform.position = new Vector3(2564.40015f, 560.868958f, 121.196419f);
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
