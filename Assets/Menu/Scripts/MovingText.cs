using UnityEngine;

public class MovingText : MonoBehaviour
{
    public float speed = 1.0f; // Скорость движения текста
    public float radius = 2.0f; // Радиус окружности, по которой будет двигаться текст

    private Vector3 startPosition;
    private float timeOffset;

    void Start()
    {
        startPosition = transform.position;
        timeOffset = Random.Range(0f, 2f * Mathf.PI); // Получаем случайное смещение времени
    }

    void Update()
    {
        float x = Mathf.Sin((Time.time + timeOffset) * speed) * radius; // Рассчитываем смещение по оси X
        float y = Mathf.Cos((Time.time + timeOffset) * speed) * radius; // Рассчитываем смещение по оси Y

        transform.position = startPosition + new Vector3(x, y, 0); // Применяем смещение к позиции текста
    }
}