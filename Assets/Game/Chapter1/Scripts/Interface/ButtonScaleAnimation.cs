using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScaleAnimation : MonoBehaviour
{
    private Vector3 defaultScale;
    [SerializeField] private float scaleDuration = 0.1f;
    [SerializeField] private float scaleAmount = 0.9f;
    [SerializeField] private GameObject _PauseScript;

    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private Dialog _dialog;
    [SerializeField] private StartTheEnd _StartTheEnd;
    [SerializeField] private AfterPortalScene _AfterPortalScene;
    public int scene_buttons = 1;
    void Start()
    {
        defaultScale = transform.localScale;
    }
    void Update()
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
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!_PauseScript.activeSelf)
            {
                StartCoroutine(ScaleButton());
            }
        }
    }
    private IEnumerator ScaleButton()
    {
        // Уменьшаем кнопку
        buttonSound.Play();
        float timer = 0f;
        while (timer < scaleDuration)
        {
            float scale = Mathf.Lerp(1f, scaleAmount, timer / scaleDuration);
            transform.localScale = defaultScale * scale;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = defaultScale * scaleAmount;

        // Восстанавливаем кнопку
        timer = 0f;
        while (timer < scaleDuration)
        {
            float scale = Mathf.Lerp(scaleAmount, 1f, timer / scaleDuration);
            transform.localScale = defaultScale * scale;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = defaultScale;

        switch (gameObject.name)
        {
            case "Button1":
                if (scene_buttons == 1)
                {
                    _dialog.AnswerButton(1);
                }
                if (scene_buttons == 2)
                {
                    _AfterPortalScene.AnswerButton(1);
                }
                if (scene_buttons == 3)
                {
                    _StartTheEnd.AnswerButton(1);
                }
                break;
            case "Button2":
                if (scene_buttons == 1)
                {
                    _dialog.AnswerButton(2);
                }
                if (scene_buttons == 2)
                {
                    _AfterPortalScene.AnswerButton(2);
                }
                if (scene_buttons == 3)
                {
                    _StartTheEnd.AnswerButton(2);
                }
                break;
            default:
                if (scene_buttons == 1)
                {
                    _dialog.AnswerButton(1);
                }
                if (scene_buttons == 2)
                {
                    _AfterPortalScene.AnswerButton(1);
                }
                if (scene_buttons == 3)
                {
                    _StartTheEnd.AnswerButton(1);
                }
                break;
        }

    }
}
