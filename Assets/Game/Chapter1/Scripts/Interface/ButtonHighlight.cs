using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    // ќпредел¤ем цвет, использу¤ Color.Lerp дл¤ более легкой настройки
    private Color mRed = new Color(0.812f, 0.267f, 0.267f); // —оответствие HEX #CF4444
    [SerializeField] private Text highlightedText;
    private void OnEnable()
    {
        highlightedText.color = Color.white;
    }
    private void OnMouseEnter()
    {
        // »змен¤ем цвет текста на mRed при наведении мыши
        highlightedText.color = mRed;
    }

    private void OnMouseExit()
    {
        // ¬озвращаем цвет текста в белый при выходе курсора мыши
        highlightedText.color = Color.white;
    }
}