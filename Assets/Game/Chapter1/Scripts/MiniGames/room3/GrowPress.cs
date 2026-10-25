using UnityEngine;
using System.Collections;

public class GrowPress : MonoBehaviour
{
    public GameObject nextGrow;
    public AudioSource growSnd;
    public float delay = 1f;

    private bool isClickable = false; // Флаг, разрешающий или запрещающий клик
    private void Start()
    {
        isClickable = true;
        StartCoroutine(HandleMouseDown());
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
            if (!isClickable) return; // Если объект недоступен для нажатия, выходим

            isClickable = false; // Запрещаем повторный клик
            gameObject.SetActive(false);
            nextGrow.SetActive(true);
            growSnd.Play();
        }
    }

    private IEnumerator HandleMouseDown()
    {
        yield return new WaitForSeconds(delay); // Задержка в 1 секунду
        isClickable = true; // Снова разрешаем клик
    }
}
