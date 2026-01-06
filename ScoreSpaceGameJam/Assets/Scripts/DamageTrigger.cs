using UnityEngine;
using System;

public class DamageTrigger : MonoBehaviour
{

    public enum TriggerType { Top, Bottom };
    public TriggerType currentTriggerType;

    [SerializeField] BoxCollider2D boxCollider;

    [SerializeField] AudioSource lifeSource;
    [SerializeField] AudioClip minusLife;

    public static event Action<int> OnLoseLife;

    void OnTriggerStay2D(Collider2D other)
    {
        Bounds net = boxCollider.bounds;
        Bounds ball = other.bounds;

        Rigidbody2D ballRb = other.GetComponent<Rigidbody2D>();
        if (ballRb == null) return;

        bool fullyInside =
            ball.min.x >= net.min.x && ball.max.x <= net.max.x &&
            ball.min.y >= net.min.y && ball.max.y <= net.max.y;

        if (!fullyInside) return;
        if (currentTriggerType == TriggerType.Top)
        {
            if (ballRb.linearVelocity.y > 0f)
            {
                Debug.Log("You lost a life");


                lifeSource.PlayOneShot(minusLife);
                OnLoseLife?.Invoke(1); //Event goes to GameManager & StreakManager

                /*GameManager.Instance?.LoseLife(1);*/

                Destroy(other.gameObject);
            }
        }
        else if (currentTriggerType == TriggerType.Bottom)
        {
            if (ballRb.linearVelocity.y < 0f)
            {
                Debug.Log("You lost a life");

                lifeSource.PlayOneShot(minusLife);
                OnLoseLife?.Invoke(1); //Event goes to GameManager & StreakManager

                /*GameManager.Instance?.LoseLife(1);*/

                Destroy(other.gameObject);
            }
        }
    }
}
