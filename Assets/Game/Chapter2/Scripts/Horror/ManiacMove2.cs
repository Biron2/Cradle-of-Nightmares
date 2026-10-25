using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class ManiacMove2 : MonoBehaviour
{
    public int current_room = 1;
    public int move_id = 11;
    [SerializeField] private RectTransform[] rooms_maniac_pos;
    [SerializeField] private Vector3[] maniac_pos;
    public bool inShkaf = false;
    public Chapter2Screamer chapter1Screamer;
    private RectTransform rectTransform;
    public TimeManager timeManager;
    public AudioSource step;
    [SerializeField] private AudioSource babyLaugh, video1, video2, video3;

    [SerializeField] public AudioSource mHahaSnd;
    private float delay = 2.25f;
    public bool onceAction = true, onceAction2 = true;
    [SerializeField] public GameObject[] maniacInShkafs;
    Coroutine moveCoroutine, respawnCrt, waitCrt;

    public VariableDeterminator variableDeterminator;
    [SerializeField] private RectTransform CircleLight;
    [SerializeField] private GameObject Glass_Container;
    private ColliderList colliderList;
    [SerializeField] private GameObject[] arrows_containers;
    public bool AllowMovement = true;

    [SerializeField] private PauseScript _PauseScript;
    [SerializeField] private GlitchController _GlitchController;
    [SerializeField] private TVmanager2 _TVmanager;
    private StepController2 _StepController;
    [SerializeField] private AudioSource[] maniacVoice;
    private Coroutine mouthMovementCoroutine, maniacMadnessCrt;
    [SerializeField] private GameObject warningAlert;
    private bool alertPlay = false;

    private bool isActive = false; // Состояние активности
    private float timer = 0.0f; // Таймер для отслеживания времени
    [SerializeField] private AudioSource warningSnd;
    [SerializeField] private Sprite[] maniacBodySprites;
    [SerializeField] private Image[] tsutomuBodyImage;
    [SerializeField] private Image[] tsutomuFlash;
    [SerializeField] private AudioSource horrorBehindShkafSource;
    [SerializeField] private AudioClip photoCameraClip, horrorTsutomuClip;


    private int[] selectedPath;

    //Маршруты маньяка
    private List<int[]> paths = new List<int[]>
{
    new int[] { 1, 2, 3, 6, 7, 8, 9, 10, 11, 10, 9, 8, 7, 6, 3, 2, 1, 2, 3,
        5, 3, 2, 4, 2, 1, 2, 3, 5, 3, 5, 6, 7, 8, 7, 6, 5, 6, 7, 8, 9, 10, 11 }, // cверху начинает движение

    new int[] { 1, 2, 5, 6, 7, 6, 5, 3, 4, 3, 5, 2, 1, 2, 5, 6, 5, 6, 7, 8,
        10, 11, 12, 11, 10, 8, 9, 8, 7, 6, 5, 3, 4, 3, 2, 1, 2, 8, 9, 8, 10, 11, 12 } // снизу начинает движение
};
    private bool waitForShkafSecondCrt = false;
    private void Awake()
    {
        _StepController = GetComponent<StepController2>();
    }
    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();

        rectTransform = GetComponent<RectTransform>();
        if (timeManager.firstNight)
        {
            delay = 2.25f;
            moveCoroutine = StartCoroutine(FollowPresetPath());
        }
        else
        {
            delay = 3f;
            moveCoroutine = StartCoroutine(FollowPresetPath());
        }
    }
    private void OnDisable()
    {
        alertPlay = false;
        if (warningAlert != null)
        {
            warningAlert.SetActive(false);
        }
    }

    public void EnableManiac()
    {
        waitForShkafSecondCrt = false;
        if (timeManager.firstNight)
        {
            if (current_room > 6)
            {
                move_id = 1;
            }
            else if (current_room < 6)
            {
                move_id = 11;
            }
            else if (current_room == 6)
            {
                move_id = 1;
            }
            rectTransform.SetParent(rooms_maniac_pos[move_id - 1].transform);
            rectTransform.anchoredPosition = maniac_pos[move_id - 1];
            delay = 2.25f;
            moveCoroutine = StartCoroutine(FollowPresetPath());
        }
        else
        {
            delay = 3f;
            moveCoroutine = StartCoroutine(FollowPresetPath());
        }
        onceAction = true;
        onceAction2 = true;
    }
    public void DisableManiac()
    {
        if (warningAlert != null)
        {
            warningAlert.SetActive(false);
        }
        StopPlayingSound();
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        if (respawnCrt != null)
        {
            StopCoroutine(respawnCrt);
        }
        step.volume = 0f;
        foreach (GameObject obj in maniacInShkafs)
        {
            obj.SetActive(false);
        }
        //Установка стандартных значений для спрайта маньяка, белого фона, вспышки
        StopCoroutineManiacMadness();
        move_id = 0;
    }

    private void Update()
    {
        if (alertPlay)
        {
            // Увеличиваем таймер
            timer += Time.deltaTime;

            // Проверяем, прошло ли 0.1 секунды
            if (timer >= 0.1f)
            {
                // Переключаем активность объекта
                isActive = !isActive;
                if (warningAlert != null)
                {
                    warningAlert.SetActive(isActive);
                    warningSnd.PlayOneShot(warningSnd.clip);
                }

                // Сбрасываем таймер
                timer = 0.0f;
            }
        }
        else
        {
            // Если alertPlay становится false, уверяемся, что предупреждение выключено
            if (warningAlert != null)
            {
                warningAlert.SetActive(false);
            }
            isActive = false; // Сбрасываем состояние
            timer = 0.0f; // Сбрасываем таймер
        }

        if (current_room == move_id && inShkaf == false)
        {
            if (onceAction2)
            {
                StopCoroutineManiacMadness();
                _GlitchController.defaultKrestNumbers();
                if (waitCrt != null)
                {
                    StopCoroutine(waitCrt);
                }
                if (respawnCrt != null)
                {
                    StopCoroutine(respawnCrt);
                }
                timeManager.isPaused = true;
                onceAction2 = false;
                foreach (GameObject obj in maniacInShkafs)
                {
                    obj.SetActive(false);
                }
                if(moveCoroutine != null && timeManager.firstNight)
                {
                    StopCoroutine(moveCoroutine);
                }
                mHahaSnd.Stop();
                step.volume = 0f;
                babyLaugh.Play();
                startStepDiedScene();
                _PauseScript.BlockCollidersActive = true;
                if(_TVmanager != null)
                {
                    _TVmanager.Stop_Videos();
                }
                colliderList.DISABLE_COLLIDER();
            }
        }
        else if (current_room == move_id && inShkaf == true)
        {
            if (onceAction)
            {
                _GlitchController.defaultKrestNumbers();
                if (current_room == 3)
                {
                    ChangeManiacPosInShkaf(1);
                }
                else if (current_room == 8)
                {
                    ChangeManiacPosInShkaf(2);
                }
                if (moveCoroutine != null && timeManager.firstNight)
                {
                    StopCoroutine(moveCoroutine);
                }
                onceAction = false;
                step.volume = 0f;
                mHahaSnd.Play();
                //Начинается каритина по смене спрайта маньяка, белого фона, вспышки
                StopCoroutineManiacMadness();
                maniacMadnessCrt = StartCoroutine(ManiacMadness());
                waitCrt = StartCoroutine(waitSpawn());

                mouthMovementCoroutine = StartCoroutine(PlayRandomSoundCoroutine());

            }
        }
    }
    private void ChangeManiacPosInShkaf(int shkaf)
    {
        switch (shkaf)
        {
            case 1:
                // Активируем случайные шкафы из группы 1
                if (Random.value > 0.5f)
                {
                    maniacInShkafs[0].SetActive(true);
                }
                else
                {
                    maniacInShkafs[1].SetActive(true);
                }
                break;
                
            case 2:
                // Активируем случайный шкаф из группы 2
                if (Random.value > 0.5f)
                {
                    maniacInShkafs[2].SetActive(true);
                }
                else
                {
                    maniacInShkafs[3].SetActive(true);
                }
                break;
        }
    }

    private IEnumerator PlayRandomSoundCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1));
        if (maniacVoice.Length > 0)
        {
            int randomIndex = Random.Range(0, Mathf.Min(maniacVoice.Length, 5));
            AudioSource selectedSound = maniacVoice[randomIndex];

            selectedSound.Play();
            float fadeDuration = 1.5f; // Длительность затухания
            float startVolume = selectedSound.volume;

            // Затухание звука
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                selectedSound.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null; // Ждем до следующего кадра
            }

            selectedSound.Stop();
            selectedSound.volume = startVolume; // Восстанавливаем громкость для следующего воспроизведения
        }
        else
        {
            Debug.LogWarning("Массив maniacVoice пуст!");
        }
    }
    public void StopPlayingSound()
    {
        if (mouthMovementCoroutine != null)
        {
            StopCoroutine(mouthMovementCoroutine);
            mouthMovementCoroutine = null;
        }
    }
    private IEnumerator waitSpawn()
    {
        waitForShkafSecondCrt = true;
        yield return new WaitForSeconds(Random.Range(5, 12));

        if (warningAlert != null)
        {
            warningAlert.SetActive(false);
        }
        alertPlay = false;
        StopPlayingSound();
        StopCoroutineManiacMadness();
        _GlitchController._SetDefaultGlitchNumbers();
        foreach (GameObject obj in maniacInShkafs)
        {
            obj.SetActive(false);
        }
        if (timeManager.firstNight)
        {
            delay = 2.25f;
        }
        else
        {
            delay = 3f;
        }
        moveCoroutine = StartCoroutine(FollowPresetPath());
        waitForShkafSecondCrt = false;
        StartCoroutine(enableBools());
    }
    private IEnumerator enableBools()
    {
        yield return new WaitForSeconds(1);
        
        onceAction = true;
        onceAction2 = true;
    }
    private IEnumerator FollowPresetPath()
    {
        int pathIndex = 0;

        switch (current_room)
        {
            case var _ when current_room < 6:
                selectedPath = paths[0];
                break;
            case var _ when current_room > 6:
                selectedPath = paths[1];
                break;
            case var _ when current_room == 6:
                if (Random.Range(0, 2) == 0)
                {
                    selectedPath = paths[0];
                }
                else
                {
                    selectedPath = paths[1];
                }
                break;
        }

        while (true)
        {
            if (!AllowMovement)
            {
                yield return null;
                continue;
            }
            if (waitForShkafSecondCrt)
            {
                yield return null;
                continue;
            }

            // Получаем следующую комнату из маршрута
            int nextRoom = selectedPath[pathIndex];

            // Обновляем move_id и вызываем логику движения
            move_id = nextRoom;
            MoveMethod(nextRoom);

            // Переходим к следующей точке маршрута
            pathIndex = (pathIndex + 1) % selectedPath.Length;
            yield return new WaitForSeconds(delay);
        }
    }

    private void MoveMethod(int room_num)
    {
        if (AllowMovement)
        {
            switch (current_room)
            {
                case 1 when room_num is 1 or 2 or 3 or 4 or 12 or 5 or 6:
                case 2 when room_num is 2 or 4 or 1 or 12 or 3 or 5 or 6 or 7:
                case 3 when room_num is 3 or 2 or 1 or 4 or 12 or 5 or 6 or 7 or 8:
                case 4 when room_num is 4 or 12 or 2 or 1 or 5 or 3 or 6:
                case 5 when room_num is 5 or 3 or 2 or 1 or 4 or 6 or 7:
                case 6 when room_num is 6 or 3 or 5 or 2 or 4 or 1 or 7 or 8 or 9:
                case 7 when room_num is 7 or 6 or 3 or 5 or 2 or 8 or 9 or 10:
                case 8 when room_num is 8 or 7 or 6 or 3 or 9 or 10 or 11:
                case 9 when room_num is 9 or 8 or 7 or 6 or 10 or 11:
                case 10 when room_num is 10 or 9 or 8 or 7 or 11:
                case 11 when room_num is 11 or 10 or 9 or 8 or 7:
                case 12 when room_num is 12 or 4 or 2 or 1 or 3:
                    alertPlay = true;
                    break;
                default:
                    alertPlay = false;
                    if (warningAlert != null)
                    {
                        warningAlert.SetActive(false);
                    }
                    break;
            }

            rectTransform.SetParent(rooms_maniac_pos[room_num - 1].transform);
            rectTransform.anchoredPosition = maniac_pos[room_num - 1];
            _StepController.Set_AnotherStep();
            _StepController.Set_StereoPan();
        }
    }

    private void startStepDiedScene()
    {
        _PauseScript.offMouse = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        variableDeterminator.DisableAllMiniGames();
        Glass_Container.SetActive(true);
        timeManager.glassesAllow = true;
        Vector3[] roomPositions = {
    new Vector3(-233f, 146f, 0f),   // Room 1
    new Vector3(-1806f, 138f, 0f),   // Room 2
    new Vector3(-3794f, 169f, 0f),  // Room 3
    new Vector3(-1348f, -902f, 0f),   // Room 4
    new Vector3(-3900f, -922f, 0f),  // Room 5
    new Vector3(-4250f, 1288f, 0f),  // Room 6
    new Vector3(-2275f, 1251f, 0f), // Room 7
    new Vector3(-212f, 1215f, 0f), // Room 8
    new Vector3(70f, 2210f, 0f), // Room 9
    new Vector3(-1982f, 2318f, 0f),   // Room 10
    new Vector3(-4104f, 2268f, 0f),  // Room 11
    new Vector3(-431f, -933f, 0f)   // Room 12
};

        // Устанавливаем позицию на основе номера комнаты
        if (current_room >= 1 && current_room <= roomPositions.Length)
        {
            CircleLight.anchoredPosition = roomPositions[current_room - 1];
        }
        else
        {
            Debug.LogWarning("Invalid room number: " + current_room);
        }
        _GlitchController._SetDefaultGlitchNumbers();
        DeadScene();
    }
    private void DeadScene()
    {
        if (warningAlert != null)
        {
            warningAlert.SetActive(false);
        }
        alertPlay = false;
        if (video1 != null)
        {
            video1.volume = 0f;
        }
        if (video2 != null)
        {
            video2.volume = 0f;
        }
        if (video3 != null)
        {
            video3.volume = 0f;
        }
        _GlitchController._SetManiacDeadGlitch();
        timeManager.glassesAllow = false;
        chapter1Screamer.StartHorror();
        if (current_room == 6)
        {
            move_id = Random.Range(0, 2) == 0 ? Random.Range(1, 4) : Random.Range(9, 13);
        }
        else
        {
            move_id = current_room > 6 ? Random.Range(1, 5) : Random.Range(8, 13);
        }
    }
    private void StopCoroutineManiacMadness()
    {
        if(maniacMadnessCrt != null)
        {
            StopCoroutine(maniacMadnessCrt);
        }

        //значения по умолчанию
        tsutomuBodyImage[0].sprite = maniacBodySprites[0];
        tsutomuBodyImage[1].sprite = maniacBodySprites[0];
        tsutomuBodyImage[2].sprite = maniacBodySprites[0];
        tsutomuBodyImage[3].sprite = maniacBodySprites[0];
        tsutomuFlash[0].color = 
            new Color(tsutomuFlash[0].color.r, tsutomuFlash[0].color.g, tsutomuFlash[0].color.b, 0f);
        tsutomuFlash[1].color =
            new Color(tsutomuFlash[1].color.r, tsutomuFlash[1].color.g, tsutomuFlash[1].color.b, 0f);
        tsutomuFlash[2].color =
            new Color(tsutomuFlash[2].color.r, tsutomuFlash[2].color.g, tsutomuFlash[2].color.b, 0f);
        tsutomuFlash[3].color =
            new Color(tsutomuFlash[3].color.r, tsutomuFlash[3].color.g, tsutomuFlash[3].color.b, 0f);
    }
    private IEnumerator ManiacMadness()
    {
        horrorBehindShkafSource.PlayOneShot(horrorTsutomuClip);
        while (true) 
        {
            yield return new WaitForSeconds(0.2f);
            if(VariableDeterminator.current_shkaf == 1)
            {
                tsutomuBodyImage[0].sprite = maniacBodySprites[1];
                tsutomuBodyImage[1].sprite = maniacBodySprites[1];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                tsutomuBodyImage[2].sprite = maniacBodySprites[1];
                tsutomuBodyImage[3].sprite = maniacBodySprites[1];
            }
            yield return new WaitForSeconds(0.25f);
            if (VariableDeterminator.current_shkaf == 1)
            {
                tsutomuBodyImage[0].sprite = maniacBodySprites[0];
                tsutomuBodyImage[1].sprite = maniacBodySprites[0];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                tsutomuBodyImage[2].sprite = maniacBodySprites[0];
                tsutomuBodyImage[3].sprite = maniacBodySprites[0];
            }
            yield return new WaitForSeconds(0.1f);
            horrorBehindShkafSource.PlayOneShot(photoCameraClip);
            if (VariableDeterminator.current_shkaf == 1)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[0], 0.1f, 0.50f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[1], 0.1f, 0.50f));
                tsutomuBodyImage[0].sprite = maniacBodySprites[1];
                tsutomuBodyImage[1].sprite = maniacBodySprites[1];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[0], 0.1f, 0.50f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[1], 0.1f, 0.50f));
                tsutomuBodyImage[2].sprite = maniacBodySprites[1];
                tsutomuBodyImage[3].sprite = maniacBodySprites[1];
            }
            yield return new WaitForSeconds(0.1f);
            if (VariableDeterminator.current_shkaf == 1)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[2], 0.1f, 0f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[3], 0.1f, 0f));
                tsutomuBodyImage[0].sprite = maniacBodySprites[0];
                tsutomuBodyImage[1].sprite = maniacBodySprites[0];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[2], 0.1f, 0f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[3], 0.1f, 0f));
                tsutomuBodyImage[2].sprite = maniacBodySprites[0];
                tsutomuBodyImage[3].sprite = maniacBodySprites[0];
            }
            yield return new WaitForSeconds(0.1f);
            horrorBehindShkafSource.PlayOneShot(photoCameraClip);
            if (VariableDeterminator.current_shkaf == 1)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[0], 0.05f, 0.8f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[1], 0.05f, 0.8f));
                tsutomuBodyImage[0].sprite = maniacBodySprites[1];
                tsutomuBodyImage[1].sprite = maniacBodySprites[1];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[2], 0.05f, 0.8f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[3], 0.05f, 0.8f));
                tsutomuBodyImage[2].sprite = maniacBodySprites[1];
                tsutomuBodyImage[3].sprite = maniacBodySprites[1];
            }
            yield return new WaitForSeconds(0.05f);
            if (VariableDeterminator.current_shkaf == 1)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[0], 0.1f, 0f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[1], 0.1f, 0f));
                tsutomuBodyImage[0].sprite = maniacBodySprites[0];
                tsutomuBodyImage[1].sprite = maniacBodySprites[0];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[2], 0.1f, 0f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[3], 0.1f, 0f));
                tsutomuBodyImage[2].sprite = maniacBodySprites[0];
                tsutomuBodyImage[3].sprite = maniacBodySprites[0];
            }
            yield return new WaitForSeconds(0.1f);
            horrorBehindShkafSource.PlayOneShot(photoCameraClip);
            if (VariableDeterminator.current_shkaf == 1)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[0], 0.1f, 0.75f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[1], 0.1f, 0.75f));
                tsutomuBodyImage[0].sprite = maniacBodySprites[1];
                tsutomuBodyImage[1].sprite = maniacBodySprites[1];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[2], 0.1f, 0.75f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[3], 0.1f, 0.75f));
                tsutomuBodyImage[2].sprite = maniacBodySprites[1];
                tsutomuBodyImage[3].sprite = maniacBodySprites[1];
            }
            yield return new WaitForSeconds(0.1f);
            if (VariableDeterminator.current_shkaf == 1)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[0], 0.1f, 0f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[1], 0.1f, 0f));
                tsutomuBodyImage[0].sprite = maniacBodySprites[0];
                tsutomuBodyImage[1].sprite = maniacBodySprites[0];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[2], 0.1f, 0f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[3], 0.1f, 0f));
                tsutomuBodyImage[2].sprite = maniacBodySprites[0];
                tsutomuBodyImage[3].sprite = maniacBodySprites[0];
            }
            yield return new WaitForSeconds(0.01f);
            horrorBehindShkafSource.PlayOneShot(photoCameraClip);
            if (VariableDeterminator.current_shkaf == 1)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[0], 0.5f, 1f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[1], 0.5f, 1f));
                tsutomuBodyImage[0].sprite = maniacBodySprites[1];
                tsutomuBodyImage[1].sprite = maniacBodySprites[1];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[2], 0.5f, 1f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[3], 0.5f, 1f));
                tsutomuBodyImage[2].sprite = maniacBodySprites[1];
                tsutomuBodyImage[3].sprite = maniacBodySprites[1];
            }
            yield return new WaitForSeconds(0.5f);
            if (VariableDeterminator.current_shkaf == 1)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[0], 0.1f, 0f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[1], 0.1f, 0f));
                tsutomuBodyImage[0].sprite = maniacBodySprites[0];
                tsutomuBodyImage[1].sprite = maniacBodySprites[0];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[2], 0.1f, 0f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[3], 0.1f, 0f));
                tsutomuBodyImage[2].sprite = maniacBodySprites[0];
                tsutomuBodyImage[3].sprite = maniacBodySprites[0];
            }
            yield return new WaitForSeconds(0.1f);
            horrorBehindShkafSource.PlayOneShot(photoCameraClip);
            if (VariableDeterminator.current_shkaf == 1)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[0], 0.1f, 0.6f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[1], 0.1f, 0.6f));
                tsutomuBodyImage[0].sprite = maniacBodySprites[1];
                tsutomuBodyImage[1].sprite = maniacBodySprites[1];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[2], 0.1f, 0.6f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[3], 0.1f, 0.6f));
                tsutomuBodyImage[2].sprite = maniacBodySprites[1];
                tsutomuBodyImage[3].sprite = maniacBodySprites[1];
            }
            yield return new WaitForSeconds(0.1f);
            if (VariableDeterminator.current_shkaf == 1)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[0], 0.1f, 0f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[1], 0.1f, 0f));
                tsutomuBodyImage[0].sprite = maniacBodySprites[0];
                tsutomuBodyImage[1].sprite = maniacBodySprites[0];
            }
            else if (VariableDeterminator.current_shkaf == 2)
            {
                StartCoroutine(FadeToAlpha(tsutomuFlash[2], 0.1f, 0f));
                StartCoroutine(FadeToAlpha(tsutomuFlash[3], 0.1f, 0f));
                tsutomuBodyImage[2].sprite = maniacBodySprites[0];
                tsutomuBodyImage[3].sprite = maniacBodySprites[0];
            }
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