using UnityEngine;
using UnityEngine.UI;

public class MoveRoom : MonoBehaviour
{
    [SerializeField] private Transform cameraObj;
    [SerializeField] private RectTransform glasses;
    [Header("Room Number")]
    public int room_num = 1;
    public ManiacMove maniacMove;
    [Header("Other")]
    [SerializeField] private GameObject BlackImgObj;
    [SerializeField] private CanvasGroup BlackImgCanvas;
    public float TimeToFade;
    private bool _fadein, _fadeout = false;
    
    [SerializeField] private AudioSource staircaseIfNeed;
    [SerializeField] private AudioSource[] determineAudio;
    [SerializeField] private GameObject[] determineObects;

    [SerializeField] private ArrowButton[] buttonsToOff;
    [SerializeField] private VariableDeterminator variableDeterminator;
    [SerializeField] private TVmanager _TVmanager;

    [Header("Scripts")]
    public dialog_room2 mDialog_Room2;
    public DeadBodyZoom deadBodyZoom;
    public DeadFacesZoom deadFacesZoom;
    private ColliderList colliderList;
    public static bool motorStart = false;
    public static bool alreadyRoom2 = false;
    public PauseScript pauseScript;
    public ButtonFunctions buttonFunctions;
    public scalesGameFaces mScalesGameFaces1, mScalesGameFaces2, mScalesGameFaces3, mScalesGameFaces4, mScalesGameFaces5;
    public lightOnOffT _lightOnOffT;
    private void Start()
    {
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
                maniacMove.current_room = room_num;
                variableDeterminator.current_roomNumber = room_num;
                determineObects[0].SetActive(false);
                determineObects[1].SetActive(false);
                determineAudio[1].Stop();
                determineAudio[2].Stop();
                determineAudio[3].Stop();
                determineAudio[4].Stop();
                _TVmanager.Stop_Videos();
                _lightOnOffT.allowBlinking = false;

                if (variableDeterminator.faceTalking == true)
                {
                    mScalesGameFaces1.StopKarkarCoroutineMethod();
                    mScalesGameFaces2.StopKarkarCoroutineMethod();
                    mScalesGameFaces3.StopKarkarCoroutineMethod();
                    mScalesGameFaces4.StopKarkarCoroutineMethod();
                    mScalesGameFaces5.StopKarkarCoroutineMethod();
                }

                variableDeterminator.faceTalking = false;
                switch (room_num)
                {
                    case 1:
                        glasses.anchoredPosition = new Vector3(0f, 0f, 0f);
                        cameraObj.position = new Vector3(0f, 0f, 0f);
                        break;
                    case 2:
                        glasses.anchoredPosition = new Vector3(1967.58f, 0f, 0f);
                        cameraObj.position = new Vector3(17.777f, 0f, 0f);
                        if (alreadyRoom2 == false)
                        {
                            mDialog_Room2.StartDialogRoom2();
                            buttonFunctions.BlockCollidersActive = true;
                            pauseScript.BlockCollidersActive = true;
                        }
                        break;
                    case 3:
                        glasses.anchoredPosition = new Vector3(3876f, -1057.77f, 0f);
                        cameraObj.position = new Vector3(35.554f, -10f, 0f);
                        _lightOnOffT.allowBlinking = true;
                        if (motorStart == true)
                        {
                            determineAudio[3].Play();
                        }
                        if (variableDeterminator.stopPlayLight == 0)
                        {
                            determineObects[0].SetActive(true);
                        }
                        break;
                    case 4:
                        glasses.anchoredPosition = new Vector3(3876f, 0f, 0f);
                        cameraObj.position = new Vector3(35.554f, 0f, 0f);
                        deadBodyZoom.startScreamer();
                        break;
                    case 5:
                        glasses.anchoredPosition = new Vector3(1967.58f, 1066.1f, 0f);
                        cameraObj.position = new Vector3(17.777f, 10f, 0f);
                        break;
                    case 6:
                        glasses.anchoredPosition = new Vector3(0f, 1066.1f, 0f);
                        cameraObj.position = new Vector3(0f, 10f, 0f);
                        break;
                    case 7:
                        glasses.anchoredPosition = new Vector3(-1917.09f, 1066.1f, 0f);
                        cameraObj.position = new Vector3(-17.777f, 10f, 0f);
                        deadFacesZoom.startScreamer();
                        mScalesGameFaces1.startKrikIf();
                        break;
                    case 8:
                        glasses.anchoredPosition = new Vector3(-1917.09f, 2110f, 0f);
                        cameraObj.position = new Vector3(-17.777f, 20f, 0f);
                        break;
                    case 9:
                        glasses.anchoredPosition = new Vector3(-1917.09f, 3230.7f, 0f);
                        cameraObj.position = new Vector3(-17.777f, 30f, 0f);
                        _TVmanager.Start_Videos();
                        break;
                    case 10:
                        glasses.anchoredPosition = new Vector3(0f, 2110f, 0f);
                        cameraObj.position = new Vector3(0f, 20f, 0f);
                        determineAudio[1].Play();
                        if (variableDeterminator.stopPlayLight == 0)
                        {
                            determineObects[1].SetActive(true);
                        }
                        break;
                    case 11:
                        glasses.anchoredPosition = new Vector3(2016f, 2110f, 0f);
                        cameraObj.position = new Vector3(17.777f, 20f, 0f);
                        break;
                    case 12:
                        glasses.anchoredPosition = new Vector3(2016f, 3230.7f, 0f);
                        cameraObj.position = new Vector3(17.777f, 30f, 0f);
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
                if (alreadyRoom2 == true)
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
