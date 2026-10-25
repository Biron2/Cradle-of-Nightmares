using System.Collections;
using UnityEngine;

public class dragHeart : MonoBehaviour
{
    private RectTransform rectTransform;
    private AudioSource pulseSound;
    public AudioClip firstPulse, secondPulse;

    private Vector3 originalScale;
    private Coroutine pulseCoroutine;
    private float pulseSpeed = 1.0f;
    private bool isFirstPulse = true;
    
    private void Awake()
    {
        pulseSound = GetComponent<AudioSource>();
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }
    private void OnEnable()
    {
        if(!VariableDeterminator.shotedByManiac) StartPulsating();
    }

    private void OnDisable()
    {
        StopPulsating();
    }

    private void StartPulsating()
    {
        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);

        if (gameObject.activeSelf == true)
        {
            pulseCoroutine = StartCoroutine(PulseRoutine());
        }
    }

    private void StopPulsating()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
        }

        rectTransform.localScale = originalScale;
    }

    private IEnumerator PulseRoutine()
    {
        while (true)
        {
            yield return ScaleOverTime(originalScale, originalScale * 1.2f, 0.1f / pulseSpeed);

            yield return ScaleOverTime(originalScale * 1.2f, originalScale * 1.07f, 0.2f / pulseSpeed);

            if (pulseSound != null)
            {
                pulseSound.clip = isFirstPulse ? firstPulse : secondPulse;
                pulseSound.PlayOneShot(pulseSound.clip);
                isFirstPulse = !isFirstPulse;
            }
            yield return ScaleOverTime(originalScale * 1.07f, originalScale, 0.4f / pulseSpeed);
        }
    }

    private IEnumerator ScaleOverTime(Vector3 fromScale, Vector3 toScale, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 baseScale = Vector3.Lerp(fromScale, toScale, elapsedTime / duration);

            float distortionFactor = Mathf.Sin(elapsedTime / duration * Mathf.PI) * 0.1f;
            rectTransform.localScale = new Vector3(
                baseScale.x + distortionFactor,
                baseScale.y + distortionFactor * 2f,
                baseScale.z
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = toScale;
    }

    public void SetPulseSpeed(float speed)
    {
        pulseSpeed = speed;
        StartPulsating();
    }
}