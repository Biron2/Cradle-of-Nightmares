using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickSwap : MonoBehaviour
{
    public bool isSelected = false;
    public static Image selectedImage;
    public AudioSource hlop, winGameSnd;
    public VariableDeterminator variableDeterminator;

    [SerializeField] private ClickSwap[] clickSwaps;
    [SerializeField] private Image cont1, cont2, cont3, cont4, cont5, cont6, cont7, cont8, cont9, cont10, cont11, cont12, cont13, cont14, cont15, cont16;
    public static bool allowRepeatUpdate = true;
    [SerializeField] private GameObject puzzleFilledPct, pctPart2, nooberObj;
    private ColliderList colliderList;
    [SerializeField] private GameObject[] arrows_containers;
    private InventorySlot inventorySlot;
    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
    }
    public void ClickOnPart()
    {
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
                clickSwaps[9].isSelected = false;
                clickSwaps[10].isSelected = false;
                clickSwaps[11].isSelected = false;
                clickSwaps[12].isSelected = false;
                clickSwaps[13].isSelected = false;
                clickSwaps[14].isSelected = false;
                clickSwaps[15].isSelected = false;
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
    private void Update()
    {
        if(allowRepeatUpdate == true)
        {
            if (cont1.sprite.name.Equals("image_part_001") &&
                cont2.sprite.name.Equals("image_part_002") &&
                cont3.sprite.name.Equals("image_part_003") &&
                cont4.sprite.name.Equals("image_part_004") &&
                cont5.sprite.name.Equals("image_part_005") &&
                cont6.sprite.name.Equals("image_part_006") &&
                cont7.sprite.name.Equals("image_part_007") &&
                cont8.sprite.name.Equals("image_part_008") &&
                cont9.sprite.name.Equals("image_part_009") &&
                cont10.sprite.name.Equals("image_part_010") &&
                cont11.sprite.name.Equals("image_part_011") &&
                cont12.sprite.name.Equals("image_part_012") &&
                cont13.sprite.name.Equals("image_part_013") &&
                cont14.sprite.name.Equals("image_part_014") &&
                cont15.sprite.name.Equals("image_part_015") &&
                cont16.sprite.name.Equals("image_part_016"))
            {
                inventorySlot = GameObject.Find(variableDeterminator.pathOfSlot).GetComponent<InventorySlot>();
                inventorySlot.ClearItem();
                winGameSnd.Play();
                foreach (GameObject obj in arrows_containers)
                {
                    if (obj != null)
                    {
                        obj.SetActive(true);
                    }
                }
                public_actions.Instance.OnOff_SpaceTip_Close(false);
                puzzleFilledPct.SetActive(true);
                nooberObj.SetActive(false);
                pctPart2.SetActive(true);
                colliderList.ENABLE_COLLIDER();
                allowRepeatUpdate = false;
            }
        }
    }
}