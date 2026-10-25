using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePauseAfk : MonoBehaviour
{
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            ButtonFunctions.Instance.Pause();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            ButtonFunctions.Instance.Pause();
        }
    }
}
