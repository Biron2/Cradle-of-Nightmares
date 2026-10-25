using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;

public class Dialog : MonoBehaviour
{
    [SerializeField] private GameObject canvasmainObj, canvasmainObjComics;
    [SerializeField] private CanvasGroup canvasmain, canvasmainComics;
    public float TimeToFade;
    private bool _fadein1 = false, _fadeout1 = false;

    public float fadeDuration = 2f; // Продолжительность затухания
    public AudioSource audioSource; // Ссылка на компонент AudioSource

    public Text text, speaker_name;
    private string textToType1, textToType2, textToType3, textToType4, textToType5, textToType6, textToType7, textToType8, textToType9, textToType10, textToType11, textToType12, textToType13, textToType14, textToType15, textToType4Alternative;
    public float typingSpeed = 0.05f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    [SerializeField] private GameObject TextContainer_dia;

    public Image[] faces;
    public Image[] faces2;
    private int initialFaceIndex = 0;
    private int initialFaceIndex1 = 0;
    private int currentFaceIndex = 0; // Текущий индекс формы лица
    private int currentFaceIndex1 = 0; // Текущий индекс формы лица

    private string faustText, gennadiyText, unknownCharacter;
    public int language;
    public VideoPlayer mComicsVideo;
    [SerializeField] private GameObject contunieObj, skipScene, openSkipWindow,
        _PC_skip, _mobile_skip;
    [SerializeField] private RectTransform _PC_continue, _mobile_continue;
    [SerializeField] private GameObject comicsObj, loadingText;
    public Subtitles subtitles;

    [SerializeField] private GameObject pauseMenu;


    [SerializeField] private GameObject RUS_Faust_PF_SELECT, EN_Faust_PF_SELECT, black_back_PF;
    private GameObject Faust_PF;
    [SerializeField] private AudioSource showPersonalFileSnd, closePersonalFileSnd, showButtonsSnd;
    [SerializeField] private PauseScript _PauseScript;

    [SerializeField] private GameObject buttons_container;
    [SerializeField] private Text button_text1, button_text2;
    private int _Branching = 1, question_num = 1;

    [SerializeField] private GameStartGame _gSg;
    private Coroutine mouthMovementCoroutine, typeTextCoroutine;
    [SerializeField] private GameObject _RenderTexture;

    [Header("SlideShow")]
    [SerializeField] private CanvasGroup SlideShow;
    [SerializeField] private Image displayImage;
    [SerializeField] private Text displayText;
    [SerializeField] private Sprite[] images; // массив изображений для слайд-шоу
    [SerializeField] [TextArea] private string[] texts; // массив изображений для слайд-шоу
    [SerializeField] [TextArea] private string[] textsEn; // массив изображений для слайд-шоу
    public float totalShowTime = 30f; // общее время слайд-шоу
    private AudioSource loadingMusic;
    private AchievementsManager _AchievementsManager;

    private int numberOfTrueAnswers = 0; 
    private int needNumbersForAchieve = 2;
    private Timer _Timer;
    private bool skipCoroutine = false;
    [SerializeField] private BoxCollider2D[] buttonsCollider;

    private bool mobileContinuePressed = false;

