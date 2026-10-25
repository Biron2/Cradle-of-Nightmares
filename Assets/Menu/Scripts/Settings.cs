using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif
public class Settings : MonoBehaviour
{
    [SerializeField] private AudioSource uiButtonsSnd;
    [SerializeField] private Toggle fullScreenTog;
    [SerializeField] private Transform resLeftBtn, resRightBtn;

    [SerializeField] private List<ResItem> resolutions = new List<ResItem>();
    public int selectedResolution;
    public Text resolutionLable;
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Text musicPercentageText;
    [SerializeField] private Slider soundsSlider;
    [SerializeField] private Text soundsPercentageText;
    [SerializeField] private Slider voicesSlider;
    [SerializeField] private Text voicesPercentageText;
    [SerializeField] private Button usBtn, rusBtn;
    public int language;
    public int isLanguageChanged = 0;
    private string steam_uiLanguage;

    [SerializeField] private Button arrowLeft, arrowRight;

    [SerializeField] private UnityEvent _settings, _creators, _exit, _chapters;

    public GameObject[] buttons;
    private int activeIndex = 0;
    public float activeScale = 1.2f;
    public float inactiveScale = 1.0f;
    public bool isAnimating = true;
    private bool isScrolling;

    [Header("Creators Text")]
    [SerializeField] private LanguageText[] languageTexts;

    public int onceLanguage;

    public AudioSource lampSound;
    private void Start()
    {
        UpdateButtonStates1();
        
        selectedResolution = PlayerPrefs.GetInt("resolution");
        fullScreenTog.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("fullscreen", 1));
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadData();
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
        else if(language == 1)
        {
            usBtn.interactable = true;
            rusBtn.interactable = false;
        }

        isLanguageChanged = PlayerPrefs.GetInt("languageChanged", isLanguageChanged);
        onceLanguage = PlayerPrefs.GetInt("onceLanguage", onceLanguage);
        if (isLanguageChanged == 0)
        {
#if !DISABLESTEAMWORKS
if (SteamManager.Initialized)
            {
                int languageCode = GetSteamLanguageCode(); // Call the method
                if (languageCode == 0)
                {
                    // Automatic English
                    onceLanguage = 0;
                    PlayerPrefs.SetInt("onceLanguage", onceLanguage);
                    EnglishLanguage();
                }
                else if (languageCode == 1)
                {
                    // Automatic Russian
                    onceLanguage = 1;
                    PlayerPrefs.SetInt("onceLanguage", onceLanguage);
                    RussianLanguage(); // Assuming you have a method for Russian language
                }
            }
            else
            {
                onceLanguage = 0;
                PlayerPrefs.SetInt("onceLanguage", onceLanguage);
                EnglishLanguage();
            }
#else

            SystemLanguage languageCode = Application.systemLanguage; // Получаем язык системы
            if (languageCode == SystemLanguage.English)
            {
                // Сохраняем английский
                // Automatic English
                onceLanguage = 0;
                PlayerPrefs.SetInt("onceLanguage", onceLanguage);
                EnglishLanguage();
            }
            else if (languageCode == SystemLanguage.Russian)
            {
                // Сохраняем русский
                // Automatic Rus
                onceLanguage = 1;
                PlayerPrefs.SetInt("onceLanguage", onceLanguage);
                // Automatic Russian
                RussianLanguage(); // Assuming you have a method for Russian language
            }
            else
            {
                // Автоматический выбор, если язык не поддерживается
                // Automatic English
                onceLanguage = 0;
                PlayerPrefs.SetInt("onceLanguage", onceLanguage);
                EnglishLanguage();
            }
#endif
        }

#if PLAYMARKET
        arrowLeft.gameObject.SetActive(true);
        arrowRight.gameObject.SetActive(true);

