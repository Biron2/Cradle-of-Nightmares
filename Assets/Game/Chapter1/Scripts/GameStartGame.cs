using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameStartGame : MonoBehaviour
{
    [SerializeField] private GameObject canvasmainObj;
    [SerializeField] private CanvasGroup canvasmain;

    private bool _fadein = false, allowMove = false;
    public float TimeToFade;
    public Text text;
    private string textToType;
    public float typingSpeed = 0.05f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    public AudioSource audioSource; // Ссылка на компонент AudioSource
    public float fadeDuration = 2f; // Продолжительность появления музыки
    [SerializeField] private GameObject TextContainer, startDialog;
    [SerializeField] private GameObject contunieObj, _PC_continue, _mobile_continue;
    [SerializeField] public AudioSource voice;
    [SerializeField] public AudioClip[] voiceClips;
    public int language;

    public float speedMove = 1f; // Скорость перемещения
    public Transform cameraTransform;
    private Vector3 targetPosition; // Целевая позиция
    public float targetSize = 20.0f;
    public Camera camera1;
    private float transitionStartTime;
    public float transitionDuration = 1.0f;

    [SerializeField] private GameObject pauseMenu;

    private bool mobileContinuePressed = false;

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        UpdateDialogLang();

        text.text = "";

        // Запускаем корутину автопечати текста
        StartCoroutine(TypeText());

        audioSource.volume = 0f;
        StartCoroutine(FadeIn());

        targetPosition = new Vector3(0f, cameraTransform.transform.position.y, cameraTransform.transform.position.z); // Установка целевой позиции на оси X
    }
    public void UpdateDialogLang()
    {
        language = PlayerPrefs.GetInt("language", language);
        if (language == 0)
        {
            textToType = "and to save innocent souls,\nwe again sacrifice our own...";
        }
        else if (language == 1)
        {
            textToType = "и чтобы спасти невинные души,\nмы снова жертвуем своими...";
        }
    }
    void Update()
    {
        if (_fadein == true)
        {

            if (canvasmain.alpha > 0)
            {
                canvasmain.alpha -= TimeToFade * Time.deltaTime;
                if (canvasmain.alpha == 0)
                {
                    _fadein = false;
                    TextContainer.SetActive(false);
                    canvasmainObj.SetActive(false);
                }
            }
        }

        if (allowMove == true)
        {
            Vector3 newPosition = Vector3.MoveTowards(cameraTransform.transform.position, targetPosition, speedMove * Time.deltaTime);
            cameraTransform.transform.position = newPosition;
            if (cameraTransform.transform.position == targetPosition)
            {
                allowMove = false;
                StartCoroutine(TransitionCamera());
                Invoke(nameof(startDiaMethod), 3f);
            }
        }
    }

    private IEnumerator TransitionCamera()
    {
        transitionStartTime = Time.time;
        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            camera1.orthographicSize = Mathf.Lerp(48f, targetSize, t);
            yield return null;
        }
        camera1.orthographicSize = targetSize;
    }
    private void startDiaMethod()
    {
        startDialog.SetActive(true);
    }

    private IEnumerator FadeOutText()
    {
        Color startColor = text.color;
        Color transparentColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float elapsedTime = 0f;
        while (elapsedTime < TimeToFade)
        {
            // Интерполируем значение прозрачности от начального цвета до полностью прозрачного цвета
            Color newColor = Color.Lerp(startColor, transparentColor, elapsedTime / TimeToFade);

            // Применяем новый цвет к компоненту Text
            text.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator TypeText()
    {
        yield return new WaitForSeconds(1);
        voice.PlayOneShot(voiceClips[0]);
        foreach (char character in textToType)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            // Ждём указанное время перед следующим символом
            yield return new WaitForSeconds(typingSpeed);
        }

        // Печать текста закончена, изменяем значение _fadein на true
        yield return new WaitUntil(() => !voice.isPlaying);
        try
        {
            Destroy(GameObject.Find("startSnd_FROM_MENU"));
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        contunieObj.SetActive(true);
#if PLAYMARKET
        _mobile_continue.SetActive(true);
#else
        _PC_continue.SetActive(true);
#endif

        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        mobileContinuePressed = false;
        contunieObj.SetActive(false);
        _PC_continue.SetActive(false);
        _mobile_continue.SetActive(false);
        StartCoroutine(FadeOutText());
        _fadein = true;
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        allowMove = true;
    }

    private IEnumerator FadeIn()
    {
        float targetVolume = 0.50f; // Целевая громкость звука
        float startVolume = 0; // Начальная громкость звука

        float elapsedTime = 0; // Время, прошедшее с начала плавного увеличения громкости

        while (audioSource.volume < targetVolume)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; // Прогресс плавного увеличения громкости

            // Используйте другую функцию интерполяции для медленного прибавления звука
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t * t); // Квадратичная функция интерполяции
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void MobilePress()
    {
        if (gameObject.activeSelf)
        {
            mobileContinuePressed = true;
        }
    }
}