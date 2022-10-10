using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
using Unity.Netcode;

public enum eShootType
{
    Normal = 0, // simply pushes the ball to the front 
    Jump = 1, // makes the ball jump by certain amount
}

public class PlayerShoot : NetworkBehaviour
{
    // References
    [Header("References")]    
    [SerializeField] private Transform bulletPrefab; 

    [SerializeField] private Transform gunShootPos;

    [Header("Shooting")]
    [SerializeField] private eBulletType bulletType = eBulletType.Hitscan;
    
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] public eShootType shootType;
    [SerializeField] private GameObject normal_hitImpact;

    [SerializeField] private float ballSpeed;
    [SerializeField] private float ballJumpSpeed = 900f;

    private float fireRateCountDown = 0;

    Vector3 MID_SCREEN = new Vector3(Screen.width / 2, Screen.height / 2, 0f);

    private Transform ballTransform;
    private Transform camTransform;

    // Start is called before the first frame update
    void Start()
    {
        ballTransform = GameObject.FindGameObjectWithTag("Ball").transform;
        camTransform = Camera.main.transform;
        bulletType = eBulletType.Hitscan;
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        fireRateCountDown += Time.deltaTime;

        if (Input.GetButton("Fire1") && fireRateCountDown >= fireRate)
        {
            fireRateCountDown = 0f;

            Vector3 camDir = camTransform.forward;
            bool isBallShot = IsBallShotRaycast();

            OnPlayerShoot(camDir);
            OnPlayerShootServerRPC(camDir);

            if (isBallShot && bulletType == eBulletType.Hitscan)
            {
                Vector3 ballPos = ballTransform.position;

                InitiateShootBall(ballPos, camDir);
            }
        }
    }
    
    // determines if some part of ball is on center of screen using raycast
    private bool IsBallShotRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(MID_SCREEN);
        //Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name);
            return hit.collider.gameObject.CompareTag("Ball");
        }
        return false;
    }

    // made into public helper function so Bullet.cs could call this when using a projectile bullet
    public void InitiateShootBall(Vector3 position, Vector3 direction)
    {
        ShootBall(position, direction);
        ShootBallServerRPC(position, direction);
    }

    private void ShootBall(Vector3 position, Vector3 direction)
    {
        Rigidbody rb = ballTransform.GetComponent<Rigidbody>();
        switch (shootType)
        {
            case eShootType.Normal:
                // updates ball position to where it was hit so all ball instances across clients should take the same path
                ballTransform.position = position;

                // 0s velocity and adds forward force
                rb.isKinematic = false;
                rb.velocity = Vector3.zero;
                rb.AddForce(direction * ballSpeed);
                break;
            case eShootType.Jump:
                // updates ball position to where it was hit so all ball instances across clients should take the same path
                ballTransform.position = position;

                // 0s velocity and adds forward force
                rb.isKinematic = false;
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.up * ballJumpSpeed);
                break;
        }
        Debug.Log("ball shot");
    }

    private void OnPlayerShoot(Vector3 camDir)
    {
        MasterAudio.PlaySound3DAtVector3("SilencedPistol", transform.position);
        
        // spawn bullet
        Instantiate(bulletPrefab, gunShootPos.position, Quaternion.LookRotation(camDir));
    }


    [ServerRpc] // client tells server to do something
    private void ShootBallServerRPC(Vector3 position, Vector3 direction)
    {
        ShootBallClientRPC(position, direction);
    }

    [ClientRpc] // server telling the clients to do something
    private void ShootBallClientRPC(Vector3 position, Vector3 direction)
    {
        if (IsOwner) return; // ignore client that shot ball since force was already added
        ShootBall(position, direction);
    }


    [ServerRpc]
    private void OnPlayerShootServerRPC(Vector3 camDir)
    {
        OnPlayerShootClientRPC(camDir);
    }

    [ClientRpc] // server telling the clients to do something
    private void OnPlayerShootClientRPC(Vector3 camDir)
    {
        if (IsOwner) return; // ignore client that shot ball since force was already added
        OnPlayerShoot(camDir);
    }


    // determines if some part of ball is on center of screen using vectors
    // (currently not used but could be needed if we dont care about shooting through walls and care about performance)
    private bool IsBallShot()
    {
        Vector3 ballPos = ballTransform.position;
        Vector3 cameraPos = camTransform.position;

        float camToBall = Vector3.Distance(ballPos, cameraPos);
        Vector3 camVector = camToBall * camTransform.forward;
        float reticleToBall = Vector3.Distance(ballPos, cameraPos + camVector);

        return reticleToBall <= ballTransform.localScale.x / 2;
    }

    // Replenishes player ammo
    public void ReplenishAmmo()
    {

    }
}