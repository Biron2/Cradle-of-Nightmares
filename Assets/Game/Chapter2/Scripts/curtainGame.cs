using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class curtainGame : MonoBehaviour
{
    private bool switchSides1 = false, switchSides2 = true, switchSides3 = false, switchSides4 = false;
    public static int stepScales1 = 4, stepScales2 = 3, stepScales3 = 1, stepScales4 = 4;
    public Vector2 target1, target2, target3, target4;
    public AudioSource TapSnd;
    public int press_num;

    private float duration = 1.3f;
    private bool isAnimated = false;
    [SerializeField] private RectTransform[] _RectTransforms;
    [SerializeField] private Image[] mNumbers;

    [SerializeField] private AudioSource winMiniGameSound;
    

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!isAnimated)
            {
                if (press_num == 1)
                {
                    if (VariableDeterminator.allowCurtainGame == true)
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
                    if (VariableDeterminator.allowCurtainGame == true)
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
                    if (VariableDeterminator.allowCurtainGame == true)
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
                    if (VariableDeterminator.allowCurtainGame == true)
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

        if (stepScales1 == 2 && stepScales2 == 1 && stepScales3 == 4 && stepScales4 == 3 && VariableDeterminator.allowCurtainGame == true)
        {
            winMiniGameSound.PlayOneShot(winMiniGameSound.clip);
            VariableDeterminator.allowCurtainGame = false;
            SmoothAnimFINISH_Activator();

#if PLAYMARKET
            FindAnyObjectByType<YandexAds>()?.ShowInterstitial();
#endif
        }
    }
    public void SmoothAnimFINISH_Activator()
    {
        if (!VariableDeterminator.currentTimeIsNight)
        {
            foreach (RectTransform rct in _RectTransforms)
            {
                StartCoroutine(SmoothAnimFINISH(rct, 3f, true)); // Запускаем корутину
            }
        }
    }
    public void SmoothAnimFINISH_Deactivator()
    {
        if (!VariableDeterminator.currentTimeIsNight)
        {
            foreach (RectTransform rct in _RectTransforms)
            {
                StartCoroutine(SmoothAnimFINISH(rct, 3f, false)); // Запускаем корутину
            }
        }
    }
    private IEnumerator SmoothAnimFINISH(RectTransform rectTransform, float duration, bool isEnable)
    {
        yield return new WaitForSeconds(2f);

        Vector3 startPosition = rectTransform.anchoredPosition;
        Vector3 targetPosition = new Vector3(rectTransform.anchoredPosition.x, 269.7f, rectTransform.anchoredPosition.y);
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        yield return new WaitForSeconds(1f);

        // Запоминаем начальные альфа-значения для всех Image
        float[] startAlphas = new float[mNumbers.Length];
        Color[] startColors = new Color[mNumbers.Length];
        for (int i = 0; i < mNumbers.Length; i++)
        {
            startColors[i] = mNumbers[i].color;
            startAlphas[i] = startColors[i].a;
        }

        float timeElapsed2 = 0f;
        float targetAlpha = isEnable ? 1f : 0f;

        // Меняем прозрачность всех одновременно
        while (timeElapsed2 < duration)
        {
            timeElapsed2 += Time.deltaTime;
            float t = timeElapsed2 / duration;

            for (int i = 0; i < mNumbers.Length; i++)
            {
                float alpha = Mathf.Lerp(startAlphas[i], targetAlpha, t);
                mNumbers[i].color = new Color(startColors[i].r, startColors[i].g, startColors[i].b, alpha);
            }

            yield return null;
        }

        // Установим финальное значение альфы
        for (int i = 0; i < mNumbers.Length; i++)
        {
            mNumbers[i].color = new Color(startColors[i].r, startColors[i].g, startColors[i].b, targetAlpha);
        }
    }
    private IEnumerator SmoothAnim(Vector3 targetPosition, float duration)
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
