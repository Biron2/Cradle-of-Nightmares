using System.Collections;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    public Material mat;
    private Coroutine coroutine, coroutine2, coroutine3;
    public bool isGlitchActive = false;
    [SerializeField] private AudioSource glitchSnd, deadManiacGlitch, glitchKrestSnd;
    [SerializeField] private AudioClip[] clips;
    public float fadeDuration = 2f; // Продолжительность затухания
    private void Start()
    {
        mat.SetFloat("_NoiseAmount", 0f);
        mat.SetFloat("_GlitchStrength", 0f);
        mat.SetFloat("_ScanLineStrenght", 1f);
    }
    public void _SetDefaultGlitchNumbers()
    {
        StartCoroutine(FadeOut());
        isGlitchActive = false;
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        if (coroutine2 != null)
        {
            StopCoroutine(coroutine2);
        }
        mat.SetFloat("_NoiseAmount", 0f);
        mat.SetFloat("_GlitchStrength", 0f);
        mat.SetFloat("_ScanLineStrenght", 1f);
    }
    public void defaultKrestNumbers()
    {
        if (coroutine3 != null)
        {
            StopCoroutine(coroutine3);
        }
    }
    public void _SetGlitchNumbersFacesLines()
    {
        mat.SetFloat("_GlitchStrength", 100f);
        mat.SetFloat("_ScanLineStrenght", 0f);
        isGlitchActive = true;
    }
    public void _SetGlitchNumbersKrest()
    {
        isGlitchActive = true;
        coroutine3 = StartCoroutine(glitch_animation_krest());
    }
    public void _SetGlitchNumbers()
    {
        isGlitchActive = true;
        coroutine = StartCoroutine(glitch_animation());
        glitchSnd.Play();
    }
    public void _SetGlitchNumbersDeadBodyZoom()
    {
        isGlitchActive = true;
        coroutine2 = StartCoroutine(glitch_animation2());
    }
    public void _SetManiacDeadGlitch()
    {
        isGlitchActive = true;
        StartCoroutine(ManiacGlitch());
    }
    public void _SetDeadFaustDeadGlitch()
    {
        isGlitchActive = true;
        StartCoroutine(FaustDeadGlitch());
    }
    private IEnumerator glitch_animation_krest()
    {
        glitchKrestSnd.PlayOneShot(clips[0]);
        mat.SetFloat("_NoiseAmount", 75);
        mat.SetFloat("_GlitchStrength", 75);
        yield return new WaitForSeconds(0.2f);
        mat.SetFloat("_NoiseAmount", 0);
        mat.SetFloat("_GlitchStrength", 0);
        yield return new WaitForSeconds(1f);
        glitchKrestSnd.PlayOneShot(clips[1]);
        mat.SetFloat("_NoiseAmount", 100);
        mat.SetFloat("_GlitchStrength", 100);
        yield return new WaitForSeconds(0.15f);
        mat.SetFloat("_NoiseAmount", 0);
        mat.SetFloat("_GlitchStrength", 0);
        yield return new WaitForSeconds(1f);
        glitchKrestSnd.PlayOneShot(clips[2]);
        mat.SetFloat("_NoiseAmount", 65);
        mat.SetFloat("_GlitchStrength", 65);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 0);
        mat.SetFloat("_GlitchStrength", 0);
        yield return new WaitForSeconds(3f);
        glitchKrestSnd.PlayOneShot(clips[3]);
        mat.SetFloat("_NoiseAmount", 70);
        mat.SetFloat("_GlitchStrength", 70);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 0);
        mat.SetFloat("_GlitchStrength", 0);
        yield return new WaitForSeconds(2f);
        glitchKrestSnd.PlayOneShot(clips[4]);
        mat.SetFloat("_NoiseAmount", 100);
        mat.SetFloat("_GlitchStrength", 100);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 0);
        mat.SetFloat("_GlitchStrength", 0);
        isGlitchActive = false;
    }
    private IEnumerator glitch_animation()
    {
        while (true)
        {
            mat.SetFloat("_NoiseAmount", 75);
            mat.SetFloat("_GlitchStrength", 75);
            mat.SetFloat("_ScanLineStrenght", 0);
            yield return new WaitForSeconds(0.3f);
            mat.SetFloat("_NoiseAmount", 50);
            mat.SetFloat("_GlitchStrength", 50);
            yield return new WaitForSeconds(0.2f);
            mat.SetFloat("_NoiseAmount", 15);
            mat.SetFloat("_GlitchStrength", 15);
            yield return new WaitForSeconds(0.25f);
            mat.SetFloat("_NoiseAmount", 70);
            mat.SetFloat("_GlitchStrength", 70);
            yield return new WaitForSeconds(0.35f);
            mat.SetFloat("_NoiseAmount", 10);
            mat.SetFloat("_GlitchStrength", 10);
            yield return new WaitForSeconds(0.4f);
            mat.SetFloat("_NoiseAmount", 100);
            mat.SetFloat("_GlitchStrength", 100);
            yield return new WaitForSeconds(0.3f);
            mat.SetFloat("_NoiseAmount", 0);
            mat.SetFloat("_GlitchStrength", 0);
            yield return new WaitForSeconds(1f);
            mat.SetFloat("_ScanLineStrenght", 0.5f);
            mat.SetFloat("_NoiseAmount", 35);
            mat.SetFloat("_GlitchStrength", 35);
            yield return new WaitForSeconds(0.4f);
            mat.SetFloat("_NoiseAmount", 65);
            mat.SetFloat("_GlitchStrength", 65);
            yield return new WaitForSeconds(0.25f);
            mat.SetFloat("_NoiseAmount", 45);
            mat.SetFloat("_GlitchStrength", 45);
            yield return new WaitForSeconds(0.25f);
            mat.SetFloat("_NoiseAmount", 35);
            mat.SetFloat("_GlitchStrength", 35);
            mat.SetFloat("_ScanLineStrenght", 0f);
            yield return new WaitForSeconds(0.25f);
            mat.SetFloat("_NoiseAmount", 25);
            mat.SetFloat("_GlitchStrength", 35);
            yield return new WaitForSeconds(0.25f);
            mat.SetFloat("_NoiseAmount", 90);
            mat.SetFloat("_GlitchStrength", 90);
            yield return new WaitForSeconds(0.25f);
            mat.SetFloat("_NoiseAmount", 0);
            mat.SetFloat("_GlitchStrength", 0);
            yield return new WaitForSeconds(0.45f);
        }
    }
    private IEnumerator ManiacGlitch()
    {
        yield return new WaitForSeconds(0.3f);
        deadManiacGlitch.Play();
        mat.SetFloat("_NoiseAmount", 80);
        mat.SetFloat("_GlitchStrength", 80);
        mat.SetFloat("_ScanLineStrenght", 0);
        yield return new WaitForSeconds(0.2f);
        mat.SetFloat("_NoiseAmount", 50);
        mat.SetFloat("_GlitchStrength", 50);
        yield return new WaitForSeconds(0.15f);
        mat.SetFloat("_NoiseAmount", 15);
        mat.SetFloat("_GlitchStrength", 15);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 70);
        mat.SetFloat("_GlitchStrength", 70);
        yield return new WaitForSeconds(0.35f);
        mat.SetFloat("_NoiseAmount", 100);
        mat.SetFloat("_GlitchStrength", 100);
        yield return new WaitForSeconds(0.20f);
        mat.SetFloat("_NoiseAmount", 65);
        mat.SetFloat("_GlitchStrength", 65);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 10);
        mat.SetFloat("_GlitchStrength", 10);
        mat.SetFloat("_ScanLineStrenght", 1);
        yield return new WaitForSeconds(0.1f);
        mat.SetFloat("_NoiseAmount", 80);
        mat.SetFloat("_GlitchStrength", 80);
        mat.SetFloat("_ScanLineStrenght", 0);
        yield return new WaitForSeconds(0.2f);
        mat.SetFloat("_NoiseAmount", 50);
        mat.SetFloat("_GlitchStrength", 50);
        yield return new WaitForSeconds(0.15f);
        mat.SetFloat("_NoiseAmount", 15);
        mat.SetFloat("_GlitchStrength", 15);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 70);
        mat.SetFloat("_GlitchStrength", 70);
        yield return new WaitForSeconds(0.35f);
        mat.SetFloat("_NoiseAmount", 100);
        mat.SetFloat("_GlitchStrength", 100);
        yield return new WaitForSeconds(0.20f);
        mat.SetFloat("_NoiseAmount", 65);
        mat.SetFloat("_GlitchStrength", 65);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 0);
        mat.SetFloat("_GlitchStrength", 0);
        mat.SetFloat("_ScanLineStrenght", 1);
        isGlitchActive = false;
    }
    private IEnumerator FaustDeadGlitch()
    {
        yield return new WaitForSeconds(0.3f);
        deadManiacGlitch.Play();
        mat.SetFloat("_NoiseAmount", 60);
        mat.SetFloat("_GlitchStrength", 80);
        mat.SetFloat("_ScanLineStrenght", 0);
        yield return new WaitForSeconds(0.2f);
        mat.SetFloat("_NoiseAmount", 90);
        mat.SetFloat("_GlitchStrength", 50);
        yield return new WaitForSeconds(0.15f);
        mat.SetFloat("_NoiseAmount", 20);
        mat.SetFloat("_GlitchStrength", 35);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 40);
        mat.SetFloat("_GlitchStrength", 45);
        yield return new WaitForSeconds(0.35f);
        mat.SetFloat("_NoiseAmount", 100);
        mat.SetFloat("_GlitchStrength", 100);
        yield return new WaitForSeconds(0.20f);
        mat.SetFloat("_NoiseAmount", 65);
        mat.SetFloat("_GlitchStrength", 70);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 10);
        mat.SetFloat("_GlitchStrength", 10);
        mat.SetFloat("_ScanLineStrenght", 1);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 0);
        mat.SetFloat("_GlitchStrength", 0);
        mat.SetFloat("_ScanLineStrenght", 1);
        isGlitchActive = false;
    }
    private IEnumerator glitch_animation2()
    {
        mat.SetFloat("_NoiseAmount", 75);
        mat.SetFloat("_GlitchStrength", 75);
        mat.SetFloat("_ScanLineStrenght", 0);
        yield return new WaitForSeconds(0.2f);
        mat.SetFloat("_NoiseAmount", 50);
        mat.SetFloat("_GlitchStrength", 50);
        yield return new WaitForSeconds(0.15f);
        mat.SetFloat("_NoiseAmount", 15);
        mat.SetFloat("_GlitchStrength", 15);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 70);
        mat.SetFloat("_GlitchStrength", 70);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_NoiseAmount", 0);
        mat.SetFloat("_GlitchStrength", 0);
        mat.SetFloat("_ScanLineStrenght", 1);
        isGlitchActive = false;
    }
    private IEnumerator FadeOut()
    {
        float startVolume = glitchSnd.volume;

        while (glitchSnd.volume > 0)
        {
            glitchSnd.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        glitchSnd.Stop();
        glitchSnd.volume = startVolume;
    }
}
