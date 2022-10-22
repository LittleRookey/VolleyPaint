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
        currentEnergy = 0f;
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
        //currentEnergy = 0f;
    }

    public override void OnAbilityRunning()
    {
        base.OnAbilityRunning();
        StartCharge();
    }

    //public override void OnAbilityEnd()
    //{
    //    currentEnergy = 0f;
    //}

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
