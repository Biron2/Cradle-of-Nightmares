using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaShake : MonoBehaviour
{
    // Параметры тряски
    public float shakeDuration = 1f; // Длительность тряски
    public float shakeAmount = 0.1f; // Интенсивность тряски

    private Vector2 initialPosition; // Начальная позиция объекта
    private float currentShakeDuration; // Текущая длительность тряски
    bool oncePress = true, tum = false, stopMiniGame = false;
    public GameObject banka, robotHead;
    public AudioSource urnShakeSnd;
    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (oncePress == true)
            {
                urnShakeSnd.Play();
                currentShakeDuration = shakeDuration;
                oncePress = false;
                tum = true;
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

        if (stopMiniGame == false)
        {
            if (currentShakeDuration > 0)
            {
                // Генерация случайного смещения по осям
                Vector2 randomOffset = Random.insideUnitSphere * shakeAmount;

                // Применение смещения к начальной позиции
                transform.localPosition = initialPosition + randomOffset;

                currentShakeDuration -= Time.deltaTime;
            }
            else
            {
                transform.localPosition = initialPosition;
                if (tum == true)
                {
                    Invoke(nameof(startBanka), 1);
                    tum = false;
                    stopMiniGame = true;
                }
            }
        }
    }
    private void startBanka()
    {
        banka.SetActive(true);
        robotHead.SetActive(false);
    }
}