    void Start()
    {
        try
        {
            _Timer = GameObject.Find("Timer").GetComponent<Timer>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        try
        {
            _AchievementsManager = GameObject.Find("AchievementsManager").GetComponent<AchievementsManager>();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
        loadingMusic = GameObject.Find("LoadingMusic").GetComponent<AudioSource>();

        UpdateDialogLang();

        initialFaceIndex = currentFaceIndex;
        initialFaceIndex1 = currentFaceIndex1;
        // Запускаем корутину автопечати текста
        typeTextCoroutine = StartCoroutine(TypeText());
    }

    public void UpdateDialogLang()
    {
        language = PlayerPrefs.GetInt("language", language);
        if (language == 0)
        {
            Faust_PF = EN_Faust_PF_SELECT;

            faustText = "Faust";
            gennadiyText = "Gennady";
            unknownCharacter = "...";

            textToType1 = "State your name, chronogent.";
            textToType2 = "Vasily. Vasily Beiner.";
            textToType3 = "Your alias and number, please.";
            textToType4 = "Faust. 21.";
            textToType4Alternative = "Heh, you've been away for a long time, Vasily. You even forgot your pseudonym.";
            textToType5 = "Thank you. It's been quite some time since your last transition. According to the instructions,\nI should inform you about a number of circumstances:";
            textToType6 = "1. The purpose of the chronogent is to discover the deviant, collect evidence and arrest his soul and bring him to justice. Activities pursuing other goals are not allowed and are severely\npunished.";
            textToType7 = "2. You can get back through the portals that will be in each location, but you'll have to solve\na series of puzzles to unlock them.";
            textToType8 = "3. Also pay attention to the clock. This clock will signal when the deviant will begin to have\na mental breakdown, a kind of mental aggravation that causes uncontrollable outbursts of\naggression. As you know, we call this state the Carousel of Madness.";
            textToType9 = "And the best thing you can do is wait it out in a shelter.";
            textToType10 = "So. I see you're ready, Faust. Let's get started!";
            textToType11 = "Yeah, you've been away for a really long time. You're the one who coined the term. I even doubt\nwhether you should be allowed to do this assignment...Well, you still have your basic skills,\nand improvisation will help you get out of all sorts of difficult situations.";
            textToType12 = "\"Carousel of Madness\" is a state when the deviant's mental disorder worsens, reaching its\nclimax, the most horrible form of insanity. At such times, everything around you is plunged into\ndarkness. And that's when your glasses can help you see through that darkness.";
            textToType13 = "Wagner. 12.";
            textToType14 = "Got it.";
            textToType15 = "Carousel of Madness?";
        }
        else if (language == 1)
        {
            Faust_PF = RUS_Faust_PF_SELECT;

            faustText = "Фауст";
            gennadiyText = "Геннадий";
            unknownCharacter = "...";

            textToType1 = "Назовите ваше имя, хроногент.";
            textToType2 = "Василий. Василий Бейнер.";
            textToType3 = "Ваш псевдоним и номер, будьте добры.";
            textToType4 = "Фауст. 21.";
            textToType4Alternative = "Хех, долго Вы отсутствовали, Василий. Даже псевдоним свой забыли.";
            textToType5 = "Благодарю. Прошло немало времени с момента вашего последнего перехода. Согласно инструкции\nя должен проинформировать Вас по ряду обстоятельств:";
            textToType6 = "1. Цель хроногента - обнаружить девианта, собрать улики и арестовать его душу и доставить для\nосуществления правосудия. Деятельность преследующая иные цели - не допустима и строго\nкарается.";
            textToType7 = "2. Вернуться обратно Вы сможете через порталы, которые будут в каждой локации, но придется\nрешить ряд головоломок, чтобы разблокировать их.";
            textToType8 = "3. Также обратите внимание на часы. Эти часы будут сигнализировать о том, когда у девианта\nначнётся помутнение сознания, своего рода психическое обострение, вызывающее неконтролируемые\nвыпады агрессии. Как вы знаете, мы называем это состояние - \"Карусель Безумия\".";
            textToType9 = "И лучшее что Вы можете сделать - это переждать в укрытии.";
            textToType10 = "Итак. Вижу Вы готовы, Фауст. Приступим!";
            textToType11 = "Да, вы действительно долго отсутствовали. Это ведь Вы и придумали этот термин.\nЯ даже сомневаюсь, стоит ли допускать Вас к этому заданию...Ну ладно, всё же основных\nнавыков Вы не утратили, а импровизация поможет выбраться из всяких сложных ситуаций.";
            textToType12 = "\"Карусель Безумия\" - это состояние, когда психическое расстройство девианта усугубляется,\nдостигая своей кульминации, самой ужасной формы безумия. В такие моменты все окружающее\nпогружается во тьму. И тогда Вам помогут ваши очки, способные видеть сквозь эту тьму.";
            textToType13 = "Вагнер. 12.";
            textToType14 = "Понял.";
            textToType15 = "Карусель Безумия?";
        }
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

    void Update()
    {
        if(skipCoroutine)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OpenSkipWindow();
            }
        }
        if (_fadein1 == true)
        {
            if (canvasmain.alpha < 1)
            {
                canvasmainObj.SetActive(true);
                canvasmain.alpha += TimeToFade * Time.deltaTime;
                if (canvasmain.alpha == 1)
                {
                    _fadein1 = false;
                    StartCoroutine(startComicsCoroutine());
                }
            }
        }
        if (_fadeout1 == true)
        {
            if (canvasmainComics.alpha < 1)
            {
                canvasmainObjComics.SetActive(true);
                canvasmainComics.alpha += TimeToFade * Time.deltaTime;
                if (canvasmainComics.alpha == 1)
                {
                    _fadeout1 = false;
                    loadingText.SetActive(true);
                    _RenderTexture.SetActive(false);
                    mComicsVideo.frame = 0;
                    StartCoroutine(ShowSlideshow());
                }
            }
        }
    }
    public void YesSkip()
    {
        StopCoroutine(typeTextCoroutine);
        TextContainer_dia.SetActive(false);
        buttons_container.SetActive(false);
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        if (_gSg.voice.isPlaying)
        {
            _gSg.voice.Stop();

        }
        _fadeout1 = true;
        StartCoroutine(FadeOut());
        openSkipWindow.SetActive(false);
        _Timer.isSkiped = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _PauseScript.offMouse = true;
        skipScene.SetActive(false);
        _PC_skip.SetActive(false);
        _mobile_skip.SetActive(false);
    }
    public void NoSkip()
    {
        foreach (BoxCollider2D colliders in buttonsCollider)
        {
            colliders.enabled = true;
        }
        openSkipWindow.SetActive(false);

        if (!buttons_container.activeSelf)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _PauseScript.offMouse = true;
        }
    }
    private IEnumerator ShowSlideshow()
    {
        yield return new WaitForSeconds(1);

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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Chapter1");
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
        _mobile_continue.gameObject.SetActive(true);
        _mobile_continue.anchoredPosition = new Vector3(-263f, 74.60001f, 0f);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);

