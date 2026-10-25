using UnityEngine;

public class AnimateOnce : MonoBehaviour
{
    private bool hasPlayed = false; // ‘лаг, чтобы отслеживать, была ли анимаци¤ уже сыграна
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (!hasPlayed)
        {
            animator.Play("second_lozhka");
            hasPlayed = true; // ”станавливаем флаг после первого воспроизведени¤
        }
    }
}
