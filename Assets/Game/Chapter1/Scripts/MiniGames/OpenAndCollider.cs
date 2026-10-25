using UnityEngine;

public class OpenAndCollider : MonoBehaviour
{
    public GameObject objectToActive;
    private ColliderList colliderList;
    [SerializeField] private GameObject[] arrows_containers;

    private audio_for_items _Audio_For_Items;
    private float cooldown = 0.5f; // ¬рем¤ задержки в секундах
    private float lastClickTime = 0f; // ¬рем¤ последнего нажати¤
    private void Start()
    {
        _Audio_For_Items = GameObject.Find("Other/GameManager/EtcSounds/__FOR_ITEMS__").GetComponent<audio_for_items>();
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
    }
    void Update()
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
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if ((gameObject.name == "clownBalloon" || gameObject.name == "clown_balloon")
                && (VariableDeterminator.clownBalloonPuzzleFinished || !VariableDeterminator.needlesPuten))
            {
                return;
            }
            if (Time.time - lastClickTime < cooldown)
            {
                // ¬рем¤ после последнего нажати¤ меньше задержки, поэтому ничего не делаем
                return;
            }
            lastClickTime = Time.time; // ќбновл¤ем врем¤ последнего нажати¤
            _Audio_For_Items.PlaySounds(gameObject.name);
            foreach (GameObject obj in arrows_containers)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            if (gameObject.name.Contains("OpenTip"))
            {
                public_actions.Instance.OnOff_SpaceTipFile_Close(true);
            }
            else
            {
                public_actions.Instance.OnOff_SpaceTip_Close(true);
            }
            objectToActive.SetActive(true);
            colliderList.DISABLE_COLLIDER();
            VariableDeterminator.isMiniGameActive = true;
        }

    }
}