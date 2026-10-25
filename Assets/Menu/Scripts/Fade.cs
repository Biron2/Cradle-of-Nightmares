using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [Header("BLACK SCREEN")]
    [SerializeField] private CanvasGroup black_background;
    [SerializeField] private GameObject black_backgroundObj;

    [Header("NEEDED OBJECT")]
    [SerializeField] private GameObject objectToOn;

    private bool _fadein = false, _fadeout = false, _fadein2 = false, allowEscape = false, dopAllowEscape = false;

    public Button button, YTbtn, tgBtn;

    public float TimeToFade;
    [SerializeField] private Settings _Settings;

    public bool allowEsc = true;
    [SerializeField] private RectTransform _SmokePos;

    public void startFadeIn()
    {
        _fadein = true;
        allowEscape = false;
        dopAllowEscape = true;
    }
    public void startFadeOut()
    {
        _fadein2 = true;
        allowEscape = false;
        dopAllowEscape = false;
    }

    void Update()
    {
        if(allowEscape == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && allowEsc)
            {
                startFadeOut();
            }
        }
        //FADE IN - START
        if (_fadein == true)
        {
            if (black_background.alpha >= 0)
            {
                black_backgroundObj.SetActive(true);

                if (YTbtn != null)
                {
                    YTbtn.interactable = false;
                }
                if (tgBtn != null)
                {
                    tgBtn.interactable = false;
                }
                button.interactable = false;
                black_background.alpha += TimeToFade * Time.deltaTime;
                if(black_background.alpha == 1)
                {
                    _fadeout = true;
                    _fadein = false;
                    objectToOn.SetActive(true);
                    _SmokePos.anchoredPosition = Vector2.zero;
                    _Settings.lampSound.volume = 0f;
                }
            }
        }
        if (_fadeout == true)
        {
            black_background.alpha -= TimeToFade * Time.deltaTime;

            if (black_background.alpha == 0)
            {
                if (YTbtn != null)
                {
                    YTbtn.interactable = true;
                }
                if (tgBtn != null)
                {
                    tgBtn.interactable = true;
                }
                button.interactable = true;
                _fadeout = false;
                black_backgroundObj.SetActive(false);
                if (dopAllowEscape == true)
                {
                    allowEscape = true;
                }
            }
        }
        //FADE IN - END

        //FADE OUT - START
        if (_fadein2 == true)
        {
            if (black_background.alpha >= 0)
            {
                black_backgroundObj.SetActive(true);
                button.interactable = true;
                if (YTbtn != null)
                {
                    YTbtn.interactable = false;
                }
                if (tgBtn != null)
                {
                    tgBtn.interactable = false;
                }
                button.interactable = false;
                black_background.alpha += TimeToFade * Time.deltaTime;
                if (black_background.alpha == 1)
                {
                    _fadeout = true;
                    _fadein2 = false;
                    objectToOn.SetActive(false);
                    _SmokePos.anchoredPosition = new Vector2(0f, -390f);
                    _Settings.isAnimating = false;
                    _Settings.lampSound.volume = 0.3f;
                }
            }
        }
        //FADE OUT - END
    }

}
