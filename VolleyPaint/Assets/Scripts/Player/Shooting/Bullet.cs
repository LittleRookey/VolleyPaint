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
        Destroy(gameObject, 2f);
    }

    public void SetBullet(GameObject hitImpact)
    {
        this.hitImpact = hitImpact;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bulletType == eBulletType.Projectile && collision.gameObject.CompareTag("Ball"))
        {
            PlayerShoot shootScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();
            shootScript.GetComponent<PlayerShoot>().InitiateShootBall(collision.transform.position, transform.forward, shootScript.ballSpeed);
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
