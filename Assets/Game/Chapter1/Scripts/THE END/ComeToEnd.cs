using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComeToEnd : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasmain;
    [SerializeField] private GameObject canvasmainObj, interfaceObj;

    private bool _fadein = false;
    public float TimeToFade;
    public float fadeDuration = 2f; // Продолжительность затухания
    public AudioSource audioSource; // Ссылка на компонент AudioSource
    private void Start()
    {
        _fadein = true;
        StartCoroutine(FadeOut());
        interfaceObj.SetActive(false);
    }

    void Update()
    {
        if (_fadein)
        {
            if (canvasmain.alpha >= 0)
            {
                canvasmainObj.SetActive(true);
                canvasmain.alpha += TimeToFade * Time.deltaTime;
                if (canvasmain.alpha == 1)
                {
                    _fadein = false;
                    SceneManager.LoadScene("Chapter1End");
                }
            }
        }
    }

    // Метод для затухания звука
    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
