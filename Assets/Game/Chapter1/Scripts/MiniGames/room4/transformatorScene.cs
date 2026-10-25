using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transformatorScene : MonoBehaviour
{
    [SerializeField] private GameObject BlackImgObj, mInterface, lightPortal, light2d;
    [SerializeField] private CanvasGroup BlackImgCanvas;
    public float TimeToFade;
    private bool _fadein, _fadeout = false, _fadein2, _fadeout2 = false;
    [SerializeField] private Transform cameraObj;
    private ColliderList colliderList;
    [SerializeField] private GameObject[] arrows_containers;
    [SerializeField] private ManiacMove _ManiacMove;
    [SerializeField] private public_actions public_;
    [SerializeField] private VariableDeterminator variableDeterminator;
    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        _fadein = true;
        _ManiacMove.AllowMovement = false;
        variableDeterminator.stopPlayLight += 1;

#if PLAYMARKET
        FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
    }
    void Update()
    {
        if (_fadein == true)
        {
            if (BlackImgCanvas.alpha < 1)
            {
                foreach (GameObject obj in arrows_containers)
                {
                    if (obj != null)
                    {
                        obj.SetActive(false);
                    }
                }
                mInterface.SetActive(false);
                BlackImgObj.SetActive(true);
                BlackImgCanvas.alpha += TimeToFade * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 1)
            {
                cameraObj.position = new Vector3(0f, 20f, 0f);
                _fadein = false;
                _fadeout = true;
                lightPortal.SetActive(true);
                ParticleSystem pcs = lightPortal.transform.Find("FireEmbersPortal").GetComponent<ParticleSystem>();
                pcs.Play();
                light2d.SetActive(true);
                Invoke(nameof(disableLightPortalMethod), 3f);
            }
        }
        if (_fadeout == true)
        {
            if (BlackImgCanvas.alpha != 0)
            {
                BlackImgCanvas.alpha -= TimeToFade * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 0)
            {
                BlackImgObj.SetActive(false);
                _fadeout = false;
                Invoke(nameof(waitMethod), 4);
            }
        }

        if (_fadein2 == true)
        {
            if (BlackImgCanvas.alpha < 1)
            {
                BlackImgObj.SetActive(true);
                BlackImgCanvas.alpha += TimeToFade * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 1)
            {
                light2d.SetActive(false);
                cameraObj.position = new Vector3(35.554f, -10f, 0f);
                _fadein2 = false;
                _fadeout2 = true;
            }
        }
        if (_fadeout2 == true)
        {
            if (BlackImgCanvas.alpha != 0)
            {
                BlackImgCanvas.alpha -= TimeToFade * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 0)
            {
                _ManiacMove.AllowMovement = true;
                BlackImgObj.SetActive(false);
                _fadeout2 = false;
                colliderList.ENABLE_COLLIDER();
                mInterface.SetActive(true);
                foreach (GameObject obj in arrows_containers)
                {
                    if (obj != null)
                    {
                        obj.SetActive(true);
                    }
                }
            }
        }
    }

    private void disableLightPortalMethod()
    {
        lightPortal.SetActive(false);
        public_.stopElectroRoom10();
    }

    private void waitMethod()
    {
        _fadein2 = true;
    }
}
