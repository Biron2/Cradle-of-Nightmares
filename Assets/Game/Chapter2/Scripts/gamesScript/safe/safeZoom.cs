using System.Collections;
using UnityEngine;

public class safeZoom : MonoBehaviour
{
    private ColliderList colliderList;
    [SerializeField] private GameObject[] arrows_containers;
    private Camera mainCamera;

    private audio_for_items _Audio_For_Items;
    private float cooldown = 0.5f; // ¬рем¤ задержки в секундах
    private float lastClickTime = 0f; // ¬рем¤ последнего нажати¤

    private float transitionStartTime;
    public float transitionDuration = 1.0f;
    public float targetSize = 2f;
    private Vector3 wantedPosition = new Vector3(3.65f, 8.24f, 0f);
    public float lastClickTimeForPass = 0f; // ¬рем¤ последнего нажати¤
    [SerializeField] private GameObject openSafeColliderObj;

    [Header("END_OBJ")]
    [SerializeField] private GameObject closedSafe;
    [SerializeField] private GameObject openedSafe;
    [SerializeField] private AudioSource openSound;

    [SerializeField] private GameObject GLASS_IN_WINDOW_PARENT;
    private void Start()
    {
        mainCamera = Camera.main;
        _Audio_For_Items = GameObject.Find("Other/GameManager/EtcSounds/__FOR_ITEMS__").GetComponent<audio_for_items>();
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        public_actions.Instance.closeMiniGameAction += OnMinigameClosed;
    }
    private void OnDestroy()
    {
        // Важно отписаться, чтобы не было утечек
        public_actions.Instance.closeMiniGameAction -= OnMinigameClosed;
    }
    private void OnMinigameClosed()
    {
        if (VariableDeterminator.isUserInNameplatesGame && VariableDeterminator.isUserInSafeZoomIn && gameObject.activeSelf)
        {
            VariableDeterminator.isUserInSafeZoomIn = false;
            _Audio_For_Items.PlaySounds("zoomOut");

            VariableDeterminator.isMiniGameActive = false;
            StartCoroutine(TransitionCameraOut());
            public_actions.Instance.OnOff_SpaceTip_Zoom(false);

            foreach (GameObject obj in arrows_containers)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
            GLASS_IN_WINDOW_PARENT.SetActive(false);
        }
    }
    public void SafeWasClicked()
    {
        if (VariableDeterminator.isUserInNameplatesGame || !VariableDeterminator.allowSafePuzzle)
        {
            return;
        }
        if (Time.time - lastClickTime < cooldown)
        {
            // ¬рем¤ после последнего нажати¤ меньше задержки, поэтому ничего не делаем
            return;
        }
        VariableDeterminator.isUserInNameplatesGame = true;
        lastClickTime = Time.time; // ќбновл¤ем врем¤ последнего нажати¤
        _Audio_For_Items.PlaySounds("ZoomIn");
        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        colliderList.DISABLE_COLLIDER();
        StartCoroutine(TransitionCamera());
        VariableDeterminator.isMiniGameActive = true;
        GLASS_IN_WINDOW_PARENT.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            OnMinigameClosed();
        }
    }
    public void closeThisGame()
    {
        if (gameObject.activeSelf && VariableDeterminator.isUserInNameplatesGame
            && VariableDeterminator.isUserInSafeZoomIn)
        {
            VariableDeterminator.isUserInNameplatesGame = false;
            VariableDeterminator.isUserInSafeZoomIn = false;
            VariableDeterminator.isMiniGameActive = false;
            StartCoroutine(TransitionCameraOut());
            public_actions.Instance.OnOff_SpaceTip_Zoom(false);
            GLASS_IN_WINDOW_PARENT.SetActive(false);
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
                wantedPosition,
                t);

            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = wantedPosition;

        public_actions.Instance.OnOff_SpaceTip_Zoom(true);
        VariableDeterminator.isUserInSafeZoomIn = true;
    }

    private IEnumerator TransitionCameraOut()
    {
        transitionStartTime = Time.time;

        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            mainCamera.orthographicSize = Mathf.Lerp(targetSize, 5, t);
            mainCamera.transform.position = Vector3.Lerp(
              wantedPosition,
              new Vector3(0, 10f, 0f),
              t);
            yield return null;
        }

        colliderList.ENABLE_COLLIDER();
        VariableDeterminator.isMiniGameActive = false;

        mainCamera.orthographicSize = 5;
        mainCamera.transform.position = new Vector3(0, 10f, 0f);
        VariableDeterminator.isUserInNameplatesGame = false;
        openSafeColliderObj.SetActive(true);
    }
    private IEnumerator TransitionCameraOut_END()
    {
        transitionStartTime = Time.time;

        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            mainCamera.orthographicSize = Mathf.Lerp(targetSize, 5, t);
            mainCamera.transform.position = Vector3.Lerp(
              wantedPosition,
              new Vector3(0, 10f, 0f),
              t);
            yield return null;
        }

        colliderList.ENABLE_COLLIDER();
        VariableDeterminator.isMiniGameActive = false;
        GLASS_IN_WINDOW_PARENT.SetActive(false);

        mainCamera.orthographicSize = 5;
        mainCamera.transform.position = new Vector3(0, 10f, 0f);
        VariableDeterminator.isUserInNameplatesGame = false;
        closedSafe.SetActive(false);
        openedSafe.SetActive(true);
        openSound.PlayOneShot(openSound.clip);
    }

    public void CompletedSafeTask()
    {
        VariableDeterminator.isUserInSafeZoomIn = false;
        _Audio_For_Items.PlaySounds("pictureClose");

        VariableDeterminator.isMiniGameActive = false;
        StartCoroutine(TransitionCameraOut_END());
        public_actions.Instance.OnOff_SpaceTip_Zoom(false);

        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
