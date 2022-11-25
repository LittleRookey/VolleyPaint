using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// shoot down = gravity pull
[CreateAssetMenu(menuName ="Litkey/Ability/ShootDown")]
public class ShootDown : Ability
{
    [Header("Specific Settings")]
    public float ballDownSpeed = 700f;
    public AudioClip audioClip;

    public override void OnAbilityStart(GameObject parent)
    {
        base.OnAbilityStart(parent);
        
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        GameObject player = Camera.main.transform.parent.gameObject;
        player.GetComponent<PlayerShoot>().InitiateShootBall(ball.transform.position, Vector3.down, ballDownSpeed);
    }
}
