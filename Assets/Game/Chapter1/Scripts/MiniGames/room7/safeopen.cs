using UnityEngine;

public class safeopen : MonoBehaviour
{
    public Vector3 rotate1;
    public Vector3 rotate2;
    public Vector3 rotate3;
    public Vector3 rotate4;
    public Vector3 rotate5;
    public Vector3 rotate6;
    public Vector3 rotate7;
    public Vector3 rotate8;
    public Vector3 rotate9;
    public Vector3 rotate10;
    public Vector3 rotate11;
    public Vector3 rotate12;
    
    private bool openSafe = false;
    public GameObject safeOpenedObj, safeClosedObj, safeInside;

    private string password = "";

    public AudioSource clickNumber, safeOpenedSound;
    public AudioClip clickTapSafeSnd;
    [SerializeField] private GameObject[] arrows_containers;
    private ColliderList colliderList;
    [SerializeField] private GameObject _tip4, _tip2;
    public void rotate_num1()
    {
        transform.eulerAngles = rotate1;
        numberForPassword("1");
    }
    public void rotate_num2()
    {
        transform.eulerAngles = rotate2;
        numberForPassword("2");
    }
    public void rotate_num3()
    {
        transform.eulerAngles = rotate3;
        numberForPassword("3");
    }
    public void rotate_num4()
    {
        transform.eulerAngles = rotate4;
        numberForPassword("4");
    }
    public void rotate_num5()
    {
        transform.eulerAngles = rotate5;
        numberForPassword("5");
    }
    public void rotate_num6()
    {
        transform.eulerAngles = rotate6;
        numberForPassword("6");
    }
    public void rotate_num7()
    {
        transform.eulerAngles = rotate7;
        numberForPassword("7");
    }
    public void rotate_num8()
    {
        transform.eulerAngles = rotate8;
        numberForPassword("8");
    }
    public void rotate_num9()
    {
        transform.eulerAngles = rotate9;
        numberForPassword("9");
    }
    public void rotate_num10()
    {
        transform.eulerAngles = rotate10;
        numberForPassword("10");
    }
    public void rotate_num11()
    {
        transform.eulerAngles = rotate11;
        numberForPassword("11");
    }
    public void rotate_num12()
    {
        transform.eulerAngles = rotate12;
        numberForPassword("12");
    }

    private void numberForPassword(string number)
    {
        if(openSafe == false)
        {
            password += number;
            clickNumber.PlayOneShot(clickTapSafeSnd);
        }
    }
    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
    }
    private void Update()
    {
        if (openSafe == false)
        {
            if (password.Contains("1998"))
            {
                _tip2.SetActive(false);
                _tip4.SetActive(false);
                openSafe = true;
                safeOpenedObj.SetActive(true);
                safeClosedObj.SetActive(false);
                safeInside.SetActive(false);
                safeOpenedSound.Play();
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

#if PLAYMARKET
                FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
            }
        }
    }
}
