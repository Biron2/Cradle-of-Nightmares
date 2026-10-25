using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtitles : MonoBehaviour
{
    public Text text;
    private string textToType1, textToType2, textToType3, textToType4, textToType5, textToType6;
    public float typingSpeed = 0.20f;
    public float waitTime = 1f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    [SerializeField] private GameObject TextContainer_dia;
    [SerializeField] private AudioSource comicsAudio;
    [SerializeField] private GameStartGame _gSg;
    public int language;

    private void Start()
    {

        language = PlayerPrefs.GetInt("language", language);
        UpdateDialogLang();


        TextContainer_dia.SetActive(true);
        text.text = "";
    }
    public void UpdateDialogLang()
    {
        if (language == 0)
        {
            textToType1 = "50s. USA.";
            textToType2 = "As the residents of Plainfield, Wisconsin, drifted into\nslumber, Ed Gein, a murderer and body snatcher, was\ndigging up another woman's grave.";
            textToType3 = "He was most interested in women who reminded him of his\ndead mother.";
            textToType4 = "Now he will bring this corpse to his sinister mansion,\ndismember it, and skin it to make himself a new product.";
            textToType5 = "I have a good idea of what awaits me in that house.";
            textToType6 = "I must gather evidence of his crimes and arrest his soul\nas soon as possible...";
        }
        else if (language == 1)
        {
            textToType1 = "50-е годы. США.";
            textToType2 = "В то время как жители Плейнфилда, поселения, что находится\nв штате Висконсин, погружались в сон, Эд Гейн, убийца и\nпохититель трупов, выкапывал очередную женскую могилу.";
            textToType3 = "Больше всего его интересовали женщины, которые напоминали\nему его погибшую мать.";
            textToType4 = "Сейчас он принесёт этот труп в свой зловещий особняк,\nрасчленит и снимет кожу, чтобы изготовить себе новое\nизделие.";
            textToType5 = "Я хорошо представляю, что ожидает меня в этом доме.";
            textToType6 = "Я должен собрать доказательства Его преступлений\nи арестовать Его душу как можно скорее.";
        }
    }
    public IEnumerator TypeText()
    {
        yield return new WaitForSeconds(waitTime);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[17]);

        foreach (char character in textToType1)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitUntil(() => !_gSg.voice.isPlaying);

        yield return new WaitForSeconds(waitTime);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[18]);

        text.text = "";
        yield return new WaitForSeconds(typingSpeed);
        foreach (char character in textToType2)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitUntil(() => !_gSg.voice.isPlaying);

        yield return new WaitForSeconds(waitTime);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[19]);

        text.text = "";
        yield return new WaitForSeconds(typingSpeed);
        foreach (char character in textToType3)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitUntil(() => !_gSg.voice.isPlaying);

        yield return new WaitForSeconds(waitTime);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[20]);

        text.text = "";
        yield return new WaitForSeconds(typingSpeed);
        foreach (char character in textToType4)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitUntil(() => !_gSg.voice.isPlaying);

        yield return new WaitForSeconds(waitTime);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[21]);

        text.text = "";
        yield return new WaitForSeconds(typingSpeed);
        foreach (char character in textToType5)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitUntil(() => !_gSg.voice.isPlaying);

        yield return new WaitForSeconds(waitTime);

        _gSg.voice.PlayOneShot(_gSg.voiceClips[22]);

        text.text = "";
        yield return new WaitForSeconds(typingSpeed);
        foreach (char character in textToType6)
        {
            // Проверяем флаг, если печать текста закончена, выходим из цикла
            if (!_isTyping)
                yield break;

            // Добавляем новый символ к тексту
            text.text += character;

            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(waitTime);

        TextContainer_dia.SetActive(false);
    }
}
