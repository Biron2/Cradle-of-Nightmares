using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
public class BlinkingEyes : MonoBehaviour
{
    public float fadeDuration = 2.0f;
    public float delayDuration = 5.0f; 
    private List<int> numbers;
    private System.Random random;
    int randomInt;

    [Header("Mask1")]
    public Light2D eye1;
    public Light2D eye2;
    [Header("Mask2")]
    public Light2D eye3;
    public Light2D eye4;
    [Header("Mask3")]
    public Light2D eye5;
    public Light2D eye6;
    void Start()
    {
        // Создаем список чисел от 1 до 3
        numbers = new List<int> { 1, 2, 3 };
        random = new System.Random();
    }
    public IEnumerator Loop()
    {
        while (true)
        {
            randomInt = GetRandomNumber();
            yield return new WaitForSeconds(delayDuration);
            yield return StartCoroutine(FadeIn());
            yield return new WaitForSeconds(delayDuration);
            yield return StartCoroutine(FadeOut());
            yield return new WaitForSeconds(delayDuration);
        }
    }
    private IEnumerator FadeIn()
    {
        float targetIntensity;
        float step;
        if (randomInt == 1)
        {
            targetIntensity = eye1.intensity + 10f;
            step = (targetIntensity - eye1.intensity) / 120;
            for (int i = 0; i < 120; i++)
            {
                eye1.intensity += step;
                eye2.intensity += step;
                yield return new WaitForSeconds(0.025f);
            }
        }
        else if (randomInt == 2)
        {
            targetIntensity = eye3.intensity + 10f;
            step = (targetIntensity - eye3.intensity) / 120;
            for (int i = 0; i < 120; i++)
            {
                eye3.intensity += step;
                eye4.intensity += step;
                yield return new WaitForSeconds(0.025f);
            }
        }
        else if (randomInt == 3)
        {
            targetIntensity = eye5.intensity + 10f;
            step = (targetIntensity - eye5.intensity) / 120;
            for (int i = 0; i < 120; i++)
            {
                eye5.intensity += step;
                eye6.intensity += step;
                yield return new WaitForSeconds(0.025f);
            }
        }
    }
    private IEnumerator FadeOut()
    {
        float targetIntensity;
        float step;

        if(randomInt == 1)
        {
            targetIntensity = eye1.intensity - 10f;
            step = (targetIntensity - eye1.intensity) / 120;
            for (int i = 0; i < 120; i++)
            {
                eye1.intensity += step;
                eye2.intensity += step;
                yield return new WaitForSeconds(0.025f);
            }
        }
        else if(randomInt == 2)
        {
            targetIntensity = eye3.intensity - 10f;
            step = (targetIntensity - eye3.intensity) / 120;
            for (int i = 0; i < 120; i++)
            {
                eye3.intensity += step;
                eye4.intensity += step;
                yield return new WaitForSeconds(0.025f);
            }
        }
        else if(randomInt == 3)
        {
            targetIntensity = eye5.intensity - 10f;
            step = (targetIntensity - eye5.intensity) / 120;
            for (int i = 0; i < 120; i++)
            {
                eye5.intensity += step;
                eye6.intensity += step;
                yield return new WaitForSeconds(0.025f);
            }
        }
    }
    int GetRandomNumber()
    {
        // Если в списке остались числа, выбираем случайное число из них
        if (numbers.Count > 0)
        {
            int index = random.Next(0, numbers.Count);
            int randomNumber = numbers[index];
            numbers.RemoveAt(index);
            return randomNumber;
        }

        // Если все числа уже были использованы, генерируем случайное число от 1 до 3
        return random.Next(1, 4);
    }
}