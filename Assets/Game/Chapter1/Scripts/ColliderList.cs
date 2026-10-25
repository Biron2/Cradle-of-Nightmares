using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderList : MonoBehaviour
{
    [SerializeField] private Collider2D[] colliderOffOn;
    [SerializeField] private openAndCloseCloset[] openAndCloseClosets;
    public bool allowOpenShatulochka = true;
    public bool allowOnOff = true;
    public void DISABLE_COLLIDER()
    {
        if (allowOnOff)
        {
            //Выключает коллайдер - start
            foreach (Collider2D obj in colliderOffOn)
            {
                if (obj != null)
                {
                    obj.enabled = false;
                }
            }
            //Выключает коллайдер - end
            allowOpenShatulochka = false;
            changeColliderState(false);
        }
    }
    public void ENABLE_COLLIDER()
    {
        if (allowOnOff)
        {
            //Выключает коллайдер - start
            foreach (Collider2D obj in colliderOffOn)
            {
                if (obj != null)
                {
                    obj.enabled = true;
                }
            }
            //Выключает коллайдер - end
            allowOpenShatulochka = true;
            changeColliderState(true);
        }
    }

    private void changeColliderState(bool enable_disable)
    {
        foreach (var closet in openAndCloseClosets)
        {
            Collider2D[] colliders = closet.gameObject.GetComponents<Collider2D>();

            if (colliders.Length > 1)
            {
                if (closet.currentColliderState == false)
                {
                    colliders[0].enabled = enable_disable;
                    colliders[1].enabled = false;
                }
                else
                {
                    colliders[0].enabled = false;
                    colliders[1].enabled = enable_disable;
                }
            }
            else if (colliders.Length == 1)
            {
                // Если по какой-то причине только один коллайдер — работаем только с ним
                colliders[0].enabled = enable_disable;
            }
        }
    }
}
