using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class flashingPortal : MonoBehaviour
{
    public Image image;
    public float fadeDuration = 1f;

    private void Start()
    {
        StartCoroutine(CycleAlpha());
    }

    private IEnumerator CycleAlpha()
    {
        while (true)
        {
            yield return StartCoroutine(FadeAlpha(1f, 0.65f));
            yield return StartCoroutine(FadeAlpha(0.65f, 1f));
        }
    }

    private IEnumerator FadeAlpha(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            Color color = image.color;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            color.a = alpha;
            image.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Color finalColor = image.color;
        finalColor.a = endAlpha;
        image.color = finalColor;
    }
}