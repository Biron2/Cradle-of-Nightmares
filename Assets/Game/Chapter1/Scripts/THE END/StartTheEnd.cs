using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartTheEnd : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasmain, Chapter1End;
    [SerializeField] private GameObject canvasmainObj, Chapter1EndObj;

    private bool _fadein = false, _fadeout = false, _fadeout1, _fadein1 = false, allowContinueDialog = false;
    private bool _fadeinEnd = false, _fadeoutEnd = false, _fadeinEnd1 = false;
    public float TimeToFade, TimeToFade2;

    private Camera camera1;
    public float targetSize = 10.0f;
    public float targetSize2 = 5;
    public float targetSize3 = 2.5f;
    public float transitionDuration = 1.0f;
    public float transitionDuration2 = 3.0f;
    public float transitionDuration3 = 5.0f;
    public float transitionDuration4 = 2.5f;
    public float transitionDuration5 = 3f;

    private float initialSize;
    private Vector3 initialPosition;
    public float transitionStartTime;
    private Vector3 targetPos = new Vector3(1.26f, 0f, 0f);
    private Vector3 targetPos2 = new Vector3(32.9f, 0f, 0f);
    private Vector3 targetPos3 = new Vector3(-32.9f, 0f, 0f);
    private Vector3 targetPos4 = new Vector3(-24.77f, 0f, 0f);
    public int language;

    public Image[] faces;
    public Image[] faces2;
    public Image[] faces3Nosferatu;
    private int initialFaceIndex = 0;
    private int initialFaceIndex1 = 0;
    private int initialFaceIndex2 = 0;
    private int currentFaceIndex = 0; // Текущий индекс формы лица
    private int currentFaceIndex1 = 0; // Текущий индекс формы лица
    private int currentFaceIndex2 = 0; // Текущий индекс формы лица

    [SerializeField] private GameObject contunieObj, skipScene, _mobile_Skip, _PC_Skip, openSkipWindow;

    public Text text, speaker_name;
    private string faustText, GellaText, NosferatuText;

    private string NtextToType1, NtextToType2, NtextToType3, NtextToType4, NtextToType5, NtextToType6, NtextToType7, NtextToType8, NtextToType9, NtextToType10, NtextToType11;
    private string textToType1, textToType2, textToType3, textToType4, textToType5, textToType6, textToType7, textToType8, textToType9, textToType10, textToType11, textToType12;

    public float typingSpeed = 0.05f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    [SerializeField] private GameObject TextContainer_dia, endBackground;

    [SerializeField] private RectTransform _PC_continue, _mobile_continue;
    [SerializeField] private GameObject RUS_Gella_PF_SELECT, EN_Gella_PF_SELECT, RUS_Nosferatu_PF_SELECT, EN_Nosferatu_PF_SELECT, EN_Chronotron_Subject_SELECT, RUS_Chronotron_Subject_SELECT, black_back_PF;
    private GameObject Gella_PF, Nosferatu_PF, Chronotron_Subject;
    [SerializeField] private RectTransform faustTransform, gellaTransform;
    [SerializeField] private Pulse faustPulse, gellaPulse;

    public float fadeDuration = 1f; // Продолжительность затухания
    [SerializeField] private AudioSource campfireSnd, back_music, ChapterEndSnd, extraAmbient;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private PauseScript _PauseScript;
    [SerializeField] private AudioSource showButtonsSnd, showPersonalFileSnd, closePersonalFileSnd;
    [SerializeField] private GameObject buttons_container;
    [SerializeField] private Text button_text1, button_text2;
    private int question_num = 1;
    [SerializeField] private BlinkingEyes _BlinkingEyes1, _BlinkingEyes2;
    private bool startBool2WithoutFew = false;

    [SerializeField] private AudioSource voice;
    [SerializeField] private AudioClip[] voiceClips;
    private Coroutine mouthMovementCoroutine, typeTextCoroutine;
    private AchievementsManager _AchievementsManager;
    private int numberOfTrueAnswers = 0;
    private int needNumbersForAchieve = 1;
    private LeaderboardRecorder _LeaderboardRecorder;
    private Timer _Timer;

    [SerializeField] private CanvasGroup DialogGroup;

    private bool skipCoroutine = false;
    [SerializeField] private BoxCollider2D[] buttonsCollider;

    private bool mobileContinuePressed = false;

    private void Start()
    {
        try
        {
            _Timer = GameObject.Find("Timer").GetComponent<Timer>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        _LeaderboardRecorder = GetComponent<LeaderboardRecorder>();
        try
        {
            _AchievementsManager = GameObject.Find("AchievementsManager").GetComponent<AchievementsManager>();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
        Time.timeScale = 1f;
        camera1 = GetComponent<Camera>();
        _fadein = true;
        StartCoroutine(FadeInCampfire());

        initialSize = camera1.orthographicSize;
        initialPosition = transform.position;

        try
        {
            back_music = GameObject.Find("AmbientMusic").GetComponent<AudioSource>();
        }
        catch(Exception e)
        {
            Debug.Log(e);

            back_music = extraAmbient;
            //Включение запасной музыки если та не включилась на основной локации
            extraAmbient.gameObject.SetActive(true);
        }

        UpdateDialogLang();

        text.text = "";

        initialFaceIndex = currentFaceIndex;
        initialFaceIndex1 = currentFaceIndex1;
        initialFaceIndex2 = currentFaceIndex2;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void UpdateDialogLang()
    {
        language = PlayerPrefs.GetInt("language", language);
        if (language == 0)
        {
            Gella_PF = EN_Gella_PF_SELECT;
            Nosferatu_PF = EN_Nosferatu_PF_SELECT;
            Chronotron_Subject = EN_Chronotron_Subject_SELECT;
            faustText = "Faust";
            GellaText = "Gella";
            NosferatuText = "Nosferatu";

            NtextToType1 = "...Masked killers? We call them the magothas. They showed up during your vacation and made a lot of noise.";
            NtextToType9 = "Well, actually, magothas are called Slipknot fans.\nOne chronogent who's encountered them says that during the murders,\nthey play Slipknot songs and slowly and painfully kill their victims.";
            NtextToType10 = "Why are they called Magoths?";
            NtextToType11 = "Magoths...This is the problem we've been missing!";
            NtextToType2 = "Where did these magoths come from?\nFrom the way they're dressed, they look like they're from our time.";
            NtextToType3 = "That's probably what it is.";
            NtextToType4 = "But how could they have gotten into Irreality?\nNot through our Chronotron?";
            NtextToType5 = "Well, Faust, maybe we're not the only ones with talent. And someone else has come up with\nthe same \"machine\". Someone who wants to use the portal not to restore justice and save people,\nbut to kill and destroy, for which he will not be held responsible.";
            NtextToType6 = "Someone who's been rejected and disliked by everyone. and now that \"someone\" has gone mad,\ngathered a gang of similarly embittered loser incels, and together they are now traveling\nthrough time to slice and dice, heh-heh.";
            NtextToType7 = "Killing people from the past and coming back is a maniac's dream...\nYeah, I say scary things, heh-heh.\nBut it's my job to collect scary stories and then tell them to people.";
            NtextToType8 = "...And you know, these magoths are weird, too. They've never touched an innocent.\nAnd all the people they kill are the same maniacs we're after,\nbut with different methods and for different purposes. So there you go.";

            textToType1 = "Faust!?";
            textToType2 = "Gella!";
            textToType3 = "You're back! I just saw your Ed Gein video!";
            textToType4 = "Ed Gein? I literally just visited him. He's not the most hospitable, I have to admit.";
            textToType5 = "How cool! So, what, you took him to the Judgment House? What was his punishment?";
            textToType6 = "Unfortunately for me, a very unusual case has come up.";
            textToType7 = "Sounds sad. And you know Faust, I don't want to put pressure on you, but it seems to me that\nafter you became a chronogenic, you started not spending enough time with us, your \"cookies\"!";
            textToType8 = "Gella. I am..";
            textToType9 = "It's all right, Faust. It's okay. It's just not all cookies are the same. How many of your\ngroupies sew you clothes for special assignments? Just me.";
            textToType10 = "Gella! You're bringing it up again. You know how much I appreciate you. And I told you, you're one of my biggest fans. I'm the one who brought you here.";
            textToType11 = "But I don't want to be \"one of\"! I want to be first in your eyes! And that's what I'm going to\ndo!";
            textToType12 = "Well, Gella. I'll give you an opportunity! I need a great Japanese costume like no other. Because next crossing I'm going to be in Japan and I'm bringing \"rat person\" back here!";
        }
        else if (language == 1)
        {
            Gella_PF = RUS_Gella_PF_SELECT;
            Nosferatu_PF = RUS_Nosferatu_PF_SELECT;
            Chronotron_Subject = RUS_Chronotron_Subject_SELECT;
            faustText = "Фауст";
            GellaText = "Гелла";
            NosferatuText = "Носферату";

            NtextToType1 = "...Убийцы в масках? Мы называем их мэготы. Они появились во время твоего отпуска и наделали\nмного шума.";
            NtextToType9 = "Ну, вообще мэготами называют фанатов \"Slipknot\". А на этих типах как раз маски как у этих\nрокеров. Один хроногент, кто сталкивался с ними, говорит, что во время убийств они включают\nпесни этой рок - группы, и медленно и мучительно убивают своих жертв.";
            NtextToType10 = "Почему их называют мэготами?";
            NtextToType11 = "Мэготы...Этой проблемы нам не хватало!";
            NtextToType2 = "Откуда взялись эти мэготы?\nСудя по одежке они из нашего времени.";
            NtextToType3 = "Скорее всего, так оно и есть.";
            NtextToType4 = "Но как они могли попасть в ирреальность? Не через наш же Хронотрон?";
            NtextToType5 = "Ну, Фауст, может быть не одни мы такие талантливые. И кто-то тоже додумался собрать такую же\n\"машину\". Кто-то, кто хочет использовать портал не для восстановления справедливости и спасения\nлюдей, а для убийств и разрушений, за которые он не понесёт ни-ка-кой ответственности.";
            NtextToType6 = "Кто-то кого все только отвергали и никто не любил... и теперь этот \"кто-то\" сошел с ума,\nсобрал банду таких же озлобленных неудачников-инцелов, и вместе они теперь перемещаются\nво времени, чтобы резать и расчленять.";
            NtextToType7 = "Поубивал себе людишек из прошлого и вернулся обратно - мечта любого маньяка...\nДа, страшные вещи я говорю. Но работа у меня такая, страшные истории собирать,\nа потом их людям рассказывать.";
            NtextToType8 = "...И знаешь, эти мэготы тоже со странностями. Невинного ни разу не тронули.\nИ все кого они мочат - это те самые маньяки, за которыми мы тоже охотимся,\nно другими методами и для других целей. Вот так то.";

            textToType1 = "Фауст!?";
            textToType2 = "Гелла!";
            textToType3 = "Ты вернулся! А я вот недавно смотрела твой ролик про Эда Гейна!";
            textToType4 = "Эд Гейн? Да я буквально только что был у него в \"гостях\".\nНе самый гостеприимный он человек, должен признаться.";
            textToType5 = "Как круто! И что, ты доставил его в Судилище? Какое наказание ему выбрали?";
            textToType6 = "К моему сожалению..вышел очень необычный случай...";
            textToType7 = "Звучит печальненько... И вот знаешь Фауст, не хочу давить на тебя, но мне кажется, что после\nтого как ты стал хроногентом, ты начал мало времени уделять нам, своим \"печенькам\"!";
            textToType8 = "Гелла..да я же..";
            textToType9 = "Всё хорошо, Фауст. Всё хорошо. Просто не все печеньки такие одинаковые..Много ли кто из твоих\nфанаток шьёт тебе одежду для специальных заданий. Только Я.";
            textToType10 = "Гелла! Ты снова поднимаешь эту тему. Ты же прекрасно знаешь как сильно я тебя ценю. И я ведь\nговорил, что ты одна из моих лучших фанаток. Ведь это и привело тебя сюда.";
            textToType11 = "Но я не хочу быть \"одной из\"! Я хочу быть первой! И я это докажу!";
            textToType12 = "Что ж, Гелла. Я предоставлю тебе такую возможность!\nМне нужен отличный японский костюм не похожий ни на что.\nПотому что следующим переходом я окажусь в Японии и приведу сюда \"Человека-крысу\"!";
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
    private IEnumerator TransitionCamera()
    {
        transitionStartTime = Time.time;
        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            camera1.orthographicSize = Mathf.Lerp(initialSize, targetSize, t);
            transform.position = Vector3.Lerp(initialPosition, targetPos, t);
            yield return null;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        camera1.orthographicSize = targetSize;
        initialSize = camera1.orthographicSize;
        transform.position = targetPos;
        initialPosition = transform.position;
        Invoke(nameof(startCrtTheEnd2), 1f);
    }
    private IEnumerator TransitionCamera2()
    {
        transitionStartTime = Time.time;
        while (Time.time - transitionStartTime <= transitionDuration2)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration2;
            transform.position = Vector3.Lerp(initialPosition, targetPos2, t);
            yield return null;
        }
        transform.position = targetPos2;
        initialPosition = transform.position;
        Invoke(nameof(startCrtTheEnd3), 1f);
    }
    private IEnumerator TransitionCamera3()
    {
        transitionStartTime = Time.time;
        while (Time.time - transitionStartTime <= transitionDuration3)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration3;
            transform.position = Vector3.Lerp(initialPosition, targetPos3, t);
            yield return null;
        }
        transform.position = targetPos3;
        initialPosition = transform.position;
        StartCoroutine(TransitionCamera4());
    }
    private IEnumerator TransitionCamera4()
    {
        transitionStartTime = Time.time;
        while (Time.time - transitionStartTime <= transitionDuration4)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration4;
            camera1.orthographicSize = Mathf.Lerp(initialSize, targetSize2, t);
            transform.position = Vector3.Lerp(initialPosition, targetPos4, t);
            yield return null;
        }
        camera1.orthographicSize = targetSize2;
        transform.position = targetPos4;
        initialPosition = transform.position;
        initialSize = camera1.orthographicSize;
        typeTextCoroutine = StartCoroutine(methodSTARTdialog());
    }
    private IEnumerator TransitionCamera5()
    {
        transitionStartTime = Time.time;
        while (Time.time - transitionStartTime <= transitionDuration5)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration5;
            camera1.orthographicSize = Mathf.Lerp(initialSize, targetSize, t);
            transform.position = Vector3.Lerp(initialPosition, targetPos3, t);
            yield return null;
        }
        camera1.orthographicSize = targetSize;
        transform.position = targetPos3;
        initialPosition = transform.position;
        initialSize = camera1.orthographicSize;
        startBool2WithoutFew = true;
    }
    private IEnumerator TransitionCamera6()
    {
        transitionStartTime = Time.time;
        while (Time.time - transitionStartTime <= transitionDuration5)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration5;
            camera1.orthographicSize = Mathf.Lerp(initialSize, targetSize3, t);
            yield return null;
        }
        camera1.orthographicSize = targetSize3;
        initialSize = camera1.orthographicSize;
        allowContinueDialog = true;
    }
    private IEnumerator TransitionCamera2WithoutFew()
    {
        transitionStartTime = Time.time;
        while (Time.time - transitionStartTime <= transitionDuration2)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration2;
            transform.position = Vector3.Lerp(initialPosition, targetPos2, t);
            yield return null;
        }
        transform.position = targetPos2;
        initialPosition = transform.position;
        allowContinueDialog = true;
    }
    public void YesSkip()
    {
        StopCoroutine(typeTextCoroutine);
        TextContainer_dia.SetActive(false);
        buttons_container.SetActive(false);
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        if (voice.isPlaying)
        {
            voice.Stop();

        }
        _fadeinEnd = true;
        if (back_music != null)
        {
            StartCoroutine(FadeOutMusic());
        }
        if (extraAmbient != null)
        {
            StartCoroutine(FadeOutMusicExtra());
        }
        if (campfireSnd.isPlaying)
        {
            StartCoroutine(FadeOutCampfire());
        }
        openSkipWindow.SetActive(false);
        if(_Timer != null)
        {
            _Timer.isSkiped = true;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _PauseScript.offMouse = true;
        skipScene.SetActive(false);
        _PC_Skip.SetActive(false);
        _mobile_Skip.SetActive(false);
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
    void Update()
    {
        if (skipCoroutine == true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OpenSkipWindow();
            }
        }
        if (_fadein)
        {
            if (canvasmain.alpha > 0)
            {
                canvasmainObj.SetActive(true);
                canvasmain.alpha -= TimeToFade * Time.deltaTime;
                if (canvasmain.alpha == 0)
                {
                    _fadein = false;
                    Invoke(nameof(startCrtTheEnd), 2f);
                }
            }
        }
        if (_fadeout)
        {
            if (canvasmain.alpha >= 0)
            {
                canvasmainObj.SetActive(true);
                canvasmain.alpha += TimeToFade * Time.deltaTime;
                if (canvasmain.alpha == 1)
                {
                    endBackground.SetActive(true);
                    _fadeout = false;
                    transform.position = new Vector3(0f, 0f, transform.position.z);
                    Camera.main.orthographicSize = 5f;
                    initialSize = camera1.orthographicSize;

                    faustPulse.breatheIn = new Vector3(0.471f, 0.473f, 0f);
                    faustPulse.breatheOut = new Vector3(0.477f, 0.471f, 0f);

                    gellaPulse.breatheIn = new Vector3(0.395f, 0.401f, 0f);
                    gellaPulse.breatheOut = new Vector3(0.401f, 0.399f, 0f);

                    faustTransform.localScale = new Vector3(0.472f, 0.472f, 0.472f);
                    gellaTransform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

                    faustTransform.anchoredPosition = new Vector3(-151f, -446f, 0f);
                    gellaTransform.anchoredPosition = new Vector3(164.82f, -454f, 0f);
                    _fadein1 = true;

#if PLAYMARKET
                    FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
                }
            }
        }
        if (_fadein1)
        {
            if (canvasmain.alpha > 0)
            {
                canvasmainObj.SetActive(true);
                canvasmain.alpha -= TimeToFade * Time.deltaTime;
                if (canvasmain.alpha == 0)
                {
                    _fadein1 = false;
                    canvasmainObj.SetActive(false);
                    StartCoroutine(TransitionCamera6());
                }
            }
        }
        if (_fadeinEnd)
        {
            if (canvasmain.alpha >= 0)
            {
                canvasmainObj.SetActive(true);
                canvasmain.alpha += TimeToFade2 * Time.deltaTime;
                if (canvasmain.alpha == 1)
                {
                    _fadeinEnd = false;
                    _fadeinEnd1 = true;
                }
            }
        }
        if (_fadeinEnd1)
        {
            if (Chapter1End.alpha >= 0)
            {
                Chapter1EndObj.SetActive(true);
                Chapter1End.alpha += TimeToFade2 * Time.deltaTime;
                if (Chapter1End.alpha == 1)
                {
                    _fadeinEnd1 = false;
                    Invoke(nameof(startBool), 2);
                    ChapterEndSnd.Play();
                    if(back_music != null)
                    {
                        StartCoroutine(FadeOutMusic());
                    }
                }
            }
        }
        if (_fadeoutEnd)
        {
            if (Chapter1End.alpha > 0)
            {
                Chapter1EndObj.SetActive(true);
                Chapter1End.alpha -= TimeToFade2 * Time.deltaTime;
                if (Chapter1End.alpha == 0)
                {
                    _fadeoutEnd = false;
                    Invoke(nameof(startBool2), 2);
                }
            }
        }
        if (_fadeout1)
        {
            if (canvasmain.alpha >= 0)
            {
                canvasmainObj.SetActive(true);
                canvasmain.alpha += TimeToFade2 * Time.deltaTime;
                if (canvasmain.alpha == 1)
                {
                    _fadeout1 = false;
                    if (_Timer != null)
                    {
                        PlayerPrefs.SetInt("chapter1Time", _Timer.GetIntTime());
                        if (!_Timer.isSkiped)
                        {
                            _LeaderboardRecorder.SubmitScore(_Timer.GetIntTime());
                        }
                        _Timer.ResetTimer();
                        _Timer.StopTimer();
                        Destroy(_Timer.gameObject);
                    }
                    PlayerPrefs.SetFloat("chapter1", 2);
                    SceneManager.LoadScene("Menu");
                }
            }
        }
    }
    private void startBool2()
    {
        _fadeout1 = true;
    }

    private void startBool()
    {
        _fadeoutEnd = true;
    }

    private void startCrtTheEnd()
    {
        StartCoroutine(TransitionCamera());
    }
    private void startCrtTheEnd2()
    {
        StartCoroutine(TransitionCamera2());
        StartCoroutine(_BlinkingEyes1.Loop());
        StartCoroutine(_BlinkingEyes2.Loop());
    }
    private void startCrtTheEnd3()
    {
        StartCoroutine(TransitionCamera3());
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
                case 3:
                    SetNextFace3();
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
    private void SetNextFace3()
    {
        faces3Nosferatu[currentFaceIndex2].gameObject.SetActive(false);

        currentFaceIndex2++;
        if (currentFaceIndex2 >= faces3Nosferatu.Length)
        {
            currentFaceIndex2 = initialFaceIndex2;
        }

        faces3Nosferatu[currentFaceIndex2].gameObject.SetActive(true);
    }

    private void ResetFace()
    {
        StopCoroutine(mouthMovementCoroutine);
        SetFaceByIndex(initialFaceIndex1);
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
        SetFaceByIndex2(initialFaceIndex2);
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

    private void ResetFace3()
    {
        StopCoroutine(mouthMovementCoroutine);
        SetFaceByIndex3(initialFaceIndex2);
    }
    private void SetFaceByIndex3(int index)
    {
        foreach (Image face in faces3Nosferatu)
        {
            face.gameObject.SetActive(false);
        }

        if (index >= 0 && index < faces3Nosferatu.Length)
        {
            faces3Nosferatu[index].gameObject.SetActive(true);
        }
    }
    private IEnumerator methodSTARTdialog()
    {
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);

        if (PlayerPrefs.GetFloat("chapter1") == 2)
        {
            skipScene.SetActive(true);
            skipCoroutine = true;
        }

        voice.PlayOneShot(voiceClips[11]);

        speaker_name.text = NosferatuText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(3));
        foreach (char character in NtextToType1)
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
        ResetFace3();
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
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(1);
        question_num = 1;
        if (language == 0)
        {
            button_text1.text = "Why are they called\nmagoths?";
            button_text2.text = "Magoths...";
        }
        else if (language == 1)
        {
            button_text1.text = "Почему их называют\nмэготами?";
            button_text2.text = "Мэготы...";
        }
        ShowButtons();
    }
    private IEnumerator TypeTextButton1()
    {
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);
        TextContainer_dia.SetActive(true);

        voice.PlayOneShot(voiceClips[13]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in NtextToType10)
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
        StartCoroutine(TypeText3());
    }

    private IEnumerator TypeTextButton2()
    {
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);
        TextContainer_dia.SetActive(true);

        voice.PlayOneShot(voiceClips[14]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in NtextToType11)
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
        StartCoroutine(TypeText3());
    }
    private IEnumerator TypeText3()
    {

        voice.PlayOneShot(voiceClips[8]);

        text.text = "";
        speaker_name.text = NosferatuText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(3));
        foreach (char character in NtextToType9)
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
        ResetFace3();
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

        if (numberOfTrueAnswers == needNumbersForAchieve)
        {
            if(_AchievementsManager != null)
            {
                _AchievementsManager.SetAchievement("achNosferatu");
            }
        }
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[15]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in NtextToType2)
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
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);


        voice.PlayOneShot(voiceClips[10]);

        text.text = "";
        speaker_name.text = NosferatuText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(3));
        foreach (char character in NtextToType3)
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
        ResetFace3();
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

        voice.PlayOneShot(voiceClips[16]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
        foreach (char character in NtextToType4)
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

        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(1);
        showPersonalFileSnd.Play();
        black_back_PF.SetActive(true);
        Chronotron_Subject.SetActive(true);
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
        Chronotron_Subject.SetActive(false);
        //Есть шанс что пользователь сменит язык именно тогда когда открыты личные данные и нужно закрыть оба варианта языков - начало
        EN_Chronotron_Subject_SELECT.SetActive(false);
        RUS_Chronotron_Subject_SELECT.SetActive(false);
        //Есть шанс что пользователь сменит язык именно тогда когда открыты личные данные и нужно закрыть оба варианта языков - конец
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);

        voice.PlayOneShot(voiceClips[12]);

        speaker_name.text = NosferatuText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(3));
        foreach (char character in NtextToType5)
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
        ResetFace3();
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

        voice.PlayOneShot(voiceClips[6]);

        text.text = "";
        speaker_name.text = NosferatuText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(3));
        foreach (char character in NtextToType6)
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
        ResetFace3();
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

        voice.PlayOneShot(voiceClips[9]);

        text.text = "";
        speaker_name.text = NosferatuText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(3));
        foreach (char character in NtextToType7)
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
        ResetFace3();
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

        voice.PlayOneShot(voiceClips[7]);

        text.text = "";
        speaker_name.text = NosferatuText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(3));
        foreach (char character in NtextToType8)
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
        ResetFace3();
        skipCoroutine = false;
        skipScene.SetActive(false);
        _PC_Skip.SetActive(false);
        _mobile_Skip.SetActive(false);
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

        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(1);
        showPersonalFileSnd.Play();
        black_back_PF.SetActive(true);
        Nosferatu_PF.SetActive(true);
        _PC_continue.anchoredPosition = new Vector3(-484.1f, 74.60001f, 0f);
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
        closePersonalFileSnd.Play();
        black_back_PF.SetActive(false);
        Nosferatu_PF.SetActive(false);
        //Есть шанс что пользователь сменит язык именно тогда когда открыты личные данные и нужно закрыть оба варианта языков - начало
        EN_Nosferatu_PF_SELECT.SetActive(false);
        RUS_Nosferatu_PF_SELECT.SetActive(false);
        //Есть шанс что пользователь сменит язык именно тогда когда открыты личные данные и нужно закрыть оба варианта языков - конец
        yield return new WaitForSeconds(1);
        StartCoroutine(TransitionCamera5());
        yield return new WaitUntil(() => startBool2WithoutFew == true);
        StartCoroutine(TransitionCamera2WithoutFew());
        Invoke(nameof(teleportFaust), 2.5f);
        yield return new WaitUntil(() => allowContinueDialog == true);
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);

        if (PlayerPrefs.GetFloat("chapter1") == 2)
        {
            skipScene.SetActive(true);
#if PLAYMARKET
            _mobile_Skip.SetActive(true);
#else
            _PC_Skip.SetActive(true);
#endif
            skipCoroutine = true;
        }

        voice.PlayOneShot(voiceClips[0]);

        speaker_name.text = GellaText;
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

        voice.PlayOneShot(voiceClips[17]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
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
        yield return new WaitUntil(() => !voice.isPlaying);
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

        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(1);
        showPersonalFileSnd.Play();
        black_back_PF.SetActive(true);
        Gella_PF.SetActive(true);
        _PC_continue.anchoredPosition = new Vector3(-484.1f, 74.60001f, 0f);
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
        closePersonalFileSnd.Play();
        black_back_PF.SetActive(false);
        Gella_PF.SetActive(false);
        //Есть шанс что пользователь сменит язык именно тогда когда открыты личные данные и нужно закрыть оба варианта языков - начало
        EN_Gella_PF_SELECT.SetActive(false);
        RUS_Gella_PF_SELECT.SetActive(false);
        //Есть шанс что пользователь сменит язык именно тогда когда открыты личные данные и нужно закрыть оба варианта языков - конец
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => allowContinueDialog == true);
        allowContinueDialog = false;
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        speaker_name.text = "";
        text.text = "";
        _fadeout = true;
        StartCoroutine(FadeOutCampfire());
        yield return new WaitUntil(() => allowContinueDialog == true);
        allowContinueDialog = false;
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);

        voice.PlayOneShot(voiceClips[1]);

        speaker_name.text = GellaText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
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
        yield return new WaitUntil(() => !voice.isPlaying);
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

        voice.PlayOneShot(voiceClips[18]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
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
        yield return new WaitUntil(() => !voice.isPlaying);
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
        text.text = "";

        voice.PlayOneShot(voiceClips[2]);

        speaker_name.text = GellaText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
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
        yield return new WaitUntil(() => !voice.isPlaying);
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

        voice.PlayOneShot(voiceClips[19]);

        text.text = "";
        speaker_name.text = faustText;
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
        yield return new WaitUntil(() => !voice.isPlaying);
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
        text.text = "";

        voice.PlayOneShot(voiceClips[3]);

        speaker_name.text = GellaText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
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
        yield return new WaitUntil(() => !voice.isPlaying);
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

        voice.PlayOneShot(voiceClips[20]);

        text.text = "";
        speaker_name.text = faustText;
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
        yield return new WaitUntil(() => !voice.isPlaying);
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
        text.text = "";

        voice.PlayOneShot(voiceClips[4]);

        speaker_name.text = GellaText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
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
        yield return new WaitUntil(() => !voice.isPlaying);
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

        voice.PlayOneShot(voiceClips[21]);

        text.text = "";
        speaker_name.text = faustText;
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
        yield return new WaitUntil(() => !voice.isPlaying);
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
        text.text = "";

        voice.PlayOneShot(voiceClips[5]);

        speaker_name.text = GellaText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
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
        yield return new WaitUntil(() => !voice.isPlaying);
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

        voice.PlayOneShot(voiceClips[22]);

        text.text = "";
        speaker_name.text = faustText;
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
        yield return new WaitUntil(() => !voice.isPlaying);
        ResetFace2();
        skipCoroutine = false;
        skipScene.SetActive(false);
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
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        if (_AchievementsManager != null)
        {
            _AchievementsManager.SetAchievement("achGella");
        }
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        _fadeinEnd = true;
    }
    private void teleportFaust()
    {
        faustTransform.anchoredPosition = new Vector3(2547f, -902f, 0f);
    }
    private IEnumerator FadeOutCampfire()
    {
        float startVolume = campfireSnd.volume;

        while (campfireSnd.volume > 0)
        {
            campfireSnd.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        campfireSnd.Stop();
    }

    private IEnumerator FadeOutMusic()
    {
        float startVolume = back_music.volume;

        while (back_music.volume > 0)
        {
            back_music.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        back_music.Stop();
        Destroy(back_music.gameObject);
    }
    private IEnumerator FadeOutMusicExtra()
    {
        float startVolume = extraAmbient.volume;

        while (extraAmbient.volume > 0)
        {
            extraAmbient.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        extraAmbient.Stop();
    }


    private IEnumerator FadeInCampfire()
    {
        float targetVolume = 0.4f; // Целевая громкость звука
        float startVolume = 0; // Начальная громкость звука

        float elapsedTime = 0; // Время, прошедшее с начала плавного увеличения громкости

        while (campfireSnd.volume < targetVolume)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; // Прогресс плавного увеличения громкости

            // Используйте другую функцию интерполяции для медленного прибавления звука
            campfireSnd.volume = Mathf.Lerp(startVolume, targetVolume, t * t); // Квадратичная функция интерполяции
            yield return null;
        }

        campfireSnd.volume = targetVolume;
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
        if (question_num == 1)
        {
            if (1 == btn_num)
            {
                numberOfTrueAnswers++;
                typeTextCoroutine = StartCoroutine(TypeTextButton1()); // (Шо такое мэготы) Объяснения 
            }
            else
            {
                typeTextCoroutine = StartCoroutine(TypeTextButton2()); // (Мэготы...) Обычный диалог
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