        arrowLeft.onClick.AddListener(ArrowLeftPressed);
        arrowRight.onClick.AddListener(ArrowRightPressed);
#endif
    }

    private void ArrowLeftPressed()
    {
        if (!isScrolling)
        {
            SwitchButton(false);
        }
    }
    
    private void ArrowRightPressed()
    {
        if (!isScrolling)
        {
            SwitchButton(true);
        }
    }

    private void Update()
    {
        // Проверяем, идет ли взаимодействие с Scroll View
        if (!isScrolling)
        {

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                SwitchButton(false);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                SwitchButton(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchButton(false);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchButton(true);
        }
    }

    // Метод вызывается через EventTrigger для Scroll View
    public void OnBeginScroll()
    {
        isScrolling = true;
    }

    public void OnEndScroll()
    {
        isScrolling = false;
    }
    private void SwitchButton(bool clockwise)
    {
        if (isAnimating)
            return; // Ожидание завершения текущей анимации

        isAnimating = true; // Устанавливаем флаг анимации

        // Определяем новый индекс для главной кнопки
        if (clockwise)
        {
            activeIndex = (activeIndex + 1) % buttons.Length;
        }
        else
        {
            activeIndex = (activeIndex - 1 + buttons.Length) % buttons.Length;
        }

        StartCoroutine(SwapButtons(clockwise)); // Передаем направление в корутину
    }
    private IEnumerator SwapButtons(bool clockwise)
    {
        RectTransform[] buttonTransforms = new RectTransform[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttonTransforms[i] = buttons[i].GetComponent<RectTransform>();
        }

        float duration = 0.25f;
        float elapsedTime = 0f;
        Vector3[] startPositions = new Vector3[buttons.Length];
        Vector3[] endPositions = new Vector3[buttons.Length];
        Vector3[] startScales = new Vector3[buttons.Length]; // Добавляем массив для начальных масштабов

        for (int i = 0; i < buttons.Length; i++)
        {
            startPositions[i] = buttonTransforms[i].anchoredPosition;
            startScales[i] = buttonTransforms[i].localScale; // Сохраняем начальные масштабы
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            if (clockwise)
            {
                endPositions[i] = buttonTransforms[(i - 1 + buttons.Length) % buttons.Length].anchoredPosition;
            }
            else
            {
                endPositions[i] = buttonTransforms[(i + 1) % buttons.Length].anchoredPosition;
            }
        }

        buttonTransforms[activeIndex].SetAsLastSibling();

        uiButtonsSnd.PlayOneShot(uiButtonsSnd.clip);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            for (int i = 0; i < buttons.Length; i++)
            {
                // Перемещение
                buttonTransforms[i].anchoredPosition = Vector3.Lerp(startPositions[i], endPositions[i], t);

                // Изменение размера
                if (i == activeIndex)
                {
                    buttonTransforms[i].localScale = Vector3.Lerp(startScales[i], new Vector3(activeScale, activeScale, 1), t);
                }
                else
                {
                    buttonTransforms[i].localScale = Vector3.Lerp(startScales[i], new Vector3(inactiveScale, inactiveScale, 1), t);
                }
            }

            yield return null;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttonTransforms[i].anchoredPosition = endPositions[i];
            // Установка окончательных размеров (можно удалить, если нужно плавное окончание)
            if (i == activeIndex)
            {
                buttonTransforms[i].localScale = new Vector3(activeScale, activeScale, 1);
            }
            else
            {
                buttonTransforms[i].localScale = new Vector3(inactiveScale, inactiveScale, 1);
            }
        }

        UpdateButtonStates(); // Обновляем состояния кнопок после анимации
        isAnimating = false;
    }
    private void UpdateButtonStates1()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i].GetComponent<Button>();
            if (i == activeIndex)
            {
                button.interactable = true;
                button.GetComponent<Graphic>().color = Color.white;
            }
            else
            {
                button.interactable = false;
                button.GetComponent<Graphic>().color = Color.gray;
            }
        }
    }
    private void UpdateButtonStates()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i].GetComponent<Button>();
            if (i == activeIndex)
            {
                button.interactable = true;
                button.GetComponent<Graphic>().color = Color.white;
            }
            else
            {
                button.interactable = false;
                button.GetComponent<Graphic>().color = Color.gray;
            }
        }
    }
    public void ExitButton()
    {
        _exit.Invoke();
    }
    public void SettingsButton()
    {
        isAnimating = true;
        _settings.Invoke();
    }
    public void CreatorsButton()
    {
        isAnimating = true;
        _creators.Invoke();
    }
    public void ChaptersButton()
    {
        isAnimating = true;
        _chapters.Invoke();
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
    private void LoadData()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        soundsSlider.value = PlayerPrefs.GetFloat("effectVolume");
        voicesSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSoundsVolume();
        SetVoicesVolume();
        UpdateResLable();
    }
    public void EnglishLanguage()
    {
        language = 0;
        PlayerPrefs.SetInt("language", language);
        usBtn.interactable = false;
        rusBtn.interactable = true;

        foreach (LanguageText languageText in languageTexts)
        {
            languageText.SetTextNow();
        }
    }
    public void RussianLanguage()
    {
        language = 1;
        PlayerPrefs.SetInt("language", language);
        usBtn.interactable = true;
        rusBtn.interactable = false;

        foreach (LanguageText languageText in languageTexts)
        {
            languageText.SetTextNow();
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

#if !DISABLESTEAMWORKS
    public int GetSteamLanguageCode()
    {
        steam_uiLanguage = SteamUtils.GetSteamUILanguage();
        Debug.Log(steam_uiLanguage);
        switch (steam_uiLanguage)
        {
            case "english": default: return 0;
            case "russian": return 1;
        }
    }
#endif
}

[Serializable]
public class ResItem
{
    public int horizontal, vertical;
}