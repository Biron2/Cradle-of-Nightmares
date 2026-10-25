using System.Collections;
using UnityEngine;

public class phone_key_clicking : MonoBehaviour
{
    public string keyClicked;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private float pressScale = 0.9f; // масштаб при нажатии
    [SerializeField] private float pressDuration = 0.1f; // длительность анимации

    private Vector3 originalScale;
    private bool isAnimating = false;
    private Transform parentTransform;
    public static bool allowWinConditionForPhone = true;
    [SerializeField] private zoomOldPhone _ZoomOldPhone;

    private void Start()
    {
        parentTransform = transform.parent;
        originalScale = parentTransform.localScale;
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
            if (isAnimating || !allowWinConditionForPhone) return;

            if (VariableDeterminator.isUserInOldPhoneZoomIn)
            {
                _ZoomOldPhone.old_phone_number += keyClicked;
                clickSound.PlayOneShot(clickSound.clip);
                StartCoroutine(AnimateKeyPress());

                if (_ZoomOldPhone.old_phone_number.Contains("+24*04*24"))
                {
                    //win
                    _ZoomOldPhone.finishThePhonePuzzle();
                    allowWinConditionForPhone = false;
                }
            }
        }
    }

    private IEnumerator AnimateKeyPress()
    {
        isAnimating = true;

        // Уменьшаем масштаб родителя
        parentTransform.localScale = originalScale * pressScale;

        // Ждём немного
        yield return new WaitForSeconds(pressDuration);

        // Возвращаем масштаб
        parentTransform.localScale = originalScale;

        isAnimating = false;
    }
}