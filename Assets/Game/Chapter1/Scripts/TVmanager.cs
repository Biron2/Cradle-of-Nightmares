using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVmanager : MonoBehaviour
{
    [SerializeField] private GameObject video1, video2en, video2ru, renderTxt;
    [SerializeField] private AudioSource music;
    [SerializeField] private TimeManager _TimeManager;
    public bool isCassetteInserted = false;
    public int language;
    [SerializeField] private ManiacMove _ManiacMove;

    private void Awake()
    {
        language = PlayerPrefs.GetInt("language", language);
    }
    public void Start_Scene_Film_Casset()
    {
        _ManiacMove.AllowMovement = false;
        renderTxt.SetActive(false);
        isCassetteInserted = true;
        if (!_TimeManager.maniac_movements)
        {
            music.volume = 0.25f;
        }
        video1.GetComponent<VideoPlayer>().Stop();
        video1.GetComponent<AudioSource>().Stop();
        video1.SetActive(false);
        StartCoroutine(film_part2());
    }
    private IEnumerator film_part2()
    {
        yield return new WaitForSeconds(3);
        renderTxt.SetActive(true);
        video1.SetActive(true);
        video1.GetComponent<VideoPlayer>().Play();
        video1.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        video1.GetComponent<VideoPlayer>().Stop();
        video1.GetComponent<AudioSource>().Stop();
        video1.SetActive(false);

        if (language == 0)
        {
            video2en.SetActive(true);
            video2en.GetComponent<VideoPlayer>().Play();
            video2en.GetComponent<VideoPlayer>().loopPointReached += OnVideoEnd;
        }
        else if (language == 1)
        {
            video2ru.SetActive(true);
            video2ru.GetComponent<VideoPlayer>().Play();
            video2ru.GetComponent<VideoPlayer>().loopPointReached += OnVideoEnd;
        }
    }
    public void Start_Videos()
    {
        if (isCassetteInserted)
        {
            if (!_TimeManager.maniac_movements)
            {
                music.volume = 0.25f;
            }
            if (language == 0)
            {
                video2en.SetActive(true);
                video2en.GetComponent<VideoPlayer>().Play();
            }
            else if (language == 1)
            {
                video2ru.SetActive(true);
                video2ru.GetComponent<VideoPlayer>().Play();
            }
        }
        else
        {
            video1.SetActive(true);
            video1.GetComponent<VideoPlayer>().Play();
            video1.GetComponent<AudioSource>().Play();
        }

    }
    public void Stop_Videos()
    {
        video1.GetComponent<VideoPlayer>().Stop();
        if (language == 0)
        {
            video2en.GetComponent<VideoPlayer>().Stop();
        }
        else if (language == 1)
        {
            video2ru.GetComponent<VideoPlayer>().Stop();
        }
        video1.GetComponent<AudioSource>().Stop();
        video1.SetActive(false);
        if (language == 0)
        {
            video2en.SetActive(false);
        }
        else if (language == 1)
        {
            video2ru.SetActive(false);
        }
        if (!_TimeManager.maniac_movements)
        {
            music.volume = 0.75f;
        }
    }

    private void OnVideoEnd(VideoPlayer player)
    {
        isCassetteInserted = false;
        if (language == 0)
        {
            video2en.GetComponent<VideoPlayer>().Stop();
            video2en.SetActive(false);
        }
        else if (language == 1)
        {
            video2ru.GetComponent<VideoPlayer>().Stop();
            video2ru.SetActive(false);
        }
        video1.SetActive(true);
        video1.GetComponent<VideoPlayer>().Play();
        video1.GetComponent<AudioSource>().Play();
        if (!_TimeManager.maniac_movements)
        {
            music.volume = 0.75f;
        }
        _ManiacMove.AllowMovement = true;
    }
}
