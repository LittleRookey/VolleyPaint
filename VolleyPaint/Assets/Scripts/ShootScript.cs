using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShootScript : NetworkBehaviour
{
    GameObject ball;
    public float speed;

    public float ballSize = 3f;

    //public override void OnNetworkSpawn()
    //{
    //    if (!IsOwner)
    //    {
    //        this.enabled = false;
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetButtonDown("Fire1"))
        {
            if (isPointingBall())
            {
                ShootBall(ball.transform.position, transform.rotation);
                ShootBallServerRPC(ball.transform.position, transform.rotation);
            }
        }
    }

    // determines if some part of ball is on center of screen
    private bool IsBallShot()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 cameraDir = Camera.main.transform.forward;
        Vector3 ballPos = ball.transform.position;

        float camToBall = Vector3.Distance(ballPos, cameraPos);
        Vector3 camVector = camToBall * cameraDir;
        float reticleToBall = Vector3.Distance(ballPos, cameraPos + camVector);

        return reticleToBall <= ballSize;
    }

    private bool isPointingBall()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        //Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit);
        //if (Physics.Raycast(ray, out hit))
        //{
        //    Debug.Log(hit.transform.name);
        //    return hit.transform.GetComponent<VolleyBall>() != null;
        //}
        //return false;
    }
    
    private void ShootBall(Vector3 position, Quaternion rotation)
    {
        Debug.Log("ball shot");
        // rotates ball in direction of camera
        ball.transform.position = position;
        ball.transform.rotation = rotation;

        // 0s velocity and adds forward force
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.AddForce(ball.transform.forward * speed);
    }

    [ServerRpc]
    private void ShootBallServerRPC(Vector3 position, Quaternion rotation)
    {
        print("server rpc");
        ShootBallClientRPC(position, rotation);
    }

    [ClientRpc]
    private void ShootBallClientRPC(Vector3 position, Quaternion rotation)
    {
        if (IsOwner) return; // ignore client that shot ball since force was already added
        ShootBall(position, rotation);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawLine(transform.position, transform.forward * shootBallDist);
    //}
}
