using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ItemPickup : MonoBehaviour
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
    [SerializeField] private GameObject objToActions;

    private AchievementsManager _AchievementsManager;
    private void Start()
    {
        try
        {
            _AchievementsManager = GameObject.Find("AchievementsManager").GetComponent<AchievementsManager>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        material = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        targetPosition = Vector3.zero;
        targetScale = Vector3.one * 3f;
        parentObject = GameObject.Find("Interface/picked_Item");
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

        if (isMoving)
        {
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, targetPosition, Time.deltaTime * translationSpeed);
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, Time.deltaTime * scaleSpeed);
        }
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (taken == false)
            {
                if (gameObject.GetComponent<Image>().sprite.name == "cookie")
                {
                    StartCoroutine(GiveAch(1));
                }
                if (gameObject.GetComponent<Image>().sprite.name == "duck_item")
                {
                    StartCoroutine(GiveAch(2));
                }
                rectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
                taken = true;
                gameObject.SetActive(true);
                isMoving = true;
                transform.SetParent(parentObject.transform);
                material.material = null;
                StartCoroutine(AddToInv());
                mAudio_For_Items.PlaySoundsItem(itemSprite.name);
                try
                {
                    if (itemSprite.name == "knife_for_tree")
                    {
                        Destroy(objToActions);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                try
                {
                    if (itemSprite.name == "finished_photo")
                    {
                        Destroy(objToActions);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
            }
        }
    }
    private IEnumerator GiveAch(int i)
    {
        yield return new WaitForSeconds(1.5f);
        if (_AchievementsManager != null)
        {
            switch (i)
            {
                case 1:
                    _AchievementsManager.SetAchievement("achCookie");
                    break;
                case 2:
                    _AchievementsManager.SetAchievement("achDuck");
                    break;
            }
        }
    }
    private IEnumerator AddToInv()
    {
        yield return new WaitForSeconds(2.5f);
        isMoving = false;
        if (inventoryManager.AddItemToInventory(itemSprite))
        {
            Destroy(gameObject);
        }
    }
}