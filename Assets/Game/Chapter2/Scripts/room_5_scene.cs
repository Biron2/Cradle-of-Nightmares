using System.Collections;
using UnityEngine;

public class room_5_scene : MonoBehaviour
{
    [SerializeField] private GameObject[] mArrows;

    private ColliderList colliderList;
    public PauseScript pauseScript;
    public ButtonFunctions buttonFunctions;
    private Camera camera1;

    private float initialSize;
    private Vector3 initialPosition;
    public float transitionStartTime;
    private Vector3 targetPos = new Vector3(-38.19f, -10f, 0f);
    private Vector3 targetPos2 = new Vector3(-35.554f, -10f, 0f);
    public float targetSize = 5.0f;
    public float transitionDuration = 1.0f;
    [SerializeField] private GameObject Interface;

    public void StartRoom_5_Scene()
    {
        camera1 = Camera.main;
        camera1.transform.position = new Vector3(-32.89f, -10f, 0f);
        pauseScript.offMouse = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        colliderList.DISABLE_COLLIDER();
        foreach (GameObject obj in mArrows)
        {
            obj.SetActive(false);
        }

        // Запускаем корутину автопечати текста
        StartCoroutine(CameraMooving());
        camera1.orthographicSize = 3.5f;
        Interface.SetActive(false);
    }
    private IEnumerator CameraMooving()
    {
        initialPosition = camera1.transform.position;
        yield return new WaitForSeconds(1);
        transitionStartTime = Time.time;
        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            camera1.transform.position = Vector3.Lerp(initialPosition, targetPos, t);
            yield return null;
        }
        initialSize = camera1.orthographicSize;
        initialPosition = camera1.transform.position;

        //----------
        yield return new WaitForSeconds(1);

        transitionStartTime = Time.time;
        while (Time.time - transitionStartTime <= transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;
            camera1.orthographicSize = Mathf.Lerp(initialSize, targetSize, t);
            camera1.transform.position = Vector3.Lerp(initialPosition, targetPos2, t);
            yield return null;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        camera1.orthographicSize = targetSize;
        initialSize = camera1.orthographicSize;
        initialPosition = camera1.transform.position;

        colliderList.ENABLE_COLLIDER();
        foreach (GameObject obj in mArrows)
        {
            obj.SetActive(true);
        }
        HideInShkaf.allowJoinInHidePlace = true;
        buttonFunctions.BlockCollidersActive = false;
        pauseScript.BlockCollidersActive = false;
        MoveRoom2.alreadyRoom5 = true;
        pauseScript.offMouse = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Interface.SetActive(true);
    }
}
