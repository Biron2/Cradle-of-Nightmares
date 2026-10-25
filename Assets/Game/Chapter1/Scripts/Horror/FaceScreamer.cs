using System.Collections;
using UnityEngine;

public class FaceScreamer : MonoBehaviour
{
    private RectTransform rectTransform;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 5f;
    public float growDuration = 1.0f;
    public float targetScale = 4.0f;
    [SerializeField] private EscClose escClose;
    public Sprite itemSprite;
    public InventoryManager inventoryManager;
    public AudioSource klyaksa;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(ShakeAndGrow());
        escClose.denyClose = true;
    }
    private IEnumerator ShakeAndGrow()
    {
        Vector3 originalPosition = rectTransform.anchoredPosition;
        Vector3 targetScaleVector = new Vector3(targetScale, targetScale, targetScale);
        Vector3 originalScale = rectTransform.localScale;

        float elapsed = 0f;
        while (elapsed < Mathf.Max(shakeDuration, growDuration))
        {
            // –асчет тр¤ски и увеличени¤
            if (elapsed < shakeDuration)
            {
                Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeMagnitude;
                rectTransform.anchoredPosition = randomPoint;
            }

            float t = elapsed / growDuration;
            rectTransform.localScale = Vector3.Lerp(originalScale, targetScaleVector, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
        rectTransform.localScale = targetScaleVector;

        // ƒополнительна¤ логика после окончани¤ анимации
        escClose.denyClose = false;
        if (inventoryManager.AddItemToInventory(itemSprite))
        {
            klyaksa.Play();
            Destroy(gameObject);
        }
    }
}