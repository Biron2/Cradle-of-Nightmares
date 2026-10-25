using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class VariableDeterminator : MonoBehaviour
{
    public static int current_shkaf = 1;
    public int stopPlayLight = 0;
    public int current_roomNumber = 0;
    public bool faceTalking = false;
    public string pathOfSlot = "";
    public static bool isMiniGameActive = false;
    public static bool Isfullkanistra = false;
    public static bool isDraging = false;
    public static bool shotedByManiac = false;
    public static bool stopShowPause = false;
    public static bool isOpenRoom5 = false;
    public static bool allowNamePlatesGame = false;
    public static bool isUserInNameplatesGame = false;
    public static bool isUserInNameplatesGameZoomIn = false;
    public static bool isUserInSafeZoomIn = false;
    public static bool isUserInOldPhoneZoomIn = false;
    public static bool isUserInBooksZoomIn = false;
    public static bool isUserInPictureZoomIn = false;
    public static bool allowOldPhonePuzzle = true;
    public static bool allowPartsMove = true;
    public static bool allowNameplatesPazzle = true;
    public static bool allowSafePuzzle = true;
    public static bool clownBalloonPuzzleFinished = false;
    public static bool allowPhotoPainting = false;
    public static bool playerIsInTheCar = false;
    public static bool carClimpingProcessRunning = false;
    public static bool allowCurtainGame = true;
    public static bool currentTimeIsNight = false;
    public static bool allowToZoomBooks = false;
    public static bool allowToDragJusan = false;
    public static bool needlesPuten = false;
    public static bool firstNightAlreadyStarted = false;

    [Header("CloseAllGames")]
    [SerializeField] private UnityEvent events;
    [SerializeField] private EscClose[] escClose;

    [Header("DeathMenu")]
    [SerializeField] private GameObject _DeathMenuObj;
    [SerializeField] private CanvasGroup title, buttonsContainer;
    [SerializeField] private GameObject loadingScene;
    private bool isClicked = false;
    [SerializeField] private AudioSource deathLastSound;
    [SerializeField] private RectTransform _SmokePos;
    private Timer _Timer;
    private ColliderList colliderList;

    public void ReceiveObjectPath(string path)
    {
        pathOfSlot = path;
    }
    //Возвращаем статические переменные к начальным значениям
    private void Awake()
    {
        //Chapter2
        isOpenRoom5 = false;
        allowNamePlatesGame = false;
        isUserInNameplatesGame = false;
        isUserInNameplatesGameZoomIn = false;
        allowPartsMove = true; 
        allowNameplatesPazzle = true;
        MoveRoom2.alreadyRoom5 = false;
        clownBalloonPuzzleFinished = false;
        allowSafePuzzle = true;
        switchPicture4x4.stepScales1 = 1;
        switchPicture4x4.stepScales2 = 1;
        switchPicture4x4.stepScales3 = 1;
        switchPicture4x4.stepScales4 = 1;
        allowPhotoPainting = false;
        isUserInSafeZoomIn = false;
        playerIsInTheCar = false;
        carClimpingProcessRunning = false;
        MoveRoom2.carEngine = false;
		isUserInOldPhoneZoomIn = false;
		allowOldPhonePuzzle = true;
        phone_key_clicking.allowWinConditionForPhone = true;
        Block.isAllowToMoveBlocks = true;
        curtainGame.stepScales1 = 4;
        curtainGame.stepScales2 = 3;
        curtainGame.stepScales3 = 1;
        curtainGame.stepScales4 = 4;
        allowCurtainGame = true;
        DragAndDrop2.robotPartsPuten = 0;
        DragAndDrop2.bookPut = 0;
        DragAndDrop2.bubblesPut = 0;
        currentTimeIsNight = false;
        allowToZoomBooks = false;
        isUserInBooksZoomIn = false;
        allowToDragJusan = false;
        DragAndDrop2.hintPuten = 0;
        DragAndDrop2.hintPrinterPuten = 0;
        HideInShkaf2.inShkaf = false;
        HideInShkaf2.nowInShkaf = false;
        HideInShkaf2.ableToLeaveFromTheShelf = true;
        HideInShkaf2.allowJoinInHidePlace = true;
        needlesPuten = false;
        DragAndDrop2.alreadyPuten1 = false;
        DragAndDrop2.alreadyPuten2 = false;
        DragAndDrop2.tarakanKilled1 = false;
        DragAndDrop2.tarakanKilled2 = false;
        DragAndDrop2.tarakanKilled3 = false;
        DragAndDrop2.tarakanKilled4 = false;
        firstNightAlreadyStarted = false;
        scalesGameFaces.allowScalePressing = true;
        scalesGame.allowScalePressing = true;

        //Chapter1
        shotedByManiac = false;
        current_shkaf = 1;
        isDraging = false;
        Isfullkanistra = false;
        isMiniGameActive = false;
        HideInShkaf.inShkaf = false;
        HideInShkaf.nowInShkaf = false;
        HideInShkaf.ableToLeaveFromTheShelf = true;
        HideInShkaf.allowJoinInHidePlace = true;
        DragAndDrop.openGrowBool = false;
        GridScrolling.currentPage = 0;
        GridScrolling.isAnimated = false;
        ClickSwap.allowRepeatUpdate = true;
        shoeSwap.allowRepeatUpdate = true;
        scalesGameFaces.stepScaless1 = 2;
        scalesGameFaces.stepScaless2 = 3;
        scalesGameFaces.stepScaless3 = 1;
        scalesGameFaces.stepScaless4 = 3;
        scalesGameFaces.stepScaless5 = 4;
        scalesGame.stepScales1 = 4;
        scalesGame.stepScales2 = 4;
        scalesGame.stepScales3 = 4;
        scalesGame.stepScales4 = 4;
        switchPictureGame.stepScales1 = 1;
        switchPictureGame.stepScales2 = 1;
        switchPictureGame.stepScales3 = 1;
        switchPictureGame.stepScales4 = 1;
        switchPictureGame.stepScales5 = 1;
        PianoKey.password = "";
        MoveRoom.motorStart = false;
        MoveRoom.alreadyRoom2 = false;
        DragAndDrop.clickCount = 0;
        DragAndDrop.clickCountDetails = 0;
        DragAndDrop.clickTushka = 0;
    }
    private void Start()
    {
        try
        {
            _Timer = GameObject.Find("Timer").GetComponent<Timer>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
    }
    public void DisableAllMiniGames()
    {
        events.Invoke();
        foreach (EscClose escC in escClose)
        {
            escC.closeThisGame();
        }
    }
    public void CallFadeTooltip(float targetAlpha, CanvasGroup canvas, GameObject tooltipInstance, float fadeDuration)
    {
        StartCoroutine(FadeTooltip(targetAlpha, canvas, tooltipInstance, fadeDuration));
    }
    private IEnumerator FadeTooltip(float targetAlpha, CanvasGroup canvas, GameObject tooltipInstance, float fadeDuration)
    {
        if (canvas == null) yield break;

        float startAlpha = canvas.alpha;
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            if (canvas == null) yield break; // Check if the CanvasGroup has been destroyed

            elapsed += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        if (canvas != null) canvas.alpha = targetAlpha;

        if (targetAlpha == 0 && tooltipInstance != null)
        {
            Destroy(tooltipInstance); // Destroy the tooltip after it fades out
        }
    }
    public void ShowDeathMenu()
    {
        colliderList.DISABLE_COLLIDER();
        deathLastSound.Play();
        stopShowPause = true;
        StartCoroutine(DeathMenuCor(1f, 2));
        StartCoroutine(moveSmokePos());
    }
    private IEnumerator moveSmokePos()
    {
        Vector2 startPos = _SmokePos.anchoredPosition;
        Vector2 targetPos = Vector2.zero;
        float duration = 3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _SmokePos.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Гарантируем точную финальную позицию
        _SmokePos.anchoredPosition = targetPos;
    }
    private IEnumerator DeathMenuCor(float targetAlpha, float fadeDuration)
    {
        _DeathMenuObj.SetActive(true);

        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            title.alpha = Mathf.Lerp(0, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        title.alpha = targetAlpha;

        yield return new WaitForSeconds(0.5f);

        float elapsed2 = 0;

        while (elapsed2 < fadeDuration)
        {
            elapsed2 += Time.deltaTime;
            buttonsContainer.alpha = Mathf.Lerp(0, targetAlpha, elapsed2 / fadeDuration);
            yield return null;
        }

        buttonsContainer.alpha = targetAlpha;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void AgainThisLevel()
    {
        if (!isClicked)
        {
            isClicked = true;
            loadingScene.SetActive(true);
            StartCoroutine(FadeToAudio(0f, 0.8f));
            StartCoroutine(againLevelCrt(0f, 1f));
        }
    }
    public void LeaveMenu()
    {
        if (!isClicked)
        {
            if (_Timer != null)
            {
                _Timer.ResetTimer();
                _Timer.StopTimer();
                Destroy(_Timer.gameObject);
            }
            isClicked = true;
            loadingScene.SetActive(true);
            StartCoroutine(FadeToAudio(0f, 0.8f));
            StartCoroutine(leaveMenuCrt(0f, 1f));
        }
    }
    private IEnumerator againLevelCrt(float targetAlpha, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            title.alpha = Mathf.MoveTowards(title.alpha, targetAlpha, time / duration);
            buttonsContainer.alpha = title.alpha;
            yield return null;
        }

        // Убедимся, что alpha точно установлена в targetAlpha
        title.alpha = targetAlpha;
        buttonsContainer.alpha = targetAlpha;

        yield return new WaitForSeconds(0.2f);

        if (targetAlpha == 0)
        {
            title.gameObject.SetActive(false);
            buttonsContainer.gameObject.SetActive(false);
        }

        yield return null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator leaveMenuCrt(float targetAlpha, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            title.alpha = Mathf.MoveTowards(title.alpha, targetAlpha, time / duration);
            buttonsContainer.alpha = title.alpha;
            yield return null;
        }

        // Убедимся, что alpha точно установлена в targetAlpha
        title.alpha = targetAlpha;
        buttonsContainer.alpha = targetAlpha;

        yield return new WaitForSeconds(0.2f);

        if (targetAlpha == 0)
        {
            title.gameObject.SetActive(false);
            buttonsContainer.gameObject.SetActive(false);
        }

        yield return null;

        SceneManager.LoadScene("Menu");
    }
    private IEnumerator FadeToAudio(float targetAlpha, float duration)
    {
        float startAlpha = deathLastSound.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            deathLastSound.volume = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        deathLastSound.volume = targetAlpha; // Убедитесь, что мы устанавливаем конечное значение
        deathLastSound.Stop();
    }
}
