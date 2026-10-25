using UnityEngine;
using UnityEngine.UI;

public class sakiura_Pct_Swap : MonoBehaviour
{
    public bool isSelected = false;
    public static Image selectedImage;
    public AudioSource hlop, winGameSnd;

    [SerializeField] private sakiura_Pct_Swap[] clickSwaps;
    [SerializeField] private Image[] sakura_source_part_images;
    [SerializeField] private Image cont1, cont2, cont3, cont4, cont5, cont6, cont7, cont8, cont9;
    [SerializeField] private GameObject nooberObj, sakuraPctBlock;
    private ColliderList colliderList;
    [SerializeField] private GameObject[] arrows_containers;
    private Image currentImage;
    [SerializeField] private GameObject books_openColliderObj;
    private void Start()
    {
        currentImage = GetComponent<Image>();
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();

        for (int i = 0; i < sakura_source_part_images.Length; i++)
        {
            if (gameObject.name == "window_part" + (i + 1).ToString())
            {
                currentImage.sprite = sakura_source_part_images[i].sprite;
                break;
            }
        }

        GetComponent<Button>().onClick.AddListener(ClickEvent);
    }
    private void ClickEvent()
    {
        if (!VariableDeterminator.allowPartsMove)
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
                clickSwaps[4].isSelected = false;
                clickSwaps[5].isSelected = false;
                clickSwaps[6].isSelected = false;
                clickSwaps[7].isSelected = false;
                clickSwaps[8].isSelected = false;
                selectedImage = null;
                hlop.Play();

                sakura_source_part_images[0].sprite = cont1.sprite;
                sakura_source_part_images[1].sprite = cont2.sprite;
                sakura_source_part_images[2].sprite = cont3.sprite;
                sakura_source_part_images[3].sprite = cont4.sprite;
                sakura_source_part_images[4].sprite = cont5.sprite;
                sakura_source_part_images[5].sprite = cont6.sprite;
                sakura_source_part_images[6].sprite = cont7.sprite;
                sakura_source_part_images[7].sprite = cont8.sprite;
                sakura_source_part_images[8].sprite = cont9.sprite;
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
    private void Update()
    {
        if (VariableDeterminator.allowPartsMove)
        {
            if (cont1.sprite.name.Equals("image_part_001") &&
                cont2.sprite.name.Equals("image_part_002") &&
                cont3.sprite.name.Equals("image_part_003") &&
                cont4.sprite.name.Equals("image_part_004") &&
                cont5.sprite.name.Equals("image_part_005") &&
                cont6.sprite.name.Equals("image_part_006") &&
                cont7.sprite.name.Equals("image_part_007") &&
                cont8.sprite.name.Equals("image_part_008") &&
                cont9.sprite.name.Equals("image_part_009"))
            {
                winGameSnd.PlayOneShot(winGameSnd.clip);
                VariableDeterminator.allowPartsMove = false;
                nooberObj.SetActive(false);
                Destroy(sakuraPctBlock.GetComponent<OpenAndCollider>());
                Destroy(sakuraPctBlock.GetComponent<Collider2D>());
                books_openColliderObj.SetActive(true);

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