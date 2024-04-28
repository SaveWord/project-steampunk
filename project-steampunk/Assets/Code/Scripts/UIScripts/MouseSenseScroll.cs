using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MouseSenseScroll : MonoBehaviour
{
    [SerializeField] private GameObject testMenu;
    [SerializeField] private GameObject testMenuDie;
    [SerializeField] private Slider sliderSense;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider sliderVolume;
    [SerializeField] private Slider sliderVolumeMusic;
    [SerializeField] private Slider sliderVolumeSFX;

    const string _mixerMusic = "MusicParam";
    const string _mixerSfx = "SFXParam";
    const string _mixerVolume = "VolumeParam";

    public string optionsFileName;
    private string filePath;
    private CharacterControllerMove player;
    private ActionPrototypePlayer inputActionsUI;
    private bool activeSlider = false;
    private bool activeDieMenu =false;
    private void OnEnable()
    {
        Player.dieMenuEvent += ContinueDie;
        Time.timeScale = 1;
        player = transform.root.GetComponent<CharacterControllerMove>();
        inputActionsUI = SingletonActionPlayer.Instance.inputActions;
        //inputActionsUI = new ActionPrototypePlayer();
        //inputActionsUI.Enable();
        inputActionsUI.UICustom.SenseESCBuild.started += context => ActiveSlider(context);
        filePath = Application.dataPath + "/" + optionsFileName;
        LoadSense();

        //AddListener Slider sound
        sliderVolume.onValueChanged.AddListener(OnVolumeChanged);
        sliderVolumeSFX.onValueChanged.AddListener(OnSFXChanged);
        sliderVolumeMusic.onValueChanged.AddListener(OnMusicChanged);

    }
    private void Start()
    {
        mixer.SetFloat(_mixerVolume, Mathf.Log10(sliderVolume.value) * 20);
        mixer.SetFloat(_mixerMusic, Mathf.Log10(sliderVolumeMusic.value) * 20);
        mixer.SetFloat(_mixerSfx, Mathf.Log10(sliderVolumeSFX.value) * 20);

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
        string sliderString = $"{sliderSense.value} {sliderVolume.value} {sliderVolumeMusic.value} {sliderVolumeSFX.value}";
        File.WriteAllText(filePath, sliderString);
    }
    private void LoadSense()
    {
        if (File.Exists(filePath))
        {
            string sliderString = File.ReadAllText(filePath);
            string[] values = sliderString.Split(' ');
            sliderSense.value = float.Parse(values[0]);
            player.MouseSense = float.Parse(values[0]);


            sliderVolume.value = float.Parse(values[1]);
     
            sliderVolumeMusic.value = float.Parse(values[2]);

            sliderVolumeSFX.value = float.Parse(values[3]);

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
    public void LevelLoad(int level)
    {
        GameManagerSingleton.Instance.SaveSystem.DeleteAllSave();
        SceneManager.LoadSceneAsync(level);
    }
    public void BossTP()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            transform.root.position = new Vector3(2249.6001f, 583.549988f, 286.399994f);
        }
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
    public void OnMusicChanged(float music)
    {
        mixer.SetFloat(_mixerMusic,Mathf.Log10(music)*20);
    }
    public void OnSFXChanged(float sfx)
    {
        mixer.SetFloat(_mixerSfx, Mathf.Log10(sfx) * 20);
    }
    public void OnVolumeChanged(float sound)
    {
        mixer.SetFloat(_mixerVolume, Mathf.Log10(sound) * 20);
    }
}
