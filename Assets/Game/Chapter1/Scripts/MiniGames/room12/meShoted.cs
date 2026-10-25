using UnityEngine;
using UnityEngine.UI;

public class meShoted : MonoBehaviour
{
    public randomclickgame mRandomclickgame;
    public rndclckgmStart mRndclckgmStart;
    [SerializeField] private GameObject mainObjRandomGame, deathRatsScreenForPiano;
    public AudioSource tarakanHit;
    public AudioSource tarakanStep;
    public bool alreadyShoted = false;
    private Image thisAlphaRat;
    private GameObject deadRatObj;
    private void OnEnable()
    {
        alreadyShoted = false;
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
            if (alreadyShoted == false)
            {
                mRandomclickgame.shotedTarakan++;
                if (mRandomclickgame.bullets != 0)
                {
                    mRandomclickgame.RemoveBulletImg();
                    mRandomclickgame.bullets--;
                    mRandomclickgame.UpdateBulletsText();
                }
                mRndclckgmStart.mRemove_fromList(gameObject.name);
                deadRatObj = transform.Find("deadRat").gameObject;
                deadRatObj.SetActive(true);
                thisAlphaRat = GetComponent<Image>();
                var tempColor = thisAlphaRat.color;
                tempColor.a = 0f;
                thisAlphaRat.color = tempColor;
                tarakanHit.PlayOneShot(tarakanHit.clip);
                alreadyShoted = true;
                if (mRandomclickgame.shotedTarakan == 5)
                {
                    mRandomclickgame.wonRats();
                    deathRatsScreenForPiano.SetActive(true);
                    tarakanStep.Stop();
                    StopCoroutine(mRndclckgmStart.RandomizeTarakanWithInterval());
                    mRndclckgmStart.DisableCurrentTarakan();
                    mainObjRandomGame.SetActive(false);
                    mRandomclickgame.allowStartAgain = false;
                }
            }
        }
    }
}