using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfterPortalScene2 : MonoBehaviour
{
    [SerializeField] private GameObject BlackImgObj, lockPic, faustScene;
    private CanvasGroup BlackImgCanvas;
    public float TimeToFade;
    private bool _fadein = false, _fadeout = false, _fadeinEnd = false;
    [SerializeField] private AudioSource backMusic, portal;
    public float fadeDuration = 2f; // Продолжительность затухания

    [SerializeField] private TimeManager timeManager;
    public Text text, speaker_name;
    private string textToType1, textToType2, textToType3;
    public float typingSpeed = 0.05f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    [SerializeField] private GameObject TextContainer_dia;
    private string faustText, jusan;
    public int language;
    public Image[] faces, faces2;
    private int initialFaceIndex = 0;
    private int initialFaceIndex1 = 0;
    private int currentFaceIndex = 0; // Текущий индекс формы лица
    private int currentFaceIndex1 = 0; // Текущий индекс формы лица
    [SerializeField] private GameObject _PC_continue, _mobile_continue;

    [SerializeField] private GameObject InterfaceObj, arrowsObj, musicAmbientStart;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private PauseScript _PauseScript;
    [SerializeField] private ButtonFunctions _ButtonFunctions;


    [SerializeField] private AudioSource voice;
    [SerializeField] private AudioClip[] voiceClips;
    private Coroutine mouthMovementCoroutine;
    [SerializeField] private CanvasGroup DialogGroup;

    [SerializeField] private ColliderList _ColliderList;

    private bool mobileContinuePressed = false;

    private void Start()
    {
        _ColliderList.allowOnOff = false;
        _PauseScript.findCollders = false;
        _ButtonFunctions.findCollders = false;
        _PauseScript.offMouse = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(FadeOutBack());
        InterfaceObj.SetActive(false);
        arrowsObj.SetActive(false);
        BlackImgCanvas = BlackImgObj.GetComponent<CanvasGroup>();
        _fadein = true;

        UpdateDialogLang();

        initialFaceIndex = currentFaceIndex;
        initialFaceIndex1 = currentFaceIndex1;
        // Запускаем корутину автопечати текста

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
            jusan = "Jusan";

            textToType1 = "If he resists, I allow the use of proportional force.";
            textToType2 = "Do what must be done, Faust. This one will not resist.";
            textToType3 = "Tsutomu Miyazaki, you are charged with a series of murders and other crimes, which are\ncorroborated by this evidence. We will now use this portal to transport ourselves\nto the Timeless Court to seek justice. Jusan, get ready!";
        }
        else if (language == 1)
        {
            faustText = "Фауст";
            jusan = "Жусан";

            textToType1 = "Если он будет сопротивляться, разрешаю применить соразмерную силу.";
            textToType2 = "Делай, что должно, Фауст. Этот сопротивляться не станет.";
            textToType3 = "Цутому Миядзаки, вы обвиняетесь в серии убийств и других преступлениях,\nкоторые подтверждаются этими уликами. Сейчас с помощью этого портала мы перенесёмся\nво Вневременной суд для осуществления правосудия. Жусан, приготовься!";
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
        if (_fadein == true)
        {
            if (BlackImgCanvas.alpha < 1)
            {
                BlackImgObj.SetActive(true);
                BlackImgCanvas.alpha += TimeToFade * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 1)
            {
                timeManager.global_light.intensity = 0.7f;
                StartCoroutine(lightRoom10Again());
                lockPic.SetActive(false);
                faustScene.SetActive(true);
            }
        }
        if (_fadeout == true)
        {
            if (BlackImgCanvas.alpha > 0)
            {
                BlackImgCanvas.alpha -= TimeToFade * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 0)
            {
                BlackImgObj.SetActive(false);
                _fadeout = false;
                StartCoroutine(TypeText());
            }
        }
        if (_fadeinEnd == true)
        {
            if (BlackImgCanvas.alpha < 1)
            {
                BlackImgObj.SetActive(true);
                BlackImgCanvas.alpha += TimeToFade * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 1)
            {
                _fadeinEnd = false;
                SceneManager.LoadScene("Chapter2Court");
            }
        }
    }
    private IEnumerator lightRoom10Again()
    {
        yield return new WaitForSeconds(2);
        _fadein = false;
        _fadeout = true;
        musicAmbientStart.SetActive(true);
    }
    private IEnumerator TypeText()
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
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;

        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed); 
        
        voice.PlayOneShot(voiceClips[1]);

        text.text = "";
        speaker_name.text = jusan;
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

#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;

        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(1.5f);
        _fadeinEnd = true; // конец
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
    private IEnumerator FadeOutBack()
    {
        float startVolume = backMusic.volume;

        while (backMusic.volume > 0)
        {
            backMusic.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        backMusic.volume = startVolume;
        backMusic.Stop();
        if(timeManager.musicCoroutine != null)
        {
            StopCoroutine(timeManager.musicCoroutine);
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