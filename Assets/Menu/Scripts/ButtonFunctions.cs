using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonFunctions : MonoBehaviour
{
    public bool allowPause = false, findCollders = false;
    [SerializeField] private GameObject mPauseObj;
    private ColliderList colliderList;
    public TimeManager timeManager;
    public bool BlockCollidersActive = false;

    public static ButtonFunctions Instance;

    [SerializeField] private Button openSettingsMobile;

    [SerializeField] private AudioSource clickSnd;

    private void Start()
    {
        if (findCollders == true)
        {
            colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        }

        if (Instance == null)
        {
            Instance = this;
        }
#if PLAYMARKET
        openSettingsMobile?.gameObject.SetActive(true);
        openSettingsMobile?.onClick.AddListener(Pause);
#endif
    }
    public void ExitFromGame()
    {
        Application.Quit();
    }
    public void OpenTelegram()
    {
        Application.OpenURL("https://t.me/+GRwhqvnsEt5lYTEy");
    }
    public void OpenYoutube()
    {
        Application.OpenURL("https://www.youtube.com/@Faust21century");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    private void Update()
    {
        if (allowPause == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        if (mPauseObj != null && !VariableDeterminator.stopShowPause)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            mPauseObj.SetActive(true);
            Time.timeScale = 0;
            AudioListener.pause = true;
            if (BlockCollidersActive == false)
            {
                if (findCollders == true)
                {
                    if (!VariableDeterminator.isMiniGameActive)
                    {
                        colliderList.DISABLE_COLLIDER();
                    }
                }
            }
            if (timeManager != null)
            {
                timeManager.glassesCameraAllow = false;
            }

            openSettingsMobile.gameObject.SetActive(false);
        }
    }
    public void EnableOpenSettingsMobileBtnAgain()
    {
#if PLAYMARKET
        openSettingsMobile?.gameObject.SetActive(true);
        clickSnd.PlayOneShot(clickSnd.clip);
#endif
    }
}
