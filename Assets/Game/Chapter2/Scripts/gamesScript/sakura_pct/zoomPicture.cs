using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class zoomPicture : MonoBehaviour
{
    private ColliderList colliderList;
    [SerializeField] private GameObject[] arrows_containers;
    private Camera mainCamera;

    private audio_for_items _Audio_For_Items;
    private float cooldown = 0.5f; // ¬рем¤ задержки в секундах
    private float lastClickTime = 0f; // ¬рем¤ последнего нажати¤

    private float transitionStartTime;
    public float transitionDuration = 1.0f;
    public float targetSize = 0.7f;
    [SerializeField] private GameObject openSafeColliderObj;
    [SerializeField] private GameObject openSafeColliderObjParent;
    [SerializeField] private GameObject pictureAdditionalCollider;

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
        if (VariableDeterminator.isUserInNameplatesGame && VariableDeterminator.isUserInPictureZoomIn && gameObject.activeSelf)
        {
            ZoomOutPicture();
        }
    }
    public void PhoneWasClicked()
    {
        if (VariableDeterminator.isUserInNameplatesGame)
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
    public void stopZoomEffect()
    {
        openSafeColliderObjParent.SetActive(false);
    }
    public void ZoomOutPicture()
    {
        VariableDeterminator.isUserInPictureZoomIn = false;
        _Audio_For_Items.PlaySounds("zoomOut");

        VariableDeterminator.isMiniGameActive = false;
        StartCoroutine(TransitionCameraOut());
        public_actions.Instance.OnOff_SpaceTip_Zoom(false);
        colliderList.ENABLE_COLLIDER();

        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        pictureAdditionalCollider.SetActive(false);
        GLASS_IN_WINDOW_PARENT.SetActive(false);
    }
    public void closeThisGame()
    {
        if (gameObject.activeSelf && VariableDeterminator.isUserInNameplatesGame
            && VariableDeterminator.isUserInPictureZoomIn)
        {
            VariableDeterminator.isUserInNameplatesGame = false;
            VariableDeterminator.isUserInPictureZoomIn = false;
            VariableDeterminator.isMiniGameActive = false;
            StartCoroutine(TransitionCameraOut());
            public_actions.Instance.OnOff_SpaceTip_Zoom(false);
            GLASS_IN_WINDOW_PARENT.SetActive(false);
            pictureAdditionalCollider.SetActive(false);
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
                new Vector3(5.9f, 0f, initialZ),
                t);

            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = new Vector3(5.9f, 0f, initialZ);

        public_actions.Instance.OnOff_SpaceTip_Zoom(true);
        VariableDeterminator.isUserInPictureZoomIn = true;

        pictureAdditionalCollider.SetActive(true);
    }

    private IEnumerator TransitionCameraOut()
    {
        transitionStartTime = Time.time;

        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            mainCamera.orthographicSize = Mathf.Lerp(targetSize, 5, t);
            mainCamera.transform.position = Vector3.Lerp(
              new Vector3(5.9f, 0f, 0f),
              new Vector3(0f, 0f, 0f),
              t);
            yield return null;
        }

        colliderList.ENABLE_COLLIDER();
        VariableDeterminator.isMiniGameActive = false;

        mainCamera.orthographicSize = 5;
        mainCamera.transform.position = new Vector3(0f, 0f, 0f);
        VariableDeterminator.isUserInNameplatesGame = false;
        openSafeColliderObj.SetActive(true);
    }
}