using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigamesRandomizer : MonoBehaviour
{
    [SerializeField] private Image[] containers;
    [SerializeField] private Sprite[] sprites;

    void Start()
    {
        ShuffleSprites();
    }

    void ShuffleSprites()
    {
        if (containers.Length != sprites.Length)
        {
            Debug.LogError("The number of containers and sprites must be the same.");
            return;
        }

        // Создаем список индексов спрайтов
        List<int> spriteIndexes = new List<int>();
        for (int i = 0; i < sprites.Length; i++)
        {
            spriteIndexes.Add(i);
        }

        // Перемешиваем индексы с условием, чтобы ни один индекс не остался на своем месте
        bool isValidShuffle = false;

        while (!isValidShuffle)
        {
            // Перемешиваем список индексов с методом Фишера-Йейтса
            for (int i = 0; i < spriteIndexes.Count; i++)
            {
                int randomIndex = Random.Range(i, spriteIndexes.Count);
                int temp = spriteIndexes[i];
                spriteIndexes[i] = spriteIndexes[randomIndex];
                spriteIndexes[randomIndex] = temp;
            }

            // Проверяем, что ни один спрайт не остался на своем месте
            isValidShuffle = true;
            for (int i = 0; i < spriteIndexes.Count; i++)
            {
                if (spriteIndexes[i] == i)
                {
                    isValidShuffle = false;
                    break;
                }
            }
        }

        // Устанавливаем спрайты на контейнеры
        for (int i = 0; i < containers.Length; i++)
        {
            containers[i].sprite = sprites[spriteIndexes[i]];
        }
    }
}