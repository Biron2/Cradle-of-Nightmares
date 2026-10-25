using System.Collections;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    private GameObject targetObject;
    [SerializeField] float expandDuration = 1.0f;
    private float currentTime = 0f;
    [SerializeField] public Vector3 breatheIn = new Vector3(0.990f, 1.0020f, 0f);
    [SerializeField] public Vector3 breatheOut = new Vector3(1.010f, 0.9980f, 0f);
    private bool breathingIn = true;
    [SerializeField] bool pulsing = false;
    public float waitForBreathing = 0f;
    public bool isInverse = false;

    private IEnumerator Start()
    {
        targetObject = this.gameObject;

        // Установить начальное направление дыхания в зависимости от isInverse
        breathingIn = !isInverse;

        yield return new WaitForSeconds(waitForBreathing);
        pulsing = true;
    }

    private void Update()
    {
        PulseUpdate();
    }

    private void PulseUpdate()
    {
        if (pulsing)
        {
            Vector3 targetScale = breathingIn ? breatheIn : breatheOut;
            Vector3 startScale = breathingIn ? breatheOut : breatheIn;
            currentTime += Time.deltaTime;
            float lerpFactor = currentTime / expandDuration;
            targetObject.transform.localScale = Vector3.Lerp(startScale, targetScale, lerpFactor);
            if (lerpFactor >= 1.0f)
            {
                breathingIn = !breathingIn;
                currentTime = 0f;
            }
        }
    }
}