using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FoldingScreen : MonoBehaviour
{
    public AudioSource growSnd;
    public float shrinkTime = 0.5f; // Время сжатия
    public float moveFactor = 0.5f; // Коэффициент движения вправо

    private bool isClickable = true;
    private bool isShrunken = false;
    private Vector3 originalScale;
    private Vector3 originalPosition;

    [SerializeField] private GameObject dialogBox, faustCharacter, blackscreen, itemsHint;
    public PauseScript pauseScript;
    public ButtonFunctions buttonFunctions;
    private ColliderList colliderList;
    [SerializeField] private GameObject jusanCollider;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private ManiacMove2 maniacMove2;


    public Text text, speaker_name;
    private string textToType1, textToType2, textToType3, textToType4, textToType5, textToType6, textToType7, textToType8, textToType9;
    public float typingSpeed = 0.05f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    [SerializeField] private GameObject TextContainer_dia;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private RectTransform _PC_continue, _mobile_continue;
    public Image[] faces;
    public Image[] faces2;
    private int initialFaceIndex = 0;
    private int initialFaceIndex1 = 0;
    private int currentFaceIndex = 0; // Текущий индекс формы лица
    private int currentFaceIndex1 = 0; // Текущий индекс формы лица
    private string faustText, jusanText;
    [SerializeField] private GameObject RUS_Jusan_PF_SELECT, EN_Jusan_PF_SELECT, black_back_PF;
    [SerializeField] private AudioSource showPersonalFileSnd, closePersonalFileSnd;
    private GameObject Jusan_PF;
    private Coroutine mouthMovementCoroutine;
    [SerializeField] private GameObject[] arrows_containers;
    [SerializeField] private GameObject Interface;
    public int language;
    [Header("Quotes")]
    [SerializeField] private AudioSource voice;
    [SerializeField] private AudioClip[] voiceClips;

    private bool mobileContinuePressed = false;

    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        originalScale = transform.localScale;
        originalPosition = transform.position;

        UpdateDialogLang();
    }
    public void UpdateDialogLang()
    {
        language = PlayerPrefs.GetInt("language", language);
        if (language == 0)
        {
            Jusan_PF = EN_Jusan_PF_SELECT;

            faustText = "Faust";
            jusanText = "Jusan";

            textToType1 = "Jusan? How did you end up here?";
            textToType2 = "Faust! Oh, damn, you scared the hell out of me... Y-you could’ve killed me!";
            textToType3 = "Sorry, Daniil... I really didn’t expect to meet anyone here.";
            textToType4 = "Well, now you know where I hide from you when I’m too lazy to write the script for our\nnext video. Just kidding! He-heh.";
            textToType5 = "So what are you doing here?";
            textToType6 = "I’m surprised you're asking, Faust. The script for the video about Tsutomu Miyazaki won’t write\nitself. I decided not to limit myself to just documentary sources — I came to the crime scene\nmyself, so to speak, to experience everything empirically!";
            textToType7 = "That's very commendable. But I hope you cleared it with the Order's leadership?";
            textToType8 = "Of course! I was granted permission under the condition that I follow all the restrictions the\nOrder imposes on chronogents. And here I am... Oh, and they forbade me from taking the Glauzer. Not that it matters — I don’t know how to use it anyway. But they did let me bring my computer.";
            textToType9 = "By the way, Faust, bring me something for our joint mission, will you?";
        }
        else if (language == 1)
        {
            Jusan_PF = RUS_Jusan_PF_SELECT;

            faustText = "Фауст";
            jusanText = "Жусан";

            textToType1 = "Жусан? Как ты здесь оказался?";
            textToType2 = "Фауст! Ох, чёрт, ты меня страшно напугал... Т-так и убить можно!";
            textToType3 = "Прости, Даниил... Совсем не ожидал кого-нибудь здесь встретить.";
            textToType4 = "Ну, зато теперь ты знаешь, где я скрываюсь от тебя, когда мне лень писать сценарий для нашего очередного ролика. Шутка! Хе-хех.";
            textToType5 = "Так что же ты здесь делаешь?";
            textToType6 = "Удивлён, что ты спрашиваешь, Фауст. Сценарий для ролика по Цутому Миядзаки сам себя не напишет. Я решил не ограничиваться лишь документальными источниками, а самому побывать на месте\nпреступления — так сказать, воспринять всё эмпирически!";
            textToType7 = "Это очень похвально. Но надеюсь, ты согласовал это с руководством Ордена?";
            textToType8 = "Ну конечно! Мне разрешили при условии, что я буду соблюдать все ограничения, которые Орден\nнакладывает на хроногентов. И вот я здесь... Да, ещё мне запретили брать глаузер.\nНо и неважно — пользоваться им я всё равно не умею. Зато разрешили взять мой компьютер.";
            textToType9 = "Кстати, Фауст, принеси-ка мне кое-что для нашего общего дела.";
        }
    }
    void Update()
    {
        // --- ПК: клик мышью ---
        if (Input.GetMouseButtonDown(0))
        {
            CheckHit(Camera.main.ScreenPointToRay(Input.mousePosition));
        }

        // --- Телефон: касание ---
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                CheckHit(Camera.main.ScreenPointToRay(touch.position));
            }
        }
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!isClickable) return;

            isClickable = false;
            StartCoroutine(isShrunken ? AnimateToOriginal() : AnimateAndSwitch());
            growSnd.Play();

            // Пауза маньяка и стрелки часов
            timeManager.isPaused = true;
            maniacMove2.AllowMovement = false;

            jusanCollider.SetActive(true);
            //начат диалог
            Interface.SetActive(false);
            buttonFunctions.BlockCollidersActive = true;
            pauseScript.BlockCollidersActive = true;

            pauseScript.offMouse = true;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            colliderList.DISABLE_COLLIDER();

            foreach (GameObject obj in arrows_containers)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            StartCoroutine(TypeText());
        }
    }

    private IEnumerator TypeText()
    {
        blackscreen.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(blackscreen.GetComponent<CanvasGroup>(), 1.5f, 1f));
        faustCharacter.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(SmoothCanvasGroup(blackscreen.GetComponent<CanvasGroup>(), 1.5f, 0f));
        blackscreen.SetActive(false);
        yield return new WaitForSeconds(1f);
        dialogBox.SetActive(true);
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
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        _mobile_continue.gameObject.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[1]);

        text.text = "";
        speaker_name.text = jusanText;
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
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[2]);

        text.text = "";
        speaker_name.text = faustText;
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
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        _mobile_continue.gameObject.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);


        voice.PlayOneShot(voiceClips[3]);

        text.text = "";
        speaker_name.text = jusanText;
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
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        _mobile_continue.gameObject.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
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
        Jusan_PF.SetActive(true);
        _PC_continue.anchoredPosition = new Vector3(-484.1f, 74.60001f, 0f);
        _mobile_continue.anchoredPosition = new Vector3(-625f, 127f, 0f);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        _mobile_continue.gameObject.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        _PC_continue.anchoredPosition = new Vector3(-250.77f, 74.60001f, 0f);
        _mobile_continue.anchoredPosition = new Vector3(-263f, 349.2f, 0f);
        closePersonalFileSnd.Play();
        black_back_PF.SetActive(false);
        Jusan_PF.SetActive(false);
        //Есть шанс что пользователь сменит язык именно тогда когда открыты личные данные и нужно закрыть оба варианта языков - начало
        EN_Jusan_PF_SELECT.SetActive(false);
        RUS_Jusan_PF_SELECT.SetActive(false);
        //Есть шанс что пользователь сменит язык именно тогда когда открыты личные данные и нужно закрыть оба варианта языков - конец
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[4]);

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
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        
        _mobile_continue.gameObject.SetActive(false);
        _PC_continue.gameObject.SetActive(false);

        voice.PlayOneShot(voiceClips[5]);

        text.text = "";
        speaker_name.text = jusanText;
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
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        _mobile_continue.gameObject.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        voice.PlayOneShot(voiceClips[6]);

        text.text = "";
        speaker_name.text = faustText;
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
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        _mobile_continue.gameObject.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);


        voice.PlayOneShot(voiceClips[7]);

        text.text = "";
        speaker_name.text = jusanText;
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
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        _mobile_continue.gameObject.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);


        voice.PlayOneShot(voiceClips[8]);

        text.text = "";
        speaker_name.text = jusanText;
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
        yield return StartCoroutine(SmoothCanvasGroupForHint());
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        _mobile_continue.gameObject.SetActive(false);
        _PC_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        yield return StartCoroutine(SmoothCanvasGroup(TextContainer_dia.GetComponent<CanvasGroup>(), 0.5f, 0f));
        dialogBox.SetActive(false);
        text.text = "";
        speaker_name.text = "";

        blackscreen.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(blackscreen.GetComponent<CanvasGroup>(), 1.5f, 1f));
        faustCharacter.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(SmoothCanvasGroup(blackscreen.GetComponent<CanvasGroup>(), 1.5f, 0f));
        blackscreen.SetActive(false);

        Interface.SetActive(true);
        buttonFunctions.BlockCollidersActive = false;
        pauseScript.BlockCollidersActive = false;

        pauseScript.offMouse = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        colliderList.ENABLE_COLLIDER();

        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        // Возобновление маньяка и стрелки часов
        timeManager.isPaused = false;
        maniacMove2.AllowMovement = true;
    }
    public IEnumerator AnimateAndSwitch()
    {
        Vector3 targetScale = new Vector3(0.6f, originalScale.y, originalScale.z); // Сжатие по X
        float moveDistance = (originalScale.x - targetScale.x) * moveFactor; // Контролируем смещение
        Vector3 targetPosition = originalPosition + new Vector3(moveDistance, 0, 0);

        yield return AnimateTransition(originalScale, targetScale, originalPosition, targetPosition);

        isShrunken = true;
        VariableDeterminator.allowToDragJusan = true;
    }

    public IEnumerator AnimateToOriginal()
    {
        VariableDeterminator.allowToDragJusan = false;
        Vector3 targetScale = originalScale;
        Vector3 targetPosition = originalPosition;

        yield return AnimateTransition(transform.localScale, targetScale, transform.position, targetPosition);

        isShrunken = false;
    }

    private IEnumerator AnimateTransition(Vector3 startScale, Vector3 endScale, Vector3 startPos, Vector3 endPos)
    {
        float elapsedTime = 0f;
        while (elapsedTime < shrinkTime)
        {
            float t = elapsedTime / shrinkTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;
        transform.position = endPos;
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
    private IEnumerator SmoothCanvasGroupForHint()
    {
        itemsHint.SetActive(true);
        CanvasGroup canvasGroup = itemsHint.GetComponent<CanvasGroup>();
        float startAlpha = canvasGroup.alpha;

        float targetAlpha = 1;

        float elapsedTime = 0f;
        float duration = 2f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }

        // Гарантируем точное значение в конце
        canvasGroup.alpha = targetAlpha;
    }

    public IEnumerator SmoothCanvasGroupForHintDisable()
    {
        CanvasGroup canvasGroup = itemsHint.GetComponent<CanvasGroup>();
        float startAlpha = canvasGroup.alpha;

        float targetAlpha = 0;

        float elapsedTime = 0f;
        float duration = 2f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }

        // Гарантируем точное значение в конце
        canvasGroup.alpha = targetAlpha;
        itemsHint.SetActive(false);
    }

    public void MobilePress()
    {
        if (gameObject.activeSelf)
        {
            mobileContinuePressed = true;
        }
    }
}