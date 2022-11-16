using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Blobcreate.Universal;
using Blobcreate.ProjectileToolkit;
using Unity.Netcode;


public class BallBehaviour : ProjectileBehaviour
{
    public float radius = 4f;
    public float centerForce = 10f;
    public float forceUplit;
    public LayerMask scanMask;
    public LayerMask selfMask;
    //public bool isCurveBall;
    public LayerMask groundMask;
    public float torqueForce = 0f;
    public float smallA = -0.1f;
    public float bigA = -0.01f;
    public float lerpSpeed = 5f;
    //private TrajectoryPredictor trajectory;
    //public bool drawLine;
    //public bool faceDirection;
    float currentA;
    float currentTorque;

    Collider[] result = new Collider[16];

    public UnityAction OnBallEnterTeam1;
    public UnityAction OnBallEnterTeam2;

    protected override void OnLaunch()
    {

    }

    // handles collision event when the ball collides with some object
    protected override void Explosion(Collision collision)
    {
        var c = Physics.OverlapSphereNonAlloc(transform.position, radius, result, scanMask);
        for (int i = 0; i < c; i++)
        {
            if (result[i].TryGetComponent<Rigidbody>(out var rb))
                rb.AddExplosionForce(centerForce, transform.position, radius, forceUplit, ForceMode.Impulse);
            else if (result[i].TryGetComponent<CharacterMovement>(out var cm))
                cm.AddExplosionForce(centerForce, transform.position, radius, forceUplit);

            result[i] = null;
        }

        base.Explosion(collision);
    }

    private void Fire(Vector3 target, float tCurrentA)
    {
        Launch(target);
        rb.isKinematic = false;
        // Magic happens!
        var f = Projectile.VelocityByA(transform.position, target, tCurrentA);
        Debug.Log($"ball pos: {transform.position}  targetPos: {target}");
        rb.AddForce(f, ForceMode.VelocityChange);

        // Add some torque, not necessary, but interesting.
        var t = Vector3.Lerp(torqueForce * Random.onUnitSphere,
            torqueForce * (target - transform.position).normalized, currentTorque);
        rb.AddTorque(t, ForceMode.VelocityChange);
    }
    [ServerRpc]
    public void FireServerRPC(Vector3 target, float tCurrentA)
    {
        Fire(target, tCurrentA);
        //var b = Instantiate(bulletPrefab, launchPoint.position, launchPoint.rotation);
        FireClientRPC(target, tCurrentA);

    }

    [ClientRpc]
    private void FireClientRPC(Vector3 target, float tCurrentA)
    {
        if (IsOwner) return;
        Fire(target, tCurrentA);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AbilityBox"))
        {
            Debug.Log("Trigger name: " + other.gameObject.name);
        }
    }
}
