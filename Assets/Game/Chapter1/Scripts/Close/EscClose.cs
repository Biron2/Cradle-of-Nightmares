using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscClose : MonoBehaviour
{
    [SerializeField] private GameObject objectToOff;
    [SerializeField] private GameObject[] arrows_containers;
    private ColliderList colliderList;
    public bool denyClose = false;
    private audio_for_items _Audio_For_Items;
    private void Start()
    {
        _Audio_For_Items = GameObject.Find("Other/GameManager/EtcSounds/__FOR_ITEMS__").GetComponent<audio_for_items>();
        colliderList = GameObject.Find("Other/GameManager/ColliderList").GetComponent<ColliderList>();

        public_actions.Instance.closeMiniGameAction += OnMinigameClosed;
    }
    private void OnDestroy()
    {
        // Важно отписаться, чтобы не было утечек
        public_actions.Instance.closeMiniGameAction -= OnMinigameClosed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            OnMinigameClosed();
        }
    }
    private void OnMinigameClosed()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        if (denyClose == false)
        {
            _Audio_For_Items.PlaySounds(gameObject.name);
            if (gameObject.name.Contains("TipFile"))
            {
                public_actions.Instance.OnOff_SpaceTipFile_Close(false);
            }
            else
            {
                public_actions.Instance.OnOff_SpaceTip_Close(false);
            }
            objectToOff.SetActive(false);
            foreach (GameObject obj in arrows_containers)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
            VariableDeterminator.isMiniGameActive = false;
            colliderList.ENABLE_COLLIDER();
        }
    }
    public void closeThisGame()
    {
        if (denyClose == false)
        {
            if (objectToOff.activeSelf)
            {
                VariableDeterminator.isMiniGameActive = false;
                public_actions.Instance.OnOff_SpaceTip_Close(false);

                if (gameObject.name.Contains("TipFile"))
                {
                    public_actions.Instance.OnOff_SpaceTipFile_Close(false);
                }
                else
                {
                    public_actions.Instance.OnOff_SpaceTip_Close(false);
                }

                objectToOff.SetActive(false);
            }
        }
    }
}
