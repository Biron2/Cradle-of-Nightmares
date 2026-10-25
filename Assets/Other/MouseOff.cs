using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOff : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
