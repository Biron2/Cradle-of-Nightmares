using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfterPortalScene : MonoBehaviour
{
    [SerializeField] private GameObject BlackImgObj, lockPic, faustScene, flashObj, maniacWithWeapon, deadFaust, faustAlive;
    private CanvasGroup BlackImgCanvas, flashCanvas;
    public float TimeToFade, TimeToFade2;
    private bool _fadein, _fadeout = false, _fadeoutForHits = false;
    private bool _fadeinFlash = false, _fadeoutFlash = false;
    private bool _fadeinFlash2 = false, _fadeoutFlash2 = false;
    private bool _fadeinFlash3 = false, _fadeoutFlash3 = false;
    [SerializeField] private GameObject magots, maniacDead, ActiveEndScene, maniacOff;
    [SerializeField] private AudioSource backMusic, portal, hits;
    public float fadeDuration = 2f; // Продолжительность затухания

    [SerializeField] private TimeManager timeManager;
    public Text text, speaker_name;
    private string textToType1, textToType2, textToType3, textToType4, textToType5, textToType6;
    public float typingSpeed = 0.05f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    [SerializeField] private GameObject TextContainer_dia;
    private string faustText, maniac;
    public int language;
    public Image[] faces;
    private int initialFaceIndex = 0;
    private int currentFaceIndex = 0; // Текущий индекс формы лица
    [SerializeField] private GameObject _PC_continue, _mobile_continue;

    [SerializeField] private GameObject InterfaceObj, arrowsObj, musicAmbientStart;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private AudioSource showButtonsSnd, FaustHitSnd, krikSnd;
    [SerializeField] private GameObject buttons_container;
    [SerializeField] private Text button_text1, button_text2;
    [SerializeField] private PauseScript _PauseScript;
    [SerializeField] private ButtonFunctions _ButtonFunctions;

    [SerializeField] private Camera mCamera;
    [SerializeField] private Collider2D collider2D_STATUE;
    [SerializeField] private GlitchController _GlitchController;
    public VariableDeterminator variableDeterminator;


    [SerializeField] private AudioSource voice;
    [SerializeField] private AudioClip[] voiceClips;
    private Coroutine mouthMovementCoroutine;
    private AchievementsManager _AchievementsManager;
    [SerializeField] private CanvasGroup DialogGroup;

    [SerializeField] private ColliderList _ColliderList;

    private bool mobileContinuePressed = false;

    private void Start()
    {
        try
        {
            _AchievementsManager = GameObject.Find("AchievementsManager").GetComponent<AchievementsManager>();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
        _ColliderList.allowOnOff = false;
        collider2D_STATUE.enabled = false;
        _PauseScript.findCollders = false;
        _ButtonFunctions.findCollders = false;
        _PauseScript.offMouse = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(FadeOutBack());
        InterfaceObj.SetActive(false);
        arrowsObj.SetActive(false);
        BlackImgCanvas = BlackImgObj.GetComponent<CanvasGroup>();
        flashCanvas = flashObj.GetComponent<CanvasGroup>();
        _fadein = true;

        UpdateDialogLang();

        initialFaceIndex = currentFaceIndex;
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
            maniac = "Ed Gein";

            textToType1 = "Ed Gein! You are charged with murder and other crimes! I will arrest you. And through this\nportal, we will be transported to the Timeless Court for justice!";
            textToType2 = "You're..you're just another illusion.";
            textToType3 = "What the..";
            textToType4 = "What was that just..";
            textToType5 = "There you are, \"Plainfield Ghoul\"...Oh my God, it's that human skin suit!";
            textToType6 = "P-P-Plainfield ghoul? You foul illusion....";
        }
        else if (language == 1)
        {
            faustText = "Фауст";
            maniac = "Эд Гейн";

            textToType1 = "Эд Гейн! Вы обвиняетесь в убийствах и других преступлениях! Я арестую Вас. И с помощью этого\nпортала мы перенесемся во Вневременной суд для осуществления правосудия!";
            textToType2 = "Ты...ты очередная иллюзия..";
            textToType3 = "Что за..";
            textToType4 = "Что это только что было..";
            textToType5 = "Вот ты какой, \"Плейнфилдский упырь\"...\nГосподи боже, это же тот костюм из человеческой кожи!";
            textToType6 = "П-п-плейнфилдский упырь? Ты, поганная иллюзия...";
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
                StartCoroutine(portalFlash());
            }
        }

        if (_fadeoutForHits == true)
        {
            if (BlackImgCanvas.alpha != 0)
            {
                BlackImgCanvas.alpha -= TimeToFade * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 0)
            {
                BlackImgObj.SetActive(false);
                _fadeoutForHits = false;
            }
        }
        if (_fadeinFlash == true)
        {
            if (flashCanvas.alpha < 1)
            {
                flashObj.SetActive(true);
                flashCanvas.alpha += TimeToFade2 * Time.deltaTime;
                portal.Play();
            }
            else if (flashCanvas.alpha == 1)
            {
                magots.SetActive(true);
                _fadeinFlash = false;
            }
        }
        if (_fadeoutFlash == true)
        {
            if (flashCanvas.alpha != 0)
            {
                flashCanvas.alpha -= TimeToFade2 * Time.deltaTime;
            }
            else if (flashCanvas.alpha == 0)
            {
                flashObj.SetActive(false);
                _fadeoutFlash = false;
            }
        }

        // Faust death
        if (_fadeinFlash2 == true)
        {
            if (BlackImgCanvas.alpha < 1)
            {
                BlackImgObj.SetActive(true);
                BlackImgCanvas.alpha += TimeToFade2 * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 1)
            {
                maniacOff.SetActive(false);
                maniacWithWeapon.SetActive(true);
                _fadeinFlash2 = false;
            }
        }
        if (_fadeoutFlash2 == true)
        {
            if (BlackImgCanvas.alpha != 0)
            {
                BlackImgCanvas.alpha -= TimeToFade2 * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 0)
            {
                _fadeoutFlash2 = false;
            }
        }

        if (_fadeinFlash3 == true)
        {
            if (BlackImgCanvas.alpha < 1)
            {
                BlackImgObj.SetActive(true);
                BlackImgCanvas.alpha += TimeToFade * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 1)
            {
                _fadeinFlash3 = false;
                Destroy(musicAmbientStart.gameObject);
                variableDeterminator.ShowDeathMenu();
            }
        }
        if (_fadeoutFlash3 == true)
        {
            if (BlackImgCanvas.alpha != 0)
            {
                BlackImgCanvas.alpha -= TimeToFade * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 0)
            {
                _fadeoutFlash3 = false;
                BlackImgObj.SetActive(false);
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
    private IEnumerator portalFlash()
    {
        yield return new WaitForSeconds(2);
        if (language == 0)
        {
            button_text1.text = "Read the accusation";
            button_text2.text = "Amazement";
        }
        else if (language == 1)
        {
            button_text1.text = "Зачитать обвинение";
            button_text2.text = "Изумление";
        }
        ShowButtons();
    }
    private IEnumerator TypeTextWin()
    {
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);

        voice.PlayOneShot(voiceClips[2]);

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
        voice.PlayOneShot(voiceClips[0]);
        text.text = "";
        speaker_name.text = maniac;
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
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(2);
        _fadeinFlash = true;
        yield return new WaitUntil(() => _fadeinFlash == false);
        yield return new WaitForSeconds(0.5f);
        _fadeoutFlash = true;
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);
        voice.PlayOneShot(voiceClips[3]);
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
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        BlackImgObj.SetActive(true);
        BlackImgCanvas.alpha = 1;
        yield return new WaitForSeconds(1.5f);
        magots.SetActive(false);
        maniacDead.SetActive(true);
        maniacOff.SetActive(false);
        hits.Play();
        yield return new WaitForSeconds(4.25f);
        portal.Play();
        yield return new WaitForSeconds(3);
        _fadeoutForHits = true;
        yield return new WaitUntil(() => _fadeoutForHits == false);
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);
        voice.PlayOneShot(voiceClips[4]);
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
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;

        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        if (_AchievementsManager != null)
        {
            _AchievementsManager.SetAchievement("achWin");
        }
        yield return new WaitForSeconds(2);
        ActiveEndScene.SetActive(true);
    }
    private IEnumerator TypeTextLose()
    {
        yield return new WaitForSeconds(1f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);
        voice.PlayOneShot(voiceClips[5]);
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
        speaker_name.text = maniac;
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
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        yield return new WaitForSeconds(2);
        _fadeinFlash2 = true;
        yield return new WaitUntil(() => _fadeinFlash2 == false);
        yield return new WaitForSeconds(1f);
        _fadeoutFlash2 = true;
        yield return new WaitUntil(() => _fadeoutFlash2 == false);
        yield return new WaitForSeconds(1f);
        BlackImgCanvas.alpha = 1f;
        deadFaust.SetActive(true);
        faustAlive.SetActive(false);
        yield return new WaitForSeconds(1f);
        FaustHitSnd.Play();
        yield return new WaitForSeconds(3f);
        _fadeoutFlash3 = true;
        yield return new WaitUntil(() => _fadeoutFlash3 == false);
        yield return new WaitForSeconds(1f);
        if (_AchievementsManager != null)
        {
            _AchievementsManager.SetAchievement("achLose");
        }
        _GlitchController._SetDeadFaustDeadGlitch();
        mCamera.transform.position = new Vector3(-1.2f, 17.2f, 0f);
        mCamera.orthographicSize = 2.18f;
        krikSnd.Play();
        yield return new WaitForSeconds(1.5f);
        _fadeinFlash3 = true;
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
        if (1 == btn_num)
        {
           StartCoroutine(TypeTextWin()); // победа
        }
        else
        {
           StartCoroutine(TypeTextLose()); // поражение
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
