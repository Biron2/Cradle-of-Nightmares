using System.Collections;
using UnityEngine;

public class sakuraStartFalling : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(destroyComponents());
    }

    private IEnumerator destroyComponents()
    {
        yield return new WaitForSeconds(3);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
    }
}