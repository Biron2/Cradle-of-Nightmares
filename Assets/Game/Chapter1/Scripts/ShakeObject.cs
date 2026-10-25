using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public float shakeDuration = 1f;
    public float shakeAmount = 0.1f;

    private Vector2 initialPosition;
    private float currentShakeDuration;

    private void Start()
    {
        initialPosition = transform.localPosition;
        currentShakeDuration = shakeDuration;
    }

    private void Update()
    {
        if (shakeDuration > 0 || currentShakeDuration <= 0)
        {
            Vector2 randomOffset = Random.insideUnitSphere * shakeAmount;
            transform.localPosition = initialPosition + randomOffset;

            currentShakeDuration -= Time.deltaTime;

            if (currentShakeDuration <= 0)
            {
                currentShakeDuration = shakeDuration;
            }
        }
    }
}