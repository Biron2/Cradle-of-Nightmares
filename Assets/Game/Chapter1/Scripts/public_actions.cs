using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class public_actions : MonoBehaviour
{
    private ParticleSystem.MainModule mainModule1, mainModule2;
    [SerializeField] private ParticleSystem[] determineParticles;
    [SerializeField] private GameObject lightPodval;
    [SerializeField] private AudioSource back_music;
    private float fadeDuration = 1f; // Продолжительность затухания
    [SerializeField] private GameObject slidingMiniGame, key_sliding, key_of_car_item, photoOld, photoNew;
    [SerializeField] private Text TIP_TextDrying;
    public int language;

    public static public_actions Instance;
    [SerializeField] private GameObject[] objects;

    public Action closeMiniGameAction;

    [SerializeField] private AudioSource robot;
    [SerializeField] private Animator animator;
    [SerializeField] private Image eyeImage1, eyeImage2;
    [SerializeField] private Sprite eyeSprite1, eyeSprite2, eyeSpriteCommon;

    private void Start()
    {
        try
        {
            mainModule1 = determineParticles[0].main;
            mainModule2 = determineParticles[1].main;
        }catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void startFadeOutMusic()
    {
        StartCoroutine(fadeOutBackMusic());
    }
    public void stopElectroParticle()
    {
        mainModule2.startSize = 0f;
        Invoke(nameof(offLight), 0.5f);
    }
    public void stopElectroRoom10()
    {
        mainModule1.startSize = 0f;
    }
    private void offLight()
    {
        lightPodval.SetActive(false);
    }
    private IEnumerator fadeOutBackMusic()
    {
        float startVolume = back_music.volume;

        while (back_music.volume > 0)
        {
            back_music.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        back_music.Stop();
        back_music.volume = startVolume;
    }
    public void StartDisableObjectsAfterFall()
    {
        StartCoroutine(DisableObjectsAfterFall());
    }
    private IEnumerator DisableObjectsAfterFall()
    {
        // Подожди, например, 2 секунды пока key_sliding падает
        yield return new WaitForSeconds(2f);
        key_of_car_item.SetActive(true);
        Destroy(key_sliding);
        slidingMiniGame.SetActive(false);
    }
    public void start_TIP_drying()
    {
        StartCoroutine(CountdownTextDrying());
    }

    public IEnumerator robotEyesChanging()
    {
        robot.Play();
        animator.Play("robot_anim_move");
        yield return new WaitForSeconds(0.5f);
        eyeImage1.sprite =eyeSprite1; //left eye
        eyeImage2.sprite = eyeSprite2; //right eye
        yield return new WaitForSeconds(0.3f);
        eyeImage1.sprite = eyeSpriteCommon; //left eye common
        eyeImage2.sprite = eyeSpriteCommon; //right eye common
        yield return new WaitForSeconds(0.3f);
        eyeImage1.sprite =eyeSprite1; //left eye
        eyeImage2.sprite = eyeSprite2; //right eye
        yield return new WaitForSeconds(0.3f);
        eyeImage1.sprite = eyeSpriteCommon; //left eye common
        eyeImage2.sprite = eyeSpriteCommon; //right eye common
        yield return new WaitForSeconds(0.3f);
        eyeImage1.sprite =eyeSprite1; //left eye
        eyeImage2.sprite = eyeSprite2; //right eye
        StartCoroutine(robotEyesLoop());
    }

    private IEnumerator robotEyesLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            eyeImage1.sprite = eyeSpriteCommon; //left eye common
            eyeImage2.sprite = eyeSpriteCommon; //right eye common
            yield return new WaitForSeconds(1f);
            eyeImage1.sprite =eyeSprite1; //left eye 
            eyeImage2.sprite = eyeSprite2; //right eye
            yield return new WaitForSeconds(1f);
            eyeImage1.sprite = eyeSpriteCommon; //left eye common
            eyeImage2.sprite = eyeSpriteCommon; //right eye common
            yield return new WaitForSeconds(1f);
            eyeImage1.sprite =eyeSprite1; //left eye 
            eyeImage2.sprite = eyeSprite2; //right eye
            yield return new WaitForSeconds(1f);
            eyeImage1.sprite = eyeSpriteCommon; //left eye common
            eyeImage2.sprite = eyeSpriteCommon; //right eye common
        }
    }

    private IEnumerator CountdownTextDrying()
    {
        TIP_TextDrying.gameObject.SetActive(true);
        int countdown = 30;

        while (countdown >= 0)
        {
            language = PlayerPrefs.GetInt("language", language);
            if (language == 0)
            {
                TIP_TextDrying.text = $"Drying {countdown} seconds";
            }
            else if (language == 1)
            {
                TIP_TextDrying.text = $"Сушение {countdown} секунд";
            }
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        if (language == 0)
        {
            TIP_TextDrying.text = "Drying is complete!";
        }
        else if (language == 1)
        {
            TIP_TextDrying.text = "Сушение завершено!";
        }
        photoOld.SetActive(false);
        photoNew.SetActive(true);
    }

    public void OnOff_SpaceTip_Close(bool state)
    {
#if PLAYMARKET
        objects[0]?.SetActive(state);
#else
        objects[1]?.SetActive(state);
#endif
    }

    public void OnOff_SpaceTipFile_Close(bool state)
    {
#if PLAYMARKET
        objects[2]?.SetActive(state);
#else
        objects[3]?.SetActive(state);
#endif
    }

    public void OnOff_SpaceTip_ExitCar(bool state)
    {
#if PLAYMARKET
        objects[2]?.SetActive(state);
#else
        objects[3]?.SetActive(state);
#endif
    }

    public void OnOff_SpaceTip_Zoom(bool state)
    {
#if PLAYMARKET
        objects[4]?.SetActive(state);
#else
        objects[5]?.SetActive(state);
#endif
    }

    public void CloseMinigame()
    {
        closeMiniGameAction?.Invoke();
    }
}
