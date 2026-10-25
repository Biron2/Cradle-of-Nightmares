using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(RectTransform), typeof(CanvasGroup))]
public class DragItems : MonoBehaviour
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public RectTransform boundsRectTransform;

    private Collider2D col;
    private bool isDragging = false;
    private Vector2 offset;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            // Получаем все коллайдеры под курсором
            var hits = Physics2D.OverlapPointAll(mouseWorldPos);

            // Проверяем, наш ли коллайдер сверху
            foreach (var hit in hits)
            {
                if (hit == col)
                {
                    // Наш найден — проверим, не перекрыт ли другим
                    bool isTop = true;

                    foreach (var other in hits)
                    {
                        if (other != col && other.transform.GetSiblingIndex() > transform.GetSiblingIndex())
                        {
                            isTop = false;
                            break;
                        }
                    }

                    if (isTop)
                    {
                        isDragging = true;
                        canvasGroup.blocksRaycasts = false;

                        Vector2 localMousePos;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            rectTransform.parent as RectTransform,
                            Input.mousePosition,
                            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                            out localMousePos
                        );

                        offset = rectTransform.anchoredPosition - localMousePos;
                        transform.SetAsLastSibling();
                    }

                    break; // мы нашли свой коллайдер — выходим
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            canvasGroup.blocksRaycasts = true;
        }

        if (isDragging)
        {
            Vector2 localMousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                Input.mousePosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out localMousePos
            );

            Vector3 newPosition = localMousePos + offset;

            Vector3 worldSize = col.bounds.size;
            Vector3 localSize = boundsRectTransform.InverseTransformVector(worldSize);

            float halfWidth = localSize.x / 2f;
            float halfHeight = localSize.y / 2f;

            float minX = boundsRectTransform.rect.xMin + halfWidth;
            float maxX = boundsRectTransform.rect.xMax - halfWidth;
            float minY = boundsRectTransform.rect.yMin + halfHeight;
            float maxY = boundsRectTransform.rect.yMax - halfHeight;

            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            rectTransform.anchoredPosition = newPosition;
        }
    }

}