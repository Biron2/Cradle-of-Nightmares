using UnityEngine;
using System.Collections;

public class ObjectRotateLoop : MonoBehaviour
{
    public PictureOpenClose pictureOpenClose;

    public Transform transform1, transform2;
    [SerializeField] private GameObject pauseMenu;

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
            if (pauseMenu.activeSelf) return;

            if (transform1.gameObject.activeSelf && transform2.gameObject.activeSelf)
            {
                if (!pictureOpenClose.isRotating)
                {
                    pictureOpenClose.startRotateMethod();
                }
            }
        }
    }
}