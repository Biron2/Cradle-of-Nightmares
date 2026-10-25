using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PctOpened : MonoBehaviour
{
    public GameObject go1, go2, go3, go4;
    public void startOpenPictureMethod()
    {
        Invoke(nameof(openPicture), 1f);
    }
    private void openPicture()
    {
        go1.SetActive(true);
        go2.SetActive(false);
        go3.SetActive(false);
        go4.SetActive(false);
    }
}
