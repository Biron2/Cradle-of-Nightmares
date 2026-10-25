using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chapter2Screamer : MonoBehaviour
{
    [SerializeField] private GameObject sceneHorror;
    [SerializeField] private ManiacMove2 maniacMove;
    [SerializeField] private TVmanager _TVmanager;

    public Image whiteBackground;
    public TimeManager timeManager;
    [SerializeField] private AudioSource video1, video2;
    [SerializeField] private GameObject[] arrows_containers;

    public AudioSource screams;
    public AudioClip faustCatchedBackgroundSound, photoMakeSound, screamSound, polaroid1, polaroid2;

    public CanvasGroup interfaceCanvas;
    [SerializeField] private GameObject faust1, faust2, faust3, cake1, cake2, cake3;
    [SerializeField] private GameObject loadingScene;
    private ColliderList colliderList;
    public VariableDeterminator variableDeterminator;

    [SerializeField] private PauseScript _PauseScript;
    [SerializeField] private Sprite tsutomuWithPhoto, tsutomuCommon;
    [SerializeField] private GlitchController _GlitchController;
    private void Start()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
    }
    private IEnumerator startActions()
    {
        yield return new WaitForSeconds(1);
        maniacMove.transform.Find("body1").GetComponent<Image>().sprite = tsutomuWithPhoto;
        yield return new WaitForSeconds(0.2f);
        _GlitchController._SetGlitchNumbersKrest();
        //--
        StartCoroutine(FadeToAlpha(maniacMove.transform.Find("tsutomu_flash").GetComponent<Image>(), 0.1f, 0.35f));
        StartCoroutine(FadeToAlpha(whiteBackground, 0.1f, 0.35f));
        screams.PlayOneShot(photoMakeSound);
        yield return new WaitForSeconds(0.2f);
        //--
        StartCoroutine(FadeToAlpha(maniacMove.transform.Find("tsutomu_flash").GetComponent<Image>(), 0.1f, 0));
        StartCoroutine(FadeToAlpha(whiteBackground, 0.1f, 0));
        screams.PlayOneShot(polaroid1);
        yield return new WaitForSeconds(0.2f);
        //--
        StartCoroutine(FadeToAlpha(maniacMove.transform.Find("tsutomu_flash").GetComponent<Image>(), 0.1f, 0.65f));
        StartCoroutine(FadeToAlpha(whiteBackground, 0.1f, 0.65f));
        screams.PlayOneShot(photoMakeSound);
        yield return new WaitForSeconds(0.2f);
        //--
        StartCoroutine(FadeToAlpha(maniacMove.transform.Find("tsutomu_flash").GetComponent<Image>(), 0.1f, 0));
        StartCoroutine(FadeToAlpha(whiteBackground, 0.1f, 0));
        screams.PlayOneShot(polaroid2);
        yield return new WaitForSeconds(0.2f);
        //--
        StartCoroutine(FadeToAlpha(maniacMove.transform.Find("tsutomu_flash").GetComponent<Image>(), 0.1f, 1));
        StartCoroutine(FadeToAlpha(whiteBackground, 0.1f, 1));
        screams.PlayOneShot(photoMakeSound);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FadeToAlpha(maniacMove.transform.Find("tsutomu_flash").GetComponent<Image>(), 0.1f, 0));
        StartCoroutine(FadeToAlpha(whiteBackground, 0.1f, 0));
        screams.PlayOneShot(screamSound);
        yield return new WaitForSeconds(1f);
        //конец скримера & сброс данных

        _PauseScript.offMouse = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (!VariableDeterminator.stopShowPause)
        {
            if (video1 != null)
            {
                video1.volume = 0.5f;
            }
            if (video2 != null)
            {
                video2.volume = 0.5f;
            }
            foreach (GameObject obj in arrows_containers)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }

            if (maniacMove.current_room == 11)
            {
                if (_TVmanager != null)
                {
                    _TVmanager.Start_Videos();
                }
            }
            colliderList.ENABLE_COLLIDER();
            interfaceCanvas.alpha = 1;
        }
        _PauseScript.BlockCollidersActive = false;
        timeManager.isPaused = false;
        sceneHorror.SetActive(false);

        timeManager.glassesAllow = false;
        maniacMove.transform.Find("body1").GetComponent<Image>().sprite = tsutomuCommon;

        updateHealthBar();
        maniacMove.gameObject.SetActive(false);
    }

    public void updateHealthBar()
    {
        if (faust1.activeSelf)
        {
            faust1.SetActive(false);
            faust2.SetActive(true);
            cake3.SetActive(false);
        }
        else if (faust2.activeSelf)
        {
            faust2.SetActive(false);
            faust3.SetActive(true);
            cake2.SetActive(false);
        }
        else if (faust3.activeSelf)
        {
            timeManager.StopAllCoroutines();
            variableDeterminator.ShowDeathMenu();
        }
    }

    public void StartHorror()
    {
        maniacMove.DisableManiac();
        colliderList.DISABLE_COLLIDER();
        screams.PlayOneShot(faustCatchedBackgroundSound);
        sceneHorror.SetActive(true);
        interfaceCanvas.alpha = 0;
        StartCoroutine(startActions());
    }
    private IEnumerator FadeToAlpha(Image mImage, float duration, float targetAlpha)
    {
        if (mImage == null)
            yield break;

        Color color = mImage.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            color.a = newAlpha;
            mImage.color = color;
            yield return null;
        }

        // Убедимся, что значение точно достигло цели
        color.a = targetAlpha;
        mImage.color = color;
    }
}