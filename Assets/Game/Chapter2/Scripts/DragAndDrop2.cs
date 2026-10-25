using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class DragAndDrop2 : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private InventorySlot inventorySlot;
    public static bool openGrowBool = false;
    public InventoryManager inventoryManager;
    public lightOnOffT mLightOnOffT;
    public Transform prioritySlot;
    private Transform originalParent;
    [SerializeField] private GameObject[] arrows_containers;
    private ColliderList colliderList;
    [SerializeField] private TimeManager _TimeManager;
    [SerializeField] private VariableDeterminator variableDeterminator;
    [SerializeField] private carShaking _CarShaking;
    [SerializeField] private FoldingScreen _foldingScreen;
    [SerializeField] private TVmanager2 _TVmanager;
    [SerializeField] private zoomPicture _ZoomPicture;
    [SerializeField] private public_actions _Public_Actions;
    public static float clickCount = 0;
    public static float clickCountDetails = 0;
    public static float clickTushka = 0;
    private audio_for_items _Audio_For_Items;
    [SerializeField] private Tooltip[] tooltips;
    private int tarakanKilled = 0;
    private int hammerCrushed = 0;
    private int knifePctDroped = 0;
    public static int bookPut = 0;
    public static int robotPartsPuten = 0;
    public static int bubblesPut = 0;
    public static int hintPuten = 0;
    public static int hintPrinterPuten = 0;
    public static bool tarakanKilled1 = false, tarakanKilled2 = false, tarakanKilled3 = false, tarakanKilled4 = false;
    public static bool alreadyPuten1 = false, alreadyPuten2 = false;
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
                case "chest_item":
                    if (colliderList.allowOpenShatulochka == true)
                    {
                        _Audio_For_Items.PlaySounds("Shkatulochka");
                        variableDeterminator.ReceiveObjectPath(GetGameObjectPath(gameObject));
                        determineObjects[46].SetActive(true);
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
                case "safe_code_item":
                    _Audio_For_Items.PlaySounds("safeCodePaperOpen");
                    determineObjects2[2].SetActive(true);
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
        if(GetComponent<Image>().sprite.name == "nameplate2_item" || GetComponent<Image>().sprite.name == "nameplate5_item" || GetComponent<Image>().sprite.name == "nameplate6_item")
        {
            determineObjects[50].SetActive(true);
        }
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


            if (inventorySlot.itemImage.sprite.name.Equals("robot_head"))
            {
                rectTransform.localScale = new Vector3(2f, 2f, 2f);
            }
            if (inventorySlot.itemImage.sprite.name.Equals("robot_hand"))
            {
                rectTransform.localScale = new Vector3(2f, 2f, 2f);
            }
            if (inventorySlot.itemImage.sprite.name.Equals("nameplate2_item"))
            {
                rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            if (inventorySlot.itemImage.sprite.name.Equals("nameplate5_item"))
            {
                rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            if (inventorySlot.itemImage.sprite.name.Equals("nameplate6_item"))
            {
                rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            if (inventorySlot.itemImage.sprite.name.Equals("hammer_item"))
            {
                rectTransform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
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
            if (inventorySlot.itemImage.sprite.name.Equals("Spray"))
            {
                if (determineObjects[15] != null)
                {
                    Collider2D closetColliderBug1 = determineObjects[15].GetComponent<Collider2D>();
                    if (thisCollider != null && closetColliderBug1 != null && thisCollider.IsTouching(closetColliderBug1))
                    {
                        if (!tarakanKilled1)
                        {
                            determineObjects[15].GetComponent<BugMove>().StopRunning();
                            tarakanKilled1 = true;
                            tarakanKilled++;
                        }
                        determineAudio[0].Play();
                        StartCoroutine(FadeOutCoroutine(determineObjects[15], 0.6f));
                    }
                }

                if (determineObjects[16] != null)
                {
                    Collider2D closetColliderBug2 = determineObjects[16].GetComponent<Collider2D>();
                    if (thisCollider != null && closetColliderBug2 != null && thisCollider.IsTouching(closetColliderBug2))
                    {
                        if (!tarakanKilled2)
                        {
                            determineObjects[16].GetComponent<BugMove>().StopRunning();
                            tarakanKilled2 = true;
                            tarakanKilled++;
                        }
                        determineAudio[0].Play();
                        StartCoroutine(FadeOutCoroutine(determineObjects[16], 0.6f));
                    }
                }

                if (determineObjects[17] != null)
                {
                    Collider2D closetColliderBug3 = determineObjects[17].GetComponent<Collider2D>();
                    if (thisCollider != null && closetColliderBug3 != null && thisCollider.IsTouching(closetColliderBug3))
                    {
                        if (!tarakanKilled3)
                        {
                            determineObjects[17].GetComponent<BugMove>().StopRunning();
                            tarakanKilled3 = true;
                            tarakanKilled++;
                        }
                        determineAudio[0].Play();
                        StartCoroutine(FadeOutCoroutine(determineObjects[17], 0.6f));
                    }
                }

                if (determineObjects[18] != null)
                {
                    Collider2D closetColliderBug4 = determineObjects[18].GetComponent<Collider2D>();
                    if (thisCollider != null && closetColliderBug4 != null && thisCollider.IsTouching(closetColliderBug4))
                    {
                        if (!tarakanKilled4)
                        {
                            determineObjects[18].GetComponent<BugMove>().StopRunning();
                            tarakanKilled4 = true;
                            tarakanKilled++;
                        }
                        determineAudio[0].Play();
                        StartCoroutine(FadeOutCoroutine(determineObjects[18], 0.6f));
                    }
                }
            }

        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("knife_for_tree"))
            {
                if (determineObjects[5] != null)
                {
                    Collider2D closetColliderWood2 = determineObjects[5].GetComponent<Collider2D>();
                    if (thisCollider != null && closetColliderWood2 != null && thisCollider.IsTouching(closetColliderWood2))
                    {
                        determineAudio[1].Play();
                        determineObjects[5].SetActive(false);
                        determineObjects[6].SetActive(true);
                    }
                }
                if (determineObjects[6] != null)
                {
                    Collider2D closetColliderWood3 = determineObjects[6].GetComponent<Collider2D>();
                    if (thisCollider != null && closetColliderWood3 != null && thisCollider.IsTouching(closetColliderWood3))
                    {
                        determineAudio[1].Play();
                        determineObjects[6].SetActive(false);
                        determineObjects[7].SetActive(true);
                    }
                }
                if (determineObjects[7] != null)
                {
                    Collider2D closetColliderWood4 = determineObjects[7].GetComponent<Collider2D>();
                    if (thisCollider != null && closetColliderWood4 != null && thisCollider.IsTouching(closetColliderWood4))
                    {
                        determineAudio[1].Play();
                        determineObjects[7].SetActive(false);
                        determineObjects[8].SetActive(true);
                        determineObjects[9].SetActive(true);
                        determineParticles[0].Stop();
                    }
                }

                if (determineObjects[44] != null)
                {
                    if (!VariableDeterminator.allowPartsMove)
                    {
                        Collider2D closetColliderPCTAdditional = determineObjects2[1].GetComponent<Collider2D>();
                        if (thisCollider != null && closetColliderPCTAdditional != null && thisCollider.IsTouching(closetColliderPCTAdditional))
                        {
                            knifePctDroped++;
                            switch (knifePctDroped)
                            {
                                case 1:
                                    determineAudio[24].Play();
                                    determineObjects[43].SetActive(true);
                                    determineObjects[43].GetComponent<Image>().sprite = determineSprites[20];
                                    break;
                                case 2:
                                    determineAudio[24].Play();
                                    determineObjects[43].GetComponent<Image>().sprite = determineSprites[21];
                                    break;
                                case 3:
                                    determineAudio[24].Play();
                                    determineObjects[43].GetComponent<Image>().sprite = determineSprites[22];
                                    determineObjects[59].SetActive(true);
                                    _ZoomPicture.ZoomOutPicture();
                                    _ZoomPicture.stopZoomEffect();
                                    break;
                            }
                        }
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("gold_key"))
            {
                Collider2D closetColliderWood1 = determineObjects[19].GetComponent<Collider2D>();
                if (thisCollider != null && closetColliderWood1 != null && thisCollider.IsTouching(closetColliderWood1))
                {
                    determineAudio[6].Play();
                    VariableDeterminator.isOpenRoom5 = true;
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("nameplate2_item"))
            {
                Collider2D closetColliderWood1 = determineObjects[50].GetComponent<Collider2D>();
                if (thisCollider != null && closetColliderWood1 != null && thisCollider.IsTouching(closetColliderWood1))
                {
                    if (VariableDeterminator.isUserInNameplatesGameZoomIn)
                    {
                        determineAudio[23].Play();
                        determineObjects[13].SetActive(true);
                        inventorySlot.ClearItem();
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("nameplate5_item"))
            {
                Collider2D closetColliderWood1 = determineObjects[50].GetComponent<Collider2D>();
                if (thisCollider != null && closetColliderWood1 != null && thisCollider.IsTouching(closetColliderWood1))
                {
                    if (VariableDeterminator.isUserInNameplatesGameZoomIn)
                    {
                        determineAudio[23].Play();
                        determineObjects[14].SetActive(true);
                        inventorySlot.ClearItem();
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("nameplate6_item"))
            {
                Collider2D closetColliderWood1 = determineObjects[50].GetComponent<Collider2D>();
                if (thisCollider != null && closetColliderWood1 != null && thisCollider.IsTouching(closetColliderWood1))
                {
                    if (VariableDeterminator.isUserInNameplatesGameZoomIn)
                    {
                        determineAudio[23].Play();
                        determineObjects2[6].SetActive(true);
                        inventorySlot.ClearItem();
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
        {
            string spriteName = inventorySlot.itemImage.sprite.name;
            Collider2D whiteContainer = determineObjects[0].GetComponent<Collider2D>();
            Collider2D purpuleContainer = determineObjects[1].GetComponent<Collider2D>();
            Collider2D yellowContainer = determineObjects[2].GetComponent<Collider2D>();
            Collider2D transparentContainer = determineObjects[3].GetComponent<Collider2D>();

            if (spriteName.Equals("noColorPhoto") && VariableDeterminator.allowPhotoPainting)
            {
                if (whiteContainer != null && thisCollider.IsTouching(whiteContainer))
                {
                    determineAudio[22].Play();
                    GetComponent<Image>().sprite = determineSprites[4];
                }
                else if (purpuleContainer != null && thisCollider.IsTouching(purpuleContainer))
                {
                    GetComponent<Image>().sprite = determineSprites[0];
                    determineAudio[3].Play();
                }
                else if (yellowContainer != null && thisCollider.IsTouching(yellowContainer))
                {
                    determineAudio[22].Play();
                    GetComponent<Image>().sprite = determineSprites[4];
                }
                else if (transparentContainer != null && thisCollider.IsTouching(transparentContainer))
                {
                    determineAudio[22].Play();
                    GetComponent<Image>().sprite = determineSprites[4];
                }
            }
            else if (spriteName.Equals("purpulePhoto") && VariableDeterminator.allowPhotoPainting)
            {
                if (whiteContainer != null && thisCollider.IsTouching(whiteContainer))
                {
                    determineAudio[22].Play();
                    GetComponent<Image>().sprite = determineSprites[4];
                }
                else if (purpuleContainer != null && thisCollider.IsTouching(purpuleContainer))
                {
                    determineAudio[22].Play();
                    GetComponent<Image>().sprite = determineSprites[4];
                }
                else if (yellowContainer != null && thisCollider.IsTouching(yellowContainer))
                {
                    GetComponent<Image>().sprite = determineSprites[1];
                    determineAudio[3].Play();
                }
                else if (transparentContainer != null && thisCollider.IsTouching(transparentContainer))
                {
                    determineAudio[22].Play();
                    GetComponent<Image>().sprite = determineSprites[4];
                }
            }
            else if (spriteName.Equals("yellowPhoto") && VariableDeterminator.allowPhotoPainting)
            {
                if (whiteContainer != null && thisCollider.IsTouching(whiteContainer))
                {
                    GetComponent<Image>().sprite = determineSprites[2];
                    determineAudio[3].Play();
                }
                else if (purpuleContainer != null && thisCollider.IsTouching(purpuleContainer))
                {
                    GetComponent<Image>().sprite = determineSprites[4];
                    determineAudio[22].Play();
                }
                else if (yellowContainer != null && thisCollider.IsTouching(yellowContainer))
                {
                    determineAudio[22].Play();
                    GetComponent<Image>().sprite = determineSprites[4];
                }
                else if (transparentContainer != null && thisCollider.IsTouching(transparentContainer))
                {
                    determineAudio[22].Play();
                    GetComponent<Image>().sprite = determineSprites[4];
                }
            }
            else if (spriteName.Equals("whitePhoto") && VariableDeterminator.allowPhotoPainting)
            {
                if (whiteContainer != null && thisCollider.IsTouching(whiteContainer))
                {
                    GetComponent<Image>().sprite = determineSprites[4];
                    determineAudio[22].Play();
                }
                else if (purpuleContainer != null && thisCollider.IsTouching(purpuleContainer))
                {
                    GetComponent<Image>().sprite = determineSprites[4];
                    determineAudio[22].Play();
                }
                else if (yellowContainer != null && thisCollider.IsTouching(yellowContainer))
                {
                    GetComponent<Image>().sprite = determineSprites[4];
                    determineAudio[22].Play();
                }
                else if (transparentContainer != null && thisCollider.IsTouching(transparentContainer))
                {
                    GetComponent<Image>().sprite = determineSprites[3];
                    determineAudio[3].Play();
                    determineAudio[5].Play();
                    // ПОБЕДА
                }
            }
        }

        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("hammer_item"))
            {
                Collider2D closetCollider = determineObjects[20].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider) && MoveRoom2.carEngine)
                {
                    hammerCrushed +=1;
                    switch (hammerCrushed)
                    {
                        case 1:
                            determineAudio[7].PlayOneShot(determineAudio[7].clip);
                            determineObjects[20].GetComponent<Image>().sprite = determineSprites[5];
                            determineObjects2[3].SetActive(false);
                            break;
                        case 2:
                            determineAudio[7].PlayOneShot(determineAudio[7].clip);
                            determineObjects[20].GetComponent<Image>().sprite = determineSprites[6];
                            break;
                        case 3:
                            determineAudio[8].PlayOneShot(determineAudio[8].clip);
                            determineObjects[20].GetComponent<Image>().sprite = determineSprites[7];
                            inventorySlot.ClearItem();
                            determineObjects[21].SetActive(true);
                            break;
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("needles_item"))
            {
                Collider2D closetCollider1 = determineObjects2[4].GetComponent<Collider2D>();
                Collider2D closetCollider2 = determineObjects2[5].GetComponent<Collider2D>();
                if ((thisCollider != null && closetCollider1 != null && thisCollider.IsTouching(closetCollider1))
                    || (thisCollider != null && closetCollider2 != null && thisCollider.IsTouching(closetCollider2)))
                {
                    determineAudio[4].PlayOneShot(determineAudio[4].clip);
                    inventorySlot.ClearItem();
                    VariableDeterminator.needlesPuten = true;
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("bubble_white_item"))
            {
                Collider2D closetCollider = determineObjects[26].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[26].GetComponent<Image>().sprite = determineSprites[17];
                    determineObjects[25].GetComponent<Image>().sprite = determineSprites[13];
                    determineAudio[9].PlayOneShot(determineAudio[9].clip);
                    inventorySlot.ClearItem();
                    if (++bubblesPut == 4)
                    {
                        VariableDeterminator.allowPhotoPainting = true;
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("bubble_purpule_item"))
            {
                Collider2D closetCollider = determineObjects[27].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[27].GetComponent<Image>().sprite = determineSprites[15];
                    determineObjects[24].GetComponent<Image>().sprite = determineSprites[11];
                    determineAudio[9].PlayOneShot(determineAudio[9].clip);
                    inventorySlot.ClearItem();
                    if (++bubblesPut == 4)
                    {
                        VariableDeterminator.allowPhotoPainting = true;
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("bubble_yellow_item"))
            {
                Collider2D closetCollider = determineObjects[28].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[28].GetComponent<Image>().sprite = determineSprites[18];
                    determineObjects[23].GetComponent<Image>().sprite = determineSprites[14];
                    determineAudio[9].PlayOneShot(determineAudio[9].clip);
                    inventorySlot.ClearItem();
                    if (++bubblesPut == 4)
                    {
                        VariableDeterminator.allowPhotoPainting = true;
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("bubble_transparent_item"))
            {
                Collider2D closetCollider = determineObjects[29].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[29].GetComponent<Image>().sprite = determineSprites[16];
                    determineObjects[22].GetComponent<Image>().sprite = determineSprites[12];
                    determineAudio[9].PlayOneShot(determineAudio[9].clip);
                    inventorySlot.ClearItem();
                    if (++bubblesPut == 4)
                    {
                        VariableDeterminator.allowPhotoPainting = true;
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("wetPhoto"))
            {
                Collider2D closetCollider = determineObjects[30].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[31].SetActive(true);
                    Destroy(determineObjects[31].GetComponent<Collider2D>());

                    _Public_Actions.start_TIP_drying();
                    determineAudio[10].PlayOneShot(determineAudio[10].clip);
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("robot_hand"))
            {
                Collider2D[] closetColliders = determineObjects[34].GetComponents<Collider2D>();
                if (thisCollider != null && closetColliders[0] != null && thisCollider.IsTouching(closetColliders[0]))
                {
                    determineObjects[33].SetActive(true);
                    determineAudio[11].PlayOneShot(determineAudio[11].clip);
                    inventorySlot.ClearItem();
                    if(++robotPartsPuten == 5)
                    {
                        StartCoroutine(_Public_Actions.robotEyesChanging());
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("robot_head"))
            {
                Collider2D[] closetColliders = determineObjects[34].GetComponents<Collider2D>();
                if (thisCollider != null && closetColliders[1] != null && thisCollider.IsTouching(closetColliders[1]))
                {
                    determineObjects[35].SetActive(true);
                    determineAudio[11].PlayOneShot(determineAudio[11].clip);
                    inventorySlot.ClearItem();
                    if (++robotPartsPuten == 5)
                    {
                        StartCoroutine(_Public_Actions.robotEyesChanging());
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("robot_eye"))
            {
                //левый глаз робота
                Collider2D[] closetColliders = determineObjects[35].GetComponents<Collider2D>();
                if(thisCollider != null)
                {
                    if (closetColliders[0] != null && thisCollider.IsTouching(closetColliders[0])
                        && !alreadyPuten1)
                    {
                        alreadyPuten1 = true;
                        determineObjects[36].SetActive(true);
                        determineAudio[11].PlayOneShot(determineAudio[11].clip);
                        inventorySlot.ClearItem();
                        if (++robotPartsPuten == 5)
                        {
                            StartCoroutine(_Public_Actions.robotEyesChanging());
                        }
                    }
                    else if (closetColliders[1] != null && thisCollider.IsTouching(closetColliders[1])
                        && !alreadyPuten2)
                    {
                        alreadyPuten2 = true;
                        determineObjects[37].SetActive(true);
                        determineAudio[11].PlayOneShot(determineAudio[11].clip);
                        inventorySlot.ClearItem();
                        if (++robotPartsPuten == 5)
                        {
                            StartCoroutine(_Public_Actions.robotEyesChanging());
                        }
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("plata_item"))
            {
                //правый глаз робота
                Collider2D[] closetColliders = determineObjects[34].GetComponents<Collider2D>();
                Collider2D[] closetColliders2 = determineObjects[35].GetComponents<Collider2D>();
                if ((thisCollider != null && closetColliders[1] != null && thisCollider.IsTouching(closetColliders[1])) ||
                    (thisCollider != null && closetColliders[2] != null && thisCollider.IsTouching(closetColliders[2])) ||
                    (thisCollider != null && closetColliders2[0] != null && thisCollider.IsTouching(closetColliders2[0])) ||
                    (thisCollider != null && closetColliders2[1] != null && thisCollider.IsTouching(closetColliders2[1])))
                {
                    determineAudio[11].PlayOneShot(determineAudio[11].clip);
                    inventorySlot.ClearItem();
                    if (++robotPartsPuten == 5)
                    {
                        StartCoroutine(_Public_Actions.robotEyesChanging());
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("key_for_the_car"))
            {
                Collider2D closetCollider = determineObjects[39].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    inventorySlot.ClearItem();
                    determineAudio[13].Play();
                    MoveRoom2.carEngine = true;
                    Invoke(nameof(continueSndEngineCar), determineClips[0].length);
                    StartCoroutine(enableLightsOfCar());
                    determineObjects[41].SetActive(true);

#if PLAYMARKET
                    FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("book_purp1_item"))
            {
                Collider2D closetCollider = determineObjects[47].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineAudio[15].PlayOneShot(determineAudio[15].clip);
                    determineObjects[48].SetActive(true);
                    if(++bookPut == 2)
                    {
                        VariableDeterminator.allowToZoomBooks = true;
#if PLAYMARKET
                        FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
                    }
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("book_purp4_item"))
            {
                Collider2D closetCollider = determineObjects[47].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineAudio[15].PlayOneShot(determineAudio[15].clip);
                    determineObjects[49].SetActive(true);
                    if (++bookPut == 2)
                    {
                        VariableDeterminator.allowToZoomBooks = true;
#if PLAYMARKET
                        FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
                    }
                    inventorySlot.ClearItem();
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("finished_photo"))
            {
                Collider2D closetCollider = determineObjects[51].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    if (VariableDeterminator.allowToDragJusan)
                    {
                        determineObjects[53].GetComponent<Image>().color = Color.white;
                        determineAudio[10].PlayOneShot(determineAudio[10].clip);
                        inventorySlot.ClearItem();
                        if(++hintPuten == 2)
                        {
                            //дает ключ
                            StartCoroutine(_foldingScreen.SmoothCanvasGroupForHintDisable());
                            determineObjects[54].SetActive(true);
                        }
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("liquorice"))
            {
                Collider2D closetCollider = determineObjects[51].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    if (VariableDeterminator.allowToDragJusan)
                    {
                        determineObjects[52].GetComponent<Image>().color = Color.white;
                        determineAudio[10].PlayOneShot(determineAudio[10].clip);
                        inventorySlot.ClearItem();
                        if (++hintPuten == 2)
                        {
                            //дает ключ
                            StartCoroutine(_foldingScreen.SmoothCanvasGroupForHintDisable());
                            determineObjects[54].SetActive(true);
                        }
                    }
                }
            }

        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("toner_item"))
            {
                Collider2D closetCollider = determineObjects[55].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[56].GetComponent<Image>().color = Color.white;
                    determineAudio[17].PlayOneShot(determineAudio[17].clip);
                    inventorySlot.ClearItem();
                    if (++hintPrinterPuten == 3)
                    {
                        StartCoroutine(givePaperPassInFewSec());
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("paper_stuck_item"))
            {
                Collider2D closetCollider = determineObjects[55].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[57].GetComponent<Image>().color = Color.white;
                    determineAudio[17].PlayOneShot(determineAudio[17].clip);
                    inventorySlot.ClearItem();
                    if (++hintPrinterPuten == 3)
                    {
                        StartCoroutine(givePaperPassInFewSec());
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("cartrige_item"))
            {
                Collider2D closetCollider = determineObjects[55].GetComponent<Collider2D>();
                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    determineObjects[58].GetComponent<Image>().color = Color.white;
                    determineAudio[17].PlayOneShot(determineAudio[17].clip);
                    inventorySlot.ClearItem();
                    if (++hintPrinterPuten == 3)
                    {
                        StartCoroutine(givePaperPassInFewSec());
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("portal_key"))
            {
                Collider2D closetCollider = determineObjects[60].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    _TimeManager.stopStartClockCicle();
                    inventorySlot.ClearItem();
                    determineObjects[62].SetActive(true);
                    determineAudio[18].Play();
                    determineAudio[19].Play();
                    determineObjects[61].SetActive(false);
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("casset_red_item"))
            {
                Collider2D closetCollider = determineObjects2[0].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    if (!_TVmanager.isCassetteInserted)
                    {
                        _TVmanager.Start_Scene_Film_Casset("red");
                        inventorySlot.ClearItem();
                        determineAudio[20].Play();
                    }
                }
            }
        if (inventorySlot.itemImage.sprite != null)
            if (inventorySlot.itemImage.sprite.name.Equals("casset_yellow_item"))
            {
                Collider2D closetCollider = determineObjects2[0].GetComponent<Collider2D>();

                if (thisCollider != null && closetCollider != null && thisCollider.IsTouching(closetCollider))
                {
                    if (!_TVmanager.isCassetteInserted)
                    {
                        _TVmanager.Start_Scene_Film_Casset("yellow");
                        inventorySlot.ClearItem();
                        determineAudio[20].Play();
                    }
                }
            }

        // Убедитесь, что объект возвращается в начальную позицию
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one; // Финальное подтверждение масштаба

        VariableDeterminator.isDraging = false;
        determineObjects[50].SetActive(false);
    }

    private IEnumerator enableLightsOfCar()
    {
        yield return new WaitForSeconds(0.2f);
        determineAudio[12].Play();
        yield return new WaitForSeconds(0.9f);
        _CarShaking.startShaking();
        determineObjects[40].SetActive(true);
        determineObjects2[3].SetActive(true);
        determineObjects[39].SetActive(false);
        determineObjects[63].SetActive(true);
    }

    private void continueSndEngineCar()
    {
        if (variableDeterminator.current_roomNumber == 4)
        {
            determineAudio[12].loop = true;
            determineAudio[12].clip = determineClips[1];
            determineAudio[12].Play();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (inventorySlot.itemImage.sprite != null)
        {
            float scaleFactor = canvas.scaleFactor > 0 ? canvas.scaleFactor : 1f;
            rectTransform.anchoredPosition += eventData.delta / scaleFactor;
        }

    }

    private IEnumerator givePaperPassInFewSec()
    {
        yield return new WaitForSeconds(1f);
        determineObjects[56].SetActive(false);
        determineObjects[57].SetActive(false);
        determineObjects[58].SetActive(false);
        determineAudio[16].PlayOneShot(determineAudio[16].clip);
        yield return new WaitUntil(() => !determineAudio[16].isPlaying);
        //дает записку с паролем к сейфу
        determineObjects[4].SetActive(true);
    }
    private IEnumerator FadeOutCoroutine(GameObject gameObjectForCanvas, float duration)
    {
        CanvasGroup canvasGroup = gameObjectForCanvas.GetComponent<CanvasGroup>();
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f; // Убедимся, что alpha стал 0
       
        if (tarakanKilled == 4)
        {
            determineAudio[21].PlayOneShot(determineAudio[21].clip);
            VariableDeterminator.isMiniGameActive = false;
            VariableDeterminator.allowNamePlatesGame = true;
            Destroy(determineObjects[11].GetComponent<Collider2D>());
            determineObjects[38].SetActive(false);
            foreach (GameObject obj in arrows_containers)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
            public_actions.Instance.OnOff_SpaceTip_Close(false);
            colliderList.ENABLE_COLLIDER();
            StartCoroutine(moveUpNamePlates());
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObjectForCanvas);
    }
    private IEnumerator moveUpNamePlates()
    {
        RectTransform rectTransform = determineObjects[10].GetComponent<RectTransform>();
        Vector2 startPos = rectTransform.anchoredPosition;

        // Определяем конечную позицию (поднимем объект на 25 единиц по оси Y)
        Vector2 targetPos = startPos + new Vector2(0, 25f);
        float duration = 0.75f;
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = targetPos;
    }
}