using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PianoKey : MonoBehaviour
{
    private AudioSource audioSource;
    private Image image;
    public static string password = "";
    public Piano piano;
    [SerializeField] private GameObject statueHand, PianoGame;
    [SerializeField] private GameObject[] arrows_containers;
    private ColliderList colliderList;
    public AudioSource winSnd;
    [SerializeField] private GameObject _tip7;
    private void Awake()
    {
        image = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            StartCoroutine(PlayAndAnimate());
            if (gameObject.name.Equals("PianoKey1"))
            {
                numberForPassword("1");
            }
            if (gameObject.name.Equals("PianoKey2"))
            {
                numberForPassword("2");
            }
            if (gameObject.name.Equals("PianoKey3"))
            {
                numberForPassword("3");
            }
            if (gameObject.name.Equals("PianoKey4"))
            {
                numberForPassword("4");
            }
            if (gameObject.name.Equals("PianoKey5"))
            {
                numberForPassword("5");
            }
        }
    }

    private void numberForPassword(string number)
    {
        if(piano.allowOpenPianoGame == true)
        {
            password += number;
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
    private IEnumerator PlayAndAnimate()
    {
        image.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        image.color = Color.white;
    }
    private void Update()
    {
        // --- ПК: клик мышью ---
        if (Input.GetMouseButtonDown(0))
        {
            CheckHit(Camera.main.ScreenPointToRay(Input.mousePosition));
        }

        // --- Телефон: касание ---
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                CheckHit(Camera.main.ScreenPointToRay(touch.position));
            }
        }

        if (piano.allowOpenPianoGame == true)
        {
            if (password.Contains("1114423555"))
            {
                _tip7.SetActive(false);
                audioSource.Stop();
                winSnd.Play();
                statueHand.SetActive(true);
                PianoGame.SetActive(false);
                piano.allowOpenPianoGame = false;
                foreach (GameObject obj in arrows_containers)
                {
                    if (obj != null)
                    {
                        obj.SetActive(true);
                    }
                }
                colliderList.ENABLE_COLLIDER();
                VariableDeterminator.isMiniGameActive = false;

                public_actions.Instance.OnOff_SpaceTip_Close(false);
            }
        }
    }
}