using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClockScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasmain, Chapter2End;
    [SerializeField] private GameObject canvasmainObj;

    private bool _fadein = false, _fadeEnd;
    public float TimeToFade;

    
    public int language;

    public Image[] faces;
    public Image[] faces2;
    private int initialFaceIndex = 0;
    private int initialFaceIndex1 = 0;
    private int currentFaceIndex = 0; // Текущий индекс формы лица
    private int currentFaceIndex1 = 0; // Текущий индекс формы лица

    [SerializeField] private GameObject contunieObj;
    [SerializeField] private RectTransform _PC_continue, _mobile_continue;

    public Text text, speaker_name;
    private string vilenaText, faustText;

    private string textToType1, textToType2, textToType3, textToType4, textToType5, textToType6, textToType7,
        textToType8, textToType9, textToType10, textToType11, textToType12, textToType13, textToType14,
        textToType15, textToType16, textToType17;
    
    public float typingSpeed = 0.05f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    [SerializeField] private GameObject TextContainer_dia;

    public float fadeDuration = 1f; // Продолжительность затухания
    [SerializeField] private AudioSource back_music, extraAmbient, ChapterEndSnd;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject[] operationPeopleScene;

    [SerializeField] private ActivePauseAfk _ActivePauseAfk;
    [SerializeField] private PauseScript _PauseScript;

    [SerializeField] private AudioSource voice;
    [SerializeField] private AudioClip[] voiceClips;
    private Coroutine mouthMovementCoroutine, typeTextCoroutine;

    [SerializeField] private CanvasGroup DialogGroup;
    private Timer _Timer;

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
        try
        {
            back_music = GameObject.Find("AmbientMusic").GetComponent<AudioSource>();
        }
        catch (Exception e)
        {
            Debug.Log(e);

            back_music = extraAmbient;
            //Включение запасной музыки если та не включилась на основной локации
            extraAmbient.gameObject.SetActive(true);
        }
        Time.timeScale = 1f;
        _fadein = true;

        UpdateDialogLang();

        text.text = "";

        initialFaceIndex = currentFaceIndex;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void UpdateDialogLang()
    {
        language = PlayerPrefs.GetInt("language", language);
        if (language == 0)
        {
            vilenaText = "Vilena Sergeyevna";
            faustText = "Faust";

            //Faust
            textToType1 = "Vilena Sergeyevna, are you alright?";
            //Vilena
            textToType2 = "I'm fine, Faust. Just a bit of a headache — today's trial was especially difficult. I always take\ncases involving children very hard.";
            textToType3 = "It may seem like you get used to it over time, that after so many cases you stop feeling\nanything... But no. It still hits you. Every time — right in the heart.";
            //Faust
            textToType4 = "When children become victims, it's always terrible. But the important thing is — we brought justice\nto that Deviant.";
            textToType5 = "Moreover, we will study his mind, to turn the evil he caused in the past into something useful\nfor the future, to help prevent crimes like this.";
            //Vilena
            textToType6 = "There's a lot of work ahead, Faust. Everything the Order of Nostra Edemas has done so far\nmatters greatly. But it's not enough.";
            textToType7 = "We have a monumental task ahead — to uncover many hidden crimes.";
            textToType8 = "And the spatial anomalies are becoming more dangerous each time — and we still can't say for\nsure what they really are.";
            textToType9 = "Tensions between the Schools are rising, and serious changes may occur within the Order.\nAnd those masked killers — someone must’ve let them in through our portal, or they wouldn’t\nhave made it inside.";
            //Faust
            textToType10 = "Professor, if you believe I can serve the Order not only as a chrono-agent, but in another\ncapacity as well — I’m honored. And grateful for your trust in my abilities.";
            //Vilena
            textToType11 = "I had no doubt you’d say that. That’s why I called for you.";
            textToType12 = "Vasily Beyner, from this day forward, I appoint you as the teacher and head of the Faust\nGroup — a special operations unit answering directly to me! You will investigate both Deviant\ncrimes and secret matters within the Order itself.";
            //Faust
            textToType13 = "I’ll do everything in my power not to let you down. Faust — But who are my students, Professor?";
            //Vilena
            textToType14 = "Gella Ivashichi — expert in disguise and secret codes.\nGonzo — works through the Astral, can see what is hidden.";
            textToType15 = "Pyotr Kandinsky — communicates with spirits using a radio.\nYuri Krimin — outstanding criminologist, knows the minds of killers, a brilliant detective...";
            //Faust
            textToType16 = "Hello. I am Vasily Beyner. My callsign is Faust21.\nFrom this day, I will be your mentor. We have a lot of difficult work ahead. But before we begin,\nI will teach you everything I know.";
            textToType17 = "And… before we start, remember this above all: \"This world is not as kind as it seems…\"";
        }
        else if (language == 1)
        {
            vilenaText = "Вилена Сергеевна";
            faustText = "Фауст";

            //фауст
            textToType1 = "Вилена Сергеевна, вы в порядке?";
            //вилена
            textToType2 = "Всё нормально, Фауст. Просто немного болит голова — сегодняшний судебный процесс выдался\nособенно тяжёлым. Я всегда тяжело переживаю дела, связанные с детьми.";
            textToType3 = "Это только кажется, что со временем привыкаешь, что после стольких процессов это перестаёт\nвызывать эмоции… Но нет. Всё равно откликается. И каждый раз — в самом сердце.";
            //фауст
            textToType4 = "Когда жертвами становятся дети, это всегда ужасно. Но главное — мы совершили правосудие над\nэтим Девиантом.";
            textToType5 = "Более того, мы будем изучать его разум, чтобы обратить то зло, что он причинил в прошлом,\nво благо в будущем, чтобы предотвратить подобные преступления.";
            //вилена
            textToType6 = "Впереди много работы, Фауст. Всё, что Орден Nostra Edemas делал до этого, имеет огромное\nзначение. Но этого недостаточно.";
            textToType7 = "Нам предстоит проделать колоссальный объём работы, раскрыть множество неизведанных\nпреступлений.";
            textToType8 = "К тому же пространственные аномалии становятся всё опаснее с каждым разом — а мы до сих пор\nне можем точно ответить, что именно они собой представляют.";
            textToType9 = "Отношения между Школами обостряются, и в ордене могут произойти серьёзные перемены.\nА эти убийцы в масках - кто-то наверняка впустил их через наш портал, иначе бы они туда\nне попали.";
            //фауст
            textToType10 = "Профессор, если вы считаете, что я могу быть полезен Ордену не только как хроногент,\nно и в иной роли, то я лишь рад. И благодарен за уверенность во моих силах.";
            //вилена
            textToType11 = "Я не сомневалась, что ты ответишь именно так. Поэтому  вызвала именно тебя.";
            textToType12 = "Василий Бейнер, с этого дня я назначаю вас учителем и главой Группы Фауста — особого\nоперативного отряда, подчиняющегося лично мне! Вы будете вести расследования как преступлений\nДевиантов, так и тайные дела внутри самого Ордена.";
            //фауст
            textToType13 = "Я сделаю всё, что в моих силах, чтобы не подвести вас. Фауст - Но кто мои ученики, профессор?";
            //вилена
            textToType14 = "Гелла Ивашичи — специалист по маскировке и тайным шифрам.\nГонзо — работает через Астрал, способен видеть то, что скрыто.";
            textToType15 = "Пётр Кандинский — говорит с духами с помощью радио.\nЮрий Кримин — выдающийся криминалист, отлично знает психологию убийц, хороший сыщик...";
            //фауст
            textToType16 = "Здравствуйте. Я Василий Бейнер. Мой позывной — Фауст21.\nС этого дня я буду вашим наставником. Впереди у нас много сложной работы.\nНо прежде чем мы начнём, я обучу вас всему, что знаю.";
            textToType17 = "И... перед тем как мы начнем запомните главное: \"Этот мир не такой добрый, как вам кажется…\"";
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
        if (_fadein)
        {
            if (canvasmain.alpha > 0)
            {
                canvasmainObj.SetActive(true);
                canvasmain.alpha -= TimeToFade * Time.deltaTime;
                if (canvasmain.alpha == 0)
                {
                    _fadein = false;
                    StartCoroutine(methodSTARTdialog());
                }
            }
        }
        if (_fadeEnd)
        {
            if (canvasmain.alpha >= 0)
            {
                canvasmainObj.SetActive(true);
                canvasmain.alpha += TimeToFade * Time.deltaTime;
                if (canvasmain.alpha == 1)
                {
                    _fadeEnd = false;
                    StartCoroutine(endCoroutine());
                }
            }
        }
    }
    private IEnumerator endCoroutine()
    {
        Chapter2End.gameObject.SetActive(true);
        ChapterEndSnd.Play();
        yield return StartCoroutine(SmoothCanvasGroup(Chapter2End, 2f, 1f));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(SmoothCanvasGroup(Chapter2End, 2f, 0f));
        yield return new WaitForSeconds(3f);
        if (_Timer != null)
        {
            PlayerPrefs.SetInt("chapter2Time", _Timer.GetIntTime());
            _Timer.ResetTimer();
            _Timer.StopTimer();
            Destroy(_Timer.gameObject);
        }
        PlayerPrefs.SetFloat("chapter2", 2);
        SceneManager.LoadScene("Menu");

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
    private void ResetFace2()
    {
        StopCoroutine(mouthMovementCoroutine);
        SetFaceByIndex2(initialFaceIndex1);
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
    private void SetFaceByIndex2(int index)
    {
        foreach (Image face in faces2)
        {
            face.gameObject.SetActive(false);
        }

        if (index >= 0 && index < faces2.Length)
        {
            faces2[index].gameObject.SetActive(true);
        }
    }

    private IEnumerator methodSTARTdialog()
    {
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
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
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[1]);

        text.text = "";
        speaker_name.text = vilenaText;
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[2]);

        text.text = "";
        speaker_name.text = vilenaText;
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[3]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[4]);

        text.text = "";
        speaker_name.text = faustText;
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[5]);

        text.text = "";
        speaker_name.text = vilenaText;
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[6]);

        text.text = "";
        speaker_name.text = vilenaText;
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[7]);

        text.text = "";
        speaker_name.text = vilenaText;
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[8]);

        text.text = "";
        speaker_name.text = vilenaText;
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[9]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[10]);

        text.text = "";
        speaker_name.text = vilenaText;
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[11]);

        text.text = "";
        speaker_name.text = vilenaText;
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[12]);

        text.text = "";
        speaker_name.text = faustText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        speaker_name.text = "";
        text.text = "";

        canvasmainObj.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(canvasmain, 2f, 1));
        foreach(GameObject operationist in operationPeopleScene)
        {
            operationist.SetActive(true);
        }
#if PLAYMARKET
        FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(SmoothCanvasGroup(canvasmain, 2f, 0));
        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));

        voice.PlayOneShot(voiceClips[13]);

        text.text = "";
        speaker_name.text = vilenaText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[14]);

        text.text = "";
        speaker_name.text = vilenaText;
        mouthMovementCoroutine = StartCoroutine(mouthMovement(2));
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
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
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
        foreach (char character in textToType16)
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
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
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
        foreach (char character in textToType17)
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
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        _fadeEnd = true;
        StartCoroutine(FadeOutMusic());
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

    public void MobilePress()
    {
        if (gameObject.activeSelf)
        {
            mobileContinuePressed = true;
        }
    }
}
