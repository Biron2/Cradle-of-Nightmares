using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FaustRoomScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasmain;
    [SerializeField] private GameObject canvasmainObj;

    private bool _fadein = false, _fadeEnd;
    public float TimeToFade;

    
    public int language;

    public Image[] faces;
    private int initialFaceIndex = 0;
    private int currentFaceIndex = 0; // Текущий индекс формы лица

    [SerializeField] private GameObject contunieObj;
    [SerializeField] private RectTransform _PC_continue, _mobile_continue;

    public Text text, speaker_name;
    private string faustText;

    private string textToType1, textToType2, textToType3, textToType4, textToType5, textToType6;
    
    public float typingSpeed = 0.05f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    [SerializeField] private GameObject TextContainer_dia;

    public float fadeDuration = 1f; // Продолжительность затухания
    [SerializeField] private AudioSource back_music, campfireSnd, pagerSound;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private ActivePauseAfk _ActivePauseAfk;
    [SerializeField] private PauseScript _PauseScript;

    [SerializeField] private AudioSource voice;
    [SerializeField] private AudioClip[] voiceClips;
    private Coroutine mouthMovementCoroutine, typeTextCoroutine;

    [SerializeField] private CanvasGroup DialogGroup;
    [SerializeField] private RectTransform pagerPos;
    public float duration = 1f; // Время анимации
    public Vector2 targetPosition; // Конечная позиция

    private Vector2 startPosition;
    public Text pagerText;
    public Image headImage;
    [SerializeField] private Sprite[] heads;

    private bool mobileContinuePressed = false;

    private void Start()
    {
        Time.timeScale = 1f;
        _fadein = true;
        StartCoroutine(FadeInCampfire());
        StartCoroutine(FadeInMusic());

        UpdateDialogLang();

        text.text = "";

        initialFaceIndex = currentFaceIndex;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Начальная позиция — под экраном (ниже текущего положения)
        startPosition = new Vector2(pagerPos.anchoredPosition.x, -Screen.height);

        pagerPos.anchoredPosition = startPosition;

#if PLAYMARKET
        FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
    }
    public void UpdateDialogLang()
    {
        language = PlayerPrefs.GetInt("language", language);
        if (language == 0)
        {
            faustText = "Faust";

            textToType1 = "It’s night outside — cold, and the rain is pouring. And here I am, sitting by the\nfireplace — warm and safe.";
            textToType2 = "The Nostra Edemas order has many rooms, and I’m glad that in one of them — my office — I can\nfeel at home.";
            textToType3 = "With the nature of my work, you learn to appreciate the simple things — warmth, peace,\nand silence.";
            textToType4 = "Brief moments of simple human joy — that’s what fills life with warmth.";
            textToType5 = "Yes, Vilena Sergeevna.";
            textToType6 = "Vasiliy, please come in if\nyou're free. There's\nsomething important we\nneed to discuss.";
        }
        else if (language == 1)
        {
            faustText = "Фауст";

            textToType1 = "На улице ночь, холод и идёт сильный дождь. А я сижу здесь, у камина — в тепле и безопасности.";
            textToType2 = "В ордене Nostra Edemas много комнат, и я рад, что в одной из них — в моём кабинете — я могу\nчувствовать себя как дома.";
            textToType3 = "С родом моей деятельности начинаешь лучше ценить простые вещи — тепло, покой и тишину.";
            textToType4 = "Короткие моменты простой человеческой радости — именно они наполняют жизнь теплотой.";
            textToType5 = "— Да, Вилена Сергеевна.";
            textToType6 = "— Василий, прошу, зайди\nко мне, если свободен.\nНужно обсудить кое-что\nважное."; //вилена
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
                    StartCoroutine(startNextSceneInSec());
                }
            }
        }
    }
    private IEnumerator startNextSceneInSec()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Chapter2Clock");
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
        speaker_name.text = faustText;
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
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(2f);
        pagerSound.Play(); //PAGER MESSAGE INCOMING
        yield return new WaitForSeconds(1f);
        pagerPos.gameObject.SetActive(true);
        StartCoroutine(SlideUp());
        yield return new WaitForSeconds(1f);

        voice.PlayOneShot(voiceClips[4]);

        pagerText.text = "";
        headImage.gameObject.SetActive(true);
        headImage.sprite = heads[1];
        mouthMovementCoroutine = StartCoroutine(mouthMovement(1));
        foreach (char character in textToType5)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            pagerText.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !voice.isPlaying);
        ResetFace();
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);

        _PC_continue.GetComponent<Text>().color = Color.white;
        _PC_continue.anchoredPosition = new Vector3(-408f, 108f, 0f);
        _mobile_continue.GetComponent<Text>().color = Color.white;
        _mobile_continue.anchoredPosition = new Vector3(-408f, 108f, 0f);
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

        pagerText.text = "";
        headImage.sprite = heads[0];
        foreach (char character in textToType6)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            pagerText.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => !voice.isPlaying);
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
        yield return StartCoroutine(SlideDown());
        pagerPos.gameObject.SetActive(false);
        _fadeEnd = true;

        if (campfireSnd.isPlaying)
        {
            StartCoroutine(FadeOutCampfire());
        }
    }

    private IEnumerator SlideUp()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            pagerPos.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        pagerPos.anchoredPosition = targetPosition; // Убедиться, что в конце точная позиция
    }
    private IEnumerator SlideDown()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            pagerPos.anchoredPosition = Vector2.Lerp(targetPosition, startPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        pagerPos.anchoredPosition = startPosition; // Убедиться, что в конце точная позиция
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
    private IEnumerator FadeInMusic()
    {
        float targetVolume = 0.75f; // Целевая громкость звука
        float startVolume = 0; // Начальная громкость звука

        float elapsedTime = 0; // Время, прошедшее с начала плавного увеличения громкости

        while (back_music.volume < targetVolume)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; // Прогресс плавного увеличения громкости

            // Используйте другую функцию интерполяции для медленного прибавления звука
            back_music.volume = Mathf.Lerp(startVolume, targetVolume, t * t); // Квадратичная функция интерполяции
            yield return null;
        }

        back_music.volume = targetVolume;
    }

    public void MobilePress()
    {
        if (gameObject.activeSelf)
        {
            mobileContinuePressed = true;
        }
    }
}
