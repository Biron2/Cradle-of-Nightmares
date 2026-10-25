using UnityEngine;

public class HeartGameManager : MonoBehaviour
{
    [SerializeField] private dragHeart _DragHeart;
    [SerializeField] private AudioSource breath;
    [SerializeField] public AudioClip breathSlow, breathFast;

    public bool isNight = false;

    public void OnHeartClose()
    {
        if (isNight)
        {
            if (breath.clip != breathSlow || !breath.isPlaying)
            {
                breath.clip = breathSlow;
                if (!breath.isPlaying)
                {
                    breath.Play();
                }
            }
            _DragHeart.SetPulseSpeed(1);
        }
    }
    public void OnHeartFar1()
    {
        if (isNight)
        {
            if (breath.clip != breathFast || !breath.isPlaying)
            {
                breath.clip = breathFast;
                if (!breath.isPlaying)
                {
                    breath.Play();
                }
            }
            _DragHeart.SetPulseSpeed(2);
        }
    }
    public void OnHeartFar2()
    {
        if (isNight)
        {
            if (breath.clip != breathFast || !breath.isPlaying)
            {
                breath.clip = breathFast;
                if (!breath.isPlaying)
                {
                    breath.Play();
                }
            }
            _DragHeart.SetPulseSpeed(3);
        }
    }
    public void OnHeartFar3()
    {
        if (isNight)
        {
            if (breath.clip != breathFast || !breath.isPlaying)
            {
                breath.clip = breathFast;
                if (!breath.isPlaying)
                {
                    breath.Play();
                }
            }
            _DragHeart.SetPulseSpeed(4);
        }
    }
    public void OnHeartFar4()
    {
        if (isNight)
        {
            if (breath.clip != breathFast || !breath.isPlaying)
            {
                breath.clip = breathFast;
                if (!breath.isPlaying)
                {
                    breath.Play();
                }
            }
            _DragHeart.SetPulseSpeed(5);
        }
    }
    public void BreathReset()
    {
        breath.clip = breathSlow;
    }

}