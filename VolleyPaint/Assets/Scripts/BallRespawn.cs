using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallRespawn : NetworkBehaviour
{
    [SerializeField] private Transform teamOneBallSpawn;
    [SerializeField] private Transform teamTwoBallSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ClientRpc]
    public void RespawnBallClientRpc(Team team)
    {
        Transform ball = GameObject.FindGameObjectWithTag("Ball").transform;
        ball.GetComponent<Rigidbody>().isKinematic = true;
        if (team == Team.teamOne)
        {
            ball.position = teamOneBallSpawn.position;
        }
        else if (team == Team.teamTwo)
        {
            ball.position = teamTwoBallSpawn.position;
        }
    }

    public Vector3 GetSpawnPos(Team team)
    {
        if (team == Team.teamOne)
        {
            return teamOneBallSpawn.position;
        }
        else if (team == Team.teamTwo)
        {
            return teamTwoBallSpawn.position;
        }
        return new Vector3(0, 0, 0);
    }


}
