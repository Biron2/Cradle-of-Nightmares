using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using NUnit.Framework;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab; // Префаб шара
    public List<RectTransform> spawnPoints; // Точки спавна
    public float spawnIntervalMin = 1f; // Мин. задержка спавна
    public float spawnIntervalMax = 3f; // Макс. задержка спавна
    public float spawnRadius = 2f; // Радиус случайного смещения
    public int maxReachBalloons = 0;
    private Coroutine coroutine;
    [SerializeField] private GameObject mBalloonGameObject, finishedItemBalloonOnTheFloor, clownHandleBalloonOff, thread1, thread2, thread3, clownBody;
    [SerializeField] private GameObject[] arrows_containers;
    private ColliderList colliderList; 
    [SerializeField] private AudioSource winMiniGameSound;
    private void OnEnable()
    {
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();
        coroutine = StartCoroutine(SpawnBalloons());
    }
    private void OnDisable()
    {
        // Удаляем только объекты с префабом balloonPrefab и allowPress == true
        foreach (var spawnPoint in spawnPoints)
        {
            foreach (Transform child in spawnPoint)
            {
                BalloonSize balloon = child.GetComponent<BalloonSize>();

                if (balloon != null && balloon.allowDelete)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        // Останавливаем корутину (если она запущена)
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
    public void UpdateBalloonsInt()
    {
        maxReachBalloons++;
        if(maxReachBalloons == 9)
        {
            //действия после победы
            winMiniGameSound.PlayOneShot(winMiniGameSound.clip);
            VariableDeterminator.clownBalloonPuzzleFinished = true;
            mBalloonGameObject.SetActive(false);
            finishedItemBalloonOnTheFloor.SetActive(true);
            clownHandleBalloonOff.SetActive(false);
            VariableDeterminator.isMiniGameActive = false;
            public_actions.Instance.OnOff_SpaceTip_Close(false);
            foreach (GameObject obj in arrows_containers)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
            colliderList.ENABLE_COLLIDER();
            thread1.SetActive(false);
            thread2.SetActive(false);
            thread3.SetActive(false);
            clownBody.GetComponent<Animator>().enabled = false;
        }
    }
    IEnumerator SpawnBalloons()
    {
        int lastSpawnIndex = -1;

        while (true)
        {
            if (spawnPoints.Count > 0)
            {
                int spawnIndex;

                if (spawnPoints.Count == 1)
                {
                    spawnIndex = 0; // только один возможный
                }
                else
                {
                    // выбираем индекс, отличный от предыдущего
                    do
                    {
                        spawnIndex = Random.Range(0, spawnPoints.Count);
                    }
                    while (spawnIndex == lastSpawnIndex);
                }

                lastSpawnIndex = spawnIndex;

                RectTransform spawnPoint = spawnPoints[spawnIndex];

                // Инстанциируем и размещаем шарик
                RectTransform newBalloon = Instantiate(balloonPrefab, spawnPoint).GetComponent<RectTransform>();
                Vector2 localPosition = spawnPoint.InverseTransformPoint(spawnPoint.transform.position);
                newBalloon.anchoredPosition = localPosition;
            }

            float delay = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(delay);
        }
    }
}