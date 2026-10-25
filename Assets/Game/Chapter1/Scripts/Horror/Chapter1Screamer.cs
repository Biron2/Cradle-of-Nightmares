using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chapter1Screamer : MonoBehaviour
{
    [SerializeField] private GameObject sceneHorror;
    [SerializeField] private CanvasGroup canvasSceneHorror;
    [SerializeField] private GameObject sceneHorror2;
    [SerializeField] private CanvasGroup canvasSceneHorror2;
    [SerializeField] private RectTransform maniacRectTransform;
    [SerializeField] private ManiacMove maniacMove;
    [SerializeField] private TVmanager _TVmanager;


    // Параметры тряски
    public float shakeDuration = 0.5f;    // Продолжительность тряски
    public float shakeAmount = 100f;      // Амплитуда тряски

    private Vector3 originalPosition;      // Исходная позиция объекта
    private float shakeTimer = 0f;         // Таймер тряски
    bool allowShake = false;
    bool allowShake2 = false;

    public Image redCorners1, redBackground1;

    public Image redCorners2, redBackground2;
    public float duration1 = 1f;

    private float targetAlpha1 = 1f;
    private float currentAlpha1;
    private float timer1 = 0f;

    public float duration2 = 3f;

    private float targetAlpha2 = 1f;
    private float currentAlpha2;
    private float timer2 = 0f;
    public float growDuration = 3f;

    private bool canvasAllow1 = false;
    private bool canvasAllow2 = false;
    public float TimeToFade;
    public TimeManager timeManager;
    [SerializeField] private AudioSource video1, video2;
    [SerializeField] private GameObject[] arrows_containers;

    public AudioSource screams;
    public AudioClip scream1, scream2;

    public CanvasGroup interfaceCanvas;
    [SerializeField] private GameObject faust1, faust2, faust3, cake1, cake2, cake3;
    [SerializeField] private GameObject loadingScene;
    private ColliderList colliderList;
    public VariableDeterminator variableDeterminator;
    public scalesGameFaces mScalesGameFaces1, mScalesGameFaces2, mScalesGameFaces3, mScalesGameFaces4, mScalesGameFaces5;

    [SerializeField] private PauseScript _PauseScript;
    [SerializeField] private Animator animatorGein;

    private int i = 1;
    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        originalPosition = maniacRectTransform.anchoredPosition;
        currentAlpha1 = 0f;
        redCorners1.color = new Color(redCorners1.color.r, redCorners1.color.g, redCorners1.color.b, currentAlpha1);
        redCorners2.color = new Color(redCorners2.color.r, redCorners2.color.g, redCorners2.color.b, currentAlpha1);

        targetAlpha1 = 1f;

        currentAlpha2 = 0f;
        redBackground1.color = new Color(redBackground1.color.r, redBackground1.color.g, redBackground1.color.b, currentAlpha2);
        redBackground2.color = new Color(redBackground2.color.r, redBackground2.color.g, redBackground2.color.b, currentAlpha2);

        targetAlpha2 = 1f;
    }
    void Update()
    {
        if(allowShake == true)
        {
            if (shakeTimer >= shakeDuration)
            {
                maniacRectTransform.anchoredPosition = originalPosition;
                return;
            }

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            maniacRectTransform.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            shakeTimer += Time.deltaTime;

            // Запускаем таймер
            timer1 += Time.deltaTime;

            // Вычисляем новую прозрачность изображения
            currentAlpha1 = Mathf.Lerp(0f, targetAlpha1, timer1 / duration1);
            redCorners1.color = new Color(redCorners1.color.r, redCorners1.color.g, redCorners1.color.b, currentAlpha1);
            if (currentAlpha1 >= targetAlpha1)
            {
                timer1 = 0f;
                targetAlpha1 = 0f;
            }

            timer2 += Time.deltaTime;
            currentAlpha2 = Mathf.Lerp(0f, targetAlpha2, timer2 / duration2);
            redBackground1.color = new Color(redBackground1.color.r, redBackground1.color.g, redBackground1.color.b, currentAlpha2);
            if (currentAlpha2 >= targetAlpha2)
            {
                allowShake = false;
                canvasAllow1 = true;
            }
        }

        if(canvasAllow1 == true)
        {
            if (canvasSceneHorror.alpha != 0)
            {
                canvasSceneHorror.alpha -= TimeToFade * Time.deltaTime;
            }
            else if (canvasSceneHorror.alpha == 0)
            {
                if (variableDeterminator.faceTalking == true)
                {
                    mScalesGameFaces1.karkarCoroutine = StartCoroutine(mScalesGameFaces1.startKrikMethod());
                }
                foreach (GameObject obj in arrows_containers)
                {
                    if (obj != null)
                    {
                        obj.SetActive(true);
                    }
                }
                video1.volume = 0.5f;
                video2.volume = 0.5f;
                _PauseScript.offMouse = false;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if(maniacMove.current_room == 9)
                {
                    _TVmanager.Start_Videos();
                }
                _PauseScript.BlockCollidersActive = false; 
                timeManager.isPaused = false;
                colliderList.ENABLE_COLLIDER();
                canvasAllow1 = false;
                sceneHorror.SetActive(false);
                //возвращаем значения по умолчанию
                redCorners1.color = new Color(redCorners1.color.r, redCorners1.color.g, redCorners1.color.b, 0f);
                redBackground1.color = new Color(redBackground1.color.r, redBackground1.color.g, redBackground1.color.b, 0f);

                targetAlpha1 = 1f;
                currentAlpha1 = 0f;
                timer1 = 0f; 
                duration1 = 2f;
                duration2 = 4f;

                targetAlpha2 = 1f;
                currentAlpha2 = 0f;
                timer2 = 0f;
                shakeDuration = 6f;
                shakeAmount = 100f;
                shakeTimer = 0f;

                timeManager.glassesAllow = false;
                interfaceCanvas.alpha = 1;

                updateHealthBar();
            }
        }

        //----------------------------------------


        //----------------------------------------


        //----------------------------------------



        if (allowShake2 == true)
        {
            animatorGein.Play("geinScaleUp");
            // Запускаем таймер
            timer1 += Time.deltaTime;

            // Вычисляем новую прозрачность изображения
            currentAlpha1 = Mathf.Lerp(0f, targetAlpha1, timer1 / growDuration);
            redCorners2.color = new Color(redCorners2.color.r, redCorners2.color.g, redCorners2.color.b, currentAlpha1);
            if (currentAlpha1 >= targetAlpha1)
            {
                timer1 = 0f;
                targetAlpha1 = 0f;
            }

            timer2 += Time.deltaTime;
            currentAlpha2 = Mathf.Lerp(0f, targetAlpha2, timer2 / growDuration);
            redBackground2.color = new Color(redBackground2.color.r, redBackground2.color.g, redBackground2.color.b, currentAlpha2);
            if (currentAlpha2 >= targetAlpha2)
            {
                allowShake2 = false;
                canvasAllow2 = true;
            }
        }

        if (canvasAllow2 == true)
        {
            if (canvasSceneHorror2.alpha != 0)
            {
                canvasSceneHorror2.alpha -= TimeToFade * Time.deltaTime;
            }
            else if (canvasSceneHorror2.alpha == 0)
            {
                _PauseScript.offMouse = false;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if (!VariableDeterminator.stopShowPause)
                {
                    video1.volume = 0.5f;
                    video2.volume = 0.5f;
                    if (variableDeterminator.faceTalking == true)
                    {
                        mScalesGameFaces1.karkarCoroutine = StartCoroutine(mScalesGameFaces1.startKrikMethod());
                    }
                    foreach (GameObject obj in arrows_containers)
                    {
                        if (obj != null)
                        {
                            obj.SetActive(true);
                        }
                    }

                    if (maniacMove.current_room == 9)
                    {
                        _TVmanager.Start_Videos();
                    }
                    colliderList.ENABLE_COLLIDER();
                    interfaceCanvas.alpha = 1;
                }
                _PauseScript.BlockCollidersActive = false;
                timeManager.isPaused = false;
                canvasAllow2 = false;
                sceneHorror2.SetActive(false);
                //возвращаем значения по умолчанию
                redCorners2.color = new Color(redCorners2.color.r, redCorners2.color.g, redCorners2.color.b, 0f);
                redBackground2.color = new Color(redBackground2.color.r, redBackground2.color.g, redBackground2.color.b, 0f);

                targetAlpha1 = 1f;
                currentAlpha1 = 0f;
                timer1 = 0f;
                duration1 = 2f;
                duration2 = 4f;

                targetAlpha2 = 1f;
                currentAlpha2 = 0f;
                timer2 = 0f;
                shakeDuration = 6f;
                shakeAmount = 100f;
                shakeTimer = 0f;

                timeManager.glassesAllow = false;

                updateHealthBar();
            }
        }
    }

    public void updateHealthBar()
    {
        if (faust1.activeSelf)
        {
            faust1.SetActive(false);
            faust2.SetActive(true);
            cake3.SetActive(false);
        }
        else if (faust2.activeSelf)
        {
            faust2.SetActive(false);
            faust3.SetActive(true);
            cake2.SetActive(false);
        }
        else if (faust3.activeSelf)
        {
            timeManager.StopAllCoroutines();
            variableDeterminator.ShowDeathMenu();
        }
    }

    public void StartHorror()
    {
        maniacMove.DisableManiac();
        colliderList.DISABLE_COLLIDER();
        if (i == 1)
        {
            canvasSceneHorror2.alpha = 1f;
            screams.PlayOneShot(scream1);
            sceneHorror2.SetActive(true);
            allowShake2 = true;
            i++;
        }
        else
        {
            canvasSceneHorror.alpha = 1f;
            screams.PlayOneShot(scream2);
            sceneHorror.SetActive(true);
            allowShake = true;
        }
        interfaceCanvas.alpha = 0;
    }
}