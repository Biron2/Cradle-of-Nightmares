using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject mPauseObj, settingsScreen, are_you_sureStartAgain, are_you_sureExit;
    private ColliderList colliderList;
    public int closeSetOrPause = 0;
    public TimeManager timeManager;
    public bool findCollders = false, offMouse = true;
    public bool BlockCollidersActive = false;

    public bool StopAmbient = false;
    [SerializeField] public AudioSource back_music;
    public bool isPauseCameraAble = false;


    private Timer _Timer;

    [Header("Settings")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Text musicPercentageText;
    [SerializeField] private Slider soundsSlider;
    [SerializeField] private Text soundsPercentageText;
    [SerializeField] private Slider voicesSlider;
    [SerializeField] private Text voicesPercentageText;
    [SerializeField] private Toggle fullScreenTog;
    [SerializeField] private Transform resLeftBtn, resRightBtn;

    [SerializeField] private List<ResItem> resolutions = new List<ResItem>();
    public int selectedResolution;
    public Text resolutionLable;
    [SerializeField] private Button usBtn, rusBtn;
    public int language;
    public int isLanguageChanged = 0;

    [Header("Creators Text")]
    [SerializeField] private LanguageText[] languageTexts;
    [SerializeField] private LanguageLargeText[] languageLargeTexts;
    [SerializeField] private UnityEvent _Events;
    [SerializeField] private Tooltip[] tooltips;

    public static PauseScript Instance;
    private void Start()
    {
        selectedResolution = PlayerPrefs.GetInt("resolution");
        fullScreenTog.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("fullscreen", 1));
        try
        {
            _Timer = GameObject.Find("Timer").GetComponent<Timer>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        if (StopAmbient == true)
        {
            try
            {
                back_music = GameObject.Find("AmbientMusic").GetComponent<AudioSource>();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        if (findCollders == true)
        {
            colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        }
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSoundsVolume();
            SetVoicesVolume();
        }
        language = PlayerPrefs.GetInt("language", language);
        if (language == 0)
        {
            usBtn.interactable = false;
            rusBtn.interactable = true;
        }
        else if (language == 1)
        {
            usBtn.interactable = true;
            rusBtn.interactable = false;
        }

        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        musicPercentageText.text = Mathf.RoundToInt(volume * 100) + "%";
    }
    public void SetSoundsVolume()
    {
        float volume = soundsSlider.value;
        myMixer.SetFloat("sounds", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("effectVolume", volume);
        soundsPercentageText.text = Mathf.RoundToInt(volume * 100) + "%";
    }
    public void SetVoicesVolume()
    {
        float volume = voicesSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
        voicesPercentageText.text = Mathf.RoundToInt(volume * 100) + "%";
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        soundsSlider.value = PlayerPrefs.GetFloat("effectVolume");
        voicesSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSoundsVolume();
        SetVoicesVolume();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && mPauseObj != null && !VariableDeterminator.stopShowPause)
        {
            UnPause();
        }
    }
    public void OpenSettings()
    {
        if (settingsScreen != null)
        {
            closeSetOrPause = 1;
            settingsScreen.SetActive(true);
        }
    }
    public void CloseSettings()
    {
        if (settingsScreen != null)
        {
            closeSetOrPause = 0;
            settingsScreen.SetActive(false);
        }
    }
    public void OpenAreYouSureStartAgain()
    {
        if (settingsScreen != null)
        {
            closeSetOrPause = 1;
            are_you_sureStartAgain.SetActive(true);
        }
    }
    public void CloseAreYouSureStartAgain()
    {
        if (settingsScreen != null)
        {
            closeSetOrPause = 0;
            are_you_sureStartAgain.SetActive(false);
        }
    }
    public void OpenAreYouSureExit()
    {
        if (settingsScreen != null)
        {
            closeSetOrPause = 1;
            are_you_sureExit.SetActive(true);
        }
    }
    public void CloseAreYouSureExit()
    {
        if (settingsScreen != null)
        {
            closeSetOrPause = 0;
            are_you_sureExit.SetActive(false);
        }
    }
    public void UnPause()
    {
        if (closeSetOrPause == 0)
        {
            if (HideInShkaf.nowInShkaf
                && SceneManager.GetActiveScene().name == "Chapter1"
                && SceneManager.GetActiveScene().name == "Chapter2")
            {
                Cursor.visible = false;
            }
            ButtonFunctions.Instance.EnableOpenSettingsMobileBtnAgain();
            AudioListener.pause = false;
            mPauseObj.SetActive(false);
            Time.timeScale = 1;
            if (offMouse == true)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (BlockCollidersActive == false)
            {
                if (findCollders == true)
                {
                    if (!VariableDeterminator.isMiniGameActive)
                    {
                        colliderList.ENABLE_COLLIDER();
                    }
                }
            }
            if (timeManager != null)
            {
                timeManager.glassesCameraAllow = true;
            }
        }
        else if (closeSetOrPause == 1)
        {
            closeSetOrPause = 0;
            if (settingsScreen != null)
            {
                if (settingsScreen.activeSelf)
                {
                    settingsScreen.SetActive(false);
                }
            }
            if (are_you_sureExit != null)
            {
                if (are_you_sureExit.activeSelf)
                {
                    are_you_sureExit.SetActive(false);
                }
            }
            if (are_you_sureStartAgain != null)
            {
                if (are_you_sureStartAgain.activeSelf)
                {
                    are_you_sureStartAgain.SetActive(false);
                }
            }
        }
    }
    public void ExitToMenu()
    {
        if(StopAmbient == true)
        {
            if(back_music != null)
            {
                Destroy(back_music.gameObject);
            }
        }
        if (_Timer != null)
        {
            Destroy(_Timer.gameObject);
        }
        AudioListener.pause = false;
        SceneManager.LoadScene("Menu");
    }
    public void StartAgainGame()
    {
        if (StopAmbient == true)
        {
            if (back_music != null)
            {
                Destroy(back_music.gameObject);
            }
        }
        if (_Timer != null)
        {
            Destroy(_Timer.gameObject);
        }
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void StartAgainChapter()
    {
        if (StopAmbient == true)
        {
            if (back_music != null)
            {
                Destroy(back_music.gameObject);
            }
        }
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void StartAgainChapterEnd()
    {
        if (StopAmbient == true)
        {
            if (back_music != null)
            {
                Destroy(back_music.gameObject);
            }
        }
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void EnglishLanguage()
    {
        language = 0;
        PlayerPrefs.SetInt("language", language);
        usBtn.interactable = false;
        rusBtn.interactable = true;

        if (languageTexts != null && languageTexts.Length > 0)
        {
            foreach (LanguageText languageText in languageTexts)
            {
                languageText.SetTextNow();
            }
        }

        if (languageLargeTexts != null && languageLargeTexts.Length > 0)
        {
            foreach (LanguageLargeText languageLText in languageLargeTexts)
            {
                languageLText.SetTextNow();
            }
        }

        if (tooltips != null && tooltips.Length > 0)
        {
            foreach (Tooltip _tooltips in tooltips)
            {
                _tooltips.langSwitch();
            }
        }

        if (_Events != null)
        {
            _Events.Invoke();
        }
    }
    public void RussianLanguage()
    {
        language = 1;
        PlayerPrefs.SetInt("language", language);
        usBtn.interactable = true;
        rusBtn.interactable = false;

        if (languageTexts != null && languageTexts.Length > 0)
        {
            foreach (LanguageText languageText in languageTexts)
            {
                languageText.SetTextNow();
            }
        }

        if (languageLargeTexts != null && languageLargeTexts.Length > 0)
        {
            foreach (LanguageLargeText languageLText in languageLargeTexts)
            {
                languageLText.SetTextNow();
            }
        }

        if (tooltips != null && tooltips.Length > 0)
        {
            foreach (Tooltip _tooltips in tooltips)
            {
                _tooltips.langSwitch();
            }
        }

        if (_Events != null)
        {
            _Events.Invoke();
        }
    }
    public void ChangedLanguageByUser()
    {
        isLanguageChanged = 1;
        PlayerPrefs.SetInt("languageChanged", isLanguageChanged);
    }
    public void ResLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        UpdateResLable();
    }
    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }
        UpdateResLable();
    }
    public void UpdateResLable()
    {
        PlayerPrefs.SetInt("resolution", selectedResolution);
        PlayerPrefs.SetInt("fullscreen", Convert.ToInt32(fullScreenTog.isOn));
        resolutionLable.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullScreenTog.isOn);
    }
}
