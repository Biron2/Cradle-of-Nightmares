using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scalesGame : MonoBehaviour
{
    private bool switchSides1 = false, switchSides2 = false, switchSides3 = false, switchSides4 = false;
    public static bool allowScalePressing = false;
    public static int stepScales1 = 4, stepScales2 = 4, stepScales3 = 4, stepScales4 = 4;
    public Vector2 target1, target2, target3, target4;
    public GameObject mStatueHandGive;
    public AudioSource TapSnd;
    public int press_num;
    [SerializeField] private KrestRotate krestRotate;
    public AudioSource murmur;

    private float duration = 1.2f;
    private bool isAnimated = false;

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!isAnimated)
            {
                if (press_num == 1)
                {
                    if (allowScalePressing == true)
                    {
                        TapSnd.PlayOneShot(TapSnd.clip);
                        if (switchSides1 == false)
                        {
                            if (stepScales1 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScales1 -= 1;
                            }
                            else if (stepScales1 == 3)
                            {
                                MoveToTarget(target2, duration);
                                stepScales1 -= 1;
                            }
                            else if (stepScales1 == 2)
                            {
                                MoveToTarget(target1, duration);
                                stepScales1 -= 1;
                            }
                            else if (stepScales1 == 1)
                            {
                                MoveToTarget(target2, duration);
                                stepScales1 += 1;
                                switchSides1 = true;
                            }
                        }
                        else
                        {
                            if (stepScales1 == 2)
                            {
                                MoveToTarget(target3, duration);
                                stepScales1 += 1;
                            }
                            else if (stepScales1 == 3)
                            {
                                MoveToTarget(target4, duration);
                                stepScales1 += 1;
                            }
                            else if (stepScales1 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScales1 -= 1;
                                switchSides1 = false;
                            }
                        }
                    }
                }
                if (press_num == 2)
                {
                    if (allowScalePressing == true)
                    {
                        TapSnd.PlayOneShot(TapSnd.clip);
                        if (switchSides2 == false)
                        {
                            if (stepScales2 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScales2 -= 1;
                            }
                            else if (stepScales2 == 3)
                            {
                                MoveToTarget(target2, duration);
                                stepScales2 -= 1;
                            }
                            else if (stepScales2 == 2)
                            {
                                MoveToTarget(target1, duration);
                                stepScales2 -= 1;
                            }
                            else if (stepScales2 == 1)
                            {
                                MoveToTarget(target2, duration);
                                stepScales2 += 1;
                                switchSides2 = true;
                            }
                        }
                        else
                        {
                            if (stepScales2 == 2)
                            {
                                MoveToTarget(target3, duration);
                                stepScales2 += 1;
                            }
                            else if (stepScales2 == 3)
                            {
                                MoveToTarget(target4, duration);
                                stepScales2 += 1;
                            }
                            else if (stepScales2 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScales2 -= 1;
                                switchSides2 = false;
                            }
                        }
                    }
                }
                if (press_num == 3)
                {
                    if (allowScalePressing == true)
                    {
                        TapSnd.PlayOneShot(TapSnd.clip);
                        if (switchSides3 == false)
                        {
                            if (stepScales3 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScales3 -= 1;
                            }
                            else if (stepScales3 == 3)
                            {
                                MoveToTarget(target2, duration);
                                stepScales3 -= 1;
                            }
                            else if (stepScales3 == 2)
                            {
                                MoveToTarget(target1, duration);
                                stepScales3 -= 1;
                            }
                            else if (stepScales3 == 1)
                            {
                                MoveToTarget(target2, duration);
                                stepScales3 += 1;
                                switchSides3 = true;
                            }
                        }
                        else
                        {
                            if (stepScales3 == 2)
                            {
                                MoveToTarget(target3, duration);
                                stepScales3 += 1;
                            }
                            else if (stepScales3 == 3)
                            {
                                MoveToTarget(target4, duration);
                                stepScales3 += 1;
                            }
                            else if (stepScales3 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScales3 -= 1;
                                switchSides3 = false;
                            }
                        }
                    }
                }
                if (press_num == 4)
                {
                    if (allowScalePressing == true)
                    {
                        TapSnd.PlayOneShot(TapSnd.clip);
                        if (switchSides4 == false)
                        {
                            if (stepScales4 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScales4 -= 1;
                            }
                            else if (stepScales4 == 3)
                            {
                                MoveToTarget(target2, duration);
                                stepScales4 -= 1;
                            }
                            else if (stepScales4 == 2)
                            {
                                MoveToTarget(target1, duration);
                                stepScales4 -= 1;
                            }
                            else if (stepScales4 == 1)
                            {
                                MoveToTarget(target2, duration);
                                stepScales4 += 1;
                                switchSides4 = true;
                            }
                        }
                        else
                        {
                            if (stepScales4 == 2)
                            {
                                MoveToTarget(target3, duration);
                                stepScales4 += 1;
                            }
                            else if (stepScales4 == 3)
                            {
                                MoveToTarget(target4, duration);
                                stepScales4 += 1;
                            }
                            else if (stepScales4 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScales4 -= 1;
                                switchSides4 = false;
                            }
                        }
                    }
                }
            }
        }
    }

    private void Update()
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

        if (stepScales1 == 3 && stepScales2 == 4 && stepScales3 == 2 && stepScales4 == 1 && allowScalePressing == true)
        {
#if PLAYMARKET
            FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
            allowScalePressing = false;
            mStatueHandGive.SetActive(true);
            krestRotate.enabled = true;
            murmur.Play();
        }
    }

    public IEnumerator SmoothAnim(Vector3 targetPosition, float duration)
    {
        RectTransform rectTransform = GetComponent<RectTransform>(); // Получаем RectTransform
        Vector3 startPosition = rectTransform.anchoredPosition; // Начальная позиция
        float timeElapsed = 0f; // Временной коэффициент

        while (timeElapsed < duration)
        {
            // Увеличиваем пройденное время
            timeElapsed += Time.deltaTime;

            // Интерполируем текущую позицию на основе времени
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);

            yield return null; // Ждем следующего кадра
        }

        // Убедимся, что объект точно переместится в целевую позицию
        rectTransform.anchoredPosition = targetPosition;
        isAnimated = false;
    }

    // Пример вызова корутины
    public void MoveToTarget(Vector3 targetPosition, float duration)
    {
        isAnimated = true;
        StartCoroutine(SmoothAnim(targetPosition, duration)); // Запускаем корутину
    }
}
