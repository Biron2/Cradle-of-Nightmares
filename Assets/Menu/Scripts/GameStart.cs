using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasmain;
    [SerializeField] private GameObject canvasmainObj;

    private bool _fadein = false;
    public float TimeToFade;
    public float fadeDuration = 2f; // Продолжительность затухания
    public AudioSource audioSource; // Ссылка на компонент AudioSource
    private int chapterNumVar = 1;

    public void StartGameScene(int chapterNum)
    {
        chapterNumVar = chapterNum;
        _fadein = true;
        StartCoroutine(FadeOut());
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
                    switch (chapterNumVar)
                    {
                        case 1:
                            SceneManager.LoadScene("Preview1");
                            break;
                        case 2:
                            SceneManager.LoadScene("Preview2");
                            break;
                        case 3:
                            //SceneManager.LoadScene("Preview3");
                            break;
                        default:
                            SceneManager.LoadScene("Preview1");
                            break;
                    }
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
