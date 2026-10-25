using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class scalesGameFaces : MonoBehaviour
{
    private bool switchSides1 = false, switchSides2 = false, switchSides3 = false, switchSides4 = false, switchSides5 = false;
    public static int stepScaless1 = 2, stepScaless2 = 3, stepScaless3 = 1, stepScaless4 = 3, stepScaless5 = 4;
    public Vector2 target1, target2, target3, target4, target5;
    public static bool allowScalePressing = true;

    public AudioSource verevkaSnd;
    public float speed = 1f;

    public int num_press;

    [SerializeField] private AudioSource krik1, krik2, krik3, krik4, krik5;
    [SerializeField] private RectTransform rectFace1, rectFace2, rectFace3, rectFace4, rectFace5;
    [SerializeField] private RectTransform pianoNote1, pianoNote2, pianoNote3, pianoNote4, pianoNote5;
    // Параметры тряски
    public float shakeDuration = 1f; // Длительность тряски
    public float shakeAmount = 0.1f; // Интенсивность тряски

    private Vector2 initialPosition1; // Начальная позиция объекта
    private Vector2 initialPosition2; // Начальная позиция объекта
    private Vector2 initialPosition3; // Начальная позиция объекта
    private Vector2 initialPosition4; // Начальная позиция объекта
    private Vector2 initialPosition5; // Начальная позиция объекта
    private float currentShakeDuration1; // Текущая длительность тряски
    private float currentShakeDuration2; // Текущая длительность тряски
    private float currentShakeDuration3; // Текущая длительность тряски
    private float currentShakeDuration4; // Текущая длительность тряски
    private float currentShakeDuration5; // Текущая длительность тряски

    public VariableDeterminator variableDeterminator;
    public Coroutine karkarCoroutine;
    [SerializeField] private GameObject _tip3;

    private float duration = 0.8f;
    private bool isAnimated = false;

    [SerializeField] private AudioSource winMiniGameSound;
    private void Start()
    {
        allowScalePressing = true;
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!isAnimated)
            {
                if (num_press == 1)
                {
                    if (allowScalePressing == true)
                    {
                        verevkaSnd.PlayOneShot(verevkaSnd.clip);
                        if (switchSides1 == false)
                        {
                            if (stepScaless1 == 1)
                            {
                                MoveToTarget(target2, duration);
                                stepScaless1 += 1;
                            }
                            else if (stepScaless1 == 2)
                            {
                                MoveToTarget(target3, duration);
                                stepScaless1 += 1;
                            }
                            else if (stepScaless1 == 3)
                            {
                                MoveToTarget(target4, duration);
                                stepScaless1 += 1;
                            }
                            else if (stepScaless1 == 4)
                            {
                                MoveToTarget(target5, duration);
                                stepScaless1 += 1;
                                switchSides1 = true;
                            }
                        }
                        else
                        {
                            if (stepScaless1 == 5)
                            {
                                MoveToTarget(target4, duration);
                                stepScaless1 -= 1;
                            }
                            else if (stepScaless1 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScaless1 -= 1;
                            }
                            else if (stepScaless1 == 3)
                            {
                                MoveToTarget(target2, duration);
                                stepScaless1 -= 1;
                            }
                            else if (stepScaless1 == 2)
                            {
                                MoveToTarget(target1, duration);
                                stepScaless1 -= 1;
                                switchSides1 = false;
                            }
                        }
                    }
                }
                if (num_press == 2)
                {
                    if (allowScalePressing == true)
                    {
                        verevkaSnd.PlayOneShot(verevkaSnd.clip);
                        if (switchSides2 == false)
                        {
                            if (stepScaless2 == 1)
                            {
                                MoveToTarget(target2, duration);
                                stepScaless2 += 1;
                            }
                            else if (stepScaless2 == 2)
                            {
                                MoveToTarget(target3, duration);
                                stepScaless2 += 1;
                            }
                            else if (stepScaless2 == 3)
                            {
                                MoveToTarget(target4, duration);
                                stepScaless2 += 1;
                            }
                            else if (stepScaless2 == 4)
                            {
                                MoveToTarget(target5, duration);
                                stepScaless2 += 1;
                                switchSides2 = true;
                            }
                        }
                        else
                        {
                            if (stepScaless2 == 5)
                            {
                                MoveToTarget(target4, duration);
                                stepScaless2 -= 1;
                            }
                            else if (stepScaless2 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScaless2 -= 1;
                            }
                            else if (stepScaless2 == 3)
                            {
                                MoveToTarget(target2, duration);
                                stepScaless2 -= 1;
                            }
                            else if (stepScaless2 == 2)
                            {
                                MoveToTarget(target1, duration);
                                stepScaless2 -= 1;
                                switchSides2 = false;
                            }
                        }
                    }
                }
                if (num_press == 3)
                {
                    if (allowScalePressing == true)
                    {
                        verevkaSnd.PlayOneShot(verevkaSnd.clip);
                        if (switchSides3 == false)
                        {
                            if (stepScaless3 == 1)
                            {
                                MoveToTarget(target2, duration);
                                stepScaless3 += 1;
                            }
                            else if (stepScaless3 == 2)
                            {
                                MoveToTarget(target3, duration);
                                stepScaless3 += 1;
                            }
                            else if (stepScaless3 == 3)
                            {
                                MoveToTarget(target4, duration);
                                stepScaless3 += 1;
                            }
                            else if (stepScaless3 == 4)
                            {
                                MoveToTarget(target5, duration);
                                stepScaless3 += 1;
                                switchSides3 = true;
                            }
                        }
                        else
                        {
                            if (stepScaless3 == 5)
                            {
                                MoveToTarget(target4, duration);
                                stepScaless3 -= 1;
                            }
                            else if (stepScaless3 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScaless3 -= 1;
                            }
                            else if (stepScaless3 == 3)
                            {
                                MoveToTarget(target2, duration);
                                stepScaless3 -= 1;
                            }
                            else if (stepScaless3 == 2)
                            {
                                MoveToTarget(target1, duration);
                                stepScaless3 -= 1;
                                switchSides3 = false;
                            }
                        }
                    }
                }
                if (num_press == 4)
                {
                    if (allowScalePressing == true)
                    {
                        verevkaSnd.PlayOneShot(verevkaSnd.clip);
                        if (switchSides4 == false)
                        {
                            if (stepScaless4 == 1)
                            {
                                MoveToTarget(target2, duration);
                                stepScaless4 += 1;
                            }
                            else if (stepScaless4 == 2)
                            {
                                MoveToTarget(target3, duration);
                                stepScaless4 += 1;
                            }
                            else if (stepScaless4 == 3)
                            {
                                MoveToTarget(target4, duration);
                                stepScaless4 += 1;
                            }
                            else if (stepScaless4 == 4)
                            {
                                MoveToTarget(target5, duration);
                                stepScaless4 += 1;
                                switchSides4 = true;
                            }
                        }
                        else
                        {
                            if (stepScaless4 == 5)
                            {
                                MoveToTarget(target4, duration);
                                stepScaless4 -= 1;
                            }
                            else if (stepScaless4 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScaless4 -= 1;
                            }
                            else if (stepScaless4 == 3)
                            {
                                MoveToTarget(target2, duration);
                                stepScaless4 -= 1;
                            }
                            else if (stepScaless4 == 2)
                            {
                                MoveToTarget(target1, duration);
                                stepScaless4 -= 1;
                                switchSides4 = false;
                            }
                        }
                    }
                }
                if (num_press == 5)
                {
                    if (allowScalePressing == true)
                    {
                        verevkaSnd.PlayOneShot(verevkaSnd.clip);
                        if (switchSides5 == false)
                        {
                            if (stepScaless5 == 1)
                            {
                                MoveToTarget(target2, duration);
                                stepScaless5 += 1;
                            }
                            else if (stepScaless5 == 2)
                            {
                                MoveToTarget(target3, duration);
                                stepScaless5 += 1;
                            }
                            else if (stepScaless5 == 3)
                            {
                                MoveToTarget(target4, duration);
                                stepScaless5 += 1;
                            }
                            else if (stepScaless5 == 4)
                            {
                                MoveToTarget(target5, duration);
                                stepScaless5 += 1;
                                switchSides5 = true;
                            }
                        }
                        else
                        {
                            if (stepScaless5 == 5)
                            {
                                MoveToTarget(target4, duration);
                                stepScaless5 -= 1;
                            }
                            else if (stepScaless5 == 4)
                            {
                                MoveToTarget(target3, duration);
                                stepScaless5 -= 1;
                            }
                            else if (stepScaless5 == 3)
                            {
                                MoveToTarget(target2, duration);
                                stepScaless5 -= 1;
                            }
                            else if (stepScaless5 == 2)
                            {
                                MoveToTarget(target1, duration);
                                stepScaless5 -= 1;
                                switchSides5 = false;
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

        if (stepScaless1 == 3 && stepScaless2 == 4 && stepScaless3 == 2 && stepScaless4 == 1 && stepScaless5 == 5 && allowScalePressing == true)
        {
#if PLAYMARKET
            FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
            winMiniGameSound.PlayOneShot(winMiniGameSound.clip);
            _tip3.SetActive(false);
            allowScalePressing = false;
            karkarCoroutine = StartCoroutine(startKrikMethod());
        }

        if (currentShakeDuration1 > 0)
        {
            Vector2 randomOffset = Random.insideUnitSphere * shakeAmount;
            rectFace1.anchoredPosition = initialPosition1 + randomOffset;

            currentShakeDuration1 -= Time.deltaTime;
        }
        if (currentShakeDuration2 > 0)
        {
            Vector2 randomOffset = Random.insideUnitSphere * shakeAmount;
            rectFace2.anchoredPosition = initialPosition2 + randomOffset;

            currentShakeDuration2 -= Time.deltaTime;
        }
        if (currentShakeDuration3 > 0)
        {
            Vector2 randomOffset = Random.insideUnitSphere * shakeAmount;
            rectFace3.anchoredPosition = initialPosition3 + randomOffset;

            currentShakeDuration3 -= Time.deltaTime;
        }
        if (currentShakeDuration4 > 0)
        {
            Vector2 randomOffset = Random.insideUnitSphere * shakeAmount;
            rectFace4.anchoredPosition = initialPosition4 + randomOffset;

            currentShakeDuration4 -= Time.deltaTime;
        }
        if (currentShakeDuration5 > 0)
        {
            Vector2 randomOffset = Random.insideUnitSphere * shakeAmount;
            rectFace5.anchoredPosition = initialPosition5 + randomOffset;

            currentShakeDuration5 -= Time.deltaTime;
        }
    }

    public void startKrikIf()
    {
        if (allowScalePressing == false)
        {
            variableDeterminator.faceTalking = true;
            karkarCoroutine = StartCoroutine(startKrikMethod());
        }
    }

    public IEnumerator startKrikMethod()
    {
        variableDeterminator.faceTalking = true;
        while (true)
        {
            initialPosition1 = rectFace1.anchoredPosition;
            initialPosition2 = rectFace2.anchoredPosition;
            initialPosition3 = rectFace3.anchoredPosition;
            initialPosition4 = rectFace4.anchoredPosition;
            initialPosition5 = rectFace5.anchoredPosition;
            yield return new WaitForSeconds(1f);
            krik1.Play();
            currentShakeDuration1 = shakeDuration;

            pianoNote1.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.gameObject.SetActive(false);
            pianoNote1.localScale = new Vector3(1f, 1f, 1f);

            yield return new WaitForSeconds(0.5f);
            currentShakeDuration1 = shakeDuration;
            krik1.Play();

            pianoNote1.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.gameObject.SetActive(false);
            pianoNote1.localScale = new Vector3(1f, 1f, 1f);

            yield return new WaitForSeconds(0.5f);
            currentShakeDuration1 = shakeDuration;
            krik1.Play();

            pianoNote1.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote1.gameObject.SetActive(false);
            pianoNote1.localScale = new Vector3(1f, 1f, 1f);

            yield return new WaitForSeconds(0.5f);
            currentShakeDuration2 = shakeDuration;
            krik2.Play();

            pianoNote2.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.gameObject.SetActive(false);
            pianoNote2.localScale = new Vector3(1f, 1f, 1f);

            yield return new WaitForSeconds(0.5f);
            currentShakeDuration2 = shakeDuration;
            krik2.Play();

            pianoNote2.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote2.gameObject.SetActive(false);
            pianoNote2.localScale = new Vector3(1f, 1f, 1f);

            yield return new WaitForSeconds(0.5f);
            currentShakeDuration3 = shakeDuration;
            krik3.Play();

            pianoNote3.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.075f);
            pianoNote3.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote3.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            yield return new WaitForSeconds(0.075f);
            pianoNote3.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            yield return new WaitForSeconds(0.075f);
            pianoNote3.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            yield return new WaitForSeconds(0.075f);
            pianoNote3.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote3.gameObject.SetActive(false);
            pianoNote3.localScale = new Vector3(1f, 1f, 1f);

            yield return new WaitForSeconds(0.5f);
            currentShakeDuration4 = shakeDuration;
            krik4.Play();

            pianoNote4.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.075f);
            pianoNote4.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote4.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            yield return new WaitForSeconds(0.075f);
            pianoNote4.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            yield return new WaitForSeconds(0.075f);
            pianoNote4.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            yield return new WaitForSeconds(0.075f);
            pianoNote4.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote4.gameObject.SetActive(false);
            pianoNote4.localScale = new Vector3(1f, 1f, 1f);

            yield return new WaitForSeconds(0.5f);
            currentShakeDuration5 = shakeDuration;
            krik5.Play();

            pianoNote5.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.gameObject.SetActive(false);
            pianoNote5.localScale = new Vector3(1f, 1f, 1f);

            yield return new WaitForSeconds(0.5f);
            currentShakeDuration5 = shakeDuration;
            krik5.Play();

            pianoNote5.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.gameObject.SetActive(false);
            pianoNote5.localScale = new Vector3(1f, 1f, 1f);

            yield return new WaitForSeconds(0.5f);
            currentShakeDuration5 = shakeDuration;
            krik5.Play();

            pianoNote5.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSeconds(0.075f);
            pianoNote5.gameObject.SetActive(false);
            pianoNote5.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    public void StopKarkarCoroutineMethod()
    {
        pianoNote1.gameObject.SetActive(false);
        pianoNote1.localScale = new Vector3(1f, 1f, 1f);
        pianoNote2.gameObject.SetActive(false);
        pianoNote2.localScale = new Vector3(1f, 1f, 1f);
        pianoNote3.gameObject.SetActive(false);
        pianoNote3.localScale = new Vector3(1f, 1f, 1f);
        pianoNote4.gameObject.SetActive(false);
        pianoNote4.localScale = new Vector3(1f, 1f, 1f);
        pianoNote5.gameObject.SetActive(false);
        pianoNote5.localScale = new Vector3(1f, 1f, 1f);

        if (karkarCoroutine != null)
        {
            StopCoroutine(karkarCoroutine);
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
