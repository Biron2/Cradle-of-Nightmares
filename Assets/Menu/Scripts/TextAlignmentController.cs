using UnityEngine;
using UnityEngine.UI;

public class TextAlignmentController : MonoBehaviour
{
    private Text textComponent; // Ссылка на компонент текста

    void Start()
    {
        // Получаем компонент текста
        textComponent = GetComponent<Text>();

        // Проверяем, превышает ли текст ширину родительской рамки
        if (textComponent.text.Length >= 16) // Небольшой допуск
        {
            // Устанавливаем выравнивание "слева"
            textComponent.alignment = TextAnchor.MiddleLeft;
        }
        else
        {
            // Устанавливаем выравнивание "по центру"
            textComponent.alignment = TextAnchor.MiddleCenter;
        }
    }
}
