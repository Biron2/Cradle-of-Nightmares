using UnityEngine;
using UnityEngine.UI;

public class swipe : MonoBehaviour
{
    public GameObject scrollbar;
    private float scroll_pos = 0;
    float[] pos;
    private bool runIt = false;
    private float smoothTime = 0.25f;
    private float velocity = 0f;
    private int targetBtnNumber;
    private int currentBtnNumber;
    public Button[] buttons;
    [SerializeField] private DLC_Manager _DLC_Manager;
    [SerializeField] private AudioSource swipedSound;

    void Start()
    {
        pos = new float[transform.childCount];
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = i / (float)(pos.Length - 1);
        }
    }

    void Update()
    {
        float distance = 1f / (pos.Length - 1f);

        if (runIt)
        {
            scrollbar.GetComponent<Scrollbar>().value = Mathf.SmoothDamp(scrollbar.GetComponent<Scrollbar>().value, pos[targetBtnNumber], ref velocity, smoothTime);

            if (Mathf.Abs(scrollbar.GetComponent<Scrollbar>().value - pos[targetBtnNumber]) < 0.001f)
            {
                scrollbar.GetComponent<Scrollbar>().value = pos[targetBtnNumber];
                scroll_pos = pos[targetBtnNumber];
                currentBtnNumber = targetBtnNumber;
                runIt = false;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            }
            else
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                    {
                        scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                        currentBtnNumber = i;
                    }
                }
            }

            // Дополнительное управление с клавиатуры и колесика мыши
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                MoveLeft();
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                MoveRight();
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            float scale = Mathf.Lerp(0.8f, 1f, 1 - Mathf.Abs(scrollbar.GetComponent<Scrollbar>().value - pos[i]) / distance);
            transform.GetChild(i).localScale = new Vector2(scale, scale);
        }
    }

    private void MoveLeft()
    {
        if (currentBtnNumber > 0)
        {
            targetBtnNumber = currentBtnNumber - 1;
            swipedSound.PlayOneShot(swipedSound.clip);
            runIt = true;
        }
    }

    private void MoveRight()
    {
        if (currentBtnNumber < pos.Length - 1)
        {
            targetBtnNumber = currentBtnNumber + 1;
            swipedSound.PlayOneShot(swipedSound.clip);
            runIt = true;
        }
    }

    public void btnChapter1()
    {
        HandleButtonPress(0, _DLC_Manager.activateChapter1);
    }

    public void btnChapter2()
    {
        HandleButtonPress(1, _DLC_Manager.activateChapter2);
    }

    private void HandleButtonPress(int buttonIndex, System.Action activateChapter)
    {
        if (currentBtnNumber == buttonIndex && !runIt)
        {
            activateChapter.Invoke();
        }
        else
        {
            swipedSound.PlayOneShot(swipedSound.clip);
            targetBtnNumber = buttonIndex;
            runIt = true;
        }
    }
}
