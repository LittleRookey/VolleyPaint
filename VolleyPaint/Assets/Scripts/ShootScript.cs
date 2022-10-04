using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShootScript : NetworkBehaviour
{
    public float speed;

    private Transform ballTransform;
    private Transform camTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        ballTransform = GameObject.FindGameObjectWithTag("Ball").transform;
        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetButtonDown("Fire1"))
        {
            if (IsBallShotRaycast())
            {
                Vector3 ballPos = ballTransform.position;
                Vector3 camDir = camTransform.forward;

                ShootBall(ballPos, camDir);
                ShootBallServerRPC(ballPos, camDir);
            }
        }
    }

    // determines if some part of ball is on center of screen using vectors
    private bool IsBallShot()
    {
        Vector3 ballPos = ballTransform.position;
        Vector3 cameraPos = camTransform.position;

        float camToBall = Vector3.Distance(ballPos, cameraPos);
        Vector3 camVector = camToBall * camTransform.forward;
        float reticleToBall = Vector3.Distance(ballPos, cameraPos + camVector);

        return reticleToBall <= ballTransform.localScale.x / 2;
    }

    // determines if some part of ball is on center of screen using raycast
    private bool IsBallShotRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        //Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject.tag == "Ball";
        }
        return false;
    }
    
    private void ShootBall(Vector3 position, Vector3 direction)
    {
        Debug.Log("ball shot");
        // updates ball position to where it was hit so all ball instances across clients should take the same path
        ballTransform.position = position;

        // 0s velocity and adds forward force
        Rigidbody rb = ballTransform.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * speed);
    }

    [ServerRpc]
    private void ShootBallServerRPC(Vector3 position, Vector3 direction)
    {
        ShootBallClientRPC(position, direction);
    }

    [ClientRpc]
    private void ShootBallClientRPC(Vector3 position, Vector3 direction)
    {
        if (IsOwner) return; // ignore client that shot ball since force was already added
        ShootBall(position, direction);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawLine(transform.position, transform.forward * shootBallDist);
    //}
}
