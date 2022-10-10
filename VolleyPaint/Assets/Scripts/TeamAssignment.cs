using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TeamAssignment : NetworkBehaviour
{
    public Team assignedTeam;
    public bool spawned;

    // Start is called before the first frame update
    void OnNetworkSpawn()
    {
        assignedTeam = GameObject.Find("GameManager").GetComponent<GameManagement>().GetTeamToAutoAssignTo();
        spawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned)
        {
            transform.position = GameObject.Find("Spawn Points").GetComponent<BallRespawn>().GetSpawnPos(assignedTeam);
            spawned = true;
        }
    }
}
