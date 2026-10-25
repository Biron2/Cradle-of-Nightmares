using System.Collections;
using UnityEngine;

public class resBtnPress : MonoBehaviour
{
    [SerializeField] private Settings mSettings;
    [SerializeField] private PauseScript mPauseScript;
    public bool isPauseAbleScene = false;
    public AudioSource buttonSound;
    public bool isLeft = true;

    public void HandleClick()
    {
        if (isLeft)
        {
            if (isPauseAbleScene)
            {
                mPauseScript.ResLeft();
            }
            else
            {
                mSettings.ResLeft();
            }
        }
        else
        {
            if (isPauseAbleScene)
            {
                mPauseScript.ResRight();
            }
            else
            {
                mSettings.ResRight();
            }
        }

        buttonSound.Play();
    }
}