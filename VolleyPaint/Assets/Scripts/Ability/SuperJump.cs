using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Litkey/Ability/SuperJump")]
public class SuperJump : Ability
{
    [SerializeField] private float maxEnergy = 500f;
    [SerializeField] private float currentEnergy = 0f;
    [SerializeField] private float energyGainSpeed = 0.1f;
    public float reducedHeight;
    float originHeight;

    [Range(0.01f, 1.0f)]
    public float reduceSpeed = 0.4f;
    float originSpeed;

    CapsuleCollider collider;
    Rigidbody rb;

    // first crouch and charge energy, can move slowly
    // on key up, super jump
    public override void UseAbility(GameObject parent)
    {
        base.UseAbility(parent);
        collider = parent.GetComponent<CapsuleCollider>();
        rb = parent.GetComponent<Rigidbody>();
        originHeight = collider.height;
        Debug.Log("Run Ability");
        
        collider.height = reducedHeight;
        //StartCharge();
    }

    // On key up
    public override void BeginCooldown(GameObject parent)
    {
        base.BeginCooldown(parent);
        // run ability 
        collider.height = originHeight;
        rb.AddForce(Vector3.up * currentEnergy);
    }

    public override void OnAbilityStart(GameObject parent)
    {
        base.OnAbilityStart(parent);
        PlayerMovement playerMovement = parent.GetComponent<PlayerMovement>();
        originSpeed = playerMovement.walkSpeed;
        playerMovement.walkSpeed *= reduceSpeed;
    }
    public override void OnAbilityRunning(GameObject parent)
    {
        base.OnAbilityRunning(parent);
        StartCharge();
    }

    public override void OnAbilityEnd(GameObject parent)
    {
        base.OnAbilityEnd(parent);
        currentEnergy = 0f;
        parent.GetComponent<PlayerMovement>().walkSpeed = originSpeed;
    }

    void StartCharge()
    {
        if (isUsingAbility)
        {
            
            if (currentEnergy <= maxEnergy)
            {
                currentEnergy += Time.deltaTime * energyGainSpeed;
            }
        }
    }
}
