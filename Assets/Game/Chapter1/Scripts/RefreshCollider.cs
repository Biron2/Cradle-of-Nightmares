using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshCollider : MonoBehaviour
{
    private Collider2D mCollider;
    void Start()
    {
        mCollider = GetComponent<Collider2D>();

        mCollider.enabled = false;

        mCollider.enabled = true;
    }
}
