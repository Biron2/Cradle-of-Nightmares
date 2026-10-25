using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class podushkaHand : MonoBehaviour
{
    private RectTransform rectTransform;
    public Vector3 targetPosition;
    public float translationSpeed = 1f;
    public AudioSource klyaksa;

    private bool achiveCoo = false;
    void Start()
    {
        klyaksa.Play();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (achiveCoo == false)
            {
                rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, targetPosition, Time.deltaTime * translationSpeed);
                if (rectTransform.localPosition == targetPosition)
                {
                    achiveCoo = true;
                }
            }
        }
    }
}
