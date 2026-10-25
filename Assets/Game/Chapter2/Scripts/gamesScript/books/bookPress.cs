using UnityEngine;
using UnityEngine.UI;

public class bookPress : MonoBehaviour
{
    [SerializeField] private zoomBooks _ZoomBooks;
    public int bookNum;
    public bool clicked = false;

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
            if (VariableDeterminator.allowToZoomBooks && !clicked)
            {
                // Преобразуем hex-цвет #595959 в Unity Color
                Color newColor;
                if (ColorUtility.TryParseHtmlString("#595959", out newColor))
                {
                    transform.parent.GetComponent<Image>().color = newColor;
                }
                else
                {
                    Debug.LogError("Не удалось преобразовать цвет #595959");
                }
                clicked = true;
                _ZoomBooks.Enter_a_Number(bookNum.ToString());
            }
        }
    }
}