        StopCoroutine(slideshowCoroutine);
        // Плавно уменьшаем прозрачность
        yield return StartCoroutine(FadeTo(0f, 1.5f));
        SlideShow.gameObject.SetActive(false);

        // Активируем сцену
        asyncLoad.allowSceneActivation = true;
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
    private IEnumerator TypeText()
    {
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);

        if(PlayerPrefs.GetFloat("chapter1") == 2)
        {
            skipScene.SetActive(true);
#if PLAYMARKET
            _mobile_skip.SetActive(true);
#else
            _PC_skip.SetActive(true);
#endif
            skipCoroutine = true;
        }

        _gSg.voice.PlayOneShot(_gSg.voiceClips[1]);

        text.text = "";
        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
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
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[12]);

        text.text = "";
        speaker_name.text = unknownCharacter;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
        foreach (char character in textToType2)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(1);
        showPersonalFileSnd.Play();
        black_back_PF.SetActive(true);
        Faust_PF.SetActive(true);
        _PC_continue.anchoredPosition = new Vector3(-484.1f, 74.60001f, 0f);
        _mobile_continue.anchoredPosition = new Vector3(-625f, 127f, 0f);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        _PC_continue.anchoredPosition = new Vector3(-250.77f, 74.60001f, 0f);
        _mobile_continue.anchoredPosition = new Vector3(-263f, 349.2f, 0f);
        closePersonalFileSnd.Play();
        black_back_PF.SetActive(false);
        Faust_PF.SetActive(false);
        //Есть шанс что пользователь сменит язык именно тогда когда открыты личные данные и нужно закрыть оба варианта языков - начало
        EN_Faust_PF_SELECT.SetActive(false);
        RUS_Faust_PF_SELECT.SetActive(false);
        //Есть шанс что пользователь сменит язык именно тогда когда открыты личные данные и нужно закрыть оба варианта языков - конец
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[2]);

        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in textToType3)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(1);
        question_num = 1;
        if (language == 0)
        {
            button_text1.text = "Faust. 21.";
            button_text2.text = "Wagner. 12.";
        }
        else if (language == 1)
        {
            button_text1.text = "Фауст. 21.";
            button_text2.text = "Вагнер. 12.";
        }
        ShowButtons();
    }
    private IEnumerator TypeText2()
    {
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);


        text.text = "";
        speaker_name.text = unknownCharacter;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
        if (_Branching == 1)
        {

            _gSg.voice.PlayOneShot(_gSg.voiceClips[13]);

            foreach (char character in textToType4)
            {
                // Проверяем флаг, если печать текста закончена, выходим из цикла
                if (!_isTyping)
                    yield break;

                // Добавляем новый символ к тексту
                text.text += character;

                // Ждём указанное время перед следующим символом
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        else if(_Branching == 2)
        {

            _gSg.voice.PlayOneShot(_gSg.voiceClips[14]);
            
            foreach (char character in textToType13)
            {
                // Проверяем флаг, если печать текста закончена, выходим из цикла
                if (!_isTyping)
                    yield break;

                // Добавляем новый символ к тексту
                text.text += character;

                // Ждём указанное время перед следующим символом
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        if(_Branching == 1)
        {
            StartCoroutine(TypeText2continue());
        }
        if (_Branching == 2)
        {
            StartCoroutine(TypeText2continue2());
        }
    }
    private IEnumerator TypeText2continue2()
    {

        _gSg.voice.PlayOneShot(_gSg.voiceClips[3]);

        text.text = "";
        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in textToType4Alternative)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        StartCoroutine(TypeText2continue());
    }
    private IEnumerator TypeText2continue()
    {

#if PLAYMARKET
        FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
        _gSg.voice.PlayOneShot(_gSg.voiceClips[4]);

        text.text = "";
        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in textToType5)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[5]);

        text.text = "";
        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in textToType6)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[6]);

        text.text = "";
        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in textToType7)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[7]);

        text.text = "";
        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in textToType8)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[8]);

        text.text = "";
        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in textToType9)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(1);
        question_num = 2;
        if (language == 0)
        {
            button_text1.text = "Got it.";
            button_text2.text = "Carousel of Madness?";
        }
        else if (language == 1)
        {
            button_text1.text = "Понял.";
            button_text2.text = "Карусель Безумия?";
        }
        ShowButtons();
    }

    private IEnumerator TypeText3()
    {
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[15]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
        foreach (char character in textToType14)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[9]);

        text.text = "";
        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in textToType10)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        skipCoroutine = false;
        openSkipWindow.SetActive(false);
        skipScene.SetActive(false);
        _PC_skip.SetActive(false);
        _mobile_skip.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        _fadein1 = true;
        if (numberOfTrueAnswers == needNumbersForAchieve)
        {
            if(_AchievementsManager != null)
            {
                _AchievementsManager.SetAchievement("achGennady");
            }
        }
    }

    private IEnumerator end_typing()
    {
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[16]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
        foreach (char character in textToType15)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[10]);

        text.text = "";
        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in textToType11)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[11]);

        text.text = "";
        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in textToType12)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[9]);

        text.text = "";
        speaker_name.text = gennadiyText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in textToType10)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !_gSg.voice.isPlaying);
        ResetFace2();
        skipCoroutine = false;
        skipScene.SetActive(false);
        _PC_skip.SetActive(false);
        _mobile_skip.SetActive(false);
        openSkipWindow.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed) && !openSkipWindow.activeSelf); mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        _fadein1 = true;
        if(numberOfTrueAnswers == needNumbersForAchieve)
        {
            if (_AchievementsManager != null)
            {
                _AchievementsManager.SetAchievement("achGennady");
            }
        }
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
                case 2:
                    SetNextFace2();
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
    private void SetNextFace2()
    {
        faces2[currentFaceIndex1].gameObject.SetActive(false);

        currentFaceIndex1++;
        if (currentFaceIndex1 >= faces2.Length)
        {
            currentFaceIndex1 = initialFaceIndex1;
        }

        faces2[currentFaceIndex1].gameObject.SetActive(true);
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
    private void ResetFace2()
    {
        StopCoroutine(mouthMovementCoroutine);
        SetFaceByIndex2(initialFaceIndex1);
    }
    private void SetFaceByIndex2(int index2)
    {
        foreach (Image face2 in faces2)
        {
            face2.gameObject.SetActive(false);
        }

        if (index2 >= 0 && index2 < faces2.Length)
        {
            faces2[index2].gameObject.SetActive(true);
        }
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
    private IEnumerator startComicsCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        comicsObj.SetActive(true);
        _RenderTexture.SetActive(true);
        mComicsVideo.frame = 0;
        mComicsVideo.loopPointReached += OnVideoEnd;
        StartCoroutine(subtitles.TypeText());
    }
    private void OnVideoEnd(VideoPlayer player)
    {
        _fadeout1 = true;
        StartCoroutine(FadeOut());
    }
    private void ShowButtons()
    {
        showButtonsSnd.Play();
        buttons_container.SetActive(true);
        _PauseScript.offMouse = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void HideButtons()
    {
        buttons_container.SetActive(false);
        _PauseScript.offMouse = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void AnswerButton(int btn_num)
    {
        HideButtons();
        if(question_num == 1)
        {
            if (1 == btn_num)
            {
                numberOfTrueAnswers++;
                typeTextCoroutine = StartCoroutine(TypeText2());
                _Branching = 1;
            }
            else
            {
                typeTextCoroutine = StartCoroutine(TypeText2());
                _Branching = 2;
            }
        }
        if (question_num == 2)
        {
            if (1 == btn_num)
            {
                typeTextCoroutine = StartCoroutine(TypeText3());
            }
            else
            {
                numberOfTrueAnswers++;
                typeTextCoroutine = StartCoroutine(end_typing());
            }
        }
    }

    public void MobilePress()
    {
        if (gameObject.activeSelf)
        {
            mobileContinuePressed = true;
        }
    }

    public void OpenSkipWindow()
    {
        if (skipCoroutine)
        {
            if (!openSkipWindow.activeSelf && pauseMenu.activeSelf == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                openSkipWindow.SetActive(true);
                _PauseScript.offMouse = true;
                foreach (BoxCollider2D colliders in buttonsCollider)
                {
                    colliders.enabled = false;
                }
            }
        }
    }
}