using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartChapter : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasmain, canvasinterface;
    [SerializeField] private GameObject canvasmainObj;
    public float TimeToFade;
    private bool _fadein = false;
    [SerializeField] private ArrowButton arrowBtn;
    private ColliderList colliderList;
    [SerializeField] private AudioSource woman_laugh;
    public Coroutine laughtCrt;
    private AchievementsManager _AchievementsManager;

    private AudioSource loadingMusic;
    void Start()
    {
        TryInitializeLoadingMusic();

        laughtCrt = StartCoroutine(StartWomanLaught());
        Time.timeScale = 1f;
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        _fadein = true;

        try
        {
            _AchievementsManager = GameObject.Find("AchievementsManager").GetComponent<AchievementsManager>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
#if PLAYMARKET
        FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
    }

    private void TryInitializeLoadingMusic()
    {
        try
        {
            loadingMusic = GameObject.Find("LoadingMusic").GetComponent<AudioSource>();

            if (loadingMusic != null)
            {
                StartCoroutine(FadeToAudio(0f, 1.5f));
            }

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private IEnumerator StartWomanLaught()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(650, 1950));
        woman_laugh.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (_fadein == true)
        {
            if (canvasmain.alpha != 0)
            {
                canvasmain.alpha -= TimeToFade * Time.deltaTime;
                canvasinterface.alpha += TimeToFade * Time.deltaTime;
                if (canvasmain.alpha == 0 && canvasinterface.alpha == 1)
                {
                    _fadein = false;
                    canvasmainObj.SetActive(false);


                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    arrowBtn.activeArrowBtn = true;

                    Time.timeScale = 1f;
                    colliderList.ENABLE_COLLIDER();

                    if (_AchievementsManager != null)
                    {
                        if(SceneManager.GetActiveScene().name == "Chapter2")
                        {
                            _AchievementsManager.SetAchievement("achHouse");
                        }
                    }
                }
            }
        }
    }

    private IEnumerator FadeToAudio(float targetAlpha, float duration)
    {
        float startAlpha = loadingMusic.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            loadingMusic.volume = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        loadingMusic.volume = targetAlpha; // Убедитесь, что мы устанавливаем конечное значение
        Destroy(loadingMusic.gameObject);
    }
}
