using UnityEngine;
using UnityEngine.UI;
public class LanguageLargeText : MonoBehaviour
{
    public int language;
    [TextArea] public string[] text;
    private Text textLine;
    void Start()
    {
        language = PlayerPrefs.GetInt("language", language);
        textLine = GetComponent<Text>();
        textLine.text = "" + text[language];

#if PLAYMARKET
        if (gameObject.name == "tip2")
        {
            text[0] = "Rapid heartbeat causes heavy and loud breathing,\nwhich will make the Deviant hear and find you.\n\nTo avoid this, press the screen in time\nwhen the marker reaches the scale.";
            text[1] = "Учащенное сердцебиение вызывает усиленное и\nгромкое дыхание, из-за которого Девиант \nвас услышит и найдет.\n\nЧтобы избежать этого, вовремя нажимайте на экран,\nкогда отметка достигает шкалы.";
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
