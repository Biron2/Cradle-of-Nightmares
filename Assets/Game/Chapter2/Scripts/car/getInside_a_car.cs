using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class getInside_a_car : MonoBehaviour
{
    public GameObject objectToActive;
    public CanvasGroup blackBackground;
    private ColliderList colliderList;
    [SerializeField] private GameObject[] arrows_containers;

    private audio_for_items _Audio_For_Items;
    private float cooldown = 0.5f; // ¬рем¤ задержки в секундах
    private float lastClickTime = 0f; // ¬рем¤ последнего нажати¤

    private float transitionStartTime;
    public float transitionDuration = 1.0f;
    public float targetSize = 2f;
    private Camera mainCamera;
    private Coroutine coroutine;

    [SerializeField] private AudioSource getInsideTheCarSound;
    [SerializeField] private GameObject mLamp, carLamp;
    [SerializeField] private AudioSource mLampSnd;

    [SerializeField] private Sprite openedCarSprite;
    private void Start()
    {
        mainCamera = Camera.main;
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
            if (VariableDeterminator.carClimpingProcessRunning)
            {
                return;
            }
            if (Time.time - lastClickTime < cooldown)
            {
                // ¬рем¤ после последнего нажати¤ меньше задержки, поэтому ничего не делаем
                return;
            }
            lastClickTime = Time.time; // ќбновл¤ем врем¤ последнего нажати¤
            _Audio_For_Items.PlaySounds(gameObject.name);
            foreach (GameObject obj in arrows_containers)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            colliderList.DISABLE_COLLIDER();
            VariableDeterminator.isMiniGameActive = true;
            StartCoroutine(getInsideTheCar());
            coroutine = StartCoroutine(TransitionCamera());
            VariableDeterminator.carClimpingProcessRunning = true;
            getInsideTheCarSound.PlayOneShot(getInsideTheCarSound.clip);
            GetComponent<Image>().sprite = openedCarSprite;
        }
    }
    private IEnumerator TransitionCamera()
    {
        transitionStartTime = Time.time;
        Vector2 initialPosition = mainCamera.transform.position;
        float initialSize = mainCamera.orthographicSize;

        // Берем начальное значение Z позиции камеры
        float initialZ = mainCamera.transform.position.z;

        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            mainCamera.orthographicSize = Mathf.Lerp(initialSize, targetSize, t);

            // Применяем Lerp к Vector3, сохраняем Z координату
            mainCamera.transform.position = Vector3.Lerp(
                new Vector3(initialPosition.x, initialPosition.y, initialZ),
                new Vector3(-17.99f, -12.36f, initialZ),
                t);

            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = new Vector3(-17.99f, -12.36f, initialZ);
    }

    private IEnumerator getInsideTheCar()
    {
        blackBackground.gameObject.SetActive(true);
        float duration = 0.75f; // длительность анимации в секундах
        float elapsed = 0f;

        // Плавное повышение альфы до 1
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            blackBackground.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        blackBackground.alpha = 1f;
        mLamp.SetActive(false);
        mLampSnd.volume = 0f;
        carLamp.SetActive(false);
        objectToActive.SetActive(true);
        StopCoroutine(coroutine);
        mainCamera.orthographicSize = 5f;
        mainCamera.transform.position = new Vector3(-17.777f, -10f, 0f);
        // Подождать немного перед затемнением обратно (если нужно)
        yield return new WaitForSeconds(0.5f);
        // Плавное понижение альфы обратно до 0
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            blackBackground.alpha = Mathf.Clamp01(1f - (elapsed / duration));
            yield return null;
        }

        blackBackground.alpha = 0f;
        VariableDeterminator.playerIsInTheCar = true;
        public_actions.Instance.OnOff_SpaceTip_ExitCar(true);
        blackBackground.gameObject.SetActive(false);
        VariableDeterminator.carClimpingProcessRunning = false;
    }
    public void startGetOutside(GameObject objToOff)
    {
        StartCoroutine(getOutsideTheCar(objToOff));
    }
    private IEnumerator getOutsideTheCar(GameObject objToOff)
    {
        blackBackground.gameObject.SetActive(true);
        float duration = 0.75f; // длительность анимации в секундах
        float elapsed = 0f;

        // Плавное повышение альфы до 1
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            blackBackground.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        blackBackground.alpha = 1f;
        mLamp.SetActive(true);
        mLampSnd.volume = 1f;
        objToOff.SetActive(false);

        // Подождать немного перед затемнением обратно (если нужно)
        yield return new WaitForSeconds(0.5f);

        // Плавное понижение альфы обратно до 0
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            blackBackground.alpha = Mathf.Clamp01(1f - (elapsed / duration));
            yield return null;
        }

        blackBackground.alpha = 0f;
        blackBackground.gameObject.SetActive(false);
        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        colliderList.ENABLE_COLLIDER();
        VariableDeterminator.carClimpingProcessRunning = false;
        VariableDeterminator.isMiniGameActive = false;
    }
}
