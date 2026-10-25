using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TimeManager : MonoBehaviour
{
    public AudioSource chik;
    [SerializeField] private RectTransform clock_arrowPos;
    public float hour12 = 0f;
    public float hour3 = -87.9f;
    public float hour6 = -177.3f;
    public float hour9 = 89.8f;
    public Light2D global_light;
    [SerializeField] private GameObject CircleLight;
    [SerializeField] private float speed = 2.0f;
    public AudioSource lightOn, lightOff, background_music, clownHaha;
    [SerializeField] private AudioClip[] backgroundMusic;

    [SerializeField] private PauseScript _PauseScript;
    [SerializeField] public AudioSource mHahaSnd;
    public float fadeDuration = 1f; // Продолжительность затухания
    [SerializeField] private ManiacMove maniacMove;
    [SerializeField] private ManiacMove2 maniacMove2;
    [Header("Horror")]
    public bool glassesAllow = false;
    public bool glassesCameraAllow = true;
    private int previousIndex = -1; // переменная для хранения предыдущего индекса звука

    public AudioSource murmur;
    private ParticleSystem.MainModule mainModule;
    [SerializeField] private ParticleSystem benzinEffect;

    [SerializeField] private AudioSource horrorSource;
    [SerializeField] private AudioClip[] horrorClips;
    [SerializeField] private GameObject[] eachRedGlasses;
    Coroutine horror_sounds_loop, ArrowCoroutine, startCoroutineCicle;

    public bool isPaused = false;
    public bool maniac_movements = false;
    public Coroutine musicCoroutine, warningCoroutine, handleLightCoroutine, handleHourCycleCoroutine, warningFirstCoroutine, FirstCycleCoroutine;

    [SerializeField] private GameObject _RedEyesFaust, _SecondWhiteEyesFaust;
    [SerializeField] private AudioSource warningAlertSnd;
    private bool startCounterForHandleHour = false;

    public bool firstNight = true;
    
    private int skipCycles = 0; // Счётчик для пропуска часов
    [SerializeField] private HeartGameManager _HeartGameManager;
    [SerializeField] private ScaleController _ScaleController;
    [SerializeField] private ScaleController2 _ScaleController2;

    private Coroutine rotateCoroutine;
    [SerializeField] private StartChapter _StartChapter;

    [SerializeField] public CanvasGroup heartGame, bloodOverlay, blackFullscreen, blackOverlayUppenOnInterface;
    private bool isAlreadyStarted = false;

    [SerializeField] private GameObject[] lightsShelfCommon;
    [SerializeField] private AudioSource night_back_music;
    [SerializeField] private curtainGame _CurtainGame;
    [SerializeField] private FoldingScreen _FoldingScreen;
    private void Start()
    {
        mainModule = benzinEffect.main;
    }
    private void Update()
    {
        if (glassesAllow == false)
        {
            if (glassesCameraAllow == true)
            {
#if PLAYMARKET
if (Input.touchCount > 0)
{
    Touch touch = Input.GetTouch(0);
    Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
    Vector3 targetPosition = new Vector3(touchPos.x, touchPos.y, CircleLight.transform.position.z);
    CircleLight.transform.position = Vector3.MoveTowards(CircleLight.transform.position, targetPosition, speed * Time.deltaTime);
}
#else
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 targetPosition = new Vector3(mousePos.x, mousePos.y, CircleLight.transform.position.z);
                CircleLight.transform.position = Vector3.MoveTowards(CircleLight.transform.position, targetPosition, speed * Time.deltaTime);
#endif

            }
        }
    }
    public IEnumerator FirstCicle()
    {
        if(_FoldingScreen != null)
        {
            _FoldingScreen.AnimateToOriginal();
        }
        if (!VariableDeterminator.allowCurtainGame && SceneManager.GetActiveScene().name == "Chapter2")
        {
            _CurtainGame.SmoothAnimFINISH_Deactivator();
        }
        VariableDeterminator.currentTimeIsNight = true;
        firstNight = true;
        warningFirstCoroutine = StartCoroutine(WarningEffect2());
        clock_arrowPos.eulerAngles = new Vector3(0f, 0f, 0f);
        StartCoroutine(onBlackFirst());

        // Сохранение ссылки на RotateCoroutine
        rotateCoroutine = StartCoroutine(RotateCoroutine(hour3, 90f));
        yield return rotateCoroutine;

        StartCoroutine(offBlackFirst());
        startCounterForHandleHour = true;
        yield return new WaitForSeconds(0.5f);

        rotateCoroutine = StartCoroutine(RotateCoroutine(hour6, 90f));
        yield return rotateCoroutine;

        yield return new WaitForSeconds(0.5f);
        firstNight = false;
        startCoroutineCicle = StartCoroutine(startClockCicle());
    }
    public IEnumerator startClockCicle()
    {
        while (true)
        {
            // Вращение стрелки к 9 часам
            rotateCoroutine = StartCoroutine(RotateCoroutine(hour9, 90f));
            yield return rotateCoroutine;

            // Вращение стрелки к 12 часам
            rotateCoroutine = StartCoroutine(RotateCoroutine(hour12, 90f));
            yield return rotateCoroutine;

            // Вращение стрелки к 3 часам
            rotateCoroutine = StartCoroutine(RotateCoroutine(hour3, 90f));
            yield return rotateCoroutine;

            // Вращение стрелки к 6 часам
            rotateCoroutine = StartCoroutine(RotateCoroutine(hour6, 90f));
            yield return rotateCoroutine;
        }
    }
    private int failAttempts = 0; // Счётчик неудачных попыток

    private IEnumerator HandleHourCycle()
    {
        if (skipCycles > 0) // Если цикл нужно пропустить
        {
            skipCycles--; // Уменьшаем счётчик
            yield break; // Завершаем выполнение корутины
        }

        if (Random.value > 0.6f || failAttempts >= 4) // Условие успеха или 4-й раз
        {
            failAttempts = 0; // Сбрасываем счётчик неудачных попыток

            // Вызов WarningEffect за 5 секунд до HandleLight
            warningCoroutine = StartCoroutine(WarningEffect());
            yield return new WaitForSeconds(3f);

            // Запуск HandleLight
            handleLightCoroutine = StartCoroutine(HandleLight(true));

            // Устанавливаем счётчик на 3 пропуска
            skipCycles = 3;
        }
        else
        {
            failAttempts++; // Увеличиваем счётчик неудачных попыток
        }
    }

    private IEnumerator HandleLight(bool enableLight)
    {
        if (enableLight)
        {
            // Включаем свет
            StartCoroutine(OnBlackAndLights());

            yield return new WaitForSeconds(90f);

            // Выключаем свет
            StartCoroutine(OffBlackAndLights());
        }
    }

    private IEnumerator WarningEffect2()
    {
        float blinkDuration = 3f; // Длительность мигания
        float elapsedTime = 0f;
        bool isActive = false;

        while (elapsedTime < blinkDuration)
        {
            // Переключаем активность объекта
            isActive = !isActive;
            warningAlertSnd.PlayOneShot(warningAlertSnd.clip);
            _RedEyesFaust.SetActive(isActive);

            // Ждём 0.5 секунды перед следующей сменой состояния
            yield return new WaitForSeconds(0.1f);

            elapsedTime += 0.1f;
        }
        _RedEyesFaust.SetActive(true);
    }
    private IEnumerator WarningEffect()
    {
        float blinkDuration = 5f; // Длительность мигания
        float elapsedTime = 0f;
        bool isActive = false;

        while (elapsedTime < blinkDuration)
        {
            // Переключаем активность объекта
            isActive = !isActive;
            warningAlertSnd.PlayOneShot(warningAlertSnd.clip);
            _RedEyesFaust.SetActive(isActive);

            // Ждём 0.5 секунды перед следующей сменой состояния
            yield return new WaitForSeconds(0.2f);

            elapsedTime += 0.2f;
        }
        _RedEyesFaust.SetActive(true);
    }
    public void stopStartClockCicle()
    {
        if (_StartChapter.laughtCrt != null)
        {
            StopCoroutine(_StartChapter.laughtCrt);
        }
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }
        if (startCoroutineCicle != null)
        {
            StopCoroutine(startCoroutineCicle);
        }
        if (warningCoroutine != null)
        {
            StopCoroutine(warningCoroutine);
        }
        if (handleLightCoroutine != null)
        {
            StopCoroutine(handleLightCoroutine);
        }
        if (handleHourCycleCoroutine != null)
        {
            StopCoroutine(handleHourCycleCoroutine);
        }
        if (warningFirstCoroutine != null)
        {
            StopCoroutine(warningFirstCoroutine);
        }
        if (FirstCycleCoroutine != null)
        {
            StopCoroutine(FirstCycleCoroutine);
        }
        global_light.intensity = 0.7f;
        CircleLight.SetActive(false);
        _RedEyesFaust.SetActive(false);
        _SecondWhiteEyesFaust.SetActive(false);

        if(SceneManager.GetActiveScene().name == "Chapter1")
        {
            if (maniacMove != null)
            {
                Destroy(maniacMove.gameObject);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Chapter2")
        {
            if (maniacMove2 != null)
            {
                Destroy(maniacMove2.gameObject);
            }
        }
    }

    private IEnumerator OnBlackAndLights()
    {
        if (_FoldingScreen != null)
        {
            _FoldingScreen.AnimateToOriginal();
        }
        if (!VariableDeterminator.allowCurtainGame && SceneManager.GetActiveScene().name == "Chapter2")
        {
            _CurtainGame.SmoothAnimFINISH_Deactivator();
        }
        VariableDeterminator.currentTimeIsNight = true;
        maniac_movements = true;
        float targetIntensity;
        float step;
        background_music.volume = 0;
        if(SceneManager.GetActiveScene().name == "Chapter1")
        {
            Color myColor = new Color32(0x33, 0x32, 0x19, 0xFF);
            mainModule.startColor = myColor;
        }
        horror_sounds_loop = StartCoroutine(Play_Random_Scream());
        Invoke(nameof(ManiacEnable), 3);
        chik.Play();
        targetIntensity = global_light.intensity - 0.695f;
        step = (targetIntensity - global_light.intensity) / 120;
        for (int i = 0; i < 120; i++)
        {
            global_light.intensity += step;
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(2);
        CircleLight.SetActive(true);
        lightOn.Play();
        on_glassShkaf();
    }

    public void startFadeOutFullscreenPublic()
    {
        Invoke(nameof(startFadeOutFullscreenPublic2), 1);
    }
    private void startFadeOutFullscreenPublic2()
    {
        StartCoroutine(SmoothCanvasGroup(blackOverlayUppenOnInterface, 3f, 0, false));
    }

    private IEnumerator OffBlackAndLights()
    {
        if (_FoldingScreen != null)
        {
            _FoldingScreen.AnimateAndSwitch();
        }
        _RedEyesFaust.SetActive(false);
        _PauseScript.BlockCollidersActive = false;
        maniac_movements = false;
        float targetIntensity;
        float step;
        CircleLight.SetActive(false);
        lightOff.Play();
        off_glassShkaf();
        yield return new WaitForSeconds(2);
        if (SceneManager.GetActiveScene().name == "Chapter1")
        {
            Color myColor = new Color32(0x60, 0x5F, 0x33, 0xFF);
            mainModule.startColor = myColor;
        }
        StopCoroutine(horror_sounds_loop);
        targetIntensity = global_light.intensity + 0.695f;
        step = (targetIntensity - global_light.intensity) / 120;
        for (int i = 0; i < 120; i++)
        {
            global_light.intensity += step;
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(1);
        if (HideInShkaf.nowInShkaf == true)
        {
            background_music.volume = 0.35f;
        }
        else
        {
            background_music.volume = 0.75f;
        }
        VariableDeterminator.shotedByManiac = false;
        VariableDeterminator.currentTimeIsNight = false;
        if (!VariableDeterminator.allowCurtainGame && SceneManager.GetActiveScene().name == "Chapter2")
        {
            _CurtainGame.SmoothAnimFINISH_Activator();
        }
    }
    private IEnumerator onBlackFirst()
    {
        maniac_movements = true;
        horror_sounds_loop = StartCoroutine(Play_Random_Scream());
        if (SceneManager.GetActiveScene().name == "Chapter1")
        {
            Color myColor = new Color32(0x33, 0x32, 0x19, 0xFF);
            mainModule.startColor = myColor;
        }
        background_music.volume = 0;
        yield return new WaitForSeconds(2);
        CircleLight.SetActive(true);
        lightOn.Play();
        on_glassShkaf();
    }
    private IEnumerator offBlackFirst()
    {
        if (!VariableDeterminator.allowCurtainGame && SceneManager.GetActiveScene().name == "Chapter2")
        {
            _CurtainGame.SmoothAnimFINISH_Activator();
        }
        if (_FoldingScreen != null)
        {
            _FoldingScreen.AnimateAndSwitch();
        }
        VariableDeterminator.currentTimeIsNight = false;
        _RedEyesFaust.SetActive(false);
        _PauseScript.BlockCollidersActive = false;
        maniac_movements = false;
        float targetIntensity;
        float step;
        CircleLight.SetActive(false);
        lightOff.Play();
        off_glassShkaf();
        yield return new WaitForSeconds(2);
        StopCoroutine(horror_sounds_loop);
        if (SceneManager.GetActiveScene().name == "Chapter1")
        {
            Color myColor = new Color32(0x60, 0x5F, 0x33, 0xFF);
            mainModule.startColor = myColor;
        }
        targetIntensity = global_light.intensity + 0.695f;
        step = (targetIntensity - global_light.intensity) / 120;
        for (int i = 0; i < 120; i++)
        {
            global_light.intensity += step;
            yield return new WaitForSeconds(0.025f);
        }
        clownHaha.Play();
        yield return new WaitForSeconds(3);
        musicCoroutine = StartCoroutine(PlayBackgroundMusic());
        background_music.Play();
        if (HideInShkaf.nowInShkaf == true)
        {
            background_music.volume = 0.35f;
        }
        else
        {
            background_music.volume = 0.75f;
        }
        VariableDeterminator.shotedByManiac = false;
    }
    //   ===============================================================
    private IEnumerator RotateCoroutine(float targetAngle, float duration)
    {
        chik.Play();
        Quaternion startRotation = clock_arrowPos.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        float elapsedTime = 0;
        bool hourCycleStarted = false; // Флаг для запуска HandleHourCycle

        while (elapsedTime < duration)
        {
            if (!isPaused) // Проверяем, не поставлена ли пауза
            {
                float t = elapsedTime / duration;
                clock_arrowPos.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

                elapsedTime += Time.deltaTime;

                // Если остаётся 5 секунд до окончания и HandleHourCycle ещё не запускался
                if (!hourCycleStarted && duration - elapsedTime <= 3f)
                {
                    hourCycleStarted = true; // Устанавливаем флаг
                    if (startCounterForHandleHour)
                    {
                        handleHourCycleCoroutine = StartCoroutine(HandleHourCycle()); // Запускаем HandleHourCycle
                    }
                }
            }
            yield return null;
        }
        clock_arrowPos.rotation = targetRotation;
        ManiacDisable();
        mHahaSnd.Stop();
    }

    //    ===============================================================
    private void ManiacEnable()
    {
        if (SceneManager.GetActiveScene().name == "Chapter1")
        {
            maniacMove.gameObject.SetActive(true);
            maniacMove.EnableManiac();
        }
        else if (SceneManager.GetActiveScene().name == "Chapter2")
        {
            maniacMove2.gameObject.SetActive(true);
            maniacMove2.EnableManiac();
        }
    }
    private void ManiacDisable()
    {
        if (SceneManager.GetActiveScene().name == "Chapter1")
        {
            maniacMove.DisableManiac();
        }
        else if (SceneManager.GetActiveScene().name == "Chapter2")
        {
            maniacMove2.DisableManiac();
            maniacMove2.gameObject.SetActive(false);
        }
    }

    private IEnumerator Play_Random_Scream()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(20, 30));

            int randomIndex = GetUniqueRandomIndex();

            if (randomIndex != -1) // проверка наличия доступных звуков
            {
                AudioClip clipSource = horrorClips[randomIndex];
                horrorSource.spatialBlend = Random.Range(0f, 1f);
                horrorSource.volume = Random.Range(0f, 1f);
                horrorSource.PlayOneShot(clipSource);
            }
        }
    }
    private int GetUniqueRandomIndex()
    {
        if (horrorClips.Length <= 1)
        {
            return 0; // возвращаем доступный (единственный) индекс
        }

        int randomIndex = Random.Range(0, horrorClips.Length);

        if (randomIndex == previousIndex) // проверяем совпадение с предыдущим индексом
        {
            randomIndex = (randomIndex + 1) % horrorClips.Length; // если совпадает, берем следующий по кругу
        }

        previousIndex = randomIndex; // обновляем предыдущий индекс

        return randomIndex;
    }

    private void off_glassShkaf()
    {
        StartCoroutine(FadeToAudio(0, 2));
        foreach (GameObject lightsObj in lightsShelfCommon)
        {
            lightsObj.SetActive(false);
        }
        _HeartGameManager.isNight = false;

        if (SceneManager.GetActiveScene().name == "Chapter1")
        {
            if (_ScaleController.isShotAnimPlaying)
            {
                _ScaleController.isTimeManagerTurnOffTheNight = true;
            }
            else
            {
                StartCoroutine(SmoothCanvasGroup(heartGame, 1.5f, 0, false));
            }
        }
        else if (SceneManager.GetActiveScene().name == "Chapter2")
        {
            if (_ScaleController2.isShotAnimPlaying)
            {
                _ScaleController2.isTimeManagerTurnOffTheNight = true;
            }
            else
            {
                StartCoroutine(SmoothCanvasGroup(heartGame, 1.5f, 0, false));
            }
        }
        if (_HeartGameManager.gameObject.activeSelf)
        {
            _HeartGameManager.BreathReset();
        }
        foreach (GameObject glasses in eachRedGlasses)
        {
            glasses.SetActive(false);
        }
    }
    private void on_glassShkaf()
    {
        night_back_music.Play();
        foreach (GameObject lightsObj in lightsShelfCommon)
        {
            lightsObj.SetActive(true);
        }
        _HeartGameManager.isNight = true;
        StartCoroutine(SmoothCanvasGroup(heartGame, 1.5f, 1, true));
        foreach (GameObject glasses in eachRedGlasses)
        {
            glasses.SetActive(true);
        }
    }
    private IEnumerator FadeToAudio(float targetAlpha, float duration)
    {
        float startAlpha = night_back_music.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            night_back_music.volume = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        night_back_music.volume = targetAlpha; // Убедитесь, что мы устанавливаем конечное значение
        night_back_music.Stop();
    }
    private IEnumerator PlayBackgroundMusic()
    {
        while (true) // Бесконечный цикл
        {
            // Воспроизводим клип с индексом 1
            background_music.clip = backgroundMusic[1];
            background_music.Play();
            yield return new WaitForSeconds(background_music.clip.length); // Ждем окончания клипа

            // Воспроизводим клип с индексом 0
            background_music.clip = backgroundMusic[0];
            background_music.Play();
            yield return new WaitForSeconds(background_music.clip.length); // Ждем окончания клипа
        }
    }
    public IEnumerator SmoothCanvasGroup(CanvasGroup canvasGroup, float duration, float targetAlpha, bool active)
    {
        if (isAlreadyStarted) yield break;
        isAlreadyStarted = true;
        if (active) canvasGroup.gameObject.SetActive(true);
        // Начальное значение alpha
        float startAlpha = canvasGroup.alpha;

        // Время, прошедшее с начала изменения
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Линейная интерполяция между стартовым и конечным значением
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            yield return null; // Ждем следующий кадр
        }

        // Убедиться, что значение alpha точно равно конечной цели
        canvasGroup.alpha = targetAlpha;

        if(!active) canvasGroup.gameObject.SetActive(false);
        isAlreadyStarted = false;
    }
}