using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScaleController2 : MonoBehaviour
{
    public RectTransform redBox; // Порог
    public RectTransform targetLine; // Двигающаяся линия
    public CanvasGroup thresholdCanvasGroup; // Для плавного появления порога
    public float targetSpeed = 2.0f; // Скорость движения линии
    public float thresholdFadeSpeed = 5.0f; // Скорость появления порога
    
    public Vector2[] thresholdPositions; // Массив позиций для порогов
    private int currentThresholdIndex = 0; // Текущий индекс порога

    private float targetY; // Текущая Y-позиция черной линии
    private bool movingUp = true; // Направление движения черной линии

    [SerializeField] private HeartGameManager _HeartGameManager;
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private AudioClip succesfullHit, failedHit;
    [SerializeField] private ManiacMove2 _ManiacMove;

    private float timer = 0.0f; // Таймер для задержек
    [SerializeField] private Slider indicatorSlider;
    [SerializeField] private GameObject shotSpark;
    [SerializeField] private GameObject mPause;
    [SerializeField] private GameObject blackCommon;
    [SerializeField] private GameObject blackOpen, maniacWithWeapon;

    [SerializeField] private HideInShkaf2[] _HideInShkafs;
    private bool onceForceDeath = true;
    [SerializeField] private TimeManager _TimeManager;
    [SerializeField] private AudioSource shotSource, faustScreamSound, hammeringNails, mHahaSound, ContusionSnd;
    [SerializeField] private Camera cameraObj;
    public AudioSource breath, dungeon;

    private ColliderList colliderList;
    [SerializeField] private GameObject SpaceTipPC, SpaceTipMobile;
    [SerializeField] private GameObject[] arrows_containers;
    [SerializeField] private Collider2D[] colliderOffOn;
    [SerializeField] private GameObject insideShkaf1, insideShkaf2, BlackForInside, glassContainer, heartGameParent;
    [SerializeField] private GameObject[] shkafEveryOpened;
    [SerializeField] private GameObject interfaceSlots, rightLeft, slotPriority;
    public bool allowClickLine = true;
    [SerializeField] private CanvasGroup heartWindowCanvasGroup, panelWindowCanvasGroup, heartGameCanvas;
    [SerializeField] private Chapter2Screamer _Chapter2Screamer;

    private bool colorCoroutineAbleToStart = true;
    [SerializeField] private AudioSource shelfOpenByManiacSnd;
    [SerializeField] private AudioClip rotateShelfOpened, commonShelfOpened;
    public AudioClip faustCatchedBackgroundSound, photoMakeSound, screamSound, polaroid1, polaroid2;

    public bool isShotAnimPlaying = false, isTimeManagerTurnOffTheNight = false;
    [SerializeField] private GlitchController _GlitchController;

    private int lastStage = -1; // начальное значение, которого нет в диапазоне стадий
    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        // Убедиться, что массив thresholdPositions имеет хотя бы одну позицию
        if (thresholdPositions.Length == 0)
        {
            Debug.LogError("Threshold positions are not set!");
            return;
        }

        // Установить первый порог
        SetNewThreshold();
    }

    private void PressPanel()
    {
        if (allowClickLine && !mPause.activeSelf && !VariableDeterminator.shotedByManiac)
        {
            if (IsTargetIntersectingThreshold())
            {
                SetNewThreshold();
                OnSuccess();
            }
            else
            {
                OnFailure();
            }
        }
    }

    private void Update()
    {
        // Движение черной линии вверх-вниз
        Vector2 targetPosition = targetLine.localPosition;
        targetY += (movingUp ? 1 : -1) * targetSpeed * Time.deltaTime;
        targetY = Mathf.Clamp(targetY, 0.0f, 1.0f);
        movingUp = targetY switch
        {
            1.0f => false,
            0.0f => true,
            _ => movingUp
        };

        targetPosition.y = Mathf.Lerp(-260, 260, targetY);
        targetLine.localPosition = targetPosition;

#if PLAYMARKET
        // Проверка касания экрана (Android, iOS и т.п.)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PressPanel();
        }
