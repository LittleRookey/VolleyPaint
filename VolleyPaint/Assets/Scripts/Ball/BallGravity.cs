using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGravity : MonoBehaviour
{
    public float gravity;
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // cache reference since GetComponent is expensive
    }

    void FixedUpdate()
    {  // use FixedUpdate for physics stuff
        rb.AddForce(new Vector3(0f, gravity, 0f));
    }
}
