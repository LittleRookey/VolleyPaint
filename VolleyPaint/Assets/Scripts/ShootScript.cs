using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public GameObject ball;
    public float speed;

    public float shootBallDist = 3f;

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
                Debug.Log("ball shot");
                // rotates ball in direction of camera
                ball.transform.rotation = transform.rotation;
                
                // 0s velocity and adds forward force
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                //rb.velocity = Vector3.zero;
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

        return reticleToBall <= shootBallDist;
    }

    private bool isPointingBall()
    {
        RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(transform.position, transform.forward, out hit, shootBallDist))
        {
            return hit.transform.GetComponent<VolleyBall>() != null;
        }
        return false;
    }

}