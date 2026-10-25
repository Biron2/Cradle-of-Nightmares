using UnityEngine;

public class openAndCloseCloset : MonoBehaviour
{
    public GameObject objectToActive;

    [SerializeField] private AudioSource openAndCloseSound;
    private float cooldown = 0.5f;
    private float lastClickTime = 0f;

    private Collider2D[] colliders;
    private int activeColliderIndex = 0;
    public bool currentColliderState = false;

    private void Start()
    {
        // Получаем коллайдеры на этом объекте
        colliders = GetComponents<Collider2D>();

        if (colliders.Length > 0)
        {
            SetActiveCollider(0);
            currentColliderState = false;
        }
        else
        {
            Debug.LogWarning("No 2D colliders found on the object with the script.");
            enabled = false;
        }
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
                return;
            }

            lastClickTime = Time.time;

            openAndCloseSound.PlayOneShot(openAndCloseSound.clip);
            objectToActive.SetActive(!objectToActive.activeSelf);

            activeColliderIndex = (activeColliderIndex + 1) % colliders.Length;
            SetActiveCollider(activeColliderIndex);
            currentColliderState = !currentColliderState;
        }
    }


    private void SetActiveCollider(int index)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = (i == index);
        }
    }
}