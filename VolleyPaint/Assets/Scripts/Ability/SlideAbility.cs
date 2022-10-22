using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Litkey/Ability/SlideAbility")]
public class SlideAbility : Ability
{
    [Header("Specific Settings")]
    public float slideForce = 10f;
    public float reducedHeight;
    float originHeight;

    CapsuleCollider collider;
    Rigidbody rb;
    public override void UseAbility(GameObject parent)
    {
        base.UseAbility(parent); // sets isUsingAbility to true
        // initialize
        Debug.Log(" Ability used");
        collider = parent.GetComponent<CapsuleCollider>();
        rb = parent.GetComponent<Rigidbody>();
        originHeight = collider.height;


        // use ability
        collider.height = reducedHeight;
        rb.AddForce(parent.transform.forward * slideForce, ForceMode.VelocityChange);

    }

    public override void BeginCooldown(GameObject parent)
    {
        base.BeginCooldown(parent); // sets isUsingAbility to false
        collider.height = originHeight;
    }

}
