using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blobcreate.Universal;
using Blobcreate.ProjectileToolkit;

public class BallBehaviour : ProjectileBehaviour
{
    public float radius = 4f;
    public float centerForce = 10f;
    public float forceUplit;
    public LayerMask scanMask;
    public LayerMask selfMask;
    //public bool isCurveBall;

    Collider[] result = new Collider[16];

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AbilityBox"))
        {
            Debug.Log("Trigger name: " + other.gameObject.name);
        }
    }
}
