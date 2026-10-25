using System;
using UnityEngine;
using UnityEngine.UI;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

public class DLC_Manager : MonoBehaviour
{
    [SerializeField] private GameObject chapter1, chapter2;
    [SerializeField] private GameObject playSoundActivate;
    [SerializeField] private GameStart _GameStart;
    [SerializeField] private Fade[] _Fades;

#if !DISABLESTEAMWORKS
    private Callback<DlcInstalled_t> dlcInstalledCallback;
#endif
    void Awake()
    {
#if !DISABLESTEAMWORKS
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam не инициализирован!");
            return;
        }
#endif
    }

    void OnEnable()
    {
        updateButtonsState();
#if !DISABLESTEAMWORKS
        dlcInstalledCallback = Callback<DlcInstalled_t>.Create(OnDlcInstalled);
#endif
    }
    private void OnDisable()
    {
#if !DISABLESTEAMWORKS
        dlcInstalledCallback?.Unregister();
#endif
    }

    private void updateButtonsState()
    {
        try
        {
            if (PlayerPrefs.HasKey("chapter1Time"))
            {
                chapter1.transform.Find("Completed").gameObject.SetActive(true);
                chapter1.transform.Find("Completed/completed_time").GetComponent<Text>().text = GetFormattedTime(PlayerPrefs.GetInt("chapter1Time"));
            }
            if (PlayerPrefs.HasKey("chapter2Time"))
            {
                chapter2.transform.Find("Completed").gameObject.SetActive(true);
                chapter2.transform.Find("Completed/completed_time").GetComponent<Text>().text = GetFormattedTime(PlayerPrefs.GetInt("chapter2Time"));
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

#if !DISABLESTEAMWORKS
        if (SteamApps.BIsDlcInstalled(new AppId_t(3534120)))
        {
            chapter2.transform.Find("Blocked").gameObject.SetActive(false);

            if (!PlayerPrefs.HasKey("chapter1Time"))
            {
                chapter2.transform.Find("No_reach").gameObject.SetActive(true);
            }
        }
        else
        {
            chapter2.transform.Find("Blocked").gameObject.SetActive(true);
        }
#else
        chapter2.transform.Find("Blocked").gameObject.SetActive(false);
        if (!PlayerPrefs.HasKey("chapter1Time"))
            {
                chapter2.transform.Find("No_reach").gameObject.SetActive(true);
            }
#endif
    }

    private string GetFormattedTime(int i)
    {
        int hours = Mathf.FloorToInt(i / 3600f);
        int minutes = Mathf.FloorToInt((i % 3600) / 60f);
        int seconds = Mathf.FloorToInt(i % 60f);
        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }

    public void activateChapter1()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playSoundActivate.SetActive(true);
        _GameStart.StartGameScene(1);

        foreach (Fade fades in _Fades)
        {
            fades.allowEsc = false;
        }
    }

    public void activateChapter2()
    {
#if !DISABLESTEAMWORKS
        if(SteamApps.BIsDlcInstalled(new AppId_t(3534120)))
        {
            if (PlayerPrefs.HasKey("chapter1Time"))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                playSoundActivate.SetActive(true);
                _GameStart.StartGameScene(2);

                foreach (Fade fades in _Fades)
                {
                    fades.allowEsc = false;
                }
            }
        }
        else
        {
            SteamFriends.ActivateGameOverlayToStore(new AppId_t(3534120), EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
        }
#else
        if (PlayerPrefs.HasKey("chapter1Time"))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                playSoundActivate.SetActive(true);
                _GameStart.StartGameScene(2);

                foreach (Fade fades in _Fades)
                {
                    fades.allowEsc = false;
                }
            }
#endif
    }
#if !DISABLESTEAMWORKS

    private void OnDlcInstalled(DlcInstalled_t callback)
    {
        if (callback.m_nAppID == new AppId_t(3534120))
        {
            Debug.Log("DLC Chapter 2 куплен и установлен!");
            updateButtonsState();
        }
    }
#endif
}
