using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class BalloonSize : MonoBehaviour
{
    public float growthDuration = 1.0f; // Время увеличения
    public float pulseSpeed = 2.0f; // Скорость пульсации
    public float pulseAmount = 0.05f; // Амплитуда пульсации
    public float explosionScale = 2.5f; // Размер при взрыве
    public float explosionDuration = 0.1f; // Длительность взрыва
    public float returnDuration = 0.1f; // Длительность возврата

    private Vector3 targetScale = Vector3.one;
    private float elapsedTime = 0f;
    private bool isGrown = false;
    public Sprite burstBalloon;
    private bool allowPress = true;
    public bool allowDelete = true;

    private AudioSource inflateSound;
    private AudioSource popSound;
    private Coroutine mCoroutine;
    private BalloonSpawner _BalloonSpawner;
     
    void Start()
    {
        _BalloonSpawner = GameObject.Find("Interface/GeneralGames/clownBalloonsGame/balloonsParent").GetComponent<BalloonSpawner>();
        transform.localScale = Vector3.zero; // Начальный размер (скрыт)

        inflateSound = GameObject.Find("Other/GameManager/EtcSounds/balloon_increase").GetComponent<AudioSource>();
        popSound = GameObject.Find("Other/GameManager/EtcSounds/balloon_pop").GetComponent<AudioSource>();

        if (_BalloonSpawner.maxReachBalloons != 9)
        {
            inflateSound.PlayOneShot(inflateSound.clip);
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.45f);
        allowPress = false;
        float fadeDuration = 0.75f; // Длительность исчезновения
        Vector3 startScale = transform.localScale;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);
            yield return null;
        }

        transform.localScale = Vector3.zero; // Гарантируем, что размер стал 0
        Destroy(gameObject);
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (allowPress)
            {
                _BalloonSpawner.UpdateBalloonsInt();
                // Получаем родительский объект
                Transform parentObject = transform.parent;

                if (parentObject != null)
                {
                    // Преобразуем в RectTransform
                    RectTransform parentRect = parentObject.GetComponent<RectTransform>();

                    if (parentRect != null && _BalloonSpawner.spawnPoints.Contains(parentRect))
                    {
                        // Удаляем объект из списка
                        _BalloonSpawner.spawnPoints.Remove(parentRect);
                    }
                }

                if (mCoroutine != null)
                {
                    StopCoroutine(mCoroutine);
                }
                allowPress = false;
                allowDelete = false;
                GetComponent<Image>().sprite = burstBalloon;

                // Останавливаем надувание
                if (inflateSound != null) inflateSound.Stop();

                // Воспроизводим звук лопания
                if (popSound != null) popSound.PlayOneShot(popSound.clip);

                // Запускаем анимацию взрыва
                if (_BalloonSpawner.maxReachBalloons != 9)
                {
                    StartCoroutine(ExplodeBalloon());
                }
            }
        }
    }

    private IEnumerator ExplodeBalloon()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 explosionScaleVector = originalScale * explosionScale;

        float timer = 0f;
        while (timer < explosionDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / explosionDuration;
            transform.localScale = Vector3.Lerp(originalScale, explosionScaleVector, progress);
            yield return null;
        }

        timer = 0f;
        while (timer < returnDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / returnDuration;
            transform.localScale = Vector3.Lerp(explosionScaleVector, Vector3.one, progress);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        // Падение вниз перед уничтожением
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.down * 10f; // Летит вниз на 10 единиц
        float fallDuration = 1f; // Очень быстро (0.3 секунды)
        timer = 0f;

        while (timer < fallDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fallDuration;
            transform.position = Vector3.Lerp(startPos, endPos, progress);
            yield return null;
        }

        Destroy(gameObject);
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

        if (!isGrown)
        {
            // Рост с ускорением в начале и замедлением в конце
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / growthDuration);
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, smoothProgress);

            if (progress >= 1f)
            {
                isGrown = true;
                // **Запускаем FadeOut только если allowPress не был отключен (т.е. шарик не взорван)**
                if (allowPress)
                {
                    mCoroutine = StartCoroutine(FadeOut());
                }
            }
        }
        else
        {
            if (allowPress)
            {
                // Пульсация после роста (если не взорван)
                float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
                transform.localScale = targetScale * (1f + pulse);
            }
        }
    }
}