using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HideInShkaf2 : MonoBehaviour
{
    [SerializeField] private CanvasGroup BlackBackground;
    public bool closedShkaf = true;
    [SerializeField] private GameObject glassContainer, Opened_Shkaf;
    public AudioSource joinShkaf, outShkaf;
    [SerializeField] private GameObject[] arrows_containers;
    public static bool allowJoinInHidePlace = true;
    [SerializeField] private Collider2D[] colliderOffOn;
    private ColliderList colliderList;
    [SerializeField] private ManiacMove2 maniacMove;
    [SerializeField] private Camera mCamera;
    [SerializeField] private GameObject insideShkaf1, insideShkaf2, BlackForInside, blackInBlack;
    private float transitionStartTime;
    public float transitionDuration = 1.0f;
    public float speedMove = 1f; // —корость перемещени¤
    public float targetSize = 2f;
    public AudioSource breath, dungeon;
    [SerializeField] private GameObject SpaceTipPC, SpaceTipMobile;
    public static bool inShkaf = false;
    [SerializeField] private GameObject interfaceSlots, rightLeft, slotPriority;
    public float targetSize2 = 3.90f;
    [SerializeField] private TimeManager _TimeManager;
    [SerializeField] private AudioSource back_music;
    [SerializeField] private GameObject[] toOffOn;

    private float cooldown = 1.5f; // ¬рем¤ задержки в секундах
    private float lastClickTime = 0f; // ¬рем¤ последнего нажати¤
    [SerializeField] private AudioListener _AudioListener;
    public static bool nowInShkaf = false;

    private Vector3 closet1Pos = new Vector3(-35.554f, 0f, 0f);
    private Vector3 closet2Pos = new Vector3(0, 10f, 0f);

    private bool blockShkafBool = false;
    [SerializeField] private GameObject heartGameParent;
    public static bool ableToLeaveFromTheShelf = true;
    public int numberOftheShelf = 1;
    [SerializeField] private HeartGameManager _HeartGameManager;
    [SerializeField] private CanvasGroup canvasTipShelf;

    [SerializeField] private Button ExitMobileBtn;
    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();

        ExitMobileBtn.onClick.AddListener(ExitShkaf);
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!blockShkafBool)
            {
                if (closedShkaf == true)
                {
                    if (allowJoinInHidePlace == true)
                    {
                        if (Time.time - lastClickTime < cooldown)
                        {
                            // ¬рем¤ после последнего нажати¤ меньше задержки, поэтому ничего не делаем
                            return;
                        }
                        lastClickTime = Time.time; // ќбновл¤ем врем¤ последнего нажати¤
                        rightLeft.SetActive(false);
                        slotPriority.SetActive(false);
                        interfaceSlots.SetActive(false);
                        colliderList.DISABLE_COLLIDER();
                        VariableDeterminator.isMiniGameActive = true;
                        StartCoroutine(TransitionCamera());
                        //приближение
                        maniacMove.inShkaf = true;
                        allowJoinInHidePlace = false;
                        glassContainer.SetActive(false);
                        joinShkaf.Play();
                        Opened_Shkaf.SetActive(true);
                        foreach (GameObject obj in arrows_containers)
                        {
                            if (obj != null)
                            {
                                obj.SetActive(false);
                            }
                        }
                        foreach (Collider2D obj in colliderOffOn)
                        {
                            if (obj != null)
                            {
                                obj.enabled = false;
                            }
                        }
                        colliderList.allowOpenShatulochka = false;
                        Cursor.visible = false;
                        nowInShkaf = true;
                    }
                }
            }
        }
    }

    private IEnumerator TransitionCamera()
    {
        StartCoroutine(SmoothBlackMain(transitionDuration, 1, true));
        transitionStartTime = Time.time;
        Vector3 initialPosition = mCamera.transform.position;
        float initialSize = mCamera.orthographicSize;

        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            mCamera.orthographicSize = Mathf.Lerp(initialSize, targetSize, t);
            mCamera.transform.position = Vector3.Lerp(initialPosition, transform.position, t);
            yield return null;
        }

        mCamera.orthographicSize = targetSize;
        mCamera.transform.position = transform.position;

#if PLAYMARKET
        SpaceTipMobile.SetActive(true);
#else
        SpaceTipPC.SetActive(true);
