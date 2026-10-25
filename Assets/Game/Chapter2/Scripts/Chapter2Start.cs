using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Chapter2Start : MonoBehaviour
{
    [SerializeField] private GameObject canvasmainObj, canvasmainObjComics;
    [SerializeField] private CanvasGroup canvasmain;
    public float TimeToFade;
    private bool _fadeout1 = false;

    public float fadeDuration = 2f; // Продолжительность затухания
    public AudioSource audioSource; // Ссылка на компонент AudioSource

    public Text text, speaker_name;
    private string textToType1;
    public float typingSpeed = 0.05f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    [SerializeField] private GameObject TextContainer_dia;

    public Image[] faces;
    private int initialFaceIndex = 0;
    private int currentFaceIndex = 0; // Текущий индекс формы лица
    private Coroutine mouthMovementCoroutine;

    private string faustText;
    public int language;
    [SerializeField] private GameObject contunieObj;
    [SerializeField] private GameObject _PC_continue, _mobile_continue;
    [SerializeField] private GameObject pauseMenu, loadingText;

    [SerializeField] private BoxCollider2D[] buttonsCollider;

    [SerializeField] private AudioSource voice;
    [SerializeField] private AudioClip[] voiceClips;
    [SerializeField] private GameObject buttons_container;
    [SerializeField] private ActivePauseAfk _ActivePauseAfk;
    [SerializeField] private PauseScript _PauseScript;


    [Header("SlideShow")]
    [SerializeField] private CanvasGroup SlideShow;
    [SerializeField] private Image displayImage;
    [SerializeField] private Text displayText;
    [SerializeField] private Sprite[] images; // массив изображений для слайд-шоу
    [SerializeField] [TextArea] private string[] texts; // массив изображений для слайд-шоу
    [SerializeField] [TextArea] private string[] textsEn; // массив изображений для слайд-шоу
    public float totalShowTime = 30f; // общее время слайд-шоу
    private AudioSource loadingMusic;
    // Start is called before the first frame update

    private bool mobileContinuePressed = false;

    void Start()
    {
        loadingMusic = GameObject.Find("LoadingMusic").GetComponent<AudioSource>();
        UpdateDialogLang();

        // Запускаем корутину автопечати текста
        StartCoroutine(TypeText());
    }
    // Update is called once per frame
    void Update()
    {
        if (_fadeout1 == true)
        {
            if (canvasmain.alpha < 1)
            {
                canvasmainObj.SetActive(true);
                canvasmain.alpha += TimeToFade * Time.deltaTime;
                if (canvasmain.alpha == 1)
                {
                    _fadeout1 = false;
                    loadingText.SetActive(true);
                    StartCoroutine(ShowSlideshow());
                }
            }
        }
    }

    public void UpdateDialogLang()
    {
        language = PlayerPrefs.GetInt("language", language);
        if (language == 0)
        {
            faustText = "Faust";

            textToType1 = "Here I am — this road will lead me to the house where Tsutomu Miyazaki lives.";
        }
        else if (language == 1)
        {
            faustText = "Фауст";

            textToType1 = "Ну вот я и на месте — эта дорога приведёт меня к дому, где живёт Цутому Миядзаки.";
        }
    }
    private IEnumerator ShowSlideshow()
    {
        yield return new WaitForSeconds(1);
        canvasmainObjComics.SetActive(true);
        // Показываем слайдшоу до начала загрузки
        SlideShow.gameObject.SetActive(true);

        // Запускаем корутину для смены изображений
        Coroutine slideshowCoroutine = StartCoroutine(ChangeSlides());
        StartCoroutine(FadeTo(1f, 1f));
        StartCoroutine(FadeToAudio(1f, 1.5f));
        loadingMusic.Play();
        yield return new WaitForSeconds(1f);

        // Оптимизация: освобождаем память перед загрузкой
        Resources.UnloadUnusedAssets();
        GC.Collect();

        yield return new WaitForSeconds(0.5f);
        // Начинаем асинхронную загрузку сцены
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Chapter2");
        asyncLoad.allowSceneActivation = false; // Отключаем автоматическую активацию
        Application.backgroundLoadingPriority = ThreadPriority.Low;

        // Ждём завершения загрузки сцены
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                break; // Сцена готова к активации
            }

            yield return null; // Переход на следующий кадр
        }

        loadingText.SetActive(false);

        yield return new WaitForSeconds(2.5f);
        // Отображение сообщения "нажмите, чтобы продолжить"
        var text_continue = contunieObj.transform.Find("pcContinue").GetComponent<Text>();
        var text_continue2 = contunieObj.transform.Find("mobileContinue").GetComponent<Text>();
        text_continue.text = language == 0 ? "Space / RMB to start" : "Пробел / ПКМ, чтобы начать";
        text_continue2.text = language == 0 ? "Start" : "Начать";
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.SetActive(true);
        _mobile_continue.GetComponent<RectTransform>().anchoredPosition = new Vector3(-263f, 74.60001f, 0f);
