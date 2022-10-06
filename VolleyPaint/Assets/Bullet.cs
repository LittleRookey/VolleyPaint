using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DarkTonic.MasterAudio;
public enum eBulletType
{
    Normal = 0, // simply pushes the ball to the front 
    Jump = 1, // makes the ball jump by certain amount
}

public class Bullet : MonoBehaviour
{
    float moveSpeed = 100f;
    private Vector3 shootDir;

    eBulletType bulletType;
    
    /** External Forces **/
    float ballShootForwardForce = 900f;
    float ballJumpUpForce = 900f;

    bool isHitScan;
    GameObject hitImpact;

    public void Setup(Vector3 shootDir, float moveSpeed, GameObject hitImpact, bool isHitScan = true)
    {
        this.moveSpeed = moveSpeed;
        this.shootDir = shootDir;
        this.hitImpact = hitImpact;
        this.isHitScan = isHitScan;

        Destroy(gameObject, 5f);
        if (hitImpact.GetComponent<AutoDestroy>() == null)
        {
            hitImpact.AddComponent<AutoDestroy>();
        }
    }

    public void Setup(Vector3 shootDir, float moveSpeed, eBulletType bulletType, GameObject hitImpact, bool isHitScan = true)
    {
        this.shootDir = shootDir;
        this.moveSpeed = moveSpeed;
        this.hitImpact = hitImpact;
        this.bulletType = bulletType;
        this.isHitScan = isHitScan;
        Destroy(gameObject, 5f);
        if (hitImpact.GetComponent<AutoDestroy>() == null)
        {
            hitImpact.AddComponent<AutoDestroy>();
        }
    }



    // Update is called once per frame
    void Update()
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //bool hitBall = false;
        ContactPoint contact = collision.GetContact(0);
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Hit ball");
            //hit = true;
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            GameObject vb = GameObject.FindGameObjectWithTag("Ball");
            if (isHitScan)
            {
                switch (bulletType)
                {
                    case eBulletType.Normal:
                        rb.velocity = Vector3.zero;
                        vb.transform.rotation = Camera.main.transform.rotation;
                        rb.isKinematic = false;
                        rb.velocity = Vector3.zero;
                        rb.AddForce(vb.transform.forward * ballShootForwardForce);
                        break;
                    case eBulletType.Jump:
                        //vb.transform.rotation = Camera.main.transform.rotation;

                        rb.isKinematic = false;
                        //ball_rb.velocity = Vector3.zero;
                        rb.AddForce(Vector3.up * ballJumpUpForce);
                        break;

                }
            }
        }


        Transform hitImpactPos = Instantiate(hitImpact).transform;
        hitImpactPos.position = contact.point;
        hitImpactPos.forward = contact.normal;
        MasterAudio.PlaySound3DAtVector3("OnHit_Ball", transform.position);
    }

}
