using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MouseSenseScroll : MonoBehaviour
{
    [SerializeField] private GameObject testMenu;
    [SerializeField] private GameObject testMenuDie;
    [SerializeField] private Slider sliderSense;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider sliderVolume;
    [SerializeField] private Slider sliderVolumeMusic;
    [SerializeField] private Slider sliderVolumeSFX;

    [Header("Video Player and Credits(CatScene)")]
    [SerializeField] private VideoPlayer video;
    [SerializeField] private GameObject screenCredits;
    [SerializeField] private GameObject rawImage;

    const string _mixerMusic = "MusicParam";
    const string _mixerSfx = "SFXParam";
    const string _mixerVolume = "VolumeParam";

    public string optionsFileName;
    private string filePath;
    private CharacterControllerMove player;
    private ActionPrototypePlayer inputActionsUI;
    private bool activeSlider = false;
    private bool activeDieMenu = false;
    private void OnEnable()
    {
        //Player.dieMenuEvent += ContinueDie;
        Time.timeScale = 1;
        player = transform.root.GetComponent<CharacterControllerMove>();
        inputActionsUI = SingletonActionPlayer.Instance.inputActions;
        //inputActionsUI = new ActionPrototypePlayer();
        //inputActionsUI.Enable();
        //inputActionsUI.UICustom.SenseESCBuild.started += context => ActiveSlider(context);
        CreateDirectoriesIfNotExist();
        filePath = Path.Combine(Application.streamingAssetsPath, optionsFileName);
        LoadSense();

        //AddListener Slider sound
        sliderVolume.onValueChanged.AddListener(OnVolumeChanged);
        sliderVolumeSFX.onValueChanged.AddListener(OnSFXChanged);
        sliderVolumeMusic.onValueChanged.AddListener(OnMusicChanged);

        if (SceneManager.GetActiveScene().buildIndex == 0) {
            video.loopPointReached += WaitNewGame;
            inputActionsUI.UI.CatSceneSkip.started += SkipVideoNewGame;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            //add ivent boss death    +=  FinalWinScene;
            HpBoss.OnBossDefeated += FinalWinScene;
            CreditsFinal.CreditsFinalEnd += LoadMainMenuAfterCredits;
            video.loopPointReached += WaitFinalCredits;
            inputActionsUI.UI.CatSceneSkip.started += SkipFinalScene;
        }


    }
    private void CreateDirectoriesIfNotExist()
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
    }
    private void Start()
    {
        mixer.SetFloat(_mixerVolume, Mathf.Log10(sliderVolume.value) * 20);
        mixer.SetFloat(_mixerMusic, Mathf.Log10(sliderVolumeMusic.value) * 20);
        mixer.SetFloat(_mixerSfx, Mathf.Log10(sliderVolumeSFX.value) * 20);

    }
    private void OnDisable()
    {
        //Player.dieMenuEvent -= ContinueDie;
        if (SceneManager.GetActiveScene().buildIndex != 0)
            player.MouseSense = sliderSense.value;
        //inputActionsUI.UICustom.SenseESCBuild.started -= context => ActiveSlider(context);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            video.loopPointReached -= WaitNewGame;
            inputActionsUI.UI.CatSceneSkip.started -= SkipVideoNewGame;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            //add ivent boss death     -= FinalWinScene
            HpBoss.OnBossDefeated -= FinalWinScene;
            CreditsFinal.CreditsFinalEnd -= LoadMainMenuAfterCredits;
            video.loopPointReached -= WaitFinalCredits;
            inputActionsUI.UI.CatSceneSkip.started -= SkipFinalScene;
        }

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
            if (SceneManager.GetActiveScene().buildIndex != 0)
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
    //new game and video frames сюжет
    public void NewGame()
    {
        video.Play();
        mixer.SetFloat("MuteParam", Mathf.Log10(1) * 20);
    }
    private void WaitNewGame(VideoPlayer source)
    {
        mixer.SetFloat("MuteParam", Mathf.Log10(0) * 20);
        SceneManager.LoadScene(1);
        GameManagerSingleton.Instance.SaveSystem.DeleteAllSave();
    }
    private void SkipVideoNewGame(InputAction.CallbackContext context)
    {
        if(video.isPlaying)
        {
            mixer.SetFloat("MuteParam", Mathf.Log10(0) * 20);
            SceneManager.LoadScene(1);
            GameManagerSingleton.Instance.SaveSystem.DeleteAllSave();
        }
    }
    // final video scene if boss die
    private void FinalWinScene()
    {
        inputActionsUI.Player.Disable();
        inputActionsUI.UICustom.Disable();
        inputActionsUI.UI.Enable();
        Time.timeScale = 0;
        video.Play();
        rawImage.SetActive(true);
        mixer.SetFloat("MuteParam", Mathf.Log10(1) * 20);
    }
    private void WaitFinalCredits(VideoPlayer source)
    {
        Time.timeScale = 1;
        screenCredits.SetActive(true); rawImage.SetActive(false);
        mixer.SetFloat("MuteParam", Mathf.Log10(0) * 20);
        AudioManager.InstanceAudio.PlayMusic("Credits", true);

    }
    public void LoadMainMenuAfterCredits()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
    private void SkipFinalScene(InputAction.CallbackContext context)
    {
        if (video.isPlaying || screenCredits.activeSelf == true)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            mixer.SetFloat("MuteParam", Mathf.Log10(0) * 20);
            SceneManager.LoadScene(0);
        }
    }



    //--------------------------------------------------------------------------------
    public void LevelLoad(int level)
    {
        //transform.root.SetParent(null);
        //GameManagerSingleton.Instance.SaveSystem.DeleteAllSave();
        SceneManager.LoadScene(level);
    }
    public void BossTP()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            transform.root.position = new Vector3(2249.6001f, 583.549988f, 286.399994f);
            Physics.SyncTransforms();
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
        mixer.SetFloat(_mixerMusic, Mathf.Log10(music) * 20);
    }
    public void OnSFXChanged(float sfx)
    {
        mixer.SetFloat(_mixerSfx, Mathf.Log10(sfx) * 20);
    }
    public void OnVolumeChanged(float sound)
    {
        mixer.SetFloat(_mixerVolume, Mathf.Log10(sound) * 20);
    }
    // main menu TestButton
    public void StartGame()
    {
        GameManagerSingleton.Instance.SaveSystem.LoadData();
        SceneManager.LoadScene(GameManagerSingleton.Instance.SaveSystem.playerData.sceneID);
    }
}
