using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public GameObject ball;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (IsBallShot())
            {
                // rotates ball in direction of camera
                ball.transform.rotation = transform.rotation;

                // 0s velocity and adds forward force
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.AddForce(ball.transform.forward * speed);
            }
        }
    }

    // determines if some part of ball is on center of screen
    private bool IsBallShot()
    {
        float camToBall = Vector3.Distance(ball.transform.position, transform.position);
        Vector3 camVector = camToBall * transform.forward;
        float reticleToBall = Vector3.Distance(ball.transform.position, transform.position + camVector);

        return reticleToBall <= ball.GetComponent<SphereCollider>().radius;
    }

}
