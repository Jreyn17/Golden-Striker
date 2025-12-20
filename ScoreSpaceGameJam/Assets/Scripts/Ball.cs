using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] SpriteRenderer sr;

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
        rb.gravityScale = 0f;
        ColorPicker();
        StartCoroutine("WaitToLaunch");
    }

    void ColorPicker()
    {
        sr.color = ballColors[Random.Range(0, ballColors.Count)];
    }
    

    IEnumerator WaitToLaunch()
    {
        yield return new WaitForSeconds(3f);
        rb.gravityScale = 0.85f;
        rb.linearVelocity = new Vector2(0, 13);
    }
}
