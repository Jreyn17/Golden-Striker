using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Collider2D collider;

    public float nextKickTime;

    //Color dictionary for instantiating different balls
    private Dictionary<int, Color> ballColors = new Dictionary<int, Color>()
    {
        { 0, Color.white },
        { 1, Color.red },
        { 2, Color.green },
        { 3, Color.blue }
    };

    void Start()
    {
        //Disable gravity
        rb.gravityScale = 0f;

        ColorPicker();
        StartCoroutine("WaitToLaunch");
    }

    //Picks a random color for the ball
    void ColorPicker()
    {
        sr.color = ballColors[Random.Range(0, ballColors.Count)];
    }
    
    //Waits for some amount of seconds before launching the ball
    IEnumerator WaitToLaunch()
    {
        yield return new WaitForSeconds(3f);

        //Enable gravity 
        rb.gravityScale = 0.85f;

        rb.angularVelocity = Random.Range(-360f, 360f);

        //Set upward velocity
        rb.linearVelocity = new Vector2(Random.Range(-1f, 1f), 13f);
    }
}
