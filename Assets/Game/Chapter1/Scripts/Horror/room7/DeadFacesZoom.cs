using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DeadFacesZoom : MonoBehaviour
{
    [SerializeField] private Transform cameraObj;
    [SerializeField] private GameObject ARROWS, black_screen, redScreen;
    [SerializeField] private CanvasGroup InterfaceObj;
    [SerializeField] private AudioSource scream;
    bool onceScreamer = true;
    bool shakeAllow = false;
    // Параметры тряски
    public float shakeDuration = 0.5f;    // Продолжительность тряски
    public float shakeAmount = 0.1f;      // Амплитуда тряски
    public float shakeAmountY = 0.1f;      // Амплитуда тряски

    private Vector3 originalPosition;      // Исходная позиция объекта
    private float shakeTimer = 0f;         // Таймер тряски
    [SerializeField] private Material LightMaterial;
    [SerializeField] private Image face1, face2, face3, face4, face5;
    [SerializeField] private Image verevka1, verevka2, verevka3, verevka4, verevka5;
    [SerializeField] private GlitchController _GlitchController;

    public void startScreamer()
    {
        if (onceScreamer == true)
        {
            _GlitchController._SetGlitchNumbersFacesLines();
            black_screen.SetActive(false);
            scream.Play();
            cameraObj.position = new Vector3(-14.29f, 12.5f, 0f);
            originalPosition = cameraObj.position;
            Camera.main.orthographicSize = 2;
            redScreen.SetActive(true);
            InterfaceObj.alpha = 0f;
            ARROWS.SetActive(false);
            shakeAllow = true;
            onceScreamer = false;
            Invoke(nameof(FinishWomanScreamer), 2);
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

    private void FinishWomanScreamer()
    {
        _GlitchController._SetDefaultGlitchNumbers();
        face1.material = LightMaterial;
        face2.material = LightMaterial;
        face3.material = LightMaterial;
        face4.material = LightMaterial;
        face5.material = LightMaterial;
        verevka1.material = LightMaterial;
        verevka2.material = LightMaterial;
        verevka3.material = LightMaterial;
        verevka4.material = LightMaterial;
        verevka5.material = LightMaterial;

        shakeAllow = false;
        redScreen.SetActive(false);
        cameraObj.position = new Vector3(-17.777f, 10f, 0f);
        Camera.main.orthographicSize = 5f;
        InterfaceObj.alpha = 1f;
        ARROWS.SetActive(true);
    }
}