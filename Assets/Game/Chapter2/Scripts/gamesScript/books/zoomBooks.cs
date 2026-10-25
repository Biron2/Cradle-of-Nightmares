using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class zoomBooks : MonoBehaviour
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
    [SerializeField] private GameObject[] book_colliders;
    [SerializeField] private bookPress[] bookPresses;
    [SerializeField] private Image[] books;

    [SerializeField] private AudioSource clickSnd, loseSnd;
    [SerializeField] private GameObject spray_item;

    public string password = "";
    [SerializeField] private float moveDistance = 0.5f;
    [SerializeField] private float moveDuration = 1f;

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
        if (VariableDeterminator.isUserInNameplatesGame && VariableDeterminator.isUserInBooksZoomIn && gameObject.activeSelf)
        {
            VariableDeterminator.isUserInBooksZoomIn = false;
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

            foreach (GameObject obj in book_colliders)
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
            && VariableDeterminator.isUserInBooksZoomIn)
        {
            VariableDeterminator.isUserInNameplatesGame = false;
            VariableDeterminator.isUserInBooksZoomIn = false;
            VariableDeterminator.isMiniGameActive = false;
            StartCoroutine(TransitionCameraOut());
            public_actions.Instance.OnOff_SpaceTip_Zoom(false);
            GLASS_IN_WINDOW_PARENT.SetActive(false);

            foreach (GameObject obj in book_colliders)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
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
                new Vector3(-22.593f, 19.138f, initialZ),
                t);

            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = new Vector3(-22.593f, 19.138f, initialZ);

        public_actions.Instance.OnOff_SpaceTip_Zoom(true);
        VariableDeterminator.isUserInBooksZoomIn = true;

        foreach (GameObject obj in book_colliders)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    private IEnumerator TransitionCameraOut()
    {
        ResetProgress();
        transitionStartTime = Time.time;

        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            mainCamera.orthographicSize = Mathf.Lerp(targetSize, 5, t);
            mainCamera.transform.position = Vector3.Lerp(
              new Vector3(-22.593f, 19.138f, 0f),
              new Vector3(-17.777f, 20f, 0f),
              t);
            yield return null;
        }

        colliderList.ENABLE_COLLIDER();
        VariableDeterminator.isMiniGameActive = false;

        mainCamera.orthographicSize = 5;
        mainCamera.transform.position = new Vector3(-17.777f, 20f, 0f);
        VariableDeterminator.isUserInNameplatesGame = false;
        openSafeColliderObj.SetActive(true);
    }
    public void Enter_a_Number(string num)
    {
        password += num;
        clickSnd.PlayOneShot(clickSnd.clip);
        if(password.Length >= 4)
        {
            if (password.Contains("4132"))
            {
                //Победа
                winMiniGameSound.PlayOneShot(winMiniGameSound.clip);
                GLASS_IN_WINDOW_PARENT.SetActive(false);
                spray_item.SetActive(true);
                VariableDeterminator.allowToZoomBooks = false;
                StartCoroutine(smoothBooksMoving());

                VariableDeterminator.isUserInBooksZoomIn = false;
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
                foreach (GameObject obj in book_colliders)
                {
                    if (obj != null)
                    {
                        obj.SetActive(false);
                    }
                }
            }
            else
            {
                loseSnd.PlayOneShot(loseSnd.clip);
                ResetProgress();
            }
        }
    }
    private IEnumerator smoothBooksMoving()
    {
        yield return new WaitForSeconds(1);
        openSafeColliderObj.SetActive(false);

        Vector3 startPos2 = books[2].transform.position;
        Vector3 endPos2 = startPos2 + Vector3.right * moveDistance;

        Vector3 startPos3 = books[3].transform.position;
        Vector3 endPos3 = startPos3 + Vector3.right * moveDistance;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            books[2].transform.position = Vector3.Lerp(startPos2, endPos2, elapsedTime / moveDuration);
            books[3].transform.position = Vector3.Lerp(startPos3, endPos3, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убедимся, что они точно в конечной позиции
        books[2].transform.position = endPos2;
        books[3].transform.position = endPos3;
    }
    private void ResetProgress()
    {
        foreach(bookPress _BookPress in bookPresses)
        {
            _BookPress.clicked = false;
        }
        password = "";
        Color newColor;
        foreach (Image img in books)
        {
            if (UnityEngine.ColorUtility.TryParseHtmlString("#DADADA", out newColor))
            {
                img.color = newColor;
            }
            else
            {
                Debug.LogError("Не удалось преобразовать цвет #DADADA");
            }
        }
    }
}
