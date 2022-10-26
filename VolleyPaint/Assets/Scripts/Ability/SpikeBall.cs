using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Litkey/Ability/SpikeBall")]
public class SpikeBall : Ability
{
    Transform ball;

    // stops the ball for a second and spikes the ball to the ground diagnally
    public override void UseAbility(GameObject parent)
    {
        base.UseAbility(parent);
        ball = GameObject.FindGameObjectWithTag("Ball").transform;
        //parent.GetComponent<PlayerShoot>()
        
    }

    public override void BeginCooldown(GameObject parent)
    {
        base.BeginCooldown(parent);
    }

    
}
