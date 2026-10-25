using UnityEngine;
using UnityEngine.UI;

public class ItemPickupButton : MonoBehaviour
{
    public Sprite itemSprite;
    public InventoryManager inventoryManager;
    private RectTransform rectTransform;
    private Vector3 targetPosition;
    private Vector3 targetScale;
    private float translationSpeed = 1f;
    private float scaleSpeed = 2f;
    private bool isMoving = false;
    private GameObject parentObject;
    private Image material;
    public audio_for_items mAudio_For_Items;
    private bool taken = false;

    [Header("Colliders that block pickup when overlapping")]
    [SerializeField] private Collider2D[] blockingColliders;

    private void Start()
    {
        material = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        targetPosition = Vector3.zero;
        targetScale = Vector3.one * 3f;
        parentObject = GameObject.Find("Interface");
    }

    private void Update()
    {
        if (isMoving)
        {
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, targetPosition, Time.deltaTime * translationSpeed);
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, Time.deltaTime * scaleSpeed);
        }
    }

    public void StartPickUpButton()
    {
        if (taken) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Проверка: если мышь находится над хотя бы одним коллайдером из blockingColliders — отменяем
        foreach (var blocker in blockingColliders)
        {
            if (blocker != null && blocker.OverlapPoint(mouseWorldPos))
            {
                return;
            }
        }

        // Всё чисто — запускаем логику подбора
        rectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        taken = true;
        gameObject.SetActive(true);
        isMoving = true;
        transform.SetParent(parentObject.transform);
        material.material = null;
        Invoke(nameof(AddToInv), 2.5f);
        mAudio_For_Items.PlaySoundsItem(itemSprite.name);
    }

    private void AddToInv()
    {
        isMoving = false;
        if (inventoryManager.AddItemToInventory(itemSprite))
        {
            Destroy(gameObject);
        }
    }
}