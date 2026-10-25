using UnityEngine;

public class openBookCollider : MonoBehaviour
{
    [SerializeField] private zoomBooks _SafeZoom;

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
            if (!VariableDeterminator.allowToZoomBooks)
            {
                return;
            }
            _SafeZoom.PhoneWasClicked();
            gameObject.SetActive(false);
        }
    }
}
