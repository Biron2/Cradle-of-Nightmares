using UnityEngine;

public class carShaking : MonoBehaviour
{
    [SerializeField] private GameObject carObject, carInsideObject;
    [SerializeField] private float shakeIntensityOutside = 0.02f;  // тряска для внешнего вида
    [SerializeField] private float shakeIntensityInside = 0.05f;   // тряска для салона

    private Vector3 carOriginalPos, insideOriginalPos;
    private bool isShaking = false;

    void Start()
    {
        if (carObject != null) carOriginalPos = carObject.transform.localPosition;
        if (carInsideObject != null) insideOriginalPos = carInsideObject.transform.localPosition;
    }

    void Update()
    {
        if (isShaking)
        {
            if (carObject != null)
            {
                carObject.transform.localPosition = carOriginalPos + (Vector3)(Random.insideUnitCircle * shakeIntensityOutside);
            }

            if (carInsideObject != null)
            {
                carInsideObject.transform.localPosition = insideOriginalPos + (Vector3)(Random.insideUnitCircle * shakeIntensityInside);
            }
        }
    }

    public void startShaking()
    {
        isShaking = true;
    }

    public void stopShaking()
    {
        isShaking = false;
        if (carObject != null) carObject.transform.localPosition = carOriginalPos;
        if (carInsideObject != null) carInsideObject.transform.localPosition = insideOriginalPos;
    }
}
