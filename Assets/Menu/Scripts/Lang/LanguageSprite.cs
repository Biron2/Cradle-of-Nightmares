using UnityEngine;
using UnityEngine.UI;
public class LanguageSprite : MonoBehaviour
{
    public int language;
    public Image[] imagesToChangeSprite;
    [Header("0 - EN |  1 - RU")] 
    public Sprite[] sprites;
    void Start()
    {
        language = PlayerPrefs.GetInt("language", language);
        imagesToChangeSprite[0].sprite = sprites[language];
        imagesToChangeSprite[1].sprite = sprites[language];
    }
    public void SetSpriteNow()
    {
        language = PlayerPrefs.GetInt("language", language);
        imagesToChangeSprite[0].sprite = sprites[language];
        imagesToChangeSprite[1].sprite = sprites[language];
    }
}
