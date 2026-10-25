using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowButton : MonoBehaviour
{
    private Vector3 defaultScale;
    [SerializeField] private float scaleDuration = 0.1f;
    [SerializeField] private float scaleAmount = 0.9f;
    [SerializeField] private MoveRoom moveRoom;
    [SerializeField] private MoveRoom2 moveRoom2;
    public bool activeArrowBtn = false;
    private void Start()
    {
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
            if (activeArrowBtn == true)
            {
                StartCoroutine(ScaleButton());

                switch (SceneManager.GetActiveScene().name)
                {
                    case "Chapter1":
                        if (moveRoom != null) moveRoom.ActiveBtn();
                        break;
                    case "Chapter2":
                        if (moveRoom2 != null) moveRoom2.ActiveBtn();
                        break;
                }
            }
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
    }
}
