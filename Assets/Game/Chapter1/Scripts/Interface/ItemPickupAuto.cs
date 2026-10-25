using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ItemPickupAuto : MonoBehaviour
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
    private void Start()
    {
        material = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        targetPosition = Vector3.zero;
        targetScale = Vector3.one * 3f;
        parentObject = GameObject.Find("Interface/picked_Item");

        if (gameObject != null)
        {
            rectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
            gameObject.SetActive(true);
            isMoving = true;
            transform.SetParent(parentObject.transform);
            material.material = null;
            Invoke(nameof(AddToInv), 2.5f);
            mAudio_For_Items.PlaySoundsItem(itemSprite.name);
        }
    }
    private void Update()
    {
        if (isMoving)
        {
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, targetPosition, Time.deltaTime * translationSpeed);
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, Time.deltaTime * scaleSpeed);
        }
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