#else
        // Проверка нажатия ЛКМ (ПК)
        if (Input.GetMouseButtonDown(0))
        {
            PressPanel();
        }
#endif

        if (colorCoroutineAbleToStart)
        {
            Color targetColor = IsTargetIntersectingThreshold() ? Color.white : new Color32(214, 214, 214, 255);
            StartCoroutine(ChangeColorSmoothly(targetColor));
        }

        // Таймер
        if (!VariableDeterminator.shotedByManiac) timer = Mathf.Min(timer + Time.deltaTime, 15);
        indicatorSlider.value = timer;

        // Определяем текущую стадию по таймеру
        int stage = -1;
        if (timer >= 15.0f) stage = 4;
        else if (timer >= 12.0f) stage = 3;
        else if (timer >= 9.0f) stage = 2;
        else if (timer >= 6.0f) stage = 1;
        else if (timer >= 3.0f) stage = 0;

        // Вызываем ExecuteAction только при смене стадии
        if (stage != -1 && stage != lastStage)
        {
            lastStage = stage;
            ExecuteAction(stage);
        }
    }

    public void OnSuccess()
    {
        hitSource.PlayOneShot(succesfullHit);
        Debug.Log("Успешное попадание!");

        // Гарантированно уменьшаем timer ровно на 3 секунды
        timer = Mathf.Max(0, timer - 3);
    }

    public void OnFailure()
    {
        hitSource.PlayOneShot(failedHit);
        Debug.Log("Неудача!");

        // Гарантированно увеличиваем timer, но не больше 9 секунд
        timer = Mathf.Min(timer + 3, 15);
    }
    private IEnumerator ChangeColorSmoothly(Color targetColor)
    {
        colorCoroutineAbleToStart = false;
        float duration = 0.15f; // Время для плавной смены цвета
        float elapsedTime = 0f;
        Color initialColor = redBox.GetComponent<Image>().color;

        while (elapsedTime < duration)
        {
            redBox.GetComponent<Image>().color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        redBox.GetComponent<Image>().color = targetColor; // На всякий случай установим окончательный цвет
        colorCoroutineAbleToStart = true;
    }
    private bool IsTargetIntersectingThreshold()
    {
        Collider2D targetCollider = targetLine.GetComponent<Collider2D>();
        Collider2D thresholdCollider = redBox.GetComponent<Collider2D>();

        if (targetCollider == null || thresholdCollider == null)
        {
            Debug.LogError("Missing Collider2D on targetLine or redBox!");
            return false;
        }

        // Проверка, пересекаются ли конкретные коллайдеры
        return targetCollider.IsTouching(thresholdCollider);
    }
    private void SetNewThreshold()
    {
        // Получаем текущую пороговую позицию по индекс
        Vector2 newThresholdPosition = thresholdPositions[currentThresholdIndex];
        redBox.localPosition = newThresholdPosition;

        // Обеспечиваем плавное появление порога
        StartCoroutine(FadeInThreshold());

        // Увеличиваем индекс для следующей позиции
        currentThresholdIndex++;

        // Если индекс выходит за пределы массива, сбрасываем его в начало
        if (currentThresholdIndex >= thresholdPositions.Length)
        {
            currentThresholdIndex = 0;
        }
    }

    private IEnumerator FadeInThreshold()
    {
        thresholdCanvasGroup.alpha = 0.0f; // Сначала делаем порог невидимым
        while (thresholdCanvasGroup.alpha < 1.0f)
        {
            thresholdCanvasGroup.alpha += Time.deltaTime * thresholdFadeSpeed;
            yield return null;
        }
    }

    // Метод для перехода на другие стадии
    private void ExecuteAction(int stage)
    {
        switch (stage)
        {
            case 0:
                _HeartGameManager.OnHeartClose();
                break;
            case 1:
                _HeartGameManager.OnHeartFar1();
                break;
            case 2:
                _HeartGameManager.OnHeartFar2();
                break;
            case 3:
                _HeartGameManager.OnHeartFar3();
                break;
            case 4:
                _HeartGameManager.OnHeartFar4();

                if (onceForceDeath && _ManiacMove.current_room == _ManiacMove.move_id)
                {
                    isShotAnimPlaying = true;
                    VariableDeterminator.shotedByManiac = true;
                    onceForceDeath = false;
                    allowClickLine = false;
                    HideInShkaf2.ableToLeaveFromTheShelf = false;
                    StartCoroutine(deathAnimation());
                    timer = 0.0f;
                }
                break;
        }
    }
    private void OpenShelf()
    {
        blackCommon.SetActive(false);
        blackOpen.SetActive(true);
    }
    private IEnumerator deathAnimation()
    {
        StartCoroutine(FadeToAudio(mHahaSound, 0f, 1.5f, true, false));
        yield return StartCoroutine(_TimeManager.SmoothCanvasGroup(_TimeManager.blackOverlayUppenOnInterface, 1f, 1, true));
        _TimeManager.heartGame.alpha = 0f;
        SpaceTipPC.SetActive(false);
        SpaceTipMobile.SetActive(false);
        heartWindowCanvasGroup.alpha = 0f;
        panelWindowCanvasGroup.alpha = 0f;
        foreach (GameObject obj in _ManiacMove.maniacInShkafs)
        {
            obj.SetActive(false);
        }
        shelfOpenByManiacSnd.PlayOneShot(commonShelfOpened);
        maniacWithWeapon.SetActive(true);
        _ManiacMove.DisableManiac();
        _ManiacMove.gameObject.SetActive(false);
        OpenShelf();
        ContusionSnd.Play();
        yield return StartCoroutine(_TimeManager.SmoothCanvasGroup(_TimeManager.blackOverlayUppenOnInterface, 1f, 0, false));
        _TimeManager.bloodOverlay.gameObject.SetActive(true);
        //--
        _GlitchController._SetGlitchNumbersKrest();
        StartCoroutine(FadeToAlpha(_TimeManager.bloodOverlay.GetComponent<Image>(), 0.1f, 0.35f));
        StartCoroutine(FadeToAlpha(shotSpark.GetComponent<Image>(), 0.1f, 0.35f));
        shelfOpenByManiacSnd.PlayOneShot(photoMakeSound);
        yield return new WaitForSeconds(0.2f);
        //--
        StartCoroutine(FadeToAlpha(_TimeManager.bloodOverlay.GetComponent<Image>(), 0.1f, 0));
        StartCoroutine(FadeToAlpha(shotSpark.GetComponent<Image>(), 0.1f, 0));
        shelfOpenByManiacSnd.PlayOneShot(polaroid1);
        yield return new WaitForSeconds(0.2f);
        //--
        StartCoroutine(FadeToAlpha(_TimeManager.bloodOverlay.GetComponent<Image>(), 0.1f, 0.65f));
        StartCoroutine(FadeToAlpha(shotSpark.GetComponent<Image>(), 0.1f, 0.65f));
        shelfOpenByManiacSnd.PlayOneShot(photoMakeSound);
        faustScreamSound.PlayOneShot(faustScreamSound.clip);
        yield return new WaitForSeconds(0.2f);
        //--
        StartCoroutine(FadeToAlpha(_TimeManager.bloodOverlay.GetComponent<Image>(), 0.1f, 0));
        StartCoroutine(FadeToAlpha(shotSpark.GetComponent<Image>(), 0.1f, 0));
        shelfOpenByManiacSnd.PlayOneShot(polaroid2);
        yield return new WaitForSeconds(0.2f);
        //--
        StartCoroutine(FadeToAlpha(_TimeManager.bloodOverlay.GetComponent<Image>(), 0.1f, 1));
        StartCoroutine(FadeToAlpha(shotSpark.GetComponent<Image>(), 0.1f, 1));
        shelfOpenByManiacSnd.PlayOneShot(photoMakeSound);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FadeToAlpha(_TimeManager.bloodOverlay.GetComponent<Image>(), 0.1f, 0));
        StartCoroutine(FadeToAlpha(shotSpark.GetComponent<Image>(), 0.1f, 0));
        shelfOpenByManiacSnd.PlayOneShot(screamSound);
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeToAudio(breath, 0f, 1f, true, false));
        StartCoroutine(FadeToAudio(dungeon, 0f, 1f, true, false));
        BlockShkafActive();
        timer = 0.0f; // Перезагрузка таймера
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(_TimeManager.SmoothCanvasGroup(_TimeManager.blackOverlayUppenOnInterface, 2f, 1, true));
        _Chapter2Screamer.updateHealthBar();
        maniacWithWeapon.SetActive(false);
        hammeringNails.PlayOneShot(hammeringNails.clip);
        foreach(GameObject objSE in shkafEveryOpened)
        {
            objSE.SetActive(false);
        }
        _TimeManager.bloodOverlay.gameObject.SetActive(false);
        blackOpen.SetActive(false);
        _TimeManager.blackFullscreen.alpha = 0f;
        _TimeManager.startFadeOutFullscreenPublic();
        HideInShkaf2.ableToLeaveFromTheShelf = true;
        yield return new WaitForSeconds(0.5f);
        startAndResetEverything();
    }
    private void startAndResetEverything()
    {
        heartWindowCanvasGroup.alpha = 1f;
        panelWindowCanvasGroup.alpha = 1f;
        allowClickLine = true;
        cameraObj.transform.position = new Vector3(0f, 0f, 0f);
        cameraObj.orthographicSize = 5f;
        Cursor.visible = true;

        colliderList.allowOpenShatulochka = true;
        SpaceTipPC.SetActive(false);
        SpaceTipMobile.SetActive(false);
        interfaceSlots.SetActive(true);
        rightLeft.SetActive(true);
        slotPriority.SetActive(true);
        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        if (!VariableDeterminator.stopShowPause)
        {
            foreach (Collider2D obj in colliderOffOn)
            {
                if (obj != null)
                {
                    obj.enabled = true;
                }
            }
            colliderList.ENABLE_COLLIDER();
            HideInShkaf2.allowJoinInHidePlace = true;
            VariableDeterminator.isMiniGameActive = true;
        }
        
        blackCommon.SetActive(false);
        insideShkaf1.SetActive(false);
        insideShkaf2.SetActive(false);
        HideInShkaf2.inShkaf = false;
        HideInShkaf2.nowInShkaf = false;
        _ManiacMove.inShkaf = false;
        glassContainer.SetActive(true);
        timer = 0.0f; // Перезагрузка таймера
        onceForceDeath = true;
        breath.clip = _HeartGameManager.breathSlow;
        isShotAnimPlaying = false;
        if (isTimeManagerTurnOffTheNight)
        {
            isTimeManagerTurnOffTheNight = false;
            heartGameCanvas.alpha = 0f;
            heartGameCanvas.gameObject.SetActive(false);
        }
        BlackForInside.SetActive(false);
        heartGameParent.SetActive(false);
    }
    private void BlockShkafActive()
    {
        switch (VariableDeterminator.current_shkaf)
        {
            case 1:
                _HideInShkafs[0].BlockShkaf();
                break;
            case 2:
                _HideInShkafs[1].BlockShkaf();
                break;
        }
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
    private IEnumerator FadeToAlpha(Image mImage, float duration, float targetAlpha)
    {
        if (mImage == null)
            yield break;

        Color color = mImage.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            color.a = newAlpha;
            mImage.color = color;
            yield return null;
        }

        // Убедимся, что значение точно достигло цели
        color.a = targetAlpha;
        mImage.color = color;
    }
}
