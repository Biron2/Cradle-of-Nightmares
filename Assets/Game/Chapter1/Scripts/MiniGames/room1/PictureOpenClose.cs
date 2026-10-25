using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PictureOpenClose : MonoBehaviour
{
    public RectTransform pictureRect;
    [SerializeField] private GameObject picture, pictureBackground, determineObj;
    public bool openPictureBool = true;
    private bool alreadyOpened = false;
    public Vector2 startPosition;
    public Vector2 newPosition;
    private GameObject inputPctImage2, inputPctImage3;
    [SerializeField] private GameObject[] arrows_containers;
    [SerializeField] private AudioSource rotateSnd;
    private audio_for_items _Audio_For_Items;
    private PctOpened pctOpened;

    public bool isRotating = false;
    public float rotationDuration = 0.5f;
    public Transform transform1, transform2;

    private ColliderList colliderList;

    private float cooldown = 0.5f; // Время задержки в секундах
    private float lastClickTime = 0f; // Время последнего нажатия
    public float rotateNumber1 = 10f, rotateNumber2 = 20f;
    [SerializeField] private AudioSource winMiniGameSound;
    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        pctOpened = picture.GetComponent<PctOpened>();
        _Audio_For_Items = GameObject.Find("Other/GameManager/EtcSounds/__FOR_ITEMS__").GetComponent<audio_for_items>();
        inputPctImage2 = GameObject.Find("Room1/Image/RoomObjects/Picture/inputPctImage2");
        inputPctImage3 = GameObject.Find("Room1/Image/RoomObjects/Picture/inputPctImage3");
        public_actions.Instance.closeMiniGameAction += OnMinigameClosed;
    }
    private void OnDestroy()
    {
        // Важно отписаться, чтобы не было утечек
        public_actions.Instance.closeMiniGameAction -= OnMinigameClosed;
    }

    private void OnMinigameClosed()
    {
        if (!determineObj.activeSelf)
        {
            return;
        }
        _Audio_For_Items.PlaySounds("pictureClose");

        alreadyOpened = false;
        determineObj.SetActive(false);
        pictureBackground.SetActive(false);
        pictureRect.anchoredPosition = startPosition;
        picture.transform.localScale = new Vector3(1f, 1f, 1f);
        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        public_actions.Instance.OnOff_SpaceTip_Close(false);
        VariableDeterminator.isMiniGameActive = false;
        colliderList.ENABLE_COLLIDER();
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (Time.time - lastClickTime < cooldown)
            {
                // Время после последнего нажатия меньше задержки, поэтому ничего не делаем
                return;
            }
            lastClickTime = Time.time; // Обновляем время последнего нажатия
            OpenPicture();
        }
    }

    public void OpenPicture()
    {
        if (inputPctImage2.activeSelf && inputPctImage3.activeSelf && openPictureBool == true)
        {
            if (alreadyOpened == false)
            {
                _Audio_For_Items.PlaySounds("pictureOpen");

                alreadyOpened = true;
                determineObj.SetActive(true);
                picture.transform.localScale = new Vector3(2f, 2f, 2f);
                pictureBackground.SetActive(true);
                pictureRect.anchoredPosition = newPosition;
                foreach (GameObject obj in arrows_containers)
                {
                    if (obj != null)
                    {
                        obj.SetActive(false);
                    }
                }
                public_actions.Instance.OnOff_SpaceTip_Close(true);
                VariableDeterminator.isMiniGameActive = true;
                colliderList.DISABLE_COLLIDER();
            }
        }
    }

    private void Update()
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

        if (determineObj.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
            {
                OnMinigameClosed();
            }
        }
    }
    public void startRotateMethod()
    {
        rotateSnd.PlayOneShot(rotateSnd.clip);
        StartCoroutine(RotateOverTime(transform1, rotateNumber1));
        StartCoroutine(RotateOverTime(transform2, rotateNumber2));
    }

    public IEnumerator RotateOverTime(Transform target, float angleDelta)
    {
        isRotating = true;

        Quaternion initialRotation = target.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, target.eulerAngles.z + angleDelta);
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            target.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.rotation = targetRotation;
        isRotating = false;

        // Дополнительная проверка после завершения вращения
        if (Quaternion.Angle(transform1.rotation, Quaternion.identity) < 0.01f &&
            Quaternion.Angle(transform2.rotation, Quaternion.identity) < 0.01f)
        {
            CompleteRotationSequence();
        }
    }
    private void CompleteRotationSequence()
    {
        winMiniGameSound.PlayOneShot(winMiniGameSound.clip);
        VariableDeterminator.isMiniGameActive = false;
        colliderList.ENABLE_COLLIDER();
        openPictureBool = false;
        determineObj.SetActive(false);
        pictureBackground.SetActive(false);
        pictureRect.anchoredPosition = startPosition;
        picture.transform.localScale = new Vector3(1f, 1f, 1f);
        pctOpened.startOpenPictureMethod();

        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        public_actions.Instance.OnOff_SpaceTip_Close(false);
    }
    public void closeThisGame()
    {
        if (determineObj.activeSelf)
        {
            VariableDeterminator.isMiniGameActive = false;
            alreadyOpened = false;
            determineObj.SetActive(false);
            pictureBackground.SetActive(false);
            pictureRect.anchoredPosition = startPosition;
            picture.transform.localScale = new Vector3(1f, 1f, 1f);
            public_actions.Instance.OnOff_SpaceTip_Close(false);
        }
    }
}
