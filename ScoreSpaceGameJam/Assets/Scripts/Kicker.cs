using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Kicker : MonoBehaviour
{
    [Header("Kick Feel")]
    [SerializeField] private float baseImpulse = 0.4f;
    [SerializeField] private float impulseScale = 0.16f;
    [SerializeField] private float minSwipeSpeed = 5f;
    [SerializeField] private float maxSwipeSpeed = 35f;
    [SerializeField] private float snapExponent = 1.6f;

    [Header("Direction")]
    [SerializeField] private float pushAwayBias = 0.2f;   //blends into direction
    [SerializeField] private float kickCooldown = 0.25f;  //cooldown between kicks on same ball

    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody2D kickerRb;

    private Vector2 prevMouseWorld;
    private Vector2 swipeVelocity;
    private Vector2 targetPos;

    [SerializeField] AudioSource impactSource;
    [SerializeField] AudioClip impact;

    private void Awake()
    {
        //World coords
        prevMouseWorld = GetMouseWorld();
        targetPos = prevMouseWorld;
    }

    private void Update()
    {
        //Update world coords
        Vector2 mouseWorld = GetMouseWorld();
        targetPos = mouseWorld;

        //Take current coords vs previous coords to find swipe velocity
        swipeVelocity = (mouseWorld - prevMouseWorld) / Mathf.Max(Time.deltaTime, 0.0001f);
        prevMouseWorld = mouseWorld;
    }

    private void FixedUpdate()
    {
        kickerRb.MovePosition(targetPos);
    }

    private Vector2 GetMouseWorld()
    {
        //Get Mouse World Coords
        Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(worldPos.x, worldPos.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D other = collision.collider;

        if (!other.CompareTag("Ball")) return;
        if (!other.TryGetComponent<Rigidbody2D>(out var rb)) return;
        if (!other.TryGetComponent<Ball>(out var ball)) return;

        impactSource.pitch = Random.Range(0.6f, 1.4f);
        impactSource.PlayOneShot(impact);

        if (Time.time < ball.nextKickTime) return;

        float speed = swipeVelocity.magnitude;
        if (speed < minSwipeSpeed) return;

        //Clamp speed so it doesn't launch ball into space
        speed = Mathf.Min(speed, maxSwipeSpeed);

        //Swipe direction
        Vector2 dir = swipeVelocity.normalized;

        Vector2 awayRaw = (Vector2)rb.position - (Vector2)kickerRb.position;
        Vector2 away = awayRaw.sqrMagnitude > 0.0001f ? awayRaw.normalized : Vector2.zero;

        //Mix in push away bias
        dir = (dir + away * pushAwayBias).normalized;

        //Raise speed to the power of snapExponent
        float snapped = Mathf.Pow(speed, snapExponent);

        //Final impulse amount
        float impulse = baseImpulse + impulseScale * snapped;

        //Add the Impulse Force
        rb.AddForce(dir * impulse, ForceMode2D.Impulse);

        //Add in rotation
        rb.AddTorque(Random.Range(-1f, 1f) * (impulse * 0.15f), ForceMode2D.Impulse);

        //Next kick time waits for after the kickCooldown
        ball.nextKickTime = Time.time + kickCooldown;
    }
}