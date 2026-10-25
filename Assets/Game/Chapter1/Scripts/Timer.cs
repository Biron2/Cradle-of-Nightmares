using UnityEngine;

public class Timer : MonoBehaviour
{
    private float elapsedTime = 0f; // Прошедшее время в секундах
    private bool isRunning = false; // Состояние таймера
    public bool isSkiped = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        isRunning = true;
    }

    // Метод для остановки таймера
    public void StopTimer()
    {
        isRunning = false;
    }

    // Метод для сброса таймера
    public void ResetTimer()
    {
        elapsedTime = 0f;
    }
    public int GetIntTime()
    {
        int intager_seconds = Mathf.FloorToInt(elapsedTime);
        return intager_seconds;
    }

    // Обновление таймера каждый кадр
    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }
}
