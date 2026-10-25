using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridScrolling : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public int slotsPerPage = 9;
    public AudioSource sound;
    
    public static int currentPage = 0;
    public bool back_btn = false;
    
    private Vector3 defaultScale;
    [SerializeField] private float scaleDuration = 0.1f;
    [SerializeField] private float scaleAmount = 0.9f;
    [SerializeField] private GameObject leftArrow, rightArrow;
    public static bool isAnimated = false;
    private void Start()
    {
        update_method();
        defaultScale = transform.localScale;
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
            if (!isAnimated)
            {
                isAnimated = true;
                if (!back_btn)
                {
                    StartCoroutine(ScaleButton());
                    sound.Play();
                    currentPage--;
                    UpdateSlots();
                }
                else
                {
                    StartCoroutine(ScaleButton());
                    sound.Play();
                    currentPage++;
                    UpdateSlots();
                }
            }
        }
    }
    private void update_method()
    {
        switch (currentPage)
        {
            case 0:
                leftArrow.SetActive(false);
                rightArrow.SetActive(true);
                break;
            case 1:
                leftArrow.SetActive(true);
                rightArrow.SetActive(true);
                break;
            case 2:
                leftArrow.SetActive(true);
                rightArrow.SetActive(false);
                break;
        }
    }

    private void UpdateSlots()
    {
        int startIndex = currentPage * slotsPerPage;
        int endIndex = Mathf.Min(startIndex + slotsPerPage, gridLayout.transform.childCount);

        for (int i = 0; i < gridLayout.transform.childCount; i++)
        {
            GameObject slot = gridLayout.transform.GetChild(i).gameObject;
            bool isActive = i >= startIndex && i < endIndex;
            slot.SetActive(isActive);
        }
    }
    private IEnumerator ScaleButton()
    {
        // Уменьшаем кнопку
        float timer = 0f;
        while (timer < scaleDuration)
        {
            float scale = Mathf.Lerp(1f, scaleAmount, timer / scaleDuration);
            transform.localScale = defaultScale * scale;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = defaultScale * scaleAmount;

        // Восстанавливаем кнопку
        timer = 0f;
        while (timer < scaleDuration)
        {
            float scale = Mathf.Lerp(scaleAmount, 1f, timer / scaleDuration);
            transform.localScale = defaultScale * scale;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = defaultScale;

        update_method();

        isAnimated = false;
    }
}