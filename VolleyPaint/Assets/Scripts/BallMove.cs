using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 lastVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var xStrength = Random.Range(300, 500) * (Random.Range(0, 1) == 0 ? -1 : 1);
        var yStrength = Random.Range(300, 500) * (Random.Range(0, 1) == 0 ? -1 : 1);
        Debug.Log(xStrength + " " + yStrength);
        rb.AddForce(new Vector2(xStrength, yStrength));
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        rb.velocity = direction * Mathf.Max(speed, 0f);
    }
}
