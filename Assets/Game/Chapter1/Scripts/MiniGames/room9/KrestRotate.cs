using UnityEngine;

public class KrestRotate : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float shakeIntensity = 0.1f;

    private Quaternion targetRotation;
    private Vector3 shakeOffset;
    [SerializeField] private GlitchController _GlitchController;
    private void Start()
    {
        _GlitchController._SetGlitchNumbersKrest();
        targetRotation = Quaternion.Euler(0f, 0f, 180f);
    }

    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) <= 0.01f)
        {
            targetRotation = Quaternion.Euler(0f, 0f, 180f);
        }

        shakeOffset = new Vector3(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity), 0f);
        transform.localPosition += shakeOffset;
    }
}