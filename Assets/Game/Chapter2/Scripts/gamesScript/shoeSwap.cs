using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class shoeSwap : MonoBehaviour
{
    public bool isSelected = false;
    public static Image selectedImage;
    public AudioSource hlop, winGameSnd;

    [SerializeField] private shoeSwap[] clickSwaps;
    [SerializeField] private Image cont1, cont2, cont3, cont4;
    public static bool allowRepeatUpdate = true;
    [SerializeField] private GameObject pctPart2, nooberObj, carpetBlock;
    private ColliderList colliderList;
    [SerializeField] private GameObject[] arrows_containers;
    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!allowRepeatUpdate)
            {
                return;
            }
            if (!isSelected)
            {
                if (selectedImage == null)
                {
                    isSelected = true;
                    selectedImage = GetComponent<Image>();
                    selectedImage.color = Color.red;
                }
                else
                {
                    Image currentImage = GetComponent<Image>();
                    Sprite tempSprite = selectedImage.sprite;
                    selectedImage.sprite = currentImage.sprite;
                    currentImage.sprite = tempSprite;
                    selectedImage.color = Color.white;
                    currentImage.color = Color.white;

                    isSelected = false;
                    clickSwaps[0].isSelected = false;
                    clickSwaps[1].isSelected = false;
                    clickSwaps[2].isSelected = false;
                    clickSwaps[3].isSelected = false;
                    selectedImage = null;
                    hlop.Play();
                }
            }
            else
            {
                isSelected = false;
                if (selectedImage != null)
                {
                    selectedImage.color = Color.white;
                    selectedImage = null;
                }
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

        if (allowRepeatUpdate == true)
        {
            if (cont1.sprite.name.Equals("blackShoe") &&
                cont2.sprite.name.Equals("greenShoe") &&
                cont3.sprite.name.Equals("brownShoe") &&
                cont4.sprite.name.Equals("blueShoe"))
            {
                winGameSnd.PlayOneShot(winGameSnd.clip);
                pctPart2.SetActive(true);
                allowRepeatUpdate = false;
                Destroy(carpetBlock.GetComponent<Collider2D>());
                nooberObj.SetActive(false);

                foreach (GameObject obj in arrows_containers)
                {
                    if (obj != null)
                    {
                        obj.SetActive(true);
                    }
                }
                colliderList.ENABLE_COLLIDER();
                public_actions.Instance.OnOff_SpaceTip_Close(false);
                VariableDeterminator.isMiniGameActive = false;
            }
        }
    }
}