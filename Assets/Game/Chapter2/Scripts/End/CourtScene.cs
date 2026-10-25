using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CourtScene : MonoBehaviour
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
    [SerializeField] private GameObject _PC_continue, _mobile_continue;

    public Text text, speaker_name;
    private string vilenaText;

    private string textToType1, textToType2, textToType3, textToType4, textToType5;
    
    public float typingSpeed = 0.05f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    [SerializeField] private GameObject TextContainer_dia;

    public float fadeDuration = 1f; // Продолжительность затухания
    [SerializeField] private AudioSource back_music, extraAmbient;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private ActivePauseAfk _ActivePauseAfk;
    [SerializeField] private PauseScript _PauseScript;

    [SerializeField] private AudioSource voice;
    [SerializeField] private AudioClip[] voiceClips;
    private Coroutine mouthMovementCoroutine, typeTextCoroutine;

    private Camera _camera;

    [SerializeField] private CanvasGroup DialogGroup;

    private bool mobileContinuePressed = false;

    private void Start()
    {
        _camera = GetComponent<Camera>();
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

            textToType1 = "Tsutomu Miyazaki! You stand before the Timeless Court — a court that delivers justice to the\ncriminals of the past. No murderer can escape this court, no matter how much time has passed\nsince the crime. The deeds of killers know no statute of limitations.";
            textToType2 = "You kidnapped and murdered four children. You not only took their lives, but also stole from\nthem the chance to grow, to love, to experience the joys and sorrows of life. You robbed those\ngirls of their most precious gift — life, and their parents — of their children.";
            textToType3 = "Your hand did not stop at murder. You continued your atrocities: storing body fragments, sending\nthem to the parents, and watching their suffering with perverse satisfaction.";
            textToType4 = "You were fully aware of your actions. That is why there can be no mercy. I sentence you to\neternal confinement in the Cognitive Isolator, with no right of appeal.";
            textToType5 = "Your mind will be subject to study in order to better understand deviant behavior and identify\nsimilar threats in the future. This ruling takes effect immediately. Court is adjourned.";
        }
        else if (language == 1)
        {
            vilenaText = "Вилена Сергеевна";
          
            textToType1 = "Цутому Миядзаки! Вы предстали перед Вневременным Судом — судом, который вершит правосудие над\nпреступниками прошлого. От этого суда не скроется ни один убийца, сколько бы времени ни прошло\nс момента преступления. У деяний душегубов нет срока давности.";
            textToType2 = "Вы похитили и убили четырёх детей. Вы не только отняли у них жизни, но и лишили возможности\nвырасти, любить, познавать радости и печали этой жизни. Вы отняли у этих девочек самое\nдорогое — жизнь, а у их родителей — детей.";
            textToType3 = "Ваша рука не остановилась на убийстве. Вы продолжили творить злодеяния: хранили фрагменты тел,\nотправляли их родителям, наблюдали за их страданиями с извращённым удовлетворением.";
            textToType4 = "Вы осознавали, что делаете. Именно поэтому пощады вам быть не может. Я приговариваю вас\nк вечному пребыванию в Когнитивном Изоляторе без права апелляции.";
            textToType5 = "Ваш разум будет подвергнут исследованию для лучшего понимания девиантного поведения и\nсвоевременного выявления подобных угроз в будущем. Решение вступает в юридическую силу\nнемедленно. Суд окончен.";
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
        SceneManager.LoadScene("Chapter2FaustRoom");
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
        yield return new WaitForSeconds(3f);

        Vector3 targetPosition = new Vector3(0f, -1f, 0f);
        float targetSize = 3f;

        float duration = 4f;
        float elapsed = 0f;

        Vector3 startPosition = _camera.transform.position;
        float startSize = _camera.orthographicSize;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Плавное движение и масштаб (можно заменить на Mathf.SmoothStep для сглаживания)
            _camera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            _camera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        // Гарантируем точное значение в конце
        _camera.transform.position = targetPosition;
        _camera.orthographicSize = targetSize;

        yield return new WaitForSeconds(2f);
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);

        voice.PlayOneShot(voiceClips[0]);

        text.text = "";
        speaker_name.text = vilenaText;
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
        speaker_name.text = vilenaText;
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
        speaker_name.text = vilenaText;
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
        speaker_name.text = vilenaText;
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