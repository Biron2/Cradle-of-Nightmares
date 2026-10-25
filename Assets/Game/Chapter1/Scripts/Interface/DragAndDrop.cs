using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private bool alreadyOpened1 = false;
    private bool alreadyOpened2 = false;
    private bool alreadyOpened3 = false;
    private bool alreadyOpened4 = false;
    private bool alreadyOpened5 = false;

    private bool playClown = true;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private InventorySlot inventorySlot;
    public static bool openGrowBool = false;
    public PictureOpenClose mPictureOpenClose;
    public InventoryManager inventoryManager;
    public lightOnOffT mLightOnOffT;
    public Vector2 newPosition;
    public Transform prioritySlot;
    private Transform originalParent;
    private Text kanistraPercent;
    [SerializeField] private GameObject[] arrows_containers;
    private ColliderList colliderList;
    [SerializeField] private TimeManager _TimeManager;
    public Piano mPiano;
    private bool endFillKanistra = false;
    private float countdownDuration = 5f;
    private bool countdownFinished = false;
    private bool isCountdownPaused = false;
    private int currentPercent = 0;
    private float elapsedTime = 0f;
    private ParticleSystem.MainModule mainModule;
    private bool cancelFill = true;
    public public_actions mPublic_Actions;
    [SerializeField] private VariableDeterminator variableDeterminator;
    [SerializeField] private TVmanager _TVmanager;
    public static float clickCount = 0;
    public static float clickCountDetails = 0;
    public static float clickTushka = 0;
    private audio_for_items _Audio_For_Items;
    [SerializeField] private Tooltip[] tooltips;
    [Header("ObjectsToUse")]
    [SerializeField] private GameObject[] determineObjects;
    [SerializeField] private GameObject[] determineObjects2;
    [SerializeField] private Image[] determineImages;
    [SerializeField] private AudioSource[] determineAudio;
    [SerializeField] private AudioClip[] determineClips;
    [SerializeField] private Sprite[] determineSprites;
    [SerializeField] private ParticleSystem[] determineParticles;
    [SerializeField] private Animator[] determineAnimators;
    private void Awake()
    {
        _Audio_For_Items = GameObject.Find("Other/GameManager/EtcSounds/__FOR_ITEMS__").GetComponent<audio_for_items>();
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        inventorySlot = GetComponent<InventorySlot>();
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        kanistraPercent = transform.Find("Percent").GetComponent<Text>();
        mainModule = determineParticles[0].main;
        rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);

        // Сохраняем текущего родителя объекта
        originalParent = transform.parent;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (inventorySlot.itemImage.sprite != null)
        {
            switch (inventorySlot.itemImage.sprite.name)
            {
                case "Shkatulochka":
                    if (colliderList.allowOpenShatulochka == true)
                    {
                        _Audio_For_Items.PlaySounds("Shkatulochka");
                        variableDeterminator.ReceiveObjectPath(GetGameObjectPath(gameObject));
                        determineObjects[18].SetActive(true);
                        foreach (GameObject obj in arrows_containers)
                        {
                            if (obj != null)
                            {
                                obj.SetActive(false);
                            }
                        }
                        public_actions.Instance.OnOff_SpaceTip_Close(true);
                        colliderList.DISABLE_COLLIDER();
                        VariableDeterminator.isMiniGameActive = true;
                    }
                    break;
                case "paper1":
                    _Audio_For_Items.PlaySounds("note3");
                    determineObjects[63].SetActive(true);
                    foreach (GameObject obj in arrows_containers)
                    {
                        if (obj != null)
                        {
                            obj.SetActive(false);
                        }
                    }
                    public_actions.Instance.OnOff_SpaceTip_Close(true);
                    colliderList.DISABLE_COLLIDER();
                    VariableDeterminator.isMiniGameActive = true;
                    if (playClown == true)
                    {
                        playClown = false;
                        determineAudio[17].Play();
                    }
                    break;
                case "banka_serdtse":
                    inventorySlot.itemImage.sprite = determineSprites[0];
                    determineAudio[11].Play();
                    break;
                case "banka_pechen":
                    inventorySlot.itemImage.sprite = determineSprites[1];
                    determineAudio[11].Play();
                    break;
            }
        }
    }
    public static string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        foreach(Tooltip tps in tooltips)
        {
            tps.StopTipShowing();
        }
        VariableDeterminator.isDraging = true;
        if (inventorySlot.itemImage.sprite != null)
        {
            // Делаем объект не интерактивным для других элементов UI
            canvasGroup.blocksRaycasts = false;

            // Перемещаем объект на верхний слой (чтобы был поверх других элементов)
            transform.SetAsLastSibling();

            if (originalParent == null)
            {
                Debug.LogError($"Original parent is null for {gameObject.name}");
                return;
            }

            // Меняем родителя на контейнер для приоритетного отображения
            transform.SetParent(prioritySlot, true); // true сохраняет глобальные координаты

            // Логирование для отладки
            Debug.Log($"{gameObject.name} parent changed to {prioritySlot.name}. Original parent was {originalParent.name}");

            if (inventorySlot.itemImage.sprite.name.Equals("kanistra"))
            {
                rectTransform.localScale = new Vector3(2f, 2f, 2f);
            }
            if (inventorySlot.itemImage.sprite.name.Equals("statue_hand"))
            {
                rectTransform.localScale = new Vector3(2f, 2f, 2f);
            }
            if (inventorySlot.itemImage.sprite.name.Equals("statue_head"))
            {
                rectTransform.localScale = new Vector3(2f, 2f, 2f);
            }
            if (inventorySlot.itemImage.sprite.name.Equals("saw_item"))
            {
                rectTransform.localScale = new Vector3(2f, 2f, 2f);
            }
            if (inventorySlot.itemImage.sprite.name.Equals("picture2"))
            {
                rectTransform.localScale = new Vector3(3f, 3f, 3f);
            }
            if (inventorySlot.itemImage.sprite.name.Equals("picture3"))
            {
                rectTransform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
            }
        }
        else
        {
            Debug.LogWarning("Attempted to drag an empty slot!");
        }
    }

    private void OnEnable()
    {
        transform.SetParent(originalParent); // Устанавливаем родителя
        rectTransform.localPosition = Vector3.zero; // Локальные координаты в (0, 0, 0)
        rectTransform.localScale = Vector3.one; // Единичный масштаб
        canvasGroup.blocksRaycasts = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (inventorySlot.itemImage.sprite != null)
        {
            if (originalParent != null)
            {
                // Устанавливаем родителя
                transform.SetParent(originalParent);

                // Проверяем, что родитель установлен корректно
                if (transform.parent != originalParent)
                {
                    Debug.LogError($"Failed to set parent! Current parent: {transform.parent.name}");
                    return;
                }
                
                // Сбрасываем позицию относительно нового родителя
                rectTransform.localPosition = Vector3.zero; // Локальные координаты в (0, 0, 0)
                rectTransform.localScale = Vector3.one; // Единичный масштаб
            }
            else
            {
                Debug.LogWarning("Original parent is null!");
            }

            // Разрешаем обработку событий после завершения перетаскивания
            canvasGroup.blocksRaycasts = true;
        }

        Collider2D thisCollider = GetComponent<Collider2D>();

        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("siniy_klyuch"))
            {
                Collider2D closetCollider = determineObjects[0].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[0].SetActive(false);
                    determineObjects2[5].SetActive(false);
                    determineObjects[1].SetActive(true);
                    determineAudio[5].Play();
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("gold_key"))
            {
                Collider2D closetCollider = determineObjects2[4].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects2[4].SetActive(false);
                    determineObjects[32].SetActive(false);
                    determineObjects[33].SetActive(true);
                    determineAudio[5].Play();
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("picture2"))
            {
                Collider2D closetCollider = determineObjects[4].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[2].SetActive(true);
                    determineAudio[21].Play();
                    inventorySlot.ClearItem();
                    if (determineObjects[3].activeSelf)
                    {
                        colliderList.DISABLE_COLLIDER();
                        mPictureOpenClose.OpenPicture();
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("picture3"))
            {
                Collider2D closetCollider = determineObjects[4].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[3].SetActive(true);
                    inventorySlot.ClearItem();
                    determineAudio[21].Play();
                    if (determineObjects[2].activeSelf)
                    {
                        colliderList.DISABLE_COLLIDER();
                        mPictureOpenClose.OpenPicture();
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("lozhka"))
            {
                Collider2D closetCollider1 = determineObjects[13].GetComponent<Collider2D>();
                Collider2D closetCollider2 = determineObjects[65].GetComponent<Collider2D>();
                Collider2D closetCollider3 = determineObjects[59].GetComponent<Collider2D>();
                Collider2D closetCollider4 = determineObjects[66].GetComponent<Collider2D>();
                Collider2D closetCollider5 = determineObjects2[2].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider1 != null && thisCollider.IsTouching(closetCollider1))
                {
                    colliderList.DISABLE_COLLIDER();
                    determineObjects[13].SetActive(false);
                    determineObjects[17].SetActive(true);
                    determineAudio[24].PlayOneShot(determineAudio[24].clip);
                }
                if (thisCollider != null && closetCollider2 != null && thisCollider.IsTouching(closetCollider2))
                {
                    colliderList.DISABLE_COLLIDER();
                    determineObjects[65].SetActive(false);
                    determineObjects2[0].SetActive(true);
                    determineAudio[24].PlayOneShot(determineAudio[24].clip);
                }
                if (thisCollider != null && closetCollider3 != null && thisCollider.IsTouching(closetCollider3))
                {
                    colliderList.DISABLE_COLLIDER();
                    determineObjects[59].SetActive(false);
                    determineObjects[60].SetActive(true);
                    determineAudio[24].PlayOneShot(determineAudio[24].clip);
                }
                if (thisCollider != null && closetCollider4 != null && thisCollider.IsTouching(closetCollider4))
                {
                    colliderList.DISABLE_COLLIDER();
                    determineObjects[66].SetActive(false);
                    determineObjects2[1].SetActive(true);
                    determineAudio[24].PlayOneShot(determineAudio[24].clip);
                }
                if (thisCollider != null && closetCollider5 != null && thisCollider.IsTouching(closetCollider5))
                {
                    colliderList.DISABLE_COLLIDER();
                    determineObjects2[2].SetActive(false);
                    determineObjects2[3].SetActive(true);
                    determineAudio[24].PlayOneShot(determineAudio[24].clip);
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("detali"))
            {
                Collider2D closetCollider = determineObjects[6].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineAudio[19].Play();

                    clickCountDetails = clickCountDetails + 0.5f;
                    if (determineObjects2[10].GetComponent<Image>().color == Color.white)
                    {
                        determineObjects2[11].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        determineObjects2[10].GetComponent<Image>().color = Color.white;
                    }
                    if (clickCountDetails == 1)
                    {
                        determineAudio[16].Stop();
                        mPublic_Actions.stopElectroParticle();
                        variableDeterminator.stopPlayLight = 1;
                    }

                    if (clickCount + clickCountDetails == 2)
                    {
                        foreach (GameObject obj in arrows_containers)
                        {
                            if (obj != null)
                            {
                                obj.SetActive(false);
                            }
                        }
                        Invoke(nameof(transformatorEnable), 1);
                    }
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("kanistra"))
            {
                Collider2D closetCollider = determineObjects[7].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    if (endFillKanistra == false)
                    {
                        determineAudio[8].Play();
                    }
                    else
                    {
                        determineAudio[20].Play();
                        determineObjects[7].SetActive(false);
                        determineObjects[19].SetActive(true);
                        MoveRoom.motorStart = true;
                        determineAudio[1].Play();
                        Invoke(nameof(continueSndMotor), determineClips[0].length);
                        clickCount = clickCount + 1f;
                        if (clickCount + clickCountDetails == 2)
                        {
                            foreach (GameObject obj in arrows_containers)
                            {
                                if (obj != null)
                                {
                                    obj.SetActive(false);
                                }
                            }
                            Invoke(nameof(transformatorEnable), 1);
                        }
                        rectTransform.localScale = new Vector3(1f, 1f, 1f);
                        inventorySlot.itemImage.color = Color.white;
                        inventorySlot.ClearItem();
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("spec_key1"))
            {
                Collider2D closetCollider = determineObjects[20].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    inventorySlot.ClearItem();
                    determineImages[0].color = Color.white;
                    determineAudio[22].Play();
                    if (determineImages[1].color == Color.white)
                    {
                        determineObjects[23].SetActive(true);
                        determineObjects[20].SetActive(false);
                        foreach (GameObject obj in arrows_containers)
                        {
                            if (obj != null)
                            {
                                obj.SetActive(true);
                            }
                        }
                        public_actions.Instance.OnOff_SpaceTip_Close(false);
                        colliderList.ENABLE_COLLIDER();
                        VariableDeterminator.isMiniGameActive = false;
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("spec_key2"))
            {
                Collider2D closetCollider = determineObjects[20].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    inventorySlot.ClearItem();
                    determineImages[1].color = Color.white;
                    determineAudio[22].Play();
                    if (determineImages[0].color == Color.white)
                    {
                        determineObjects[23].SetActive(true);
                        determineObjects[20].SetActive(false);
                        foreach (GameObject obj in arrows_containers)
                        {
                            if (obj != null)
                            {
                                obj.SetActive(true);
                            }
                        }
                        public_actions.Instance.OnOff_SpaceTip_Close(false);
                        colliderList.ENABLE_COLLIDER();
                        VariableDeterminator.isMiniGameActive = false;
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("full_spec_key"))
            {
                Collider2D closetCollider = determineObjects[21].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    inventorySlot.ClearItem();
                    determineObjects[21].SetActive(false);
                    determineObjects[22].SetActive(true);
                    determineAudio[2].Play();
                    if (++clickTushka == 2)
                    {
                        determineObjects[57].SetActive(true);
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("statue_head"))
            {
                Collider2D closetCollider = determineObjects[24].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[25].SetActive(true);
                    determineAudio[7].Play();
                    if (determineObjects[26].activeSelf)
                    {
                        determineObjects[27].SetActive(true);
                        determineObjects[24].SetActive(false);
                        determineObjects[25].SetActive(false);
                        determineObjects[26].SetActive(false);
                        determineObjects[28].SetActive(true);
                    }
                    else
                    {
                        determineAudio[6].Play();
                    }
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("statue_hand"))
            {
                Collider2D closetCollider = determineObjects[24].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[26].SetActive(true);
                    determineAudio[7].Play();
                    if (determineObjects[25].activeSelf)
                    {
                        determineObjects[27].SetActive(true);
                        determineObjects[24].SetActive(false);
                        determineObjects[25].SetActive(false);
                        determineObjects[26].SetActive(false);
                        determineObjects[28].SetActive(true);
                    }
                    else
                    {
                        determineAudio[6].Play();
                    }
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("portal_key"))
            {
                Collider2D closetCollider = determineObjects[29].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    if (determineObjects2[6].activeSelf)
                    {
                        determineAudio[8].Play();
                    }
                    else
                    {
                        _TimeManager.stopStartClockCicle();
                        inventorySlot.ClearItem();
                        determineObjects[30].SetActive(true);
                        determineAudio[3].Play();
                        determineAudio[4].Stop();
                        determineAudio[2].Play();
                        determineObjects[64].SetActive(false);
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("saw_item"))
            {
                Collider2D closetCollider1 = determineObjects[35].GetComponent<Collider2D>();
                Collider2D closetCollider2 = determineObjects[36].GetComponent<Collider2D>();
                Collider2D closetCollider3 = determineObjects[37].GetComponent<Collider2D>();
                Collider2D closetCollider4 = determineObjects[38].GetComponent<Collider2D>();
                Collider2D closetCollider5 = determineObjects[56].GetComponent<Collider2D>();

                if (alreadyOpened1 == false && closetCollider1 != null && thisCollider.IsTouching(closetCollider1))
                {
                    alreadyOpened1 = true;
                    determineObjects[39].SetActive(false);
                    determineObjects[40].SetActive(true);
                    determineAudio[10].Play();
                }
                if (alreadyOpened2 == false && closetCollider2 != null && thisCollider.IsTouching(closetCollider2))
                {
                    alreadyOpened2 = true;
                    determineObjects[41].SetActive(false);
                    determineObjects[42].SetActive(true);
                    determineAudio[10].Play();
                }
                if (alreadyOpened3 == false && closetCollider3 != null && thisCollider.IsTouching(closetCollider3))
                {
                    alreadyOpened3 = true;
                    determineObjects[43].SetActive(false);
                    determineObjects[44].SetActive(true);
                    determineAudio[10].Play();
                }
                if (alreadyOpened4 == false && closetCollider4 != null && thisCollider.IsTouching(closetCollider4))
                {
                    alreadyOpened4 = true;
                    determineObjects[45].SetActive(false);
                    determineObjects[46].SetActive(true);
                    determineAudio[10].Play();
                }
                if (alreadyOpened5 == false && closetCollider5 != null && thisCollider.IsTouching(closetCollider5))
                {
                    alreadyOpened5 = true;
                    determineAnimators[1].Play("tusha_moving");

                    determineAudio[10].Play();
                    if (++clickTushka == 2)
                    {
                        determineObjects[57].SetActive(true);
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("heart"))
            {
                Collider2D closetCollider = determineObjects[49].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[52].SetActive(true);
                    determineAudio[11].Play();
                    if (determineObjects[50].activeSelf && determineObjects[51].activeSelf && determineObjects[53].activeSelf)
                    {
                        determineAudio[12].Play();
                        scalesGame.allowScalePressing = true;
                        determineObjects[49].SetActive(false);
                    }
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("liver"))
            {
                Collider2D closetCollider = determineObjects[49].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[53].SetActive(true);
                    determineAudio[11].Play();
                    if (determineObjects[50].activeSelf && determineObjects[51].activeSelf && determineObjects[52].activeSelf)
                    {
                        determineAudio[12].Play();
                        scalesGame.allowScalePressing = true;
                        determineObjects[49].SetActive(false);
                    }
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("hand_item"))
            {
                Collider2D closetCollider = determineObjects[49].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[50].SetActive(true);
                    determineAudio[11].Play();
                    if (determineObjects[52].activeSelf && determineObjects[51].activeSelf && determineObjects[53].activeSelf)
                    {
                        determineAudio[12].Play();
                        scalesGame.allowScalePressing = true;
                        determineObjects[49].SetActive(false);
                    }
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("m3Without"))
            {
                Collider2D closetCollider = determineObjects[49].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[51].SetActive(true);
                    determineAudio[11].Play();
                    if (determineObjects[50].activeSelf && determineObjects[52].activeSelf && determineObjects[53].activeSelf)
                    {
                        determineAudio[12].Play();
                        scalesGame.allowScalePressing = true;
                        determineObjects[49].SetActive(false);
                    }
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("pianoThingsItem"))
            {
                Collider2D closetCollider = determineObjects[61].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects2[7].SetActive(false);
                    inventorySlot.ClearItem();
                    determineObjects[62].SetActive(true);
                    mPiano.allowOpenPianoGame = true;
                    determineAudio[18].Play();
                    foreach (GameObject obj in arrows_containers)
                    {
                        if (obj != null)
                        {
                            obj.SetActive(false);
                        }
                    }
                    public_actions.Instance.OnOff_SpaceTip_Close(true);
                    colliderList.DISABLE_COLLIDER();
                    StartCoroutine(activeTip8InSec());
                }
            }

        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("casset"))
            {
                Collider2D closetCollider = determineObjects2[9].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
#if PLAYMARKET
                    FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
                    _TVmanager.Start_Scene_Film_Casset();
                    inventorySlot.ClearItem();
                    determineAudio[23].Play();
                }
            }

        // Убедитесь, что объект возвращается в начальную позицию
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one; // Финальное подтверждение масштаба

        VariableDeterminator.isDraging = false;
    }

    private IEnumerator activeTip8InSec()
    {
        yield return new WaitForSeconds(1.5f);
        determineObjects2[8].SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inventorySlot.itemImage.sprite != null)
        {
            float scaleFactor = canvas.scaleFactor > 0 ? canvas.scaleFactor : 1f;
            rectTransform.anchoredPosition += eventData.delta / scaleFactor;
        }

    }
    private void Update()
    {
        Collider2D thisCollider = GetComponent<Collider2D>();
        Collider2D closetCollider = determineObjects[47].GetComponent<Collider2D>();

        if (inventorySlot.itemImage.sprite != null && inventorySlot.itemImage.sprite.name.Equals("kanistra"))
        {
            // Проверяем, соприкасаются ли коллайдеры

            if (determineObjects[47].activeSelf && thisCollider.IsTouching(closetCollider))
            {
                kanistraPercent.gameObject.SetActive(true);

                if (!determineAudio[9].isPlaying)
                {
                    if (!endFillKanistra)
                    {
                        determineAudio[9].volume = 1;
                        determineAudio[9].PlayOneShot(determineAudio[9].clip);
                        mainModule.startSize = 1f;
                    }
                }

                if (!endFillKanistra)
                {
                    if (cancelFill)
                    {
                        cancelFill = false;
                        StartCoroutine(Countdown());
                    }
                    ResumeCountdown();
                }
                UpdatePercent();
            }
            else
            {
                kanistraPercent.gameObject.SetActive(false);

                if (!endFillKanistra)
                {
                    PauseCountdown();
                }
                if (determineAudio[9].isPlaying)
                {
                    determineAudio[9].Stop();
                    mainModule.startSize = 0f;
                }
            }
        }
    }

    private void transformatorEnable()
    {
        determineAnimators[0].Play("transformator_level");
        mLightOnOffT._Off_blinkingMethod();
        determineObjects[10].SetActive(true);
        determineObjects[11].SetActive(true);
        determineObjects[12].SetActive(true);
        determineAudio[0].Play();
        Invoke(nameof(electroReset), 1.5f);
    }
    private void electroReset()
    {
        determineObjects[58].SetActive(true);
    }
    private void continueSndMotor()
    {
        if (variableDeterminator.current_roomNumber == 4)
        {
            determineAudio[1].loop = true;
            determineAudio[1].clip = determineClips[1];
            determineAudio[1].Play();
        }
    }
    private void UpdatePercent()
    {
        if (!countdownFinished)
        {
            currentPercent = Mathf.RoundToInt((elapsedTime / countdownDuration) * 100);
            currentPercent = Mathf.Clamp(currentPercent, 0, 100);

            if (currentPercent >= 100)
            {
                currentPercent = 100;
                countdownFinished = true;
                inventorySlot.itemImage.color = Color.yellow;
                endFillKanistra = true;
                determineAudio[9].Stop();
                mainModule.startSize = 0f;
                VariableDeterminator.Isfullkanistra = true;
            }
        }
        kanistraPercent.text = currentPercent.ToString() + "%";
    }

    private IEnumerator Countdown()
    {
        while (elapsedTime < countdownDuration)
        {
            if (!isCountdownPaused)
            {
                elapsedTime += Time.deltaTime;
                UpdatePercent();
            }
            yield return null;
        }
        UpdatePercent();
    }
    public bool IsCountdownFinished()
    {
        return elapsedTime >= countdownDuration;
    }
    public void PauseCountdown()
    {
        isCountdownPaused = true;
    }
    public void ResumeCountdown()
    {
        isCountdownPaused = false;
    }
}