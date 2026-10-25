using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getOutside_a_car : MonoBehaviour
{
    [SerializeField] private GameObject objectToOff;
    public bool denyClose = false;
    private audio_for_items _Audio_For_Items;
    [SerializeField] private getInside_a_car _GetInside_A_Car;
    [SerializeField] private AudioSource getOutsideTheCar;
    [SerializeField] private GameObject mLamp, carLamp;
    [SerializeField] private AudioSource mLampSnd;

    [SerializeField] private Sprite closedCarSprite;
    [SerializeField] private Image carImage;
    private void Start()
    {
        _Audio_For_Items = GameObject.Find("Other/GameManager/EtcSounds/__FOR_ITEMS__").GetComponent<audio_for_items>();
        public_actions.Instance.closeMiniGameAction += ExitCar;
    }
    private void OnDestroy()
    {
        // Важно отписаться, чтобы не было утечек
        public_actions.Instance.closeMiniGameAction -= ExitCar;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            ExitCar();
        }
    }

    private void ExitCar()
    {
        if (!denyClose && VariableDeterminator.playerIsInTheCar && !VariableDeterminator.carClimpingProcessRunning)
        {
            VariableDeterminator.playerIsInTheCar = false;
            _Audio_For_Items.PlaySounds(gameObject.name);
            public_actions.Instance.OnOff_SpaceTip_ExitCar(false);
            VariableDeterminator.isMiniGameActive = false;
            VariableDeterminator.carClimpingProcessRunning = true;

            _GetInside_A_Car.startGetOutside(objectToOff);
            getOutsideTheCar.PlayOneShot(getOutsideTheCar.clip);

            carImage.sprite = closedCarSprite;
        }
    }

    public void closeThisGame()
    {
        if (denyClose == false)
        {
            if (objectToOff.activeSelf)
            {
                VariableDeterminator.isMiniGameActive = false;
                public_actions.Instance.OnOff_SpaceTip_ExitCar(false);
                objectToOff.SetActive(false);
                mLamp.SetActive(true);
                mLampSnd.volume = 1f;
                carLamp.SetActive(true);
                carImage.sprite = closedCarSprite;
            }
        }
    }
}
