using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartPreview2 : MonoBehaviour
{
    [SerializeField] private GameObject canvasmainObj;
    [SerializeField] private CanvasGroup canvasmain;

    private bool _fadein = false, allowMove = false;
    public float TimeToFade;
    public AudioSource audioSource; // Ссылка на компонент AudioSource
    public float fadeDuration = 2f; // Продолжительность появления музыки
    [SerializeField] private GameObject startDialog;
    public int language;

    public float speedMove = 1f; // Скорость перемещения
    public Transform cameraTransform;
    private Vector3 targetPosition; // Целевая позиция

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Запускаем корутину автопечати текста
        StartCoroutine(TypeText());

        audioSource.volume = 0f;
        StartCoroutine(FadeIn());

        targetPosition = new Vector3(cameraTransform.transform.position.x, -65.5f, cameraTransform.transform.position.z); // Установка целевой позиции на оси X
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
                    canvasmainObj.SetActive(false);
                    allowMove = true;
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
                Invoke(nameof(startDiaMethod), 1.5f);
            }
        }
    }
    private void startDiaMethod()
    {
        startDialog.SetActive(true);
    }

    private IEnumerator TypeText()
    {
        _fadein = true;
        yield return new WaitForSeconds(1);
        try
        {
            Destroy(GameObject.Find("startSnd_FROM_MENU"));
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private IEnumerator FadeIn()
    {
        float targetVolume = 0.60f; // Целевая громкость звука
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
}