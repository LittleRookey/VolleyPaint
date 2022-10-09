using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
using Unity.Netcode;

public enum eShootType
{
    Projectile = 0,
    Hitscan = 1
};

public class PlayerShoot : NetworkBehaviour
{
    // References
    [Header("References")]
    [SerializeField] private GunController gunController;
    
    [SerializeField] private Transform pfBullet;

    [SerializeField] private Transform gunShootPos;

    [Header("Shooting")]
    [SerializeField] private eShootType shootType = eShootType.Hitscan;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private eBulletType bulletType;
    [SerializeField] private GameObject normal_hitImpact;

    private float fireRateCountDown = 0;

    Vector3 MID_SCREEN = new Vector3(Screen.width / 2, Screen.height / 2, 0f);

    public float speed;

    private Transform ballTransform;
    private Transform camTransform;

    public 
    // Start is called before the first frame update
    void Start()
    {
        ballTransform = GameObject.FindGameObjectWithTag("Ball").transform;
        camTransform = Camera.main.transform;
        shootType = eShootType.Hitscan;
    }


    // Update is called once per frame
    void Update()
    {
        fireRateCountDown += Time.deltaTime;
        if (!IsOwner) return;


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gunController.ChangeGunMode(eBulletType.Normal);
            bulletType = eBulletType.Normal;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gunController.ChangeGunMode(eBulletType.Jump);
            bulletType = eBulletType.Jump;
        }

        if (Input.GetButton("Fire1") && fireRateCountDown >= fireRate)
        {
            fireRateCountDown = 0f;
            if (IsBallShotRaycast())
            {
                Vector3 ballPos = ballTransform.position;
                Vector3 camDir = camTransform.forward;
                if (shootType == eShootType.Hitscan)
                {
                    ShootBall(ballPos, camDir);
                    ShootBallServerRPC(ballPos, camDir);
                } else if (shootType == eShootType.Projectile)
                {

                }
            }
            // TODO shoot visuals? 
            PlayerShootBullet_OnShoot(shootType == eShootType.Hitscan);
        }
    }

    // determines if some part of ball is on center of screen using raycast
    private bool IsBallShotRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        //Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name);
            return hit.collider.gameObject.tag == "Ball";
        }
        //Debug.Log(hit.collider.name);
        return false;
    }

    private void ShootBall(Vector3 position, Vector3 direction)
    {
        Rigidbody rb = ballTransform.GetComponent<Rigidbody>();
        switch (bulletType)
        {
            
            case eBulletType.Normal:
                // updates ball position to where it was hit so all ball instances across clients should take the same path
                ballTransform.position = position;

                // 0s velocity and adds forward force
                
                rb.isKinematic = false;
                rb.velocity = Vector3.zero;
                rb.AddForce(direction * speed);
                break;
                //rb.velocity = Vector3.zero;
                //vb.transform.rotation = Camera.main.transform.rotation;
                //rb.isKinematic = false;
                //rb.velocity = Vector3.zero;
                //rb.AddForce(vb.transform.forward * 900f);
                //break;
            case eBulletType.Jump:
                //vb.transform.rotation = Camera.main.transform.rotation;
                rb.isKinematic = false;
                //ball_rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.up * 900f);
                break;

        }

        Debug.Log("ball shot");
        
    }

    [ServerRpc]
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

    //private void Awake()
    //{
    //    //GetComponent<PlayerController>().OnShoot += PlayerShootBullet_OnShoot;
    //}

    private void PlayerShootBullet_OnShoot(bool hitScan = true)
    {
        MasterAudio.PlaySound3DAtVector3("SilencedPistol", transform.position);
        
        Transform bulletTransform = Instantiate(pfBullet, gunShootPos.position, Quaternion.identity);
        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        //Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 shootDir = Camera.main.transform.forward;
        bulletTransform.GetComponent<Bullet>().Setup(shootDir, bulletSpeed, normal_hitImpact, hitScan);
    }

    // determines if some part of ball is on center of screen
    //private bool IsBallShot()
    //{
    //    float camToBall = Vector3.Distance(ball.transform.position, transform.position);
    //    Vector3 camVector = camToBall * transform.forward;
    //    float reticleToBall = Vector3.Distance(ball.transform.position, transform.position + camVector);

    //    return reticleToBall <= shootBallDist;
    //}

    private bool isPointingBall()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        //Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.transform.name);
            return hit.transform.CompareTag("Ball");
        }
        return false;
    }

}
