using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class failedHit : MonoBehaviour
{
    public AudioSource failedHitSnd;
    public randomclickgame mRandomclickgame;
    [SerializeField] private Transform hit_hole_prefab, holes_parent;
    public void failed_hit_public_method()
    {
        if (mRandomclickgame.bullets != 0)
        {
            mRandomclickgame.RemoveBulletImg();
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // ѕолучаем локальные координаты
            RectTransform newHole = Instantiate(hit_hole_prefab, holes_parent).GetComponent<RectTransform>();

            // ѕреобразуем мировые координаты в локальные координаты относительно родител¤
            Vector2 localPosition = holes_parent.InverseTransformPoint(clickPosition);

            // ”станавливаем локальную позицию
            newHole.anchoredPosition = localPosition;

            mRandomclickgame.bullets--;
            failedHitSnd.PlayOneShot(failedHitSnd.clip);
            mRandomclickgame.UpdateBulletsText();
        }
    }

}
