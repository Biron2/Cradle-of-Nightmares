using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class randomclickgame : MonoBehaviour
{
    [SerializeField] private rndclckgmStart mRndclckgm;
    public AudioSource tarakanSpawned, failedSndGame;
    [SerializeField] private GameObject openTarakan;
    public bool allowStartAgain = true;
    public int shotedTarakan = 0;
    [SerializeField] private GameObject[] arrows_containers;


    [SerializeField] private Transform holes_parent;

    [SerializeField] private GameObject[] allRats;
    [SerializeField] private Image[] allRatsImg;
    [SerializeField] private GameObject[] allDeadRats;

    [SerializeField] private Image[] bulletsImg;

    public int bullets = 8;
    private ColliderList colliderList;
    [SerializeField] private Text killedCount;

    private audio_for_items _Audio_For_Items;

    private float cooldown = 0.5f; // Время задержки в секундах
    private float lastClickTime = 0f; // Время последнего нажатия
    [SerializeField] private AudioSource winMiniGameSound;
    private void Start()
    {
        _Audio_For_Items = GameObject.Find("Other/GameManager/EtcSounds/__FOR_ITEMS__").GetComponent<audio_for_items>();
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        public_actions.Instance.closeMiniGameAction += OnMinigameClosed;
    }
    private void OnDestroy()
    {
        // Важно отписаться, чтобы не было утечек
        public_actions.Instance.closeMiniGameAction -= OnMinigameClosed;
    }
    private void OnMinigameClosed()
    {
        if (openTarakan.activeSelf)
        {
            foreach (Transform child in holes_parent)
            {
                Destroy(child.gameObject);
            }
            VariableDeterminator.isMiniGameActive = false;
            colliderList.ENABLE_COLLIDER();
            bullets = 8;
            foreach (Image imgBullet in bulletsImg)
            {
                imgBullet.gameObject.SetActive(true);
            }
            UpdateBulletsText();
            tarakanSpawned.Stop();
            foreach (GameObject obj in allRats)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            foreach (Image obj in allRatsImg)
            {
                if (obj != null)
                {
                    var tempColor = obj.color;
                    tempColor.a = 1f;
                    obj.color = tempColor;
                }
            }
            foreach (GameObject obj in allDeadRats)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            foreach (GameObject obj in allRats)
            {
                if (obj != null)
                {
                    mRndclckgm.tarakans.Remove(obj);
                }
            }
            foreach (GameObject obj in allRats)
            {
                if (obj != null)
                {
                    mRndclckgm.tarakans.Add(obj);
                }
            }
            shotedTarakan = 0;
            killedCount.text = shotedTarakan + "/5";
            openTarakan.SetActive(false);
            foreach (GameObject obj in arrows_containers)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
            public_actions.Instance.OnOff_SpaceTip_Close(false);
        }
    }
    public void wonRats()
    {
        winMiniGameSound.PlayOneShot(winMiniGameSound.clip);
        foreach (GameObject obj in arrows_containers)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        public_actions.Instance.OnOff_SpaceTip_Close(false);
        colliderList.ENABLE_COLLIDER();
        VariableDeterminator.isMiniGameActive = false;
    }

    void CheckHit(Ray ray)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (Time.time - lastClickTime < cooldown)
            {
                // Время после последнего нажатия меньше задержки, поэтому ничего не делаем
                return;
            }
            lastClickTime = Time.time; // Обновляем время последнего нажатия
            if (allowStartAgain == true)
            {
                VariableDeterminator.isMiniGameActive = true;
                _Audio_For_Items.PlaySounds("RatOpen");
                colliderList.DISABLE_COLLIDER();
                shotedTarakan = 0;
                killedCount.text = shotedTarakan + "/5";
                openTarakan.SetActive(true);

                foreach (GameObject obj in arrows_containers)
                {
                    if (obj != null)
                    {
                        obj.SetActive(false);
                    }
                }
                public_actions.Instance.OnOff_SpaceTip_Close(true);
            }
        }
    }

    public void RemoveBulletImg()
    {

        // Итерируемся по массиву с конца
        for (int i = bulletsImg.Length - 1; i >= 0; i--)
        {
            // Проверяем, активно ли изображение
            if (bulletsImg[i].gameObject.activeSelf) // Если изображение активно
            {
                // Выключаем последнее активное изображение
                bulletsImg[i].gameObject.SetActive(false);
                break; // Прерываем цикл после отключения
            }
        }
    }
    public void UpdateBulletsText()
    {
        if (bullets == 0)
        {
            foreach (Transform child in holes_parent)
            {
                Destroy(child.gameObject);
            }
            VariableDeterminator.isMiniGameActive = false;
            colliderList.ENABLE_COLLIDER();
            failedSndGame.Play();
            bullets = 8;
            foreach (Image imgBullet in bulletsImg)
            {
                imgBullet.gameObject.SetActive(true);
            }
            UpdateBulletsText();
            tarakanSpawned.Stop();
            foreach (GameObject obj in allRats)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            foreach (Image obj in allRatsImg)
            {
                if (obj != null)
                {
                    var tempColor = obj.color;
                    tempColor.a = 1f;
                    obj.color = tempColor;
                }
            }
            foreach (GameObject obj in allDeadRats)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            foreach (GameObject obj in allRats)
            {
                if (obj != null)
                {
                    mRndclckgm.tarakans.Remove(obj);
                }
            }
            foreach (GameObject obj in allRats)
            {
                if (obj != null)
                {
                    mRndclckgm.tarakans.Add(obj);
                }
            }
            shotedTarakan = 0;
            openTarakan.SetActive(false);
            foreach (GameObject obj in arrows_containers)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
            public_actions.Instance.OnOff_SpaceTip_Close(false);
        }
        killedCount.text = shotedTarakan + "/5";
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            OnMinigameClosed();
        }
    }
    public void closeThisGame()
    {
        if (openTarakan.activeSelf)
        {
            VariableDeterminator.isMiniGameActive = false;
            foreach (Transform child in holes_parent)
            {
                Destroy(child.gameObject);
            }
            bullets = 8;
            UpdateBulletsText();
            tarakanSpawned.Stop();
            foreach (GameObject obj in allRats)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            foreach (Image obj in allRatsImg)
            {
                if (obj != null)
                {
                    var tempColor = obj.color;
                    tempColor.a = 1f;
                    obj.color = tempColor;
                }
            }
            foreach (GameObject obj in allDeadRats)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            foreach (GameObject obj in allRats)
            {
                if (obj != null)
                {
                    mRndclckgm.tarakans.Remove(obj);
                }
            }
            foreach (GameObject obj in allRats)
            {
                if (obj != null)
                {
                    mRndclckgm.tarakans.Add(obj);
                }
            }
            shotedTarakan = 0;
            killedCount.text = shotedTarakan + "/5";
            openTarakan.SetActive(false);
            public_actions.Instance.OnOff_SpaceTip_Close(false);
        }
    }
}
