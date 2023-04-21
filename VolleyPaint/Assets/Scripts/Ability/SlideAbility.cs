using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

[CreateAssetMenu(menuName ="Litkey/Ability/SlideAbility")]
public class SlideAbility : Ability
{
    [Header("Specific Settings")]
    public float slideForce = 10f;
    public float reducedHeight;
    float originHeight;

    CapsuleCollider collider;
    Rigidbody rb;

    public override void OnAbilityStart(GameObject parent)
    {
        // more of a dash instead of a slide if we comment out the height change

        base.OnAbilityStart(parent);
        //collider = parent.GetComponent<CapsuleCollider>();
        rb = parent.GetComponent<Rigidbody>();
        //originHeight = collider.height;

        // use ability
        //collider.height = reducedHeight;
        rb.AddForce(parent.transform.forward * slideForce, ForceMode.VelocityChange);

        Animator anim = parent.transform.Find("unitychan").GetComponent<Animator>();
        anim.SetBool("Sliding", true);

        MasterAudio.PlaySound3DAtVector3("Sliding", parent.transform.position); // Play sound
        MasterAudio.PlaySound3DAtVector3("univ1257", parent.transform.position); // Play sound
    }

    public override void OnAbilityEnd(GameObject parent)
    {
        base.OnAbilityEnd(parent);
        //collider.height = originHeight;
        Animator anim = parent.transform.Find("unitychan").GetComponent<Animator>();
        anim.SetBool("Sliding", false);
    }
}
