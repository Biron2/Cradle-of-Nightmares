using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchPictureGame : MonoBehaviour
{
    private bool switchSides1 = false, switchSides2 = false, switchSides3 = false, switchSides4 = false, switchSides5 = false, allowScalePressing = true;
    public static int stepScales1 = 1, stepScales2 = 1, stepScales3 = 1, stepScales4 = 1, stepScales5 = 1;
    public Vector2 target1, target2, target3, target4, target5, target6, target1_first, target2_first, target3_first, target4_first, target5_first, target6_first;
    public RectTransform round_main1, round_main2, round_main3, round_main4, round_main5;
    public RectTransform round_Back1, round_Back2, round_Back3, round_Back4, round_Back5;
    [SerializeField] private GameObject[] arrows_containers;
    [SerializeField] private GameObject insideSwitchGame, locked, unlocked;
    public AudioSource tapSnd, unlocked_snd;
    [SerializeField] private GameObject arrowRoom12;
    private ColliderList colliderList;
    public AudioSource winSnd;
    [SerializeField] private GameObject _tip5;

    private audio_for_items _Audio_For_Items;
    private float cooldown = 0.5f; // ¬рем¤ задержки в секундах
    private float lastClickTime = 0f; // ¬рем¤ последнего нажати¤
    private void Start()
    {
        _Audio_For_Items = GameObject.Find("Other/GameManager/EtcSounds/__FOR_ITEMS__").GetComponent<audio_for_items>();
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
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
    public void ScalePress1()
    {
        if (allowScalePressing == true)
        {
            tapSnd.Play();
            if (switchSides1 == false)
            {
                if (stepScales1 == 1)
                {
                    round_main1.anchoredPosition = target2_first;
                    round_Back1.anchoredPosition = target2_first;
                    stepScales1 += 1;
                }
                else if (stepScales1 == 2)
                {
                    round_main1.anchoredPosition = target3_first;
                    round_Back1.anchoredPosition = target3_first;
                    stepScales1 += 1;
                }
                else if (stepScales1 == 3)
                {
                    round_main1.anchoredPosition = target4_first;
                    round_Back1.anchoredPosition = target4_first;
                    stepScales1 += 1;
                }
                else if (stepScales1 == 4)
                {
                    round_main1.anchoredPosition = target5_first;
                    round_Back1.anchoredPosition = target5_first;
                    stepScales1 += 1;
                }
                else if (stepScales1 == 5)
                {
                    round_main1.anchoredPosition = target6_first;
                    round_Back1.anchoredPosition = target6_first;
                    stepScales1 += 1;
                    switchSides1 = true;
                }
            }
            else
            {
                if (stepScales1 == 6)
                {
                    round_main1.anchoredPosition = target5_first;
                    round_Back1.anchoredPosition = target5_first;
                    stepScales1 -= 1;
                }
                else if (stepScales1 == 5)
                {
                    round_main1.anchoredPosition = target4_first;
                    round_Back1.anchoredPosition = target4_first;
                    stepScales1 -= 1;
                }
                else if (stepScales1 == 4)
                {
                    round_main1.anchoredPosition = target3_first;
                    round_Back1.anchoredPosition = target3_first;
                    stepScales1 -= 1;
                }
                else if (stepScales1 == 3)
                {
                    round_main1.anchoredPosition = target2_first;
                    round_Back1.anchoredPosition = target2_first;
                    stepScales1 -= 1;
                }
                else if (stepScales1 == 2)
                {
                    round_main1.anchoredPosition = target1_first;
                    round_Back1.anchoredPosition = target1_first;
                    stepScales1 -= 1;
                    switchSides1 = false;
                }
            }
        }
    }
    public void ScalePress2()
    {
        if (allowScalePressing == true)
        {
            tapSnd.Play();
            if (switchSides2 == false)
            {
                if (stepScales2 == 1)
                {
                    round_main2.anchoredPosition = target2;
                    round_Back2.anchoredPosition = target2;
                    stepScales2 += 1;
                }
                else if (stepScales2 == 2)
                {
                    round_main2.anchoredPosition = target3;
                    round_Back2.anchoredPosition = target3;
                    stepScales2 += 1;
                }
                else if (stepScales2 == 3)
                {
                    round_main2.anchoredPosition = target4;
                    round_Back2.anchoredPosition = target4;
                    stepScales2 += 1;
                }
                else if (stepScales2 == 4)
                {
                    round_main2.anchoredPosition = target5;
                    round_Back2.anchoredPosition = target5;
                    stepScales2 += 1;
                }
                else if (stepScales2 == 5)
                {
                    round_main2.anchoredPosition = target6;
                    round_Back2.anchoredPosition = target6;
                    stepScales2 += 1;
                    switchSides2 = true;
                }
            }
            else
            {
                if (stepScales2 == 6)
                {
                    round_main2.anchoredPosition = target5;
                    round_Back2.anchoredPosition = target5;
                    stepScales2 -= 1;
                }
                else if (stepScales2 == 5)
                {
                    round_main2.anchoredPosition = target4;
                    round_Back2.anchoredPosition = target4;
                    stepScales2 -= 1;
                }
                else if (stepScales2 == 4)
                {
                    round_main2.anchoredPosition = target3;
                    round_Back2.anchoredPosition = target3;
                    stepScales2 -= 1;
                }
                else if (stepScales2 == 3)
                {
                    round_main2.anchoredPosition = target2;
                    round_Back2.anchoredPosition = target2;
                    stepScales2 -= 1;
                }
                else if (stepScales2 == 2)
                {
                    round_main2.anchoredPosition = target1;
                    round_Back2.anchoredPosition = target1;
                    stepScales2 -= 1;
                    switchSides2 = false;
                }
            }
        }
    }
    public void ScalePress3()
    {
        if (allowScalePressing == true)
        {
            tapSnd.Play();
            if (switchSides3 == false)
            {
                if (stepScales3 == 1)
                {
                    round_main3.anchoredPosition = target2;
                    round_Back3.anchoredPosition = target2;
                    stepScales3 += 1;
                }
                else if (stepScales3 == 2)
                {
                    round_main3.anchoredPosition = target3;
                    round_Back3.anchoredPosition = target3;
                    stepScales3 += 1;
                }
                else if (stepScales3 == 3)
                {
                    round_main3.anchoredPosition = target4;
                    round_Back3.anchoredPosition = target4;
                    stepScales3 += 1;
                }
                else if (stepScales3 == 4)
                {
                    round_main3.anchoredPosition = target5;
                    round_Back3.anchoredPosition = target5;
                    stepScales3 += 1;
                }
                else if (stepScales3 == 5)
                {
                    round_main3.anchoredPosition = target6;
                    round_Back3.anchoredPosition = target6;
                    stepScales3 += 1;
                    switchSides3 = true;
                }
            }
            else
            {
                if (stepScales3 == 6)
                {
                    round_main3.anchoredPosition = target5;
                    round_Back3.anchoredPosition = target5;
                    stepScales3 -= 1;
                }
                else if (stepScales3 == 5)
                {
                    round_main3.anchoredPosition = target4;
                    round_Back3.anchoredPosition = target4;
                    stepScales3 -= 1;
                }
                else if (stepScales3 == 4)
                {
                    round_main3.anchoredPosition = target3;
                    round_Back3.anchoredPosition = target3;
                    stepScales3 -= 1;
                }
                else if (stepScales3 == 3)
                {
                    round_main3.anchoredPosition = target2;
                    round_Back3.anchoredPosition = target2;
                    stepScales3 -= 1;
                }
                else if (stepScales3 == 2)
                {
                    round_main3.anchoredPosition = target1;
                    round_Back3.anchoredPosition = target1;
                    stepScales3 -= 1;
                    switchSides3 = false;
                }
            }
        }
    }
    public void ScalePress4()
    {
        if (allowScalePressing == true)
        {
            tapSnd.Play();
            if (switchSides4 == false)
            {
                if (stepScales4 == 1)
                {
                    round_main4.anchoredPosition = target2;
                    round_Back4.anchoredPosition = target2;
                    stepScales4 += 1;
                }
                else if (stepScales4 == 2)
                {
                    round_main4.anchoredPosition = target3;
                    round_Back4.anchoredPosition = target3;
                    stepScales4 += 1;
                }
                else if (stepScales4 == 3)
                {
                    round_main4.anchoredPosition = target4;
                    round_Back4.anchoredPosition = target4;
                    stepScales4 += 1;
                }
                else if (stepScales4 == 4)
                {
                    round_main4.anchoredPosition = target5;
                    round_Back4.anchoredPosition = target5;
                    stepScales4 += 1;
                }
                else if (stepScales4 == 5)
                {
                    round_main4.anchoredPosition = target6;
                    round_Back4.anchoredPosition = target6;
                    stepScales4 += 1;
                    switchSides4 = true;
                }
            }
            else
            {
                if (stepScales4 == 6)
                {
                    round_main4.anchoredPosition = target5;
                    round_Back4.anchoredPosition = target5;
                    stepScales4 -= 1;
                }
                else if (stepScales4 == 5)
                {
                    round_main4.anchoredPosition = target4;
                    round_Back4.anchoredPosition = target4;
                    stepScales4 -= 1;
                }
                else if (stepScales4 == 4)
                {
                    round_main4.anchoredPosition = target3;
                    round_Back4.anchoredPosition = target3;
                    stepScales4 -= 1;
                }
                else if (stepScales4 == 3)
                {
                    round_main4.anchoredPosition = target2;
                    round_Back4.anchoredPosition = target2;
                    stepScales4 -= 1;
                }
                else if (stepScales4 == 2)
                {
                    round_main4.anchoredPosition = target1;
                    round_Back4.anchoredPosition = target1;
                    stepScales4 -= 1;
                    switchSides4 = false;
                }
            }
        }
    }
    public void ScalePress5()
    {
        if (allowScalePressing == true)
        {
            tapSnd.Play();
            if (switchSides5 == false)
            {
                if (stepScales5 == 1)
                {
                    round_main5.anchoredPosition = target2;
                    round_Back5.anchoredPosition = target2;
                    stepScales5 += 1;
                }
                else if (stepScales5 == 2)
                {
                    round_main5.anchoredPosition = target3;
                    round_Back5.anchoredPosition = target3;
                    stepScales5 += 1;
                }
                else if (stepScales5 == 3)
                {
                    round_main5.anchoredPosition = target4;
                    round_Back5.anchoredPosition = target4;
                    stepScales5 += 1;
                }
                else if (stepScales5 == 4)
                {
                    round_main5.anchoredPosition = target5;
                    round_Back5.anchoredPosition = target5;
                    stepScales5 += 1;
                }
                else if (stepScales5 == 5)
                {
                    round_main5.anchoredPosition = target6;
                    round_Back5.anchoredPosition = target6;
                    stepScales5 += 1;
                    switchSides5 = true;
                }
            }
            else
            {
                if (stepScales5 == 6)
                {
                    round_main5.anchoredPosition = target5;
                    round_Back5.anchoredPosition = target5;
                    stepScales5 -= 1;
                }
                else if (stepScales5 == 5)
                {
                    round_main5.anchoredPosition = target4;
                    round_Back5.anchoredPosition = target4;
                    stepScales5 -= 1;
                }
                else if (stepScales5 == 4)
                {
                    round_main5.anchoredPosition = target3;
                    round_Back5.anchoredPosition = target3;
                    stepScales5 -= 1;
                }
                else if (stepScales5 == 3)
                {
                    round_main5.anchoredPosition = target2;
                    round_Back5.anchoredPosition = target2;
                    stepScales5 -= 1;
                }
                else if (stepScales5 == 2)
                {
                    round_main5.anchoredPosition = target1;
                    round_Back5.anchoredPosition = target1;
                    stepScales5 -= 1;
                    switchSides5 = false;
                }
            }
        }
    }

    private void Update()
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

        if (stepScales1 == 6 && stepScales2 == 3 && stepScales3 == 5 && stepScales4 == 3 && stepScales5 == 1 && allowScalePressing == true)
        {
            _tip5.SetActive(false);
            insideSwitchGame.SetActive(false);
            allowScalePressing = false;
            Invoke(nameof(UnlockMethodDoor), 1);
            foreach (GameObject obj in arrows_containers)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
            public_actions.Instance.OnOff_SpaceTip_Close(false);
            colliderList.ENABLE_COLLIDER();
            VariableDeterminator.isMiniGameActive = false;
            winSnd.Play();
        }
    }
    private void UnlockMethodDoor()
    {
        locked.SetActive(false);
        unlocked.SetActive(true);
        unlocked_snd.Play();
        arrowRoom12.SetActive(true);

#if PLAYMARKET
        FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
    }
}
