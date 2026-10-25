using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasgroup, canvasgroup2;

    private bool _fadein = false;

    [SerializeField] private GameObject object1, object2;

    public float TimeToFade;
    public float TimeToFade2;

    public void StartFadeOut()
    {
        _fadein = true;
    }

    void Update()
    {
        if (_fadein)
        {
            if (canvasgroup.alpha > 0)
            {
                canvasgroup.alpha -= TimeToFade * Time.deltaTime;
                canvasgroup2.alpha -= TimeToFade2 * Time.deltaTime;
                if (canvasgroup.alpha <= 0)
                {
                    object1.SetActive(false);
                    object2.SetActive(false);
                    _fadein = false;
                }
            }
        }
    }

}
