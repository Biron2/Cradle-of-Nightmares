using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class zoomOldPhone : MonoBehaviour
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
    [SerializeField] private GameObject[] phone_keys;
    public string old_phone_number = "";
    [SerializeField] private GameObject[] objToRedColor;
    public float red_Color_duration = 1.5f; // за сколько секунд они поменяют цвет

    [SerializeField] private GameObject GLASS_IN_WINDOW_PARENT;
    [SerializeField] private AudioSource winMiniGameSound;
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
        if (VariableDeterminator.isUserInNameplatesGame && VariableDeterminator.isUserInOldPhoneZoomIn && gameObject.activeSelf)
        {
            VariableDeterminator.isUserInOldPhoneZoomIn = false;
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

            foreach (GameObject obj in phone_keys)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            GLASS_IN_WINDOW_PARENT.SetActive(false);
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

    public void finishThePhonePuzzle()
    {
        winMiniGameSound.PlayOneShot(winMiniGameSound.clip);
        StartCoroutine(SmoothRedColor());
    }

    private IEnumerator SmoothRedColor()
    {
        Color targetColor = new Color(1f, 76f / 255f, 76f / 255f); // FF4C4C

        for (int i = 0; i < objToRedColor.Length; i++)
        {
            if (objToRedColor[i] != null)
            {
                Image image = objToRedColor[i].GetComponent<Image>();
                Color startColor = image.color;
                float time = 0f;

                while (time < red_Color_duration)
                {
                    time += Time.deltaTime;
                    float t = time / red_Color_duration;
                    image.color = Color.Lerp(startColor, targetColor, t);
                    yield return null;
                }

                // Обязательно устанавливаем точный финальный цвет
                image.color = targetColor;
            }
        }
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
            && VariableDeterminator.isUserInOldPhoneZoomIn)
        {
            VariableDeterminator.isUserInNameplatesGame = false;
            VariableDeterminator.isUserInOldPhoneZoomIn = false;
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
                new Vector3(-2.633f, 20.42f, initialZ),
                t);

            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = new Vector3(-2.633f, 20.42f, initialZ);

        public_actions.Instance.OnOff_SpaceTip_Zoom(true);
        VariableDeterminator.isUserInOldPhoneZoomIn = true;

        foreach (GameObject obj in phone_keys)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    private IEnumerator TransitionCameraOut()
    {
        transitionStartTime = Time.time;

        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            mainCamera.orthographicSize = Mathf.Lerp(targetSize, 5, t);
            mainCamera.transform.position = Vector3.Lerp(
              new Vector3(-2.633f, 20.42f, 0f),
              new Vector3(0f, 20f, 0f),
              t);
            yield return null;
        }

        colliderList.ENABLE_COLLIDER();
        VariableDeterminator.isMiniGameActive = false;

        mainCamera.orthographicSize = 5;
        mainCamera.transform.position = new Vector3(0f, 20f, 0f);
        VariableDeterminator.isUserInNameplatesGame = false;
        openSafeColliderObj.SetActive(true);
    }
}
