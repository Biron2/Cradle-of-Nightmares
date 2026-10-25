using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManiacMove : MonoBehaviour
{
    public int current_room = 1;
    public int move_id = 12;
    [SerializeField] private RectTransform[] rooms_maniac_pos;
    [SerializeField] private Vector3[] maniac_pos;
    public bool inShkaf = false;
    public Chapter1Screamer chapter1Screamer;
    private RectTransform rectTransform;
    public TimeManager timeManager;
    public AudioSource step;
    [SerializeField] private AudioSource babyLaugh, video1, video2;

    [SerializeField] public AudioSource mHahaSnd;
    private float delay = 2.25f;
    public bool onceAction = true, onceAction2 = true;
    [SerializeField] public GameObject[] maniacInShkafs;
    Coroutine moveCoroutine, respawnCrt, waitCrt;

    public VariableDeterminator variableDeterminator;
    public scalesGameFaces mScalesGameFaces1, mScalesGameFaces2, mScalesGameFaces3, mScalesGameFaces4, mScalesGameFaces5;
    [SerializeField] private RectTransform CircleLight;
    [SerializeField] private GameObject Glass_Container;
    private ColliderList colliderList;
    [SerializeField] private GameObject[] arrows_containers;
    public bool AllowMovement = true;

    [SerializeField] private PauseScript _PauseScript;
    [SerializeField] private GlitchController _GlitchController;
    [SerializeField] private TVmanager _TVmanager;
    private StepController _StepController;
    [SerializeField] private AudioSource[] maniacVoice;
    private Coroutine mouthMovementCoroutine;
    [SerializeField] private GameObject warningAlert;
    private bool alertPlay = false;

    private bool isActive = false; // Состояние активности
    private float timer = 0.0f; // Таймер для отслеживания времени
    [SerializeField] private AudioSource warningSnd;

    private int[] selectedPath;

    //Маршруты маньяка до открытия 3 этажа
    private List<int[]> pathsFirst = new List<int[]>
{
    new int[] { 7, 6, 5, 2, 3, 5, 6, 5, 2, 1, 2, 3, 4, 3, 2, 5, 6, 7, 6, 5, 2, 1, 2, 3, 4, 3, 5, 6, 7, 6, 5, 2, 3, 4, 3, 2, 1, 2 }, // c 7 комнаты движение
    new int[] { 1, 2, 5, 6, 7, 6, 5, 2, 3, 4, 3, 2, 1, 2, 5, 6, 7, 6, 5, 2, 1, 2, 3, 4, 3, 5, 6, 7, 6, 5, 6, 7, 6, 5, 2, 3, 5, 6 } // c 1 комнаты движение
};

    //Маршруты маньяка после открытия 3 этажа
    private List<int[]> pathsSecond = new List<int[]>
{
    new int[] { 12, 11, 10, 8, 9, 8, 7, 6, 5, 3, 4, 3, 2, 1, 2, 5, 6, 7, 8, 9, 8, 10, 11, 12, 11, 10, 8, 7, 6, 5, 6, 5, 2, 1, 2, 5, 3, 4, 3, 5, 6, 7 }, // c 12 комнаты движение
    new int[] { 1, 2, 5, 6, 7, 6, 5, 3, 4, 3, 5, 2, 1, 2, 5, 6, 5, 6, 7, 8, 10, 11, 12, 11, 10, 8, 9, 8, 7, 6, 5, 3, 4, 3, 2, 1, 2, 8, 9, 8, 10, 11, 12 } // c 1 комнаты движение
};
    private bool waitForShkafSecondCrt = false;
    private void Awake()
    {
        _StepController = GetComponent<StepController>();
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
                move_id = 12;
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
        _GlitchController._SetDefaultGlitchNumbers();
        move_id = 0;
        rectTransform.gameObject.SetActive(false);
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
                _TVmanager.Stop_Videos();
                colliderList.DISABLE_COLLIDER();
            }
        }
        else if (current_room == move_id && inShkaf == true)
        {
            if (onceAction)
            {
                _GlitchController.defaultKrestNumbers();
                if (current_room == 2)
                {
                    ChangeManiacPosInShkaf(1);
                }
                else if (current_room == 6)
                {
                    ChangeManiacPosInShkaf(2);
                }
                else if (current_room == 8)
                {
                    ChangeManiacPosInShkaf(3);
                }
                if (moveCoroutine != null && timeManager.firstNight)
                {
                    StopCoroutine(moveCoroutine);
                }
                onceAction = false;
                step.volume = 0f;
                mHahaSnd.Play();
                _GlitchController._SetGlitchNumbers();
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
                    maniacInShkafs[2].SetActive(true);
                }
                else
                {
                    maniacInShkafs[1].SetActive(true);
                    maniacInShkafs[3].SetActive(true);
                }
                break;
                
            case 2:
                // Активируем случайный шкаф из группы 2
                if (Random.value > 0.5f)
                {
                    maniacInShkafs[4].SetActive(true);
                }
                else
                {
                    maniacInShkafs[5].SetActive(true);
                }
                break;

            case 3:
                // Активируем случайный шкаф из группы 3
                if (Random.value > 0.5f)
                {
                    maniacInShkafs[6].SetActive(true);
                }
                else
                {
                    maniacInShkafs[7].SetActive(true);
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
        _GlitchController._SetDefaultGlitchNumbers();
        foreach (GameObject obj in maniacInShkafs)
        {
            obj.SetActive(false);
        }
        if (timeManager.firstNight)
        {
            delay = 2.25f;
            moveCoroutine = StartCoroutine(FollowPresetPath());
            waitForShkafSecondCrt = false;
            StartCoroutine(enableBools());
        }
        else
        {
            delay = 3f;
            moveCoroutine = StartCoroutine(FollowPresetPath());
            waitForShkafSecondCrt = false;
            StartCoroutine(enableBools());
        }
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
        if (DragAndDrop.clickTushka == 2)
        {
            switch (current_room)
            {
                case var _ when current_room < 6:
                    selectedPath = pathsSecond[0];
                    break;
                case var _ when current_room > 6:
                    selectedPath = pathsSecond[1];
                    break;
                case var _ when current_room == 6:
                    if (Random.Range(0, 2) == 0)
                    {
                        selectedPath = pathsSecond[0];
                    }
                    else
                    {
                        selectedPath = pathsSecond[1];
                    }
                    break;
            }
        }
        else
        {
            switch (current_room)
            {
                case var _ when current_room < 6:
                    selectedPath = pathsFirst[0];
                    break;
                case var _ when current_room > 6:
                    selectedPath = pathsFirst[1];
                    break;
                case var _ when current_room == 6:
                    if (Random.Range(0, 2) == 0)
                    {
                        selectedPath = pathsFirst[0];
                    }
                    else
                    {
                        selectedPath = pathsFirst[1];
                    }
                    break;
            }
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
                case 1 when room_num is 1 or 2 or 3 or 5:
                case 2 when room_num is >= 1 and <= 6:
                case 3 when room_num is 1 or 2 or 3 or 4 or 5:
                case 4 when room_num is 2 or 3 or 4 or 5:
                case 5 when room_num is 1 or 2 or 3 or 5 or 6 or 7:
                case 6 when room_num is 2 or 5 or 6 or 7 or 8:
                case 7 when room_num is 5 or 6 or 7 or 8 or 9 or 10:
                case 8 when room_num is 6 or 7 or 8 or 9 or 10 or 11:
                case 9 when room_num is 6 or 7 or 8 or 9 or 10 or 11:
                case 10 when room_num is 7 or 8 or 9 or 10 or 11 or 12:
                case 11 when room_num is 8 or 9 or 10 or 11 or 12:
                case 12 when room_num is 8 or 9 or 10 or 11 or 12:
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
    new Vector3(-197f, 204f, 0f),   // Room 1
    new Vector3(2061f, 181f, 0f),   // Room 2
    new Vector3(3545f, -868f, 0f),  // Room 3
    new Vector3(3934f, 228f, 0f),   // Room 4
    new Vector3(1326f, 1289f, 0f),  // Room 5
    new Vector3(-218f, 1222f, 0f),  // Room 6
    new Vector3(-1710f, 1269f, 0f), // Room 7
    new Vector3(-1987f, 2319f, 0f), // Room 8
    new Vector3(-2301f, 3440f, 0f), // Room 9
    new Vector3(256f, 2305f, 0f),   // Room 10
    new Vector3(1621f, 2306f, 0f),  // Room 11
    new Vector3(2436f, 3379f, 0f)   // Room 12
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
        StartCoroutine(waitForSomeSec());
    }
    private IEnumerator waitForSomeSec()
    {
        yield return new WaitForSeconds(0.45f);
        DeadScene();
    }
    private void DeadScene()
    {
        if (warningAlert != null)
        {
            warningAlert.SetActive(false);
        }
        alertPlay = false;
        video1.volume = 0f;
        video2.volume = 0f;
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

        if (variableDeterminator.faceTalking == true)
        {
            mScalesGameFaces1.StopKarkarCoroutineMethod();
            mScalesGameFaces2.StopKarkarCoroutineMethod();
            mScalesGameFaces3.StopKarkarCoroutineMethod();
            mScalesGameFaces4.StopKarkarCoroutineMethod();
            mScalesGameFaces5.StopKarkarCoroutineMethod();
        }
    }
}