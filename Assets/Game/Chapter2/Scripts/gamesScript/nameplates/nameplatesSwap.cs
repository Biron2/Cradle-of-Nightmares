using UnityEngine;
using UnityEngine.UI;

public class nameplatesSwap : MonoBehaviour
{
    public bool isSelected = false;
    public static Image selectedImage;
    public AudioSource hlop, winGameSnd;

    [SerializeField] private nameplatesSwap[] clickSwaps;
    [SerializeField] private Image cont1, cont2, cont3, cont4, cont5, cont6;
    [SerializeField] private GameObject pctPart2;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ClickEvent);
    }
    private void ClickEvent()
    {
        if (!VariableDeterminator.allowNameplatesPazzle || !VariableDeterminator.isUserInNameplatesGameZoomIn)
        {
            return;
        }
        if (!isSelected)
        {
            if (selectedImage == null)
            {
                isSelected = true;
                selectedImage = GetComponent<Image>();
                selectedImage.color = Color.gray;
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
        if (VariableDeterminator.allowNameplatesPazzle == true)
        {
            if (cont1.sprite.name.Equals("nameplate3") &&
                cont2.sprite.name.Equals("nameplate4") &&
                cont3.sprite.name.Equals("nameplate1") &&
                cont4.sprite.name.Equals("nameplate2") &&
                cont5.sprite.name.Equals("nameplate6") &&
                cont6.sprite.name.Equals("nameplate5"))
            {
                winGameSnd.PlayOneShot(winGameSnd.clip);
                pctPart2.SetActive(true);
                VariableDeterminator.allowNameplatesPazzle = false;
            }
        }
    }
}