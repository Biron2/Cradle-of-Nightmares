using UnityEngine;

public class ArrowClockRound : MonoBehaviour
{
    public Transform minuteHand;  // секундная стрелка (рывками)
    public Transform hourHand;    // минутная стрелка (плавно)
    [SerializeField] private AudioSource tickSound;
    [SerializeField] private AudioClip tick1, tick2;

    private float tickInterval = 1f;  // шаг секундной стрелки (раз в 1 сек)
    private float tickTimer = 0f;

    private int currentSecond = 0;
    private float minuteTime = 0f; // виртуальные минуты (будут расти со временем)
    private bool useFirstTick = true;

    void Start()
    {
        // Установка начального положения стрелок — 90 по Z
        minuteHand.localRotation = Quaternion.Euler(0, 0, 90f);
        hourHand.localRotation = Quaternion.Euler(0, 0, 90f);
    }

    void Update()
    {
        // --- ПЛАВНОЕ движение минутной стрелки (ускорение ×6)
        minuteTime += Time.deltaTime * 5f; // каждая секунда = 6 минут
        float minuteAngle = 90f - (minuteTime * 5f); // 6° на минуту
        hourHand.localRotation = Quaternion.Euler(0, 0, minuteAngle);

        // --- ТИК для секундной стрелки (рывками)
        tickTimer += Time.deltaTime;
        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;
            currentSecond = (currentSecond + 1) % 60;

            float secondAngle = 90f - (currentSecond * 6f); // 6° на секунду
            minuteHand.localRotation = Quaternion.Euler(0, 0, secondAngle);

            if (tickSound != null)
            {
                tickSound.PlayOneShot(useFirstTick ? tick1 : tick2);
                useFirstTick = !useFirstTick;
            }
        }
    }
}