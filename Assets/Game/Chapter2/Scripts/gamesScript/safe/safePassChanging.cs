using UnityEngine;
using UnityEngine.UI;

public class safePassChanging : MonoBehaviour
{
    public AudioSource hlop, winGameSnd;

    [SerializeField] private safeZoom _SafeZoom;
    [SerializeField] private Image pass1, pass2, pass3, pass4;
    [SerializeField] private Sprite[] pass_sprites;
    public int currentIndex = 0;

    private float cooldown = 0.15f; // ¬рем¤ задержки в секундах

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!VariableDeterminator.allowSafePuzzle || !VariableDeterminator.isUserInSafeZoomIn)
            {
                return;
            }
            if (Time.time - _SafeZoom.lastClickTimeForPass < cooldown)
            {
                // ¬рем¤ после последнего нажати¤ меньше задержки, поэтому ничего не делаем
                return;
            }
            _SafeZoom.lastClickTimeForPass = Time.time; // ќбновл¤ем врем¤ последнего нажати¤
            hlop.PlayOneShot(hlop.clip);

            // Смена спрайта
            currentIndex = (currentIndex + 1) % pass_sprites.Length;
            GetComponent<Image>().sprite = pass_sprites[currentIndex];
        }
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

        if (VariableDeterminator.allowSafePuzzle == true)
        {
            if (pass1.sprite.name.Equals("pass4") &&
                pass2.sprite.name.Equals("pass3") &&
                pass3.sprite.name.Equals("pass2") &&
                pass4.sprite.name.Equals("pass1"))
            {
                winGameSnd.Play();
                VariableDeterminator.allowSafePuzzle = false;
                _SafeZoom.CompletedSafeTask();
            }
        }
    }
}