#else
        _PC_continue.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _mobile_continue.SetActive(false);
        _PC_continue.SetActive(false);

        StopCoroutine(slideshowCoroutine);
        // Плавно уменьшаем прозрачность
        yield return StartCoroutine(FadeTo(0f, 1.5f));
        SlideShow.gameObject.SetActive(false);

        // Активируем сцену
        asyncLoad.allowSceneActivation = true;
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

        loadingMusic.volume = targetAlpha;
    }
    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = SlideShow.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            SlideShow.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        SlideShow.alpha = targetAlpha; // Убедитесь, что мы устанавливаем конечное значение
    }
    private IEnumerator ChangeSlides()
    {
        // Смена изображений в цикле
        while (true)
        {
            int randomIndex = UnityEngine.Random.Range(0, images.Length);
            displayImage.sprite = images[randomIndex];
            displayText.text = language == 0 ? textsEn[randomIndex] : texts[randomIndex];

            yield return new WaitForSeconds(totalShowTime); // Ожидание перед сменой следующего слайда
        }
    }

    private IEnumerator TypeText()
    {
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);

        voice.PlayOneShot(voiceClips[0]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
        foreach (char character in textToType1)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом

            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !voice.isPlaying);
        ResetFace();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.SetActive(true);
#else
        _PC_continue.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _mobile_continue.SetActive(false);
        _PC_continue.SetActive(false);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        _fadeout1 = true;
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    private IEnumerator SmoothCanvasGroup(CanvasGroup canvasGroup, float duration, float targetAlpha)
    {
        // Начальное значение alpha
        float startAlpha = canvasGroup.alpha;

        // Время, прошедшее с начала изменения
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Линейная интерполяция между стартовым и конечным значением
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            yield return null; // Ждем следующий кадр
        }

        // Убедиться, что значение alpha точно равно конечной цели
        canvasGroup.alpha = targetAlpha;
    }

    private IEnumerator mouthMovement(int i)
    {
        while (true)
        {
            yield return new WaitForSeconds(typingSpeed);
            switch (i)
            {
                case 1:
                    SetNextFace();
                    break;
            }
        }
    }
    private void SetNextFace()
    {
        faces[currentFaceIndex].gameObject.SetActive(false);

        currentFaceIndex++;
        if (currentFaceIndex >= faces.Length)
        {
            currentFaceIndex = initialFaceIndex;
        }

        faces[currentFaceIndex].gameObject.SetActive(true);
    }

    private void ResetFace()
    {
        StopCoroutine(mouthMovementCoroutine);
        SetFaceByIndex(initialFaceIndex);
    }

    private void SetFaceByIndex(int index)
    {
        foreach (Image face in faces)
        {
            face.gameObject.SetActive(false);
        }

        if (index >= 0 && index < faces.Length)
        {
            faces[index].gameObject.SetActive(true);
        }
    }

    public void MobilePress()
    {
        if (gameObject.activeSelf)
        {
            mobileContinuePressed = true;
        }
    }
}