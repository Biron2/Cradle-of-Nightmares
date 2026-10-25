using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MoveRoom2 : MonoBehaviour
{
    [SerializeField] private Transform cameraObj;
    [SerializeField] private RectTransform glasses;
    [Header("Room Number")]
    public int room_num = 1;
    public ManiacMove2 maniacMove2;
    [Header("Other")]
    [SerializeField] private GameObject BlackImgObj;
    [SerializeField] private CanvasGroup BlackImgCanvas;
    public float TimeToFade;
    private bool _fadein, _fadeout = false;
    
    [SerializeField] private AudioSource staircaseIfNeed;
    [SerializeField] private AudioSource[] determineAudio;

    [SerializeField] private ArrowButton[] buttonsToOff;
    [SerializeField] private VariableDeterminator variableDeterminator;
    [SerializeField] private TVmanager2 _TVmanager;

    private RectTransform door_to_Room5;
    private float shakeAmount = 10f;
    private float shakeDuration = 0.1f;
    private Vector3 initialPosition = new Vector3(553.4f, 20.776f, 0f);

    [Header("Scripts")]
    public room_5_scene mRoom_5_Scene;
    private ColliderList colliderList;
    public static bool carEngine = false;
    public static bool alreadyRoom5 = false;
    public PauseScript pauseScript;
    public ButtonFunctions buttonFunctions;
    public lightOnOffT _lightOnOffT;
    public carShaking _CarShaking;

    [Header("Night System")]
    [SerializeField] private GameObject maniacMovement;
    [SerializeField] private Light2D global_light;
    public TimeManager timeManager;
    [SerializeField] private GlitchController _GlitchController;

    private void Start()
    {
        door_to_Room5 = GameObject.Find("Rooms/Room3/Image/RoomObjects/door_to_Room5").GetComponent<RectTransform>();
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
    }
    void Update()
    {
        if (_fadein == true)
        {
            if (BlackImgCanvas.alpha < 1)
            {
                BlackImgObj.SetActive(true);
                BlackImgCanvas.alpha += TimeToFade * Time.deltaTime;
            }
            else if (BlackImgCanvas.alpha == 1)
            {
                maniacMove2.current_room = room_num;
                variableDeterminator.current_roomNumber = room_num;

                _lightOnOffT.allowBlinking = false;
                determineAudio[1].Stop();
                determineAudio[2].Stop();
                determineAudio[4].Stop();
                determineAudio[5].Stop();
                _CarShaking.stopShaking();
                _TVmanager.Stop_Videos();

                switch (room_num)
                {
                    case 1:
                        glasses.anchoredPosition = new Vector3(0f, 0f, 0f);
                        cameraObj.position = new Vector3(0f, 0f, 0f);
                        break;
                    case 2:
                        glasses.anchoredPosition = new Vector3(-1966.16f, 0f, 0f);
                        cameraObj.position = new Vector3(-17.777f, 0f, 0f);
                        break;
                    case 3:
                        glasses.anchoredPosition = new Vector3(-3758.603f, 0f, 0f);
                        cameraObj.position = new Vector3(-35.554f, 0f, 0f);
                        break;
                    case 4:
                        glasses.anchoredPosition = new Vector3(-1950.104f, -1067.674f, 0f);
                        cameraObj.position = new Vector3(-17.777f, -10f, 0f);
                        if (carEngine == true)
                        {
                            determineAudio[4].Play();
                            _CarShaking.startShaking();
                        }
                        _lightOnOffT.allowBlinking = true;
                        break;
                    case 5:
                        glasses.anchoredPosition = new Vector3(-3821.874f, -1082.638f, 0f);
                        if (alreadyRoom5 == false)
                        {
                            mRoom_5_Scene.StartRoom_5_Scene();
                            buttonFunctions.BlockCollidersActive = true;
                            pauseScript.BlockCollidersActive = true;
                        }
                        else
                        {
                            cameraObj.position = new Vector3(-35.554f, -10f, 0f);
                        }
                        break;
                    case 6:
                        glasses.anchoredPosition = new Vector3(-3821.874f, 1142f, 0f);
                        cameraObj.position = new Vector3(-35.554f, 10f, 0f);
                        break;
                    case 7:
                        glasses.anchoredPosition = new Vector3(-1808f, 1142f, 0f);
                        cameraObj.position = new Vector3(-17.777f, 10f, 0f);
                        break;
                    case 8:
                        glasses.anchoredPosition = new Vector3(66f, 1142f, 0f);
                        cameraObj.position = new Vector3(0f, 10f, 0f);
                        break;
                    case 9:
                        glasses.anchoredPosition = new Vector3(66f, 2180f, 0f);
                        cameraObj.position = new Vector3(0f, 20f, 0f);
                        if (!VariableDeterminator.firstNightAlreadyStarted)
                        {
                            VariableDeterminator.firstNightAlreadyStarted = true;
                            StartCoroutine(StartManiacSystemAtFirst());
                        }
                        break;
                    case 10:
                        glasses.anchoredPosition = new Vector3(-1985f, 2180f, 0f);
                        cameraObj.position = new Vector3(-17.777f, 20f, 0f);
                        break;
                    case 11:
                        glasses.anchoredPosition = new Vector3(-3805f, 2180f, 0f);
                        cameraObj.position = new Vector3(-35.554f, 20f, 0f);
                        _TVmanager.Start_Videos();
                        break;
                    case 12:
                        glasses.anchoredPosition = new Vector3(-25f, -1080f, 0f);
                        cameraObj.position = new Vector3(0f, -10f, 0f);

                        determineAudio[2].Play();
                        break;
                    default:
                        glasses.anchoredPosition = new Vector3(0f, 0f, 0f);
                        cameraObj.position = new Vector3(0f, 0f, 0f);
                        break;
                }
                _fadein = false;
                _fadeout = true;
            }
        }
        if(_fadeout == true)
        {
            if (BlackImgCanvas.alpha != 0)
            {
                BlackImgCanvas.alpha -= TimeToFade * Time.deltaTime;
            }
            else if(BlackImgCanvas.alpha == 0)
            {
                BlackImgObj.SetActive(false);
                _fadeout = false;
                if(room_num == 5 && alreadyRoom5 == true)
                {
                    HideInShkaf.allowJoinInHidePlace = true;
                    colliderList.ENABLE_COLLIDER();
                }
                else if(room_num != 5)
                {
                    HideInShkaf.allowJoinInHidePlace = true;
                    colliderList.ENABLE_COLLIDER();
                }
                foreach (ArrowButton obj in buttonsToOff)
                {
                    if (obj != null)
                    {
                        obj.activeArrowBtn = true;
                    }
                }
            }
        }
    }

    public void ActiveBtn()
    {
        if(room_num == 5 && !VariableDeterminator.isOpenRoom5)
        {
            StartCoroutine(DoShake(door_to_Room5));
            determineAudio[3].PlayOneShot(determineAudio[3].clip);
        }
        else
        {
            colliderList.DISABLE_COLLIDER();
            _fadein = true;
            if (determineAudio[0] != null)
            {
                determineAudio[0].Play();
            }
            if (staircaseIfNeed != null)
            {
                staircaseIfNeed.Play();
            }
            foreach (ArrowButton obj in buttonsToOff)
            {
                if (obj != null)
                {
                    obj.activeArrowBtn = false;
                }
            }
            HideInShkaf.allowJoinInHidePlace = false;
        }
    }
    private IEnumerator DoShake(RectTransform rectTransform)
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-0.25f, 0.25f) * shakeAmount;
            float y = Random.Range(-0.25f, 0.25f) * shakeAmount;

            rectTransform.localPosition = initialPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        rectTransform.localPosition = initialPosition;
    }
    private IEnumerator StartManiacSystemAtFirst()
    {
        yield return new WaitForSeconds(1.3f);
        _GlitchController._SetGlitchNumbersKrest();
        yield return new WaitForSeconds(0.2f);
        timeManager.FirstCycleCoroutine = StartCoroutine(timeManager.FirstCicle());
        global_light.intensity = 0.005f;
        yield return new WaitForSeconds(5);
        maniacMovement.SetActive(true);
    }
}
