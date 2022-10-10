using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DarkTonic.MasterAudio;
public enum eBulletType
{
    Projectile = 0,
    Hitscan = 1
};

public class Bullet : MonoBehaviour
{
    public float moveSpeed;

    eBulletType bulletType = eBulletType.Hitscan;

    public GameObject hitImpact;

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * moveSpeed, ForceMode.VelocityChange);
        //Destroy(gameObject, 2f);
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bulletType == eBulletType.Projectile && collision.gameObject.CompareTag("Ball"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>().InitiateShootBall(collision.transform.position, transform.forward);
        }

        ContactPoint contact = collision.GetContact(0);

        GameObject hitImpactObj = Instantiate(hitImpact);
        hitImpactObj.transform.position = contact.point;
        hitImpactObj.transform.forward = contact.normal;

        MasterAudio.PlaySound3DAtVector3("OnHit_Ball", transform.position);

        Destroy(hitImpactObj, 1f);

        Destroy(gameObject);
    }
}
