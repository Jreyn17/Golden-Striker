using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider;

    void OnTriggerEnter2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball == null) return;

        Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
        if (otherRb == null) return;

        //Checks if the y velocity is negative and if the ball made it to height
        if (otherRb.linearVelocity.y < 0f)
        {
            Debug.Log("You lost a life");

            //LOSS OF 1 LIFE CODE
        }
    }
}
