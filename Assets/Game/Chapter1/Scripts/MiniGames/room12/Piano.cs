using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano : MonoBehaviour
{
    public bool allowOpenPianoGame = false;
    [SerializeField] private GameObject pianoGame;
    [SerializeField] private GameObject[] arrows_containers;
    private ColliderList colliderList;

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
            if (Time.time - lastClickTime < cooldown)
            {
                // ¬рем¤ после последнего нажати¤ меньше задержки, поэтому ничего не делаем
                return;
            }
            lastClickTime = Time.time; // ќбновл¤ем врем¤ последнего нажати¤
            if (allowOpenPianoGame == true)
            {
                _Audio_For_Items.PlaySounds(gameObject.name);
                pianoGame.SetActive(true);
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
        }
    }
}
