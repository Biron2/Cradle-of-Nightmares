using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackImage;
    [SerializeField] private GameObject blackImageObj;
    private bool _fadeoutBlack = false;
    [SerializeField] private BlinkingEyes _BlinkingEyes;
    [SerializeField] private Material mat;
    [SerializeField] private Settings _Settings;

    public float TimeToFade;
    [SerializeField] private CanvasGroup canvasTip;

    [SerializeField] private VideoPlayer smokeVideo;
    private void Start()
    {
        // Подпишемся на событие окончания подготовки
        smokeVideo.prepareCompleted += OnVideoPrepared;

        // Начнем подготовку видео (оно ещё не играет)
        smokeVideo.Prepare();

        Time.timeScale = 1;
        AudioListener.pause = false;
        mat.SetFloat("_NoiseAmount", 0f);
        mat.SetFloat("_GlitchStrength", 0f);
        mat.SetFloat("_ScanLineStrenght", 1f);
    }
    private void OnVideoPrepared(VideoPlayer vp)
    {
        // Можно запускать только после полной подготовки
        vp.Play();

        // Если хочешь узнать, когда начнёт воспроизводиться:
        StartCoroutine(WaitForVideoStart(vp));
    }

    private IEnumerator WaitForVideoStart(VideoPlayer vp)
    {
        // Ждем, пока видео реально начнет играть
        while (!vp.isPlaying)
        {
            yield return null;
        }

        _fadeoutBlack = true; // дым начал воспроизводится
    }

    private void Update()
    {
        if (_fadeoutBlack)
        {
            if (blackImage.alpha != 0)
            {
                blackImage.alpha -= TimeToFade * Time.deltaTime;
                if (blackImage.alpha == 0)
                {
                    _fadeoutBlack = false;
                    blackImageObj.SetActive(false);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    StartCoroutine(_BlinkingEyes.Loop());

                    _Settings.isAnimating = false;
                    try
                    {
                        Destroy(GameObject.Find("Disclamer"));
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                    if (PlayerPrefs.GetInt("onceTip") != 3)
                    {
#if !PLAYMARKET
                        StartCoroutine(SmoothCanvasGroup(canvasTip, 2, 1));
#endif
                    }
                }
            }
        }
    }
    private IEnumerator SmoothCanvasGroup(CanvasGroup canvasGroup, float duration, float targetAlpha)
    {
        PlayerPrefs.SetInt("onceTip", 3);
        canvasGroup.gameObject.SetActive(true);
        // Ќачальное значение alpha
        float startAlpha = canvasGroup.alpha;

        // ¬рем¤, прошедшее с начала изменени¤
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Ћинейна¤ интерпол¤ци¤ между стартовым и конечным значением
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            yield return null; // ∆дем следующий кадр
        }

        // ”бедитьс¤, что значение alpha точно равно конечной цели
        canvasGroup.alpha = targetAlpha;
        yield return new WaitForSeconds(3);
        if (canvasGroup.alpha > 0) // Проверяем перед запуском
        {
            StartCoroutine(SmoothCanvasGroup(canvasTip, 2, 0));
        }
        else
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }
}