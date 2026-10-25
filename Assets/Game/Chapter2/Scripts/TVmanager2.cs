using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVmanager2 : MonoBehaviour
{
    [SerializeField] private GameObject video1, video2red, video2yellow, renderTxt;
    [SerializeField] private AudioSource music;
    [SerializeField] private TimeManager _TimeManager;
    public bool isCassetteInserted = false;
    [SerializeField] private ManiacMove2 _ManiacMove;
    public bool isRed = false;
    [SerializeField] private Sprite red_casset, yellow_casset;
    public InventoryManager inventoryManager;
    public void Start_Scene_Film_Casset(string red_or_yellow)
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
        if (red_or_yellow == "red")
        {
            StartCoroutine(film_part2("red"));
            isRed = true;
        }
        if (red_or_yellow == "yellow")
        {
            StartCoroutine(film_part2("yellow"));
            isRed = false;
        }
    }
    private IEnumerator film_part2(string red_or_yellow)
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

        if (red_or_yellow == "red")
        {
            var videoPlayer = video2red.GetComponent<VideoPlayer>();
            video2red.SetActive(true);
            videoPlayer.loopPointReached -= OnVideoEnd; // Отписка
            videoPlayer.loopPointReached += OnVideoEnd; // Подписка
            videoPlayer.Play();
        }
        else if (red_or_yellow == "yellow")
        {
            var videoPlayer = video2yellow.GetComponent<VideoPlayer>();
            video2yellow.SetActive(true);
            videoPlayer.loopPointReached -= OnVideoEnd; // Отписка
            videoPlayer.loopPointReached += OnVideoEnd; // Подписка
            videoPlayer.Play();
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
            if (isRed)
            {
                video2red.SetActive(true);
                video2red.GetComponent<VideoPlayer>().Play();
            }
            else
            {
                video2yellow.SetActive(true);
                video2yellow.GetComponent<VideoPlayer>().Play();
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

        if (isRed)
        {
            video2red.GetComponent<VideoPlayer>().Stop();
            video2red.SetActive(false);
        }
        else
        {
            video2yellow.GetComponent<VideoPlayer>().Stop();
            video2yellow.SetActive(false);
        }

        video1.GetComponent<AudioSource>().Stop();
        video1.SetActive(false);
        if (!_TimeManager.maniac_movements)
        {
            music.volume = 0.75f;
        }
    }

    private void OnVideoEnd(VideoPlayer player)
    {
        isCassetteInserted = false;
        if (isRed)
        {
            inventoryManager.AddItemToInventory(red_casset);
            video2red.SetActive(false);
        }
        else
        {
            inventoryManager.AddItemToInventory(yellow_casset);
            video2yellow.SetActive(false);
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
