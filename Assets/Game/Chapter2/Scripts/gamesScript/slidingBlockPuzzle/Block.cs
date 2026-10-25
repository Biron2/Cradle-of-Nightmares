using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public List<Vector2Int> gridPositions;
    public bool isMainBlock, isHorizontal = true;

    private RectTransform rectTransform;
    private GridManager gridManager;
    [SerializeField] private AudioSource slidingSound;
    public static bool isAllowToMoveBlocks = true;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        gridManager = transform.parent.GetComponent<GridManager>();
    }

    private void Start()
    {
        UpdateVisualPositionImmediate();
    }

    private void UpdateVisualPositionImmediate()
    {
        if (gridManager == null || gridPositions.Count == 0) return;

        Vector2Int first = gridPositions[0];
        rectTransform.anchoredPosition = new Vector2(
            first.x * gridManager.cellSize,
            -first.y * gridManager.cellSize
        );
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gridManager == null || isAllowToMoveBlocks == false) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gridManager.GetComponent<RectTransform>(),
            eventData.pressPosition,
            eventData.pressEventCamera,
            out Vector2 localStart
        );

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gridManager.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localEnd
        );

        Vector2 drag = localEnd - localStart;
        if (drag.magnitude < 30f) return;

        if (isHorizontal)
        {
            if (Mathf.Abs(drag.x) > Mathf.Abs(drag.y))
                TryMove(drag.x > 0 ? Vector2Int.right : Vector2Int.left);
        }
        else
        {
            if (Mathf.Abs(drag.y) > Mathf.Abs(drag.x))
                TryMove(drag.y > 0 ? Vector2Int.down : Vector2Int.up);
        }
    }

    private void TryMove(Vector2Int direction)
    {
        List<Vector2Int> targetPositions = new List<Vector2Int>();
        foreach (var pos in gridPositions)
            targetPositions.Add(pos + direction);

        if (!gridManager.CanMoveTo(this, targetPositions))
            return;

        gridManager.UpdateBlockPosition(this, targetPositions);
        gridPositions = targetPositions;

        StopAllCoroutines();
        StartCoroutine(AnimateMovement());
        slidingSound.PlayOneShot(slidingSound.clip);
    }

    private IEnumerator AnimateMovement()
    {
        Vector2 startPos = rectTransform.anchoredPosition;

        Vector2Int first = gridPositions[0];
        Vector2 targetPos = new Vector2(
            first.x * gridManager.cellSize,
            -first.y * gridManager.cellSize
        );

        float duration = 0.2f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;

        // Проверка победы
        if (isMainBlock && HasReachedWinPosition())
        {
            isAllowToMoveBlocks = false;
            gridManager.OnWin(); // или другой метод, отвечающий за победу
        }
    }
    private bool HasReachedWinPosition()
    {
        return gridPositions.Contains(new Vector2Int(0, 3)) &&
               gridPositions.Contains(new Vector2Int(1, 3));
    }

}