    using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class dialog_room2 : MonoBehaviour
{
    [SerializeField] private GameObject[] mArrows;
    [SerializeField] private CanvasGroup InterfaceCanvas;

    public Text text, speaker_name;
    private string textToType1;
    public float typingSpeed = 0.05f;
    private bool _isTyping = true; // Флаг, указывающий, идет ли печать текста
    [SerializeField] private GameObject TextContainer_dia;
    private string faustText;
    public int language;
    [SerializeField] private GameObject _PC_continue, _mobile_continue;
    [SerializeField] private GameObject pauseMenu;
    private ColliderList colliderList;
    public PauseScript pauseScript;
    public ButtonFunctions buttonFunctions;
    private AudioSource _source;

    [SerializeField] private CanvasGroup DialogGroup;

    private bool mobileContinuePressed = false;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }
    public void StartDialogRoom2()
    {
        pauseScript.offMouse = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        colliderList.DISABLE_COLLIDER();
        foreach (GameObject obj in mArrows)
        {
            obj.SetActive(false);
        }
        InterfaceCanvas.alpha = 0f;

        UpdateDialogLang();

        // Запускаем корутину автопечати текста
        StartCoroutine(TypeText());
    }
    public void UpdateDialogLang()
    {
        language = PlayerPrefs.GetInt("language", language);
        if (language == 0)
        {
            faustText = "Faust";

            textToType1 = "That disgusting smell...It was as if Gein had reversed the process of throwing away garbage\nexactly the opposite way, going to the dump and bringing it home...\nAnd these closets are the place to hide when everything goes dark.";
        }
        else if (language == 1)
        {
            faustText = "Фауст";

            textToType1 = "Этот отвратительный запах...Гейн словно изменил процесс выброса мусора с точностью наоборот,\nотправляясь на свалку и принося его домой...\nА в этих шкафах можно прятаться, когда все погрузится во тьму.";
        }
    }
    private IEnumerator SmoothCanvasGroup(CanvasGroup canvasGroup, float duration, float targetAlpha)
    {
        // Начальное значение alpha
        float startAlpha = canvasGroup.alpha;

        // Время, прошедшее с начала изменения
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Линейная интерполяция между стартовым и конечным значением
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            yield return null; // Ждем следующий кадр
        }

        // Убедиться, что значение alpha точно равно конечной цели
        canvasGroup.alpha = targetAlpha;
    }
    private IEnumerator TypeText()
    {
        yield return new WaitForSeconds(1f); 
        TextContainer_dia.SetActive(true);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 1f));
        yield return new WaitForSeconds(0.2f);
        _source.PlayOneShot(_source.clip);
        text.text = "";
        speaker_name.text = faustText;
        foreach (char character in textToType1)
        {
            if (!_isTyping)
                yield break;
            text.text += character;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitUntil(() => !_source.isPlaying);
#if PLAYMARKET
        _mobile_continue.gameObject.SetActive(true);
#else
        _PC_continue.gameObject.SetActive(true);
#endif
        yield return new WaitUntil(() => pauseMenu.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || mobileContinuePressed));mobileContinuePressed = false;
        _PC_continue.gameObject.SetActive(false);
        _mobile_continue.gameObject.SetActive(false);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return new WaitForSeconds(typingSpeed);
        yield return StartCoroutine(SmoothCanvasGroup(DialogGroup, 0.5f, 0f));
        TextContainer_dia.SetActive(false);
        text.text = "";
        speaker_name.text = "";
        colliderList.ENABLE_COLLIDER();
        foreach (GameObject obj in mArrows)
        {
            obj.SetActive(true);
        }
        InterfaceCanvas.alpha = 1f;
        HideInShkaf.allowJoinInHidePlace = true; 
        buttonFunctions.BlockCollidersActive = false;
        pauseScript.BlockCollidersActive = false;
        MoveRoom.alreadyRoom2 = true;
        pauseScript.offMouse = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void MobilePress()
    {
        if (gameObject.activeSelf)
        {
            mobileContinuePressed = true;
        }
    }
}
