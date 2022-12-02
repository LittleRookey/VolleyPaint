using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// shoot down = gravity pull
[CreateAssetMenu(menuName ="Litkey/Ability/ShootDown")]
public class ShootDown : Ability
{
    [Header("Specific Settings")]
    public float ballDownSpeed = 700f;
    public GameObject lightning;

    private GameObject ball;
    private GameObject player;

    public override void OnAbilityStart(GameObject parent)
    {
        base.OnAbilityStart(parent);
        
        ball = GameObject.FindGameObjectWithTag("Ball");
        player = Camera.main.transform.parent.gameObject;

        GameObject rpcManager = GameObject.Find("RPCManager");
        player.GetComponent<NetworkAudio>().PlayNetworkedAudio("ELECTRICITY", ball.transform.position);

        // spawn lightning
        player.GetComponent<NetworkSpawnOnBall>().SpawnNetworkedObject();
    }

    public override void OnAbilityEnd(GameObject parent)
    {
        base.OnAbilityEnd(parent);
        player.GetComponent<PlayerShoot>().InitiateShootBall(ball.transform.position, Vector3.down, ballDownSpeed);
    }
}