#endif

        foreach (GameObject obj in toOffOn)
        {
            obj.SetActive(false);
        }

        StartCoroutine(FadeToAudio(breath, 1f, 1f, false, true));
        StartCoroutine(FadeToAudio(dungeon, 0.6f, 1f, false, true));
        BlackForInside.SetActive(true);
        if (!_TimeManager.maniac_movements)
        {
            StartCoroutine(FadeToAudio(back_music, 0.35f, 1f, false, false));
        }
        mCamera.orthographicSize = targetSize2;

        if (gameObject.name.Equals("ClosedShelf1"))
        {
            VariableDeterminator.current_shkaf = 1;
            blackInBlack.SetActive(true);
            insideShkaf1.SetActive(true);
            mCamera.transform.position = closet1Pos;
        }
        if (gameObject.name.Equals("ClosedShelf2"))
        {
            VariableDeterminator.current_shkaf = 2;
            blackInBlack.SetActive(true);
            insideShkaf2.SetActive(true);
            mCamera.transform.position = closet2Pos;
        }
        _AudioListener.transform.rotation = new Quaternion(0f, 0f, 180f, 0);
        heartGameParent.SetActive(true);

        yield return StartCoroutine(SmoothBlackMain(transitionDuration, 0, false));
        inShkaf = true;

        if (PlayerPrefs.GetInt("onceTipShelf") != 5 && _HeartGameManager.isNight)
        {
            StartCoroutine(SmoothCanvasGroup(canvasTipShelf, 2, 1));
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            ExitShkaf();
        }
    }
    private void ExitShkaf()
    {
        if (ableToLeaveFromTheShelf && numberOftheShelf == VariableDeterminator.current_shkaf && inShkaf)
        {
            if (Time.time - lastClickTime < cooldown)
            {
                // ¬рем¤ после последнего нажати¤ меньше задержки, поэтому ничего не делаем
                return;
            }
            lastClickTime = Time.time; // ќбновл¤ем врем¤ последнего нажати¤
            StartCoroutine(FadeToAudio(breath, 0f, 1f, true, false));
            StartCoroutine(FadeToAudio(dungeon, 0f, 1f, true, false));
            if (!_TimeManager.maniac_movements)
            {
                StartCoroutine(FadeToAudio(back_music, 0.75f, 1f, false, false));
            }

            StartCoroutine(TransitionCameraOut());
        }
    }
    private IEnumerator TransitionCameraOut()
    {
        inShkaf = false;
        nowInShkaf = false;
        outShkaf.Play();
        yield return StartCoroutine(SmoothBlackMain(transitionDuration, 1, true));

        //-----
        blackInBlack.SetActive(false);
        insideShkaf1.SetActive(false);
        insideShkaf2.SetActive(false);
        BlackForInside.SetActive(false);
        maniacMove.inShkaf = false;
        allowJoinInHidePlace = true;
        glassContainer.SetActive(true);
        Opened_Shkaf.SetActive(false);
        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        foreach (Collider2D obj in colliderOffOn)
        {
            if (obj != null)
            {
                obj.enabled = true;
            }
        }
        colliderList.allowOpenShatulochka = true;

#if PLAYMARKET
        SpaceTipMobile.SetActive(false);
#else
        SpaceTipPC.SetActive(false);
#endif

        interfaceSlots.SetActive(true);
        rightLeft.SetActive(true);
        slotPriority.SetActive(true);
        _AudioListener.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        Cursor.visible = true;
        heartGameParent.SetActive(false);
        //-----

        foreach (GameObject obj in toOffOn)
        {
            obj.SetActive(true);
        }

        StartCoroutine(SmoothBlackMain(transitionDuration, 0, false));
        transitionStartTime = Time.time;

        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            mCamera.orthographicSize = Mathf.Lerp(targetSize, 5, t);
            switch (gameObject.name)
            {
                case "ClosedShelf1":
                    mCamera.transform.position = Vector3.Lerp(transform.position, closet1Pos, t);
                    break;
                case "ClosedShelf2":
                    mCamera.transform.position = Vector3.Lerp(transform.position, closet2Pos, t);
                    break;
            }
            yield return null;
        }

        colliderList.ENABLE_COLLIDER();
        VariableDeterminator.isMiniGameActive = false;

        switch (gameObject.name)
        {
            case "ClosedShelf1":
                mCamera.transform.position = closet1Pos;
                break;
            case "ClosedShelf2":
                mCamera.transform.position = closet2Pos;
                break;
        }
        mCamera.orthographicSize = 5;
    }
    private IEnumerator FadeToAudio(AudioSource audioS, float targetVolume, float duration, bool offAudio, bool playAudio)
    {
        if (playAudio)
        {
            audioS.Play();
        }
        float startAlpha = audioS.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            audioS.volume = Mathf.Lerp(startAlpha, targetVolume, time / duration);
            yield return null;
        }

        audioS.volume = targetVolume;
        if (offAudio)
        {
            audioS.Stop();
        }
    }
    public void BlockShkaf()
    {
        GameObject blockedObj = transform.Find("blockedCloset").gameObject;
        blockedObj.SetActive(true);
        blockShkafBool = true;
    }

    private IEnumerator SmoothCanvasGroup(CanvasGroup canvasGroup, float duration, float targetAlpha)
    {
        PlayerPrefs.SetInt("onceTipShelf", 5);
        canvasGroup.gameObject.SetActive(true);
        // Ќачальное значение alpha
        float startAlpha = canvasGroup.alpha;

        // ¬рем¤, прошедшее с начала изменени¤
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Ћинейна¤ интерпол¤ци¤ между стартовым и конечным значением
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            yield return null; // ∆дем следующий кадр
        }

        // ”бедитьс¤, что значение alpha точно равно конечной цели
        canvasGroup.alpha = targetAlpha;
        yield return new WaitForSeconds(3);
        if (canvasGroup.alpha > 0) // Проверяем перед запуском
        {
            StartCoroutine(SmoothCanvasGroup(canvasTipShelf, 2, 0));
        }
        else
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }
    private IEnumerator SmoothBlackMain(float duration, float targetAlpha, bool activate)
    {
        if (activate)
        {
            BlackBackground.gameObject.SetActive(true);
        }
        // Ќачальное значение alpha
        float startAlpha = BlackBackground.alpha;

        // ¬рем¤, прошедшее с начала изменени¤
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Ћинейна¤ интерпол¤ци¤ между стартовым и конечным значением
            BlackBackground.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            yield return null; // ∆дем следующий кадр
        }

        // ”бедитьс¤, что значение alpha точно равно конечной цели
        BlackBackground.alpha = targetAlpha;
        if (!activate)
        {
            BlackBackground.gameObject.SetActive(false);
        }
    }
}