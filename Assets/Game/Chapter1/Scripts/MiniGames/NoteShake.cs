using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteShake : MonoBehaviour
{
    // Параметры тряски
    public float shakeDuration = 1f; // Длительность тряски
    public float shakeAmount = 0.1f; // Интенсивность тряски

    private Vector2 initialPosition; // Начальная позиция объекта
    private float currentShakeDuration; // Текущая длительность тряски
    private void Start()
    {
        initialPosition = transform.localPosition;
        currentShakeDuration = shakeDuration;
    }
    private void Update()
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
        }
    }
}
