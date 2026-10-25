using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

public class IsIntroEnd : MonoBehaviour
{
    public VideoPlayer mIntro;
    [SerializeField] private Material mat;
    [SerializeField] private GameObject _failedSteamWindow, spaceTipPC, spaceTipMobile, loadingObj;

    private bool loadingIn = false, loadingOut = false, allowSpace = false;
    [SerializeField] private CanvasGroup loading;

    public float TimeToFade;
    public int isLanguageChanged = 0;
    private string steam_uiLanguage;

    public LanguageLargeText largeText1, largeText2;
    public LanguageText text1, text2;

    [SerializeField] private AudioSource introSource;
    [SerializeField] private AudioSource DisclaimerSound;

    private IEnumerator Start2()
    {
        yield return null; // ждём один кадр
        Canvas.ForceUpdateCanvases();

        // Принудительная активация аудиосистемы
        AudioListener.pause = false;
        AudioListener.volume = 1f;
    }

    void Start()
    {
        StartCoroutine(Start2());
        DisclaimerSound.volume = PlayerPrefs.GetFloat("sfxVolume");
        introSource.volume = PlayerPrefs.GetFloat("sfxVolume");
        mat.SetFloat("_NoiseAmount", 0f);
        mat.SetFloat("_GlitchStrength", 0f);
        mat.SetFloat("_ScanLineStrenght", 1f);

#if !DISABLESTEAMWORKS
if (SteamManager.Initialized)
            {
                mIntro.Play();
                mIntro.loopPointReached += OnVideoEnd;
            }
            else
            {
                _failedSteamWindow.SetActive(true);
            }
#else
        mIntro.Play();
        mIntro.loopPointReached += OnVideoEnd;
#endif

        isLanguageChanged = PlayerPrefs.GetInt("languageChanged", isLanguageChanged);

        if (isLanguageChanged == 0)
        {

#if !DISABLESTEAMWORKS
if (SteamManager.Initialized)
            {
                int languageCode = GetSteamLanguageCode(); // Call the method
                if (languageCode == 0)
                {
                    // Automatic English
                    PlayerPrefs.SetInt("language", languageCode);
                }
                else if (languageCode == 1)
                {
                    PlayerPrefs.SetInt("language", languageCode);
                }
            }
#else

            SystemLanguage languageCode = Application.systemLanguage; // Получаем язык системы

            if (languageCode == SystemLanguage.English)
            {
                // Сохраняем английский
                PlayerPrefs.SetInt("language", 0);
            }
            else if (languageCode == SystemLanguage.Russian)
            {
                // Сохраняем русский
                PlayerPrefs.SetInt("language", 1);
            }
            else
            {
                // Автоматический выбор, если язык не поддерживается
                PlayerPrefs.SetInt("language", 0); // По умолчанию English
            }
#endif
        }
        largeText1.SetTextNow();
        largeText2.SetTextNow();
        text1.SetTextNow();
        text2.SetTextNow();
    }

    public void SkipMobile()
    {
        if (allowSpace == true)
        {
            spaceTipPC.SetActive(false);
            spaceTipMobile.SetActive(false);
            loadingOut = true;
            StopCoroutine(loadingSpace());
        }
    }

    private void Update()
    {
        if (allowSpace == true)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
            {
                spaceTipPC.SetActive(false);
                spaceTipMobile.SetActive(false);
                loadingOut = true;
                StopCoroutine(loadingSpace());
            }
        }
        if (loadingIn)
        {
            if (loading.alpha >= 0)
            {
                loading.alpha += TimeToFade * Time.deltaTime;
                if (loading.alpha == 1)
                {
                    loadingIn = false;
                    allowSpace = true;
#if PLAYMARKET
                    spaceTipMobile.SetActive(true);
#else
                    spaceTipPC.SetActive(true);
#endif
                    StartCoroutine(loadingSpace());
                }
            }
        }
        if (loadingOut)
        {
            if (loading.alpha != 0)
            {
                loading.alpha -= TimeToFade * Time.deltaTime;
                if (loading.alpha == 0)
                {
                    loadingOut = false;
                    SceneManager.LoadScene("Menu");
                }
            }
        }
    }
    private IEnumerator loadingSpace()
    {
        DisclaimerSound.Play();
        yield return new WaitForSeconds(10);
        spaceTipPC.SetActive(false);
        spaceTipMobile.SetActive(false);
        loadingOut = true;
        allowSpace = false;
    }

    private void OnVideoEnd(VideoPlayer player)
    {
        loadingObj.SetActive(true);
        loadingIn = true;
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
