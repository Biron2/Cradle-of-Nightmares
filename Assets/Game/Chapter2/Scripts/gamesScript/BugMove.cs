using System;
using System.Collections;
using UnityEngine;

public class BugMove : MonoBehaviour
{
    [SerializeField] private float circleRadius = 100f; // Радиус круга
    [SerializeField] private float moveSpeed = 2f; // Скорость движения
    [SerializeField] private float rotationSmoothness = 5f; // Плавность поворота
    [SerializeField] private float stepHeight = 10f; // Высота шага
    [SerializeField] private float stepSpeed = 5f; // Скорость шага

    private RectTransform rectTransform;
    private float angle = 0f; // Текущий угол на окружности
    private float stepOffset = 0f; // Смещение для анимации шага
    private Vector3 targetDirection; // Направление движения
    private bool allowRun = true;
    private ParticleSystem sprayEffectObj;
    private AudioSource cockroach_stepSound;
    private Animator cockroach_runAnim;

    private void Start()
    {
        cockroach_stepSound = GetComponent<AudioSource>();
        cockroach_runAnim = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        sprayEffectObj = transform.Find("spray_effect").GetComponent<ParticleSystem>();
    }

    public void StopRunning()
    {
        cockroach_runAnim.enabled = false;
        sprayEffectObj.gameObject.SetActive(true);
        allowRun = false;
        StartCoroutine(StopEffectInSomeMS());
        StartCoroutine(FadeToAudio(0f, 0.2f));
    }

    private IEnumerator FadeToAudio(float targetAlpha, float duration)
    {
        float startAlpha = cockroach_stepSound.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            cockroach_stepSound.volume = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        cockroach_stepSound.volume = targetAlpha; // Убедитесь, что мы устанавливаем конечное значение
    }
    private IEnumerator StopEffectInSomeMS()
    {
        yield return new WaitForSeconds(0.6f);
        sprayEffectObj.Stop();
    }

    private void Update()
    {
        if (allowRun)
        {
            MoveInCircle();
            AnimateStep();
        }
    }

    private void MoveInCircle()
    {
        // Увеличиваем угол в зависимости от времени и скорости
        angle += moveSpeed * Time.deltaTime;

        // Вычисляем новую позицию на окружности
        float x = Mathf.Cos(angle) * circleRadius;
        float y = Mathf.Sin(angle) * circleRadius;

        // Устанавливаем новую позицию
        rectTransform.anchoredPosition = new Vector2(x, y);

        // Вычисляем направление движения (касательная к окружности)
        targetDirection = new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle)).normalized;

        // Плавно поворачиваем таракана в направлении движения
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        rectTransform.localRotation = Quaternion.Slerp(rectTransform.localRotation, targetRotation, rotationSmoothness * Time.deltaTime);
    }

    private void AnimateStep()
    {
        // Анимация шага с использованием синусоиды
        stepOffset = Mathf.Sin(Time.time * stepSpeed) * stepHeight;

        // Применяем смещение для создания эффекта шага
        rectTransform.anchoredPosition += new Vector2(0, stepOffset) * Time.deltaTime;
    }
}