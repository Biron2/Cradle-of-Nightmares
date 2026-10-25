using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFires : MonoBehaviour
{
    public Image[] fires;
    private int initialFaceIndex = 0;
    private int currentFaceIndex = 0; // Текущий индекс формы лица
    private void Start()
    {
        initialFaceIndex = currentFaceIndex;
        StartCoroutine(changeFiresCor());
    }
    private IEnumerator changeFiresCor()
    {
        yield return new WaitForSeconds(0.100f);
        SetNextFace();
        StartCoroutine(changeFiresCor());
    }
    private void SetNextFace()
    {
        fires[currentFaceIndex].gameObject.SetActive(false);

        currentFaceIndex++;
        if (currentFaceIndex >= fires.Length)
        {
            currentFaceIndex = initialFaceIndex;
        }

        fires[currentFaceIndex].gameObject.SetActive(true);
    }
}
