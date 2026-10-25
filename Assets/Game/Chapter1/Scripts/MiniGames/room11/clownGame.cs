using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clownGame : MonoBehaviour
{
    private bool allowScalePressing = true;
    [SerializeField] private GameObject[] arrows_containers;
    [SerializeField] private GameObject insideSwitchGame, paperGive;
    public Image mNumber1, mNumber2, mNumber3, mNumber4, mNumber5, mNumber6, mNumber7, mNumber8, mNumber9;
    private List<int> buttonSequence = new List<int>(); // —писок выбранных кнопок
    private int expectedButton = 1; // ќжидаема¤ кнопка
    [SerializeField] private AudioSource tapSnd, winMiniGameSnd, loseMiniGameSnd;
    private ColliderList colliderList;

    private audio_for_items _Audio_For_Items;
    private float cooldown = 0.5f; // ¬рем¤ задержки в секундах
    private float lastClickTime = 0f; // ¬рем¤ последнего нажати¤
    private void Start()
    {
        _Audio_For_Items = GameObject.Find("Other/GameManager/EtcSounds/__FOR_ITEMS__").GetComponent<audio_for_items>();
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
    }
    void Update()
    {
        // --- ПК: клик мышью ---
        if (Input.GetMouseButtonDown(0))
        {
            CheckHit(Camera.main.ScreenPointToRay(Input.mousePosition));
        }

        // --- Телефон: касание ---
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                CheckHit(Camera.main.ScreenPointToRay(touch.position));
            }
        }
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (Time.time - lastClickTime < cooldown)
            {
                // ¬рем¤ после последнего нажати¤ меньше задержки, поэтому ничего не делаем
                return;
            }
            lastClickTime = Time.time; // ќбновл¤ем врем¤ последнего нажати¤
            if (allowScalePressing == true)
            {
                _Audio_For_Items.PlaySounds(gameObject.name);
                insideSwitchGame.SetActive(true);
                foreach (GameObject obj in arrows_containers)
                {

                    if (obj != null)
                    {
                        obj.SetActive(false);
                    }
                }
                public_actions.Instance.OnOff_SpaceTip_Close(true);
                colliderList.DISABLE_COLLIDER();
                VariableDeterminator.isMiniGameActive = true;
            }
        }
    }

    public void Press_Number1()
    {
        if (mNumber1.color == Color.white)
        {
            tapSnd.Play();
            if (1 == expectedButton)
            {
                buttonSequence.Add(1);
                expectedButton++;
                mNumber1.color = Color.yellow;
                if (expectedButton > 9)
                {
                    WinMethod();
                    return;
                }
            }
            else
            {
                loseMiniGameSnd.Play();
                ResetSequence();
                return;
            }
        }
    }
    public void Press_Number2()
    {
        if (mNumber2.color == Color.white)
        {
            tapSnd.Play();
            if (4 == expectedButton)
            {
                buttonSequence.Add(4);
                expectedButton++;
                mNumber2.color = Color.yellow;
                if (expectedButton > 9)
                {
                    WinMethod();
                    return;
                }
            }
            else
            {
                loseMiniGameSnd.Play();
                ResetSequence();
                return;
            }
        }
    }
    public void Press_Number3()
    {
        if (mNumber3.color == Color.white)
        {
            tapSnd.Play();
            if (3 == expectedButton)
            {
                buttonSequence.Add(3);
                expectedButton++;
                mNumber3.color = Color.yellow;
                if (expectedButton > 9)
                {
                    WinMethod();
                    return;
                }
            }
            else
            {
                loseMiniGameSnd.Play();
                ResetSequence();
                return;
            }
        }
    }
    public void Press_Number4()
    {
        if (mNumber4.color == Color.white)
        {
            tapSnd.Play();
            if (9 == expectedButton)
            {
                buttonSequence.Add(9);
                expectedButton++;
                mNumber4.color = Color.yellow;
                if (expectedButton > 9)
                {
                    WinMethod();
                    return;
                }
            }
            else
            {
                loseMiniGameSnd.Play();
                ResetSequence();
                return;
            }
        }
    }
    public void Press_Number5()
    {
        if (mNumber5.color == Color.white)
        {
            tapSnd.Play();
            if (2 == expectedButton)
            {
                buttonSequence.Add(2);
                expectedButton++;
                mNumber5.color = Color.yellow;
                if (expectedButton > 9)
                {
                    WinMethod();
                    return;
                }
            }
            else
            {
                loseMiniGameSnd.Play();
                ResetSequence();
                return;
            }
        }
    }
    public void Press_Number6()
    {
        if (mNumber6.color == Color.white)
        {
            tapSnd.Play();
            if (6 == expectedButton)
            {
                buttonSequence.Add(6);
                expectedButton++;
                mNumber6.color = Color.yellow;
                if (expectedButton > 9)
                {
                    WinMethod();
                    return;
                }
            }
            else
            {
                loseMiniGameSnd.Play();
                ResetSequence();
                return;
            }
        }
    }
    public void Press_Number7()
    {
        if (mNumber7.color == Color.white)
        {
            tapSnd.Play();
            if (7 == expectedButton)
            {
                buttonSequence.Add(7);
                expectedButton++;
                mNumber7.color = Color.yellow;
                if (expectedButton > 9)
                {
                    WinMethod();
                    return;
                }
            }
            else
            {
                loseMiniGameSnd.Play();
                ResetSequence();
                return;
            }
        }
    }
    public void Press_Number8()
    {
        if (mNumber8.color == Color.white)
        {
            tapSnd.Play();
            if (8 == expectedButton)
            {
                buttonSequence.Add(8);
                expectedButton++;
                mNumber8.color = Color.yellow;
                if (expectedButton > 9)
                {
                    WinMethod();
                    return;
                }
            }
            else
            {
                loseMiniGameSnd.Play();
                ResetSequence();
                return;
            }
        }
    }
    public void Press_Number9()
    {
        if (mNumber9.color == Color.white)
        {
            tapSnd.Play();
            if (5 == expectedButton)
            {
                buttonSequence.Add(5);
                expectedButton++;
                mNumber9.color = Color.yellow;
                if (expectedButton > 9)
                {
                    WinMethod();
                    return;
                }
            }
            else
            {
                loseMiniGameSnd.Play();
                ResetSequence();
                return;
            }
        }
    }
    private void ResetSequence()
    {
        mNumber1.color = Color.white;
        mNumber2.color = Color.white;
        mNumber3.color = Color.white;
        mNumber4.color = Color.white;
        mNumber5.color = Color.white;
        mNumber6.color = Color.white;
        mNumber7.color = Color.white;
        mNumber8.color = Color.white;
        mNumber9.color = Color.white;
        buttonSequence.Clear();
        expectedButton = 1;
    }
    private void WinMethod()
    {
#if PLAYMARKET
        FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
        winMiniGameSnd.Play();
        insideSwitchGame.SetActive(false);
        allowScalePressing = false;
        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        public_actions.Instance.OnOff_SpaceTip_Close(false);
        colliderList.ENABLE_COLLIDER();
        paperGive.SetActive(true);
        VariableDeterminator.isMiniGameActive = false;
    }
}
