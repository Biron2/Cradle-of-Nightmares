using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class rndclckgmStart : MonoBehaviour
{
    [SerializeField] public List<GameObject> tarakans;
    public float interval = 1f;
    private GameObject currentObject;
    public AudioSource tarakanSpawned;
    private GameObject ratNum;
    private void OnEnable()
    {
        StartCoroutine(RandomizeTarakanWithInterval());
    }
    public IEnumerator RandomizeTarakanWithInterval()
    {
        if (tarakans.Count != 0)
        {
            tarakanSpawned.PlayOneShot(tarakanSpawned.clip);
            DisableCurrentTarakan();
            RandomizeTarakan();
        }
        yield return new WaitForSeconds(interval);
        StartCoroutine(RandomizeTarakanWithInterval());
    }
    public void RandomizeTarakan()
    {
        int randomIndex = Random.Range(0, tarakans.Count);
        currentObject = tarakans[randomIndex];
        currentObject.SetActive(true);
    }
    public void DisableCurrentTarakan()
    {
        if (currentObject != null && tarakans.Contains(currentObject))
        {
            currentObject.SetActive(false);
        }
    }
    public void mRemove_fromList(string s)
    {
        ratNum = GameObject.Find("Rooms/Room12/Image/RandomClickGame/Tarakans/" + s);
        tarakans.Remove(ratNum);
    }
}
