using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class PlayerShoot : MonoBehaviour
{
    // References
    [Header("References")]
    [SerializeField] private GunController gunController;
    
    [SerializeField] private Transform pfBullet;

    [SerializeField] private Transform gunShootPos;

    [Header("Shooting")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private eBulletType shootType;
    [SerializeField] private GameObject normal_hitImpact;

    private float fireRateCountDown = 0;

    Vector3 MID_SCREEN = new Vector3(Screen.width / 2, Screen.height / 2, 0f);


    //private void Awake()
    //{
    //    //GetComponent<PlayerController>().OnShoot += PlayerShootBullet_OnShoot;
    //}

    private void PlayerShootBullet_OnShoot()
    {
        MasterAudio.PlaySound3DAtVector3("SilencedPistol", transform.position);
        Transform bulletTransform = Instantiate(pfBullet, gunShootPos.position, Quaternion.identity);
        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        //Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 shootDir = Camera.main.transform.forward;
        bulletTransform.GetComponent<Bullet>().Setup(shootDir, bulletSpeed, shootType, normal_hitImpact);
    }

    void Update()
    {
        fireRateCountDown += Time.deltaTime;
        if (Input.GetButton("Fire1") && fireRateCountDown >= fireRate)
        {
            fireRateCountDown = 0f;
            PlayerShootBullet_OnShoot();
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gunController.ChangeGunMode(eBulletType.Normal);
            shootType = eBulletType.Normal;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gunController.ChangeGunMode(eBulletType.Jump);
            shootType = eBulletType.Jump;
        }
    }

    // determines if some part of ball is on center of screen
    //private bool IsBallShot()
    //{
    //    float camToBall = Vector3.Distance(ball.transform.position, transform.position);
    //    Vector3 camVector = camToBall * transform.forward;
    //    float reticleToBall = Vector3.Distance(ball.transform.position, transform.position + camVector);

    //    return reticleToBall <= shootBallDist;
    //}

    //private bool isPointingBall()
    //{
    //    RaycastHit hit;
    //    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
    //    //Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition);


    //    if (Physics.Raycast(ray, out hit, shootBallDist))
    //    {
    //        Debug.Log(hit.transform.name);
    //        return hit.transform.GetComponent<VolleyBall>() != null;
    //    }
    //    return false;
    //}

}
