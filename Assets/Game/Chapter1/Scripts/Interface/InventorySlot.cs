using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage;
    public bool isEmpty = true;
    internal Sprite itemSprite;
    public int slotIndex;

    public void SetItem(Sprite itemSprite, int index)
    {
        itemImage.sprite = itemSprite;
        this.itemSprite = itemSprite;
        isEmpty = false;
        itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, 255);
        slotIndex = index;

        try
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(PulseEffect());
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void ClearItem()
    {
        itemImage.sprite = null;
        itemSprite = null;
        isEmpty = true;
        itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, 0);
    }

    private void OnEnable()
    {
        itemImage.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    private System.Collections.IEnumerator PulseEffect()
    {
        Vector3 originalScale = itemImage.rectTransform.localScale;

        // Первый пульс (уменьшил силу)
        yield return Pulse(originalScale, 1.2f, 0.25f);

        // Второй мягкий
        yield return Pulse(originalScale, 1.15f, 0.2f);

        // Третий более сглаженный с лёгким отдалением
        yield return PulseWithBack(originalScale, 1.1f, 0.97f, 0.18f);

        // Вернуть исходный размер
        itemImage.rectTransform.localScale = originalScale;
    }

    private System.Collections.IEnumerator Pulse(Vector3 originalScale, float scaleMultiplier, float duration)
    {
        Vector3 targetScale = originalScale * scaleMultiplier;
        float elapsedTime = 0f;

        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / (duration / 2));
            itemImage.rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / (duration / 2));
            itemImage.rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
    }

    private System.Collections.IEnumerator PulseWithBack(Vector3 originalScale, float scaleMultiplier, float backMultiplier, float duration)
    {
        Vector3 targetScale = originalScale * scaleMultiplier;
        Vector3 backScale = originalScale * backMultiplier;
        float elapsedTime = 0f;

        while (elapsedTime < duration / 3)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / (duration / 3));
            itemImage.rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < duration / 3)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / (duration / 3));
            itemImage.rectTransform.localScale = Vector3.Lerp(targetScale, backScale, t);
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < duration / 3)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / (duration / 3));
            itemImage.rectTransform.localScale = Vector3.Lerp(backScale, originalScale, t);
            yield return null;
        }
    }

}
