using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class DeadBodyZoom : MonoBehaviour
{
    [SerializeField] private Transform cameraObj;
    [SerializeField] private GameObject ARROWS, black_screen, redScreen;
    [SerializeField] private CanvasGroup InterfaceObj;
    [SerializeField] private AudioSource scream, glitch_super;
    bool onceScreamer = true;
    bool shakeAllow = false;
    // Параметры тряски
    public float shakeDuration = 0.5f;    // Продолжительность тряски
    public float shakeAmount = 0.1f;      // Амплитуда тряски
    public float shakeAmountY = 0.1f;      // Амплитуда тряски

    private Vector3 originalPosition;      // Исходная позиция объекта
    private float shakeTimer = 0f;         // Таймер тряски

    [SerializeField] private GameObject maniacMovement;
    [SerializeField] private Light2D global_light;
    public TimeManager timeManager;
    [SerializeField] private GlitchController _GlitchController;
    public void startScreamer()
    {
        if (onceScreamer == true)
        {
            black_screen.SetActive(false);
            scream.Play();
            cameraObj.position = new Vector3(38.55f, 0.5f, 0f);
            originalPosition = cameraObj.position;
            Camera.main.orthographicSize = 3.3f;
            redScreen.SetActive(true);
            InterfaceObj.alpha = 0f;
            ARROWS.SetActive(false);
            shakeAllow = true;
            onceScreamer = false;
            StartCoroutine(FinishWomanScreamer());
            StartCoroutine(Glitch_super_activate());
        }
    }
    private void Update()
    {
        if (shakeAllow == true)
        {
            // Проверяем, если таймер тряски истек, возвращаем объект на исходную позицию
            if (shakeTimer >= shakeDuration)
            {
                return;
            }

            // Генерируем случайные смещения в 2D-пространстве
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmountY * 2 - shakeAmountY;

            // Применяем случайные смещения к позиции объекта
            cameraObj.position = originalPosition + new Vector3(offsetX, offsetY, 0f);

            // Обновляем таймер тряски
            shakeTimer += Time.deltaTime;
        }
    }

    private IEnumerator FinishWomanScreamer()
    {
        yield return new WaitForSeconds(2);
        shakeAllow = false;
        redScreen.SetActive(false);
        cameraObj.position = new Vector3(35.554f, 0f, 0f);
        Camera.main.orthographicSize = 5f;
        InterfaceObj.alpha = 1f;
        ARROWS.SetActive(true);
        yield return new WaitForSeconds(1);
        timeManager.FirstCycleCoroutine = StartCoroutine(timeManager.FirstCicle());
        global_light.intensity = 0.005f;
        yield return new WaitForSeconds(5);
        maniacMovement.SetActive(true);
    }
    private IEnumerator Glitch_super_activate()
    {
        yield return new WaitForSeconds(0.2f);
        _GlitchController._SetGlitchNumbersDeadBodyZoom();
        glitch_super.Play();
    }
}