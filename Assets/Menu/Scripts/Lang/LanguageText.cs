using UnityEngine;
using UnityEngine.UI;
public class LanguageText : MonoBehaviour
{
    public int language;
    public string[] text;
    private Text textLine;
    void Start()
    {
        language = PlayerPrefs.GetInt("language", language);
        textLine = GetComponent<Text>();
        textLine.text = "" + text[language];

#if PLAYMARKET
        if (gameObject.name == "tip1")
        {
            text[0] = "Use the arrows to scroll";
            text[1] = "Используйте стрелки для прокрутки";
            SetTextNow();
        }
#endif
    }
    public void SetTextNow()
    {
        language = PlayerPrefs.GetInt("language", language);
        textLine = GetComponent<Text>();
        textLine.text = "" + text[language];
    }
